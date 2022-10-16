using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Core.Interfaces
{
    public interface IBaseRepository
    {
        int BulkUpdate<T>(T entity);
        bool Delete<T>(T entityToDelete);
        Task<bool> DeleteAsync<T>(T entityToDelete);
        int Execute(string sql, object parameters = null, int? commandTimeout = 3600, IDbTransaction transaction = null, CommandType? commandType = null);
        IEnumerable<T> QueryStoredProdure<T>(string procName, object parameters = null);


        T ExecuteScalar<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<T>> QueryStoredProdureAsync<T>(string procName, object parameters = null);
        Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        int ExecuteStoredProdure(string procName, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<int> ExecuteStoredProdureAsync(string procName, object parameters = null, int? commandTimeout = null, IDbTransaction transaction = null);

        void BulkInsert<T>(IEnumerable<T> items);
        void BulkMerge<T>(IEnumerable<T> items);

        void Insert<T>(T entityToInsert);
        Task InsertAsync<T>(T entityToInsert);
        IEnumerable<T> Query<T, A, B, C, D, E>(string sql, Func<T, A, B, C, D, E, T> map, object parameters = null);
        IEnumerable<T> Query<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, object parameters = null);

        IEnumerable<T> Query<T, A, B, C, D, E>(string sql, Func<T, A, B, C, D, E, T> map, string splitOn, object parameters = null);
        IEnumerable<T> Query<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, string splitOn, object parameters = null);
        IEnumerable<T> Query<T, A, B, C>(string sql, Func<T, A, B, C, T> map, string splitOn, object parameters = null);
        IEnumerable<T> Query<T, A, B, C>(string sql, Func<T, A, B, C, T> map, object parameters = null);
        IEnumerable<T> Query<T, A, B>(string sql, Func<T, A, B, T> map, object parameters = null);
        IEnumerable<T> Query<T, A, B>(string sql, Func<T, A, B, T> map, string splitOn, object parameters = null, int? commandTimeout = null);
        IEnumerable<T> Query<T, A>(string sql, Func<T, A, T> map, string splitOn, object parameters = null);
        IEnumerable<T> Query<T, A>(string sql, Func<T, A, T> map, object parameters = null);
        IEnumerable<T> Query<T>(string sql, object parameters = null, int? commandTimeout = null);
        IEnumerable<TParent> QueryParentChild<TParent, TChild, TParentKey>(
                            string sql,
                            Func<TParent, TParentKey> parentKeySelector,
                            Func<TParent, IList<TChild>> childSelector,
                            dynamic param = null, IDbTransaction transaction = null,
                            bool buffered = true,
                            string splitOn = "Id",
                            int? commandTimeout = null,
                            CommandType? commandType = null);
        Task<IEnumerable<T>> QueryAsync<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, string splitOn, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T, A, B, C>(string sql, Func<T, A, B, C, T> map, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T, A, B>(string sql, Func<T, A, B, T> map, string splitOn, object parameters = null);

        Task<IEnumerable<T>> QueryAsync<T, A, B>(string sql, Func<T, A, B, T> map, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T, A>(string sql, Func<T, A, T> map, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T, A>(string sql, Func<T, A, T> map, string splitOn, object parameters = null);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, object parameters = null);
        IEnumerable<T> QueryUnbuffered<T>(string sql, object parameters = null, int? commandTimeout = null);
        T QueryFirstOrDefault<T>(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null);
        bool Update<T>(T entity);
        Task<bool> UpdateAsync<T>(T entity);
    }
}
