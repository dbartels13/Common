// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Dal.Models
{
    /// <summary>
    /// All transactions will return this object
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// If the transaction successfully completed with a commit (vs rollback)
        /// </summary>
        internal bool Success { get; set; }

        /// <summary>
        /// Commits the transaction
        /// </summary>
        /// <returns>The response object</returns>
        public static TransactionResponse Commit() => new TransactionResponse { Success = true };
        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        /// <returns>The response object</returns>
        public static TransactionResponse Rollback() => new TransactionResponse { Success = false };

        /// <summary>
        /// Commits the transaction
        /// </summary>
        /// <typeparam name="T">Type of result object</typeparam>
        /// <param name="result">The actual result from inside the transaction</param>
        /// <returns>The response object</returns>
        public static TransactionResponse<T> Commit<T>(T result) => new TransactionResponse<T> { Success = true, Result = result };
        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        /// <typeparam name="T">Type of result object</typeparam>
        /// <param name="result">The actual result from inside the transaction</param>
        /// <returns>The response object</returns>
        public static TransactionResponse<T> Rollback<T>(T result) => new TransactionResponse<T> { Success = false, Result = result };
    }

    /// <summary>
    /// All transactions will return this object
    /// </summary>
    public class TransactionResponse<T> : TransactionResponse
    {
        /// <summary>
        /// The actual result from inside the transaction
        /// </summary>
        internal T Result { get; set; }
    }
}