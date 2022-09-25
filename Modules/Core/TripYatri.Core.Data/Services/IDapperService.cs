using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TripYatri.Core.Data.Sevices
{
    public interface IDapperService
    {
        IEnumerable<dynamic> Query(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<T> Query<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<TReturn> Query<T1, T2, TReturn>(Database db, string query, Func<T1, T2, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<TReturn> Query<T1, T2, T3, TReturn>(Database db, string query, Func<T1, T2, T3, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<TReturn> Query<T1, T2, T3, T4, TReturn>(Database db, string query, Func<T1, T2, T3, T4, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<TReturn> Query<T1, T2, T3, T4, T5, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<TReturn> Query<T1, T2, T3, T4, T5, T6, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        IEnumerable<TReturn> Query<T1, T2, T3, T4, T5, T6, T7, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<T1, T2, TReturn>(Database db, string query, Func<T1, T2, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, TReturn>(Database db, string query, Func<T1, T2, T3, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, TReturn>(Database db, string query, Func<T1, T2, T3, T4, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, T5, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, T5, T6, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        Task<IEnumerable<TReturn>> QueryAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(Database db, string query, Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map, string splitOn, object parameters = null, TimeSpan? timeout = null);
        T QueryFirstOrDefault<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        T QuerySingle<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        Task<T> QuerySingleAsync<T>(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        int Execute(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        Task<int> ExecuteAsync(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        void BulkInsert<T>(Database db, IEnumerable<T> data) where T : class;
        Task<SqlMapper.GridReader> QueryMultipleAsync(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        SqlMapper.GridReader QueryMultiple(Database db, string query, object parameters = null, TimeSpan? timeout = null);
        Task BulkInsertAsync<T>(Database db, IEnumerable<T> data) where T : class;
        void Commit();
    }
}
