using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Z.Dapper.Plus;
using System.Linq;

namespace BilligKwhWebApp.Core
{
    public class BaseRepository : IBaseRepository
    {
        private int SafeCommandTimeout(int? commandTimeout)
        {
            return commandTimeout ?? 3600;
        }

        #region CRUD
        public void BulkInsert<T>(IEnumerable<T> items)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                connection.BulkInsert<T>(items);
            }
        }
        public void BulkMerge<T>(IEnumerable<T> items)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                connection.BulkMerge<T>(items);
            }
        }

        public void Insert<T>(T entityToInsert)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                connection.BulkInsert<T>(entityToInsert);
            }
        }

        public async Task InsertAsync<T>(T entityToInsert)
        {
            await Task.Run(() =>
            {
                using (var connection = ConnectionFactory.GetOpenConnection())
                {
                    connection.BulkInsert<T>(entityToInsert);
                }
            }).ConfigureAwait(false);
        }

        public bool Update<T>(T entity)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.BulkUpdate<T>(entity).Actions.Any();
            }
        }

        public Task<bool> UpdateAsync<T>(T entity)
        {
            return Task.Run(() =>
            {
                using (var connection = ConnectionFactory.GetOpenConnection())
                {
                    return connection.BulkUpdate<T>(entity).Actions.Any();
                }
            });
        }

        public int BulkUpdate<T>(T entity)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.BulkUpdate<T>(entity).Actions.Count;
            }
        }

        public bool Delete<T>(T entityToDelete)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.BulkDelete<T>(entityToDelete).Actions.Any();
            }
        }

        public Task<bool> DeleteAsync<T>(T entityToDelete)
        {
            return Task.Run(() =>
            {
                using (var connection = ConnectionFactory.GetOpenConnection())
                {
                    return connection.BulkDelete<T>(entityToDelete).Actions.Any();
                }
            });
        }

        public T ExecuteScalar<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.ExecuteScalar<T>(sql, parameters, transaction, SafeCommandTimeout(commandTimeout), commandType);
            }
        }
        #endregion

        public IEnumerable<TParent> QueryParentChild<TParent, TChild, TParentKey>(
                    string sql,
                    Func<TParent, TParentKey> parentKeySelector,
                    Func<TParent, IList<TChild>> childSelector,
                    dynamic param = null, IDbTransaction transaction = null,
                    bool buffered = true,
                    string splitOn = "Id",
                    int? commandTimeout = null,
                    CommandType? commandType = null)
        {
            Dictionary<TParentKey, TParent> cache = new Dictionary<TParentKey, TParent>();

            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                connection.Query<TParent, TChild, TParent>(
                sql,
                (parent, child) =>
                {
                    if (!cache.ContainsKey(parentKeySelector(parent)))
                    {
                        cache.Add(parentKeySelector(parent), parent);
                    }

                    TParent cachedParent = cache[parentKeySelector(parent)];
                    IList<TChild> children = childSelector(cachedParent);
                    children.Add(child);
                    return cachedParent;
                },
                param as object, transaction, buffered, splitOn, commandTimeout, commandType);

                return cache.Values;
            }
        }

        public T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parameters = null, int? commandTimeout = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query<T>(sql, parameters, null, true, SafeCommandTimeout(commandTimeout));
            }
        }

        public IEnumerable<T> Query<T, A>(string sql, Func<T, A, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters);
            }
        }
        public IEnumerable<T> Query<T, A>(string sql, Func<T, A, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters, null, true, splitOn);
            }
        }

        public IEnumerable<T> Query<T, A, B>(string sql, Func<T, A, B, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters);
            }
        }
        public IEnumerable<T> Query<T, A, B>(string sql, Func<T, A, B, T> map, string splitOn, object parameters = null, int? commandTimeout = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters, splitOn: splitOn, commandTimeout: commandTimeout);
            }
        }
        public IEnumerable<T> Query<T, A, B, C>(string sql, Func<T, A, B, C, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters, splitOn: splitOn);
            }
        }
        public IEnumerable<T> Query<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters, splitOn: splitOn);
            }
        }
        public IEnumerable<T> Query<T, A, B, C, D, E>(string sql, Func<T, A, B, C, D, E, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters, splitOn: splitOn);
            }
        }

        public IEnumerable<T> Query<T, A, B, C>(string sql, Func<T, A, B, C, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters);
            }
        }

        public IEnumerable<T> Query<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters);
            }
        }

        public IEnumerable<T> Query<T, A, B, C, D, E>(string sql, Func<T, A, B, C, D, E, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query(sql, map, parameters);
            }
        }

        public int Execute(string sql, object parameters = null, int? commandTimeout = 3600, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Execute(sql, parameters, transaction, SafeCommandTimeout(commandTimeout), commandType);
            }
        }

        public IEnumerable<T> QueryStoredProdure<T>(string procName, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return connection.Query<T>(procName, parameters, null, true, 3600, CommandType.StoredProcedure);
            }
        }

        #region Async
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync<T>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync<T>(sql, parameters, commandType: commandType).ConfigureAwait(false);
            }
        }



        public IEnumerable<T> QueryUnbuffered<T>(string sql, object parameters = null, int? commandTimeout = null)
        {
            using var connection = ConnectionFactory.GetOpenConnection();
            foreach (T item in connection.Query<T>(new CommandDefinition(sql, parameters, commandTimeout: commandTimeout, flags: CommandFlags.None)))
            {
                yield return item;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, A>(string sql, Func<T, A, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, A>(string sql, Func<T, A, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters, splitOn: splitOn).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, A, B>(string sql, Func<T, A, B, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters).ConfigureAwait(false);
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T, A, B>(string sql, Func<T, A, B, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters, splitOn: splitOn).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, A, B, C>(string sql, Func<T, A, B, C, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T, A, B, C, D>(string sql, Func<T, A, B, C, D, T> map, string splitOn, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync(sql, map, parameters, splitOn: splitOn).ConfigureAwait(false);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.ExecuteAsync(sql, parameters, transaction, SafeCommandTimeout(commandTimeout), commandType).ConfigureAwait(false);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.ExecuteScalarAsync<T>(sql, parameters, transaction, SafeCommandTimeout(commandTimeout), commandType).ConfigureAwait(false);
            }
        }

        public int ExecuteStoredProdure(string procName, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Execute(procName, parameters, SafeCommandTimeout(commandTimeout), transaction, CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteStoredProdureAsync(string procName, object parameters = null, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            return await ExecuteAsync(procName, parameters, transaction, SafeCommandTimeout(commandTimeout), CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> QueryStoredProdureAsync<T>(string procName, object parameters = null)
        {
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                return await connection.QueryAsync<T>(procName, parameters, null, commandTimeout: 3600, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
