using TripYatri.Core.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Z.Dapper.Plus;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Data.Sevices;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Providers.Dapper
{
    public class DapperProvider : IDapperService
    {
        private readonly RuntimeContext _runtimeContext;
        private readonly ISQLDataContext _sqlDataContext;
        private readonly ILogger _logger;

        public DapperProvider(
            RuntimeContext runtimeContext, 
            ISQLDataContext sqlDataContext, 
            ILogger logger)
        {
            _runtimeContext = runtimeContext;
            _sqlDataContext = sqlDataContext;
            _logger = logger;
        }

        public IEnumerable<dynamic> Query(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<T> Query<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query<T>(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<TReturn> Query<T1, T2, TReturn>(Database db, string query, Func<T1, T2, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<TReturn> Query<T1, T2, T3, TReturn>(Database db, string query, Func<T1, T2, T3, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<TReturn> Query<T1, T2, T3, T4, TReturn>(Database db, string query, Func<T1, T2, T3, T4, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<TReturn> Query<T1, T2, T3, T4, T5, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<TReturn> Query<T1, T2, T3, T4, T5, T6, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public IEnumerable<TReturn> Query<T1, T2, T3, T4, T5, T6, T7, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .Query(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync<T>(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, TReturn>(Database db, string query, Func<T1, T2, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, TReturn>(Database db, string query, Func<T1, T2, T3, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, TReturn>(Database db, string query, Func<T1, T2, T3, T4, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, T5, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, T5, T6, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryAsync(query, map, parameters, splitOn: splitOn, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public T QueryFirstOrDefault<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .QueryFirstOrDefault<T>(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QueryFirstOrDefaultAsync<T>(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public T QuerySingle<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db)
                .QuerySingle<T>(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public async Task<T> QuerySingleAsync<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .QuerySingleAsync<T>(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        public int Execute(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return _sqlDataContext.GetConnection(db).Execute(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
        }

        public async Task<int> ExecuteAsync(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            _logger.LogDebug($"SQL: {query}", parameters);
            return await _sqlDataContext.GetConnection(db)
                .ExecuteAsync(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }

        private int GetTimeoutInSeconds(TimeSpan? timeout)
        {
            if (timeout == null) timeout = _runtimeContext.RequestRemainingTime;
            return Convert.ToInt32(Math.Floor(timeout.Value.TotalSeconds));
        }

        public void BulkInsert<T>(Database db, IEnumerable<T> data) where T : class
        {
            _sqlDataContext.GetConnection(db).BulkMerge(data);
        }

        public async Task BulkInsertAsync<T>(Database db, IEnumerable<T> data) where T : class
        {
            await _sqlDataContext.GetConnection(db).BulkActionAsync(x => x.BulkMerge(data));
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            return await _sqlDataContext.GetConnection(db)
                .QueryMultipleAsync(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout))
                .ConfigureAwait(false);
        }
        public SqlMapper.GridReader QueryMultiple(Database db, string query, object parameters = null, TimeSpan? timeout = null)
        {
            return _sqlDataContext.GetConnection(db)
                .QueryMultiple(query, parameters, transaction: _sqlDataContext.GetTransaction(db), commandTimeout: GetTimeoutInSeconds(timeout));
                
        }
        public void Commit()
        {
            _sqlDataContext.Commit();
        }       
    }
}
