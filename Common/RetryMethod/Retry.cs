using System;
using System.Threading.Tasks;
using Sphyrnidae.Common.RetryMethod.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.RetryMethod
{
    /// <summary>
    /// Retry logic for execution of a method
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// Performs the method until success or retry limit reached
        /// </summary>
        /// <typeparam name="T">The type of response</typeparam>
        /// <param name="method">The method to execute in the retry loop</param>
        /// <param name="options">Retry options to customize the retry logic (Optional, default = new RetryOptions())</param>
        /// <returns>T, or default if not succeeded (or throws if so configured)</returns>
        public static async Task<T> Do<T>(Func<Task<T>> method, RetryOptions options = null)
        {
            var run = new RetryRun(options);
            while (true)
            {
                try
                {
                    return await method();
                }
                catch (Exception ex)
                {
                    if (!await run.RetryOnException(ex))
                        return default;
                }
            }
        }

        /// <summary>
        /// Performs the method until success or retry limit reached
        /// </summary>
        /// <typeparam name="T">The type of response</typeparam>
        /// <param name="method">The method to execute in the retry loop</param>
        /// <param name="options">Retry options to customize the retry logic (Optional, default = new RetryOptions())</param>
        /// <returns>T, or default if not succeeded (or throws if so configured)</returns>
        public static async Task<T> Do<T>(Func<T> method, RetryOptions options = null)
        {
            var run = new RetryRun(options);
            while (true)
            {
                try
                {
                    return method();
                }
                catch (Exception ex)
                {
                    if (!await run.RetryOnException(ex))
                        return default;
                }
            }
        }

        /// <summary>
        /// Performs the method until success or retry limit reached
        /// </summary>
        /// <param name="method">The method to execute in the retry loop</param>
        /// <param name="options">Retry options to customize the retry logic (Optional, default = new RetryOptions())</param>
        /// <returns>true if succeeded, or false/throw exception (based on configuration)</returns>
        public static async Task<bool> Do(Func<Task> method, RetryOptions options = null)
        {
            var run = new RetryRun(options);
            while (true)
            {
                try
                {
                    await method();
                    return true;
                }
                catch (Exception ex)
                {
                    if (!await run.RetryOnException(ex))
                        return false;
                }
            }
        }

        /// <summary>
        /// Performs the method until success or retry limit reached
        /// </summary>
        /// <param name="method">The method to execute in the retry loop</param>
        /// <param name="options">Retry options to customize the retry logic (Optional, default = new RetryOptions())</param>
        /// <returns>true if succeeded, or false/throw exception (based on configuration)</returns>
        public static async Task<bool> Do(Action method, RetryOptions options = null)
        {
            var run = new RetryRun(options);
            while (true)
            {
                try
                {
                    method();
                    return true;
                }
                catch (Exception ex)
                {
                    if (!await run.RetryOnException(ex))
                        return false;
                }
            }
        }
    }
}
