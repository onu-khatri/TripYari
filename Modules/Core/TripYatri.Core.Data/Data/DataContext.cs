using TripYatri.Core.Base.Providers;
using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TripYatri.Core.Providers.Dapper;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers.Logger;
using TripYatri.Core.Base.Providers.Metrics;

namespace TripYatri.Core.Data
{
    public class DataContext : ISQLDataContext, IRepositoryDataContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IOptions<DataOptions> _dataOptions;
        private readonly RuntimeContext _runtimeContext;

        private readonly IDictionary<Type, IDataRepository> _cachedRepositories =
            new Dictionary<Type, IDataRepository>();

        private IDictionary<Database, IDbConnection> Connections { get; } = new ConcurrentDictionary<Database, IDbConnection>();

        private IDictionary<Database, IDbTransaction> Transactions { get; } =
            new Dictionary<Database, IDbTransaction>();

        private bool _inTransactionMode;
        private bool _disposed;

        private static readonly IDictionary<Type, Type> ResolvedConcreteTypes = new ConcurrentDictionary<Type, Type>();

        internal DataContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger>();
            _dataOptions = serviceProvider.GetRequiredService<IOptions<DataOptions>>();
            _runtimeContext = serviceProvider.GetRequiredService<RuntimeContext>();
        }

        private static string GetConnectionString(DataOptions.ConnectionStringOptions connectionOptions)
        {
            var connectionString = new StringBuilder(connectionOptions.ConnectionString);

            if (!string.IsNullOrWhiteSpace(connectionOptions.User))
                connectionString.Append($";user id={connectionOptions.User}");
            if (!string.IsNullOrWhiteSpace(connectionOptions.Password))
                connectionString.Append($";password={connectionOptions.Password}");

            return connectionString.ToString();
        }

        public string GetConnectionString(Database db)
        {
            if (!_dataOptions.Value.Connections.TryGetValue(db.Name, out var connectionOptions))
                throw new ArgumentException($"Could not find a Connection Options with name {db}");
            if (connectionOptions == null || string.IsNullOrWhiteSpace(connectionOptions.ConnectionString))
                throw new ArgumentException(
                    $"Invalid Connection Options with name {db}: Connection String cannot be null or empty");

            return GetConnectionString(connectionOptions);
        }

        public IDbConnection GetConnection(Database db)
        {
            if (Connections.TryGetValue(db, out var connection)) return connection;

            if (!_dataOptions.Value.Connections.TryGetValue(db.ToString(), out var connectionOptions))
                throw new ArgumentException($"Could not find a Connection Options with name {db}");
            if (connectionOptions == null || string.IsNullOrWhiteSpace(connectionOptions.ConnectionString))
                throw new ArgumentException(
                    $"Invalid Connection Options with name {db}: Connection String cannot be null or empty");

            var connectionString = GetConnectionString(connectionOptions);

            switch (db.DatabaseProviderType)
            {
                case DatabaseProvider.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
                default:
                    connection = new MySqlConnector.MySqlConnection(connectionString);
                    break;
            }

            connection.Open();

            switch (db.DatabaseProviderType)
            {
                case DatabaseProvider.MySql:
                    if (!string.IsNullOrWhiteSpace(connectionOptions.Collation))
                        connection.Execute($"SET collation_connection = {connectionOptions.Collation}");
                    break;
            }


            Connections[db] = connection;

            if (_inTransactionMode)
            {
                BeginTransaction(db);
            }

            return connection;
        }

        public IDbTransaction GetTransaction(Database db)
        {
            Transactions.TryGetValue(db, out var transaction);

            return transaction;
        }

        public void BeginTransactions()
        {
            foreach (var connectedDb in Connections.Keys)
                BeginTransaction(connectedDb);
            _inTransactionMode = true;
        }

        private void BeginTransaction(Database db)
        {
            if (Transactions.ContainsKey(db)) return;

            Transactions[db] = GetConnection(db).BeginTransaction();
            _inTransactionMode = true;
        }

        public void Commit()
        {
            var committedDatabases = new List<Database>();
            var exceptions = new List<Exception>();
            foreach (var transactionPair in Transactions.ToList())
            {
                try
                {
                    transactionPair.Value?.Commit();
                    committedDatabases.Add(transactionPair.Key);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    try
                    {
                        Rollback();
                    }
                    catch (Exception inex)
                    {
                        exceptions.Add(inex);
                    }

                    if (committedDatabases.Any())
                        throw new AggregateException(
                            $"Failed to commit transaction in database {transactionPair.Key}. Database transactions already committed: {string.Join(", ", committedDatabases)}.",
                            exceptions);
                    throw new AggregateException(
                        $"Failed to commit transaction in database {transactionPair.Key}. All other transactions were rolled back.",
                        exceptions);
                }
                finally
                {
                    if (Transactions.Remove(transactionPair.Key))
                        transactionPair.Value?.Dispose();
                }
            }

            _inTransactionMode = false;
        }

        public void Rollback()
        {
            var exceptions = new List<Exception>();
            foreach (var transactionPair in Transactions.ToList())
            {
                try
                {
                    transactionPair.Value?.Rollback();
                }
                catch (Exception ex)
                {
                    exceptions.Add(
                        new ApplicationException($"Failed to rollback transaction on database {transactionPair.Key}",
                            ex));
                }
                finally
                {
                    if (Transactions.Remove(transactionPair.Key))
                        transactionPair.Value?.Dispose();
                }
            }

            _inTransactionMode = false;

            if (exceptions.Any())
                throw new AggregateException($"Failed to rollback transaction(s).", exceptions);
        }

        public virtual void Dispose()
        {
            Release(true);
            GC.SuppressFinalize(this);
        }

        private void Release(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var transactionPair in Transactions)
                    {
                        try
                        {
                            transactionPair.Value.Dispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to dispose transaction for {transactionPair.Key}");
                        }
                    }

                    Transactions.Clear();
                    _inTransactionMode = false;

                    foreach (var connectionPair in Connections)
                    {
                        try
                        {
                            connectionPair.Value.Dispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to dispose connection for {connectionPair.Key}");
                        }
                    }

                    Connections.Clear();
                }

                _disposed = true;
            }
        }

        public T GetRepository<T>() where T : IDataRepository
        {
            var concreteType = typeof(T);

            if (concreteType.IsInterface || concreteType.IsAbstract)
                concreteType = FindConcreteTypeOf(concreteType);

            if (_cachedRepositories.ContainsKey(concreteType))
                return (T) _cachedRepositories[concreteType];

            T repository;
            try
            {
                repository = (T) Activator.CreateInstance(
                    concreteType, 
                    _serviceProvider,
                    new DapperProvider(_runtimeContext, this, _logger));
            }
            catch (Exception v2Exception)
            {
                try
                {
                    var metricsProvider = _serviceProvider.GetRequiredService<IMetricsProvider>();
                    var dataOptions = _serviceProvider.GetRequiredService<IOptions<DataOptions>>();
                    repository = (T)Activator.CreateInstance(concreteType, _logger, metricsProvider, new DapperProvider(_runtimeContext, this, _logger), dataOptions);
                }
                catch (Exception v1Exception)
                {
                    throw new AggregateException($"Failed to instantiate Repository {concreteType.Name}", v2Exception, v1Exception);
                }
            }
            _cachedRepositories[concreteType] = repository;
            return repository;
        }

        private static Type FindConcreteTypeOf(Type requestedType)
        {
            if (ResolvedConcreteTypes.ContainsKey(requestedType)) return ResolvedConcreteTypes[requestedType];

            var baseDapperRepositoryType = typeof(IDataRepository);
            var concreteCandidates = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("TripYatri.Core"))
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(t => requestedType.IsAssignableFrom(t) && baseDapperRepositoryType.IsAssignableFrom(t))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToList();

            if (!concreteCandidates.Any())
                throw new InvalidOperationException(
                    $"Could not find concrete type for interface/abstract-class {requestedType.Name}");
            if (concreteCandidates.Count > 1)
                throw new InvalidOperationException(
                    $"Found {concreteCandidates.Count} (>1) candidate concrete type for interface/abstract-class {requestedType.Name}: Found types {string.Join(", ", concreteCandidates)}");

            return ResolvedConcreteTypes[requestedType] = concreteCandidates.First();
        }

        ~DataContext()
        {
            Release(false);
        }
    }
}