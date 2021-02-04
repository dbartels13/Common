using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Sphyrnidae.Common.Dal.Models;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Dal
{
    /// <summary>
    /// A base repository instance
    /// </summary>
    public abstract class BaseRepo
    {
        #region Abstract Properties
        /// <summary>
        /// The actual connection string
        /// </summary>
        public abstract string CnnStr { get; }

        /// <summary>
        /// Nice name of the database connection (eg. web.config key) used for logging
        /// </summary>
        protected abstract string CnnName { get; }

        /// <summary>
        /// The actual connection to the database
        /// </summary>
        protected abstract IDbConnection GetConnection { get; }

        /// <summary>
        /// For use with database logger: Set to false so that it won't recursively log the log call
        /// </summary>
        protected virtual bool DoLog => true;

        /// <summary>
        /// The trapped exception
        /// </summary>
        protected Exception Ex;

        /// <summary>
        /// If an exception was trapped, this will provide access to the trapped exception for proper responses
        /// </summary>
        /// <returns>The response object based on the exception</returns>
        public virtual ApiResponseObject HandleException() => null;

        /// <summary>
        /// If you need to execute something before the main call
        /// </summary>
        protected virtual Task PreCall(IDbConnection cnn, IDbTransaction trans)
        {
            cnn.Open();
            return Task.CompletedTask;
        }

        /// <summary>
        /// If you need to execute something after the main call
        /// </summary>
        protected virtual Task PostCall(IDbConnection cnn, IDbTransaction trans) => Task.CompletedTask;

        /// <summary>
        /// Implementation of the ILogger interface
        /// </summary>
        protected ILogger Logger { get; }
        #endregion

        #region Constructor
        protected BaseRepo(ILogger logger) => Logger = logger;
        #endregion

        #region Executions
        private async Task<T> ExecuteAsync<T>(
            string sql,
            object parameters,
            IDbTransaction trans,
            CommandType type,
            Func<IDbConnection, string, object, IDbTransaction, CommandType, Task<T>> method,
            bool trapExceptions = false)
        {
            var cnn = trans?.Connection ?? GetConnection;

            try
            {
                var info = DoLog ? await Logger.DatabaseEntry(CnnName, sql, parameters) : null;
                await PreCall(cnn, trans);
                var result = await SafeTry.OnException(
                    async () => await method(cnn, sql, parameters, trans, type),
                    ex =>
                    {
                        Ex = ex;
                        if (!trapExceptions)
                            throw ex;
                        return default(T);
                    });
                await PostCall(cnn, trans);
                await Logger.DatabaseExit(info);
                return result;
            }
            finally
            {
                if (trans?.Connection == null)
                    cnn.Close();
            }
        }
        #endregion

        #region Write
        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Number of records affected</returns>
        protected async Task<int> WriteSQLAsync(string sql, object parameters, IDbTransaction trans = null)
            => (await WriteAsync(sql, CommandType.Text, parameters, trans)).Value;

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Number of records affected</returns>
        protected async Task<int> WriteSPAsync(string sp, object parameters, IDbTransaction trans = null)
            => (await WriteAsync(sp, CommandType.StoredProcedure, parameters, trans)).Value;

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>False if nothing was changed, otherwise true</returns>
        protected async Task<bool> WriteSQLAsBoolAsync(string sql, object parameters, IDbTransaction trans = null)
            => (await WriteAsync(sql, CommandType.Text, parameters, trans)).Value != 0;

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>False if nothing was changed, otherwise true</returns>
        protected async Task<bool> WriteSPAsBoolAsync(string sp, object parameters, IDbTransaction trans = null)
            => (await WriteAsync(sp, CommandType.StoredProcedure, parameters, trans)).Value != 0;

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Number of records affected, or null if exception occurred</returns>
        protected async Task<int?> WriteSQLTrappingExceptionsAsync(string sql, object parameters,
            IDbTransaction trans = null)
            => await WriteAsync(sql, CommandType.Text, parameters, trans, true);

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Number of records affected, or null if exception occurred</returns>
        protected async Task<int?> WriteSPTrappingExceptionsAsync(string sp, object parameters,
            IDbTransaction trans = null)
            => await WriteAsync(sp, CommandType.StoredProcedure, parameters, trans, true);

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Null if exception occurred, false if nothing was changed, otherwise true</returns>
        protected async Task<bool?> WriteSQLAsBoolTrappingExceptionsAsync(string sql, object parameters,
            IDbTransaction trans = null)
            => await WriteAsync(sql, CommandType.Text, parameters, trans, true) switch
            {
                null => default(bool?),
                0 => false,
                _ => true
            };

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Null if exception occurred, false if nothing was changed, otherwise true</returns>
        protected async Task<bool?> WriteSPAsBoolTrappingExceptionsAsync(string sp, object parameters,
            IDbTransaction trans = null)
            => await WriteAsync(sp, CommandType.StoredProcedure, parameters, trans, true) switch
            {
                null => default(bool?),
                0 => false,
                _ => true
            };

        /// <summary>
        /// Executes something against a database
        /// </summary>
        /// <param name="sql">The sql to execute</param>
        /// <param name="type">If this is a SP/Text/etc</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <param name="trapExceptions">Optional: Default = false. If true, this will not throw out exceptions (will be caught in variable Ex)</param>
        /// <returns>Number of records affected, or null if exception occurred</returns>
        protected async Task<int?> WriteAsync(string sql, CommandType type, object parameters,
            IDbTransaction trans = null, bool trapExceptions = false)
            => await ExecuteAsync(sql, parameters, trans, type, DoWriteAsync, trapExceptions);

        private static async Task<int?> DoWriteAsync(IDbConnection cnn, string sql, object parameters,
            IDbTransaction trans, CommandType type)
            => await cnn.ExecuteAsync(sql, parameters, trans, null, type);
        #endregion

        #region Scalar

        /// <summary>
        /// Executes something against a database that returns a single result
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The result (or default if no records)</returns>
        protected async Task<T> ScalarSQLAsync<T>(string sql, object parameters, IDbTransaction trans = null)
            => await ScalarAsync<T>(sql, CommandType.Text, parameters, trans);

        /// <summary>
        /// Executes something against a database that returns a single result
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The result (or default if no records)</returns>
        protected async Task<T> ScalarSPAsync<T>(string sp, object parameters, IDbTransaction trans = null)
            => await ScalarAsync<T>(sp, CommandType.StoredProcedure, parameters, trans);

        /// <summary>
        /// Executes something against a database that returns a single result
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The result (or default if no records)</returns>
        protected async Task<T> ScalarSQLTrappingExceptionsAsync<T>(string sql, object parameters,
            IDbTransaction trans = null)
            => await ScalarAsync<T>(sql, CommandType.Text, parameters, trans, true);

        /// <summary>
        /// Executes something against a database that returns a single result
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The result (or default if no records)</returns>
        protected async Task<T> ScalarSPTrappingExceptionsAsync<T>(string sp, object parameters,
            IDbTransaction trans = null)
            => await ScalarAsync<T>(sp, CommandType.StoredProcedure, parameters, trans, true);

        /// <summary>
        /// Executes something against a database that returns a single result
        /// </summary>
        /// <param name="sql">The sql to execute</param>
        /// <param name="type">If this is a SP/Text/etc</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <param name="trapExceptions">Optional: Default = false. If true, this will not throw out exceptions (will be caught in variable Ex)</param>
        /// <returns>The result (or default if no records)</returns>
        protected async Task<T> ScalarAsync<T>(string sql, CommandType type, object parameters,
            IDbTransaction trans = null, bool trapExceptions = false)
            => await ExecuteAsync(sql, parameters, trans, type, DoScalarAsync<T>, trapExceptions);

        private static async Task<T> DoScalarAsync<T>(IDbConnection cnn, string sql, object parameters,
            IDbTransaction trans, CommandType type)
            => await cnn.ExecuteScalarAsync<T>(sql, parameters, trans, null, type);
        #endregion

        #region Insert Identity

        /// <summary>
        /// Inserts a record into the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>Response Object</returns>
        protected async Task<int?> InsertAsync(string sql, object parameters,
            IDbTransaction trans = null)
            => await InsertAsync(sql, DatabaseIdentity.Identity, parameters, trans);

        /// <summary>
        /// Inserts a record into the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The ID of the object</returns>
        protected async Task<int?> InsertTrappingExceptionsAsync(string sql, object parameters,
            IDbTransaction trans = null)
            => await InsertAsync(sql, DatabaseIdentity.Identity, parameters, trans, true);

        /// <summary>
        /// Inserts a record into the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="identity">Default = @@Identity (Identity). The type of identity to retrieve</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <param name="trapExceptions">Optional: Default = false. If true, this will not throw out exceptions (will be caught in variable Ex)</param>
        /// <returns>The ID of the object</returns>
        protected async Task<int?> InsertAsync(string sql, DatabaseIdentity identity,
            object parameters, IDbTransaction trans = null, bool trapExceptions = false)
        {
            sql = ModifySqlInsert(sql, identity);
            return await ExecuteAsync(sql, parameters, trans, CommandType.Text, DoScalarAsync<int?>, trapExceptions);
        }

        private static string ModifySqlInsert(string sql, DatabaseIdentity identity) => $"{sql}; SELECT CAST({(identity == DatabaseIdentity.Identity ? "@@IDENTITY" : "SCOPE_IDENTITY()")} AS INT)";
        #endregion

        #region Get single record
        /// <summary>
        /// Retrieves a single record from the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The record (or default/null if no records)</returns>
        protected async Task<T> GetSQLAsync<T>(string sql, object parameters, IDbTransaction trans = null)
            => await GetAsync<T>(sql, CommandType.Text, parameters, trans);
        /// <summary>
        /// Retrieves a single record from the database
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The record (or default/null if no records)</returns>
        protected async Task<T> GetSPAsync<T>(string sp, object parameters, IDbTransaction trans = null)
            => await GetAsync<T>(sp, CommandType.StoredProcedure, parameters, trans);
        /// <summary>
        /// Retrieves a single record from the database
        /// </summary>
        /// <param name="sql">The sql to execute</param>
        /// <param name="type">If this is a SP/Text/etc</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The record (or default/null if no records)</returns>
        protected async Task<T> GetAsync<T>(string sql, CommandType type, object parameters, IDbTransaction trans = null)
            => await ExecuteAsync(sql, parameters, trans, type, GetFirstOrDefaultAsync<T>);
        private static async Task<T> GetFirstOrDefaultAsync<T>(IDbConnection cnn, string sql, object parameters, IDbTransaction trans, CommandType type)
            => await cnn.QueryFirstOrDefaultAsync<T>(sql, parameters, trans, null, type);
        #endregion

        #region Get multiple records
        /// <summary>
        /// Retrieves multiple records (0, 1, or more) from the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The records (Could be 0, 1, or more)</returns>
        protected async Task<IEnumerable<T>> GetListSQLAsync<T>(string sql, object parameters, IDbTransaction trans = null)
            => await GetListAsync<T>(sql, CommandType.Text, parameters, trans);
        /// <summary>
        /// Retrieves multiple records (0, 1, or more) from the database
        /// </summary>
        /// <param name="sp">The Stored Procedure to execute</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The records (Could be 0, 1, or more)</returns>
        protected async Task<IEnumerable<T>> GetListSPAsync<T>(string sp, object parameters, IDbTransaction trans = null)
            => await GetListAsync<T>(sp, CommandType.StoredProcedure, parameters, trans);
        /// <summary>
        /// Retrieves multiple records (0, 1, or more) from the database
        /// </summary>
        /// <param name="sql">The sql to execute</param>
        /// <param name="type">If this is a SP/Text/etc</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>The records (Could be 0, 1, or more)</returns>
        protected async Task<IEnumerable<T>> GetListAsync<T>(string sql, CommandType type, object parameters, IDbTransaction trans = null)
            => await ExecuteAsync(sql, parameters, trans, type, GetListAsync<T>);
        private static async Task<IEnumerable<T>> GetListAsync<T>(IDbConnection cnn, string sql, object parameters, IDbTransaction trans, CommandType type)
            => await cnn.QueryAsync<T>(sql, parameters, trans, null, type);
        #endregion

        #region Exists
        /// <summary>
        /// Use this to add a "condition" to be used in "Exists" check
        /// </summary>
        /// <param name="fromTableAndWhere">Eg. [tableName] where [something]=variable</param>
        /// <returns>A string to be inserted into "Exists" method (multiple calls can be concatenated together)</returns>
        protected static string ExistsCondition(string fromTableAndWhere) => $" WHEN EXISTS (SELECT 1 FROM {fromTableAndWhere}) THEN CAST(1 AS BIT)";
        /// <summary>
        /// Does an existence check against the database
        /// </summary>
        /// <param name="conditions">The concatenated list of "ExistsCondition" method calls</param>
        /// <param name="parameters">Any parameters needed for the execution</param>
        /// <param name="trans">Optional: The transaction to be used</param>
        /// <returns>True if found, otherwise false</returns>
        protected async Task<bool> ExistsAsync(string conditions, object parameters, IDbTransaction trans = null)
            => await ScalarAsync<bool>($"SELECT CASE {conditions} ELSE CAST(0 AS BIT) END;", CommandType.Text, parameters, trans);
        #endregion
    }
}
