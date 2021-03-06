﻿using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;
using Sphyrnidae.Common.Dal.Models;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Utilities;
using IsolationLevel = System.Data.IsolationLevel;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Dal
{
    /// <summary>
    /// Database Transactions
    /// </summary>
    public class Transaction<TC> where TC : DbConnection, new()
    {
        #region Distributed (Multiple) Database Transaction <Func>
        /// <summary>
        /// Wrapper for multiple Sql calls across multiple databases within a transaction
        /// </summary>
        /// <remarks>This will use individual connections using a distributed transactions (performance hit)</remarks>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="method">The actual SQL calls</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Distributed<T>(ILogger logger, Func<Task<TransactionResponse<T>>> method)
            => Distributed(logger, method, ExceptionRethrow);

        /// <summary>
        /// Wrapper for multiple Sql calls across multiple databases within a transaction
        /// </summary>
        /// <remarks>This will use individual connections using a distributed transactions (performance hit)</remarks>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="defaultValue">Default = Default(T). If an exception is thrown during "method", and it is not rethrowing the exception, this will instead be returned</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Distributed<T>(ILogger logger, Func<Task<TransactionResponse<T>>> method, T defaultValue)
            => Distributed(logger, method, ExceptionDefaultVal, defaultValue);

        /// <summary>
        /// Wrapper for multiple Sql calls across multiple databases within a transaction
        /// </summary>
        /// <remarks>This will use individual connections using a distributed transactions (performance hit)</remarks>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="exceptionMethod">Default = ExceptionRethrow. If an exception is thrown during "method", how will it be handled (besides being rolled back)</param>
        /// <param name="defaultValue">Default = Default(T). If an exception is thrown during "method", and it is not rethrowing the exception, this will instead be returned</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static async Task<T> Distributed<T>(ILogger logger, Func<Task<TransactionResponse<T>>> method,
            Func<Exception, T, T> exceptionMethod, T defaultValue = default)
        {
            DatabaseInformation info = null;
            if (logger != null)
                info = logger.DatabaseEntry("Database Transaction", "Func Distributed Transaction");

            using var scope = new TransactionScope();
            var result = await SafeTry.OnException(
                async () =>
                {
                    var response = await method();
                    if (response.Success)
                        scope.Complete();
                    return response.Response;
                },
                ex => exceptionMethod(ex, defaultValue));

            if (logger != null)
                logger.DatabaseExit(info);
            return result;
        }
        #endregion

        #region Single Database Transaction <Func>
        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Run<T>(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse<T>>> method)
            => Run(logger, cnnStr, method, IsolationLevel.ReadCommitted, ExceptionRethrow);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="defaultValue">Default = Default(T). If an exception is thrown during "method", and it is not rethrowing the exception, this will instead be returned</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Run<T>(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse<T>>> method, T defaultValue)
            => Run(logger, cnnStr, method, IsolationLevel.ReadCommitted, ExceptionDefaultVal, defaultValue);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="isolation">Default = ReadCommitted. Isolation level for the transaction.</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Run<T>(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse<T>>> method, IsolationLevel isolation)
            => Run(logger, cnnStr, method, isolation, ExceptionRethrow);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="isolation">Default = ReadCommitted. Isolation level for the transaction.</param>
        /// <param name="defaultValue">Default = Default(T). If an exception is thrown during "method", and it is not rethrowing the exception, this will instead be returned</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Run<T>(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse<T>>> method, IsolationLevel isolation, T defaultValue)
            => Run(logger, cnnStr, method, isolation, ExceptionDefaultVal, defaultValue);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="exceptionMethod">Default = ExceptionRethrow. If an exception is thrown during "method", how will it be handled (besides being rolled back)</param>
        /// <param name="defaultValue">Default = Default(T). If an exception is thrown during "method", and it is not rethrowing the exception, this will instead be returned</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static Task<T> Run<T>(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse<T>>> method, Func<Exception, T, T> exceptionMethod,
            T defaultValue = default)
            => Run(logger, cnnStr, method, IsolationLevel.ReadCommitted, exceptionMethod, defaultValue);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <typeparam name="T">Return type of the complete transaction</typeparam>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="isolation">Default = ReadCommitted. Isolation level for the transaction.</param>
        /// <param name="exceptionMethod">Default = ExceptionRethrow. If an exception is thrown during "method", how will it be handled (besides being rolled back)</param>
        /// <param name="defaultValue">Default = Default(T). If an exception is thrown during "method", and it is not rethrowing the exception, this will instead be returned</param>
        /// <returns>The result from the SQL calls (method) - or possibly default(T) if there was an exception</returns>
        public static async Task<T> Run<T>(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse<T>>> method, IsolationLevel isolation,
            Func<Exception, T, T> exceptionMethod, T defaultValue = default)
        {
            DatabaseInformation info = null;
            if (logger != null)
                info = logger.DatabaseEntry(cnnStr, "Func Transaction");

            await using var cnn = new TC { ConnectionString = $"{cnnStr};MultipleActiveResultSets=True" };
            if (cnn.State != ConnectionState.Open)
                cnn.Open();
            await using var transaction = await cnn.BeginTransactionAsync(isolation);

            var result = await SafeTry.OnException(
                async () =>
                {
                    var response = await method(transaction);
                    if (response.Success)
                        await transaction.CommitAsync();
                    else
                        await transaction.RollbackAsync();
                    return response.Response;
                },
                async ex =>
                {
                    await transaction.RollbackAsync();
                    return exceptionMethod(ex, defaultValue);
                });

            if (logger != null)
                logger.DatabaseExit(info);
            return result;
        }
        #endregion

        #region Distributed (Multiple) Database Transaction <action>
        /// <summary>
        /// Wrapper for multiple Sql calls across multiple databases within a transaction
        /// </summary>
        /// <remarks>This will use individual connections using a distributed transactions (performance hit)</remarks>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="exceptionMethod">Default = ExceptionRethrow. If an exception is thrown during "method", how will it be handled (besides being rolled back)</param>
        /// <returns>True if the transaction was committed, false if rolled back</returns>
        public static async Task<bool> Distributed(ILogger logger, Func<Task<TransactionResponse>> method,
            Func<Exception, bool> exceptionMethod = null)
        {
            DatabaseInformation info = null;
            if (logger != null)
                info = logger.DatabaseEntry("Database Transaction", "Action Distributed Transaction");

            using var scope = new TransactionScope();
            var success = await SafeTry.OnException(
                async () =>
                {
                    var response = await method();
                    if (response.Success)
                        scope.Complete();
                    return response.Success;
                },
                exceptionMethod);

            if (logger != null)
                logger.DatabaseExit(info);
            return success;
        }
        #endregion

        #region Single Database Transaction <action>
        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="exceptionMethod">Default = ExceptionRethrow. If an exception is thrown during "method", how will it be handled (besides being rolled back)</param>
        /// <returns>True if the transaction was committed, false if rolled back</returns>
        public static Task<bool> Run(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse>> method, Func<Exception, bool> exceptionMethod = null)
            => Run(logger, cnnStr, method, IsolationLevel.ReadCommitted, exceptionMethod ?? ExceptionRethrow);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="isolation">Default = ReadCommitted. Isolation level for the transaction.</param>
        /// <returns>True if the transaction was committed, false if rolled back</returns>
        public static Task<bool> Run(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse>> method, IsolationLevel isolation)
            => Run(logger, cnnStr, method, isolation, ExceptionRethrow);

        /// <summary>
        /// Wrapper for multiple Sql calls (Single database) within a transaction
        /// </summary>
        /// <param name="logger">The logger for the transaction sequence</param>
        /// <param name="cnnStr">The connection string to use for all calls within the transaction</param>
        /// <param name="method">The actual SQL calls</param>
        /// <param name="isolation">Default = ReadCommitted. Isolation level for the transaction.</param>
        /// <param name="exceptionMethod">Default = ExceptionRethrow. If an exception is thrown during "method", how will it be handled (besides being rolled back)</param>
        /// <returns>True if the transaction was committed, false if rolled back</returns>
        public static async Task<bool> Run(ILogger logger, string cnnStr,
            Func<IDbTransaction, Task<TransactionResponse>> method, IsolationLevel isolation,
            Func<Exception, bool> exceptionMethod)
        {
            DatabaseInformation info = null;
            if (logger != null)
                info = logger.DatabaseEntry(cnnStr, "Action Transaction");

            await using var cnn = new TC { ConnectionString = $"{cnnStr};MultipleActiveResultSets=True" };
            if (cnn.State != ConnectionState.Open)
                await cnn.OpenAsync();
            await using var transaction = await cnn.BeginTransactionAsync(isolation);

            var success = await SafeTry.OnException(
                async () =>
                {
                    var response = await method(transaction);
                    if (response.Success)
                        await transaction.CommitAsync();
                    else
                        await transaction.RollbackAsync();
                    return response.Success;
                },
                async ex =>
                {
                    await transaction.RollbackAsync();
                    return exceptionMethod(ex);
                });

            if (logger != null)
                logger.DatabaseExit(info);
            return success;
        }
        #endregion

        #region Exception Methods
        /// <summary>
        /// Use this method as parameter for Sql().exceptionMethod
        /// </summary>
        /// <param name="ex">Exception that was thrown</param>
        /// <returns>Nothing - will rethrow</returns>
        public static bool ExceptionRethrow(Exception ex) => throw ex;
        /// <summary>
        /// Use this method as parameter for Sql().exceptionMethod
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="ex">Exception that was thrown</param>
        /// <param name="defaultValue">The default return value</param>
        /// <returns>defaultValue</returns>
        public static T ExceptionDefaultVal<T>(Exception ex, T defaultValue) => defaultValue;
        /// <summary>
        /// Use this method as parameter for Sql().exceptionMethod
        /// </summary>
        /// <typeparam name="T">Return type (unused)</typeparam>
        /// <param name="ex">Exception that was thrown</param>
        /// <param name="defaultValue">The default return value (unused)</param>
        /// <returns>Nothing - will rethrow</returns>
        public static T ExceptionRethrow<T>(Exception ex, T defaultValue) => throw ex;
        #endregion
    }
}
