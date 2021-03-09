using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.EmailUtilities;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Utilities
{
    /// <summary>
    /// Allows exceptions to be handled within a contained method
    /// </summary>
    public class SafeTry
    {
        #region OnException
        #region Func
        /// <summary>
        /// Wrapper which executes an asynchronous method safely, and allows the user to customize the exception handling asynchronously
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The asynchronous method to execute if the main method throws an exception</param>
        /// <returns>The return from either the main method or the exception handler (awaitable)</returns>
        public static async Task<T> OnException<T>(Func<Task<T>> method, Func<Exception, Task<T>> error)
        {
            try
            {
                return await method();
            }
            catch (Exception ex)
            {
                return await error(ex);
            }
        }

        /// <summary>
        /// Wrapper which executes an asynchronous method safely, and allows the user to customize the exception handling
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The method to execute if the main method throws an exception</param>
        /// <returns>The return from either the main method or the exception handler (awaitable)</returns>
        public static async Task<T> OnException<T>(Func<Task<T>> method, Func<Exception, T> error)
        {
            try
            {
                return await method();
            }
            catch (Exception ex)
            {
                return error(ex);
            }
        }

        /// <summary>
        /// Wrapper which executes a method safely, and allows the user to customize the exception handling asynchronously
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="method">The method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The asynchronous method to execute if the main method throws an exception</param>
        /// <returns>The return from either the main method or the exception handler (awaitable)</returns>
        public static async Task<T> OnException<T>(Func<T> method, Func<Exception, Task<T>> error)
        {
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                return await error(ex);
            }
        }

        /// <summary>
        /// Wrapper which executes a method safely, and allows the user to customize the exception handling
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="method">The method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The method to execute if the main method throws an exception</param>
        /// <returns>The return from either the main method or the exception handler</returns>
        public static T OnException<T>(Func<T> method, Func<Exception, T> error)
        {
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                return error(ex);
            }
        }
        #endregion

        #region Action
        /// <summary>
        /// Wrapper which executes an asynchronous method safely, and allows the user to customize the exception handling asynchronously
        /// </summary>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The asynchronous method to execute if the main method throws an exception</param>
        /// <returns>True if the main method succeeded, or True/False based on the exception method (awaitable)</returns>
        public static async Task<bool> OnException(Func<Task> method, Func<Exception, Task<bool>> error)
        {
            try
            {
                await method();
                return true;
            }
            catch (Exception ex)
            {
                return await error(ex);
            }
        }

        /// <summary>
        /// Wrapper which executes an asynchronous method safely, and allows the user to customize the exception handling
        /// </summary>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The method to execute if the main method throws an exception</param>
        /// <returns>True if the main method succeeded, or True/False based on the exception method (awaitable)</returns>
        public static async Task<bool> OnException(Func<Task> method, Func<Exception, bool> error)
        {
            try
            {
                await method();
                return true;
            }
            catch (Exception ex)
            {
                return error(ex);
            }
        }

        /// <summary>
        /// Wrapper which executes a method safely, and allows the user to customize the exception handling asynchronously
        /// </summary>
        /// <param name="method">The method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The asynchronous method to execute if the main method throws an exception</param>
        /// <returns>True if the main method succeeded, or True/False based on the exception method (awaitable)</returns>
        public static async Task<bool> OnException(Action method, Func<Exception, Task<bool>> error)
        {
            try
            {
                method();
                return true;
            }
            catch (Exception ex)
            {
                return await error(ex);
            }
        }

        /// <summary>
        /// Wrapper which executes a method safely, and allows the user to customize the exception handling
        /// </summary>
        /// <param name="method">The method to execute which will have it's exceptions sent to error method</param>
        /// <param name="error">The method to execute if the main method throws an exception</param>
        /// <returns>True if the main method succeeded, or True/False based on the exception method</returns>
        public static bool OnException(Action method, Func<Exception, bool> error)
        {
            try
            {
                method();
                return true;
            }
            catch (Exception ex)
            {
                return error(ex);
            }
        }
        #endregion
        #endregion

        #region Ignore Exception

        /// <summary>
        /// Ensures that any exceptions thrown by the containing asynchronous method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions ignored</param>
        /// <param name="defaultValue">
        /// If an exception is thrown, this will be returned instead.
        /// Default = default(t). Eg. null for most classes
        /// </param>
        /// <returns>The return value from the calling method (or defaultValue)</returns>
        public static async Task<T> IgnoreException<T>(Func<Task<T>> method, T defaultValue = default)
            => await OnException(method, ex => IgnoreExceptionError(ex, defaultValue));

        /// <summary>
        /// Ensures that any exceptions thrown by the containing method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="method">The method to execute which will have it's exceptions ignored</param>
        /// <param name="defaultValue">
        /// If an exception is thrown, this will be returned instead.
        /// Default = default(t). Eg. null for most classes
        /// </param>
        /// <returns>The return value from the calling method (or defaultValue)</returns>
        public static T IgnoreException<T>(Func<T> method, T defaultValue = default)
            => OnException(method, ex => IgnoreExceptionError(ex, defaultValue));

        /// <summary>
        /// Ensures that any exceptions thrown by the containing asynchronous method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions ignored</param>
        /// <returns>False if an exception was thrown, True if everything went off without exception</returns>
        public static async Task<bool> IgnoreException(Func<Task> method)
            => await OnException(method, ex => IgnoreExceptionError(ex, false));

        /// <summary>
        /// Ensures that any exceptions thrown by the containing method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <param name="method">The method to execute which will have it's exceptions ignored</param>
        /// <returns>False if an exception was thrown, True if everything went off without exception</returns>
        public static bool IgnoreException(Action method)
            => OnException(method, ex => IgnoreExceptionError(ex, false));

        private static T IgnoreExceptionError<T>(Exception ex, T returnValue)
        {
            Debug.WriteLine("Exception occurred and will be ignored");
            Debug.WriteLine(ex.GetFullMessage());
            Debug.WriteLine(ex.StackTrace);
            return returnValue;
        }

        #endregion

        #region EmailException
        /// <summary>
        /// Ensures that any exceptions thrown by the containing asynchronous method will be e-mailed and code will resume past this block
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="email">The implementation of the IEmail interface</param>
        /// <param name="app">The implementation of the IApplicationSettings interface</param>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions emailed</param>
        /// <param name="defaultValue">
        /// If an exception is thrown, this will be returned instead.
        /// Default = default(t). Eg. null for most classes
        /// </param>
        /// <returns>The return value from the calling method (or defaultValue)</returns>
        public static async Task<T> EmailException<T>(IEmail email, IApplicationSettings app, Func<Task<T>> method, T defaultValue = default)
            => await OnException(method, async ex => await EmailExceptionError(email, app, ex, defaultValue));

        /// <summary>
        /// Ensures that any exceptions thrown by the containing method will be e-mailed and code will resume past this block
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="email">The implementation of the IEmail interface</param>
        /// <param name="app">The implementation of the IApplicationSettings interface</param>
        /// <param name="method">The method to execute which will have it's exceptions emailed</param>
        /// <param name="defaultValue">
        /// If an exception is thrown, this will be returned instead.
        /// Default = default(t). Eg. null for most classes
        /// </param>
        /// <returns>The return value from the calling method (or defaultValue)</returns>
        public static async Task<T> EmailException<T>(IEmail email, IApplicationSettings app, Func<T> method, T defaultValue = default)
            => await OnException(method, async ex => await EmailExceptionError(email, app, ex, defaultValue));

        /// <summary>
        /// Ensures that any exceptions thrown by the containing asynchronous method will be e-mailed and code will resume past this block
        /// </summary>
        /// <param name="email">The implementation of the IEmail interface</param>
        /// <param name="app">The implementation of the IApplicationSettings interface</param>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions emailed</param>
        /// <returns>False if an exception was thrown, True if everything went off without exception</returns>
        public static async Task<bool> EmailException(IEmail email, IApplicationSettings app, Func<Task> method)
            => await OnException(method, async ex => await EmailExceptionError(email, app, ex, false));

        /// <summary>
        /// Ensures that any exceptions thrown by the containing method will be e-mailed and code will resume past this block
        /// </summary>
        /// <param name="email">The implementation of the IEmail interface</param>
        /// <param name="app">The implementation of the IApplicationSettings interface</param>
        /// <param name="method">The method to execute which will have it's exceptions emailed</param>
        /// <returns>False if an exception was thrown, True if everything went off without exception</returns>
        public static async Task<bool> EmailException(IEmail email, IApplicationSettings app, Action method)
            => await OnException(method, async ex => await EmailExceptionError(email, app, ex, false));

        private static async Task<T> EmailExceptionError<T>(IEmail email, IApplicationSettings app, Exception ex, T returnValue)
        {
            var subject = IgnoreException(() => EmailSubject(app), "Unknown Hidden Exception");
            var exceptionMessage = IgnoreException(ex.GetFullMessage, "Can't get full exception message");
            var message = IgnoreException(() => EmailContent(ex, exceptionMessage), exceptionMessage);

            await IgnoreException(async () => await Email.SendAsync(email, EmailType.HiddenException, subject, message));
            return returnValue;
        }

        private static string EmailSubject(IApplicationSettings app)
        {
            var machine = System.Environment.MachineName;
            var envName = app.Environment;
            return $"{machine} ({envName}): {app.Name}; Hidden Exception Occurred";
        }

        private static string EmailContent(Exception ex, string exceptionMessage)
            => $@"<b>Source:</b>&nbsp;{ex.Source ?? "unknown"}

<b>Message:</b>
&nbsp;&nbsp;{exceptionMessage}

<b>Stack Trace:</b>
&nbsp;&nbsp;{ex.GetStackTrace()}
";
        #endregion

        #region Log
        /// <summary>
        /// Ensures that any exceptions thrown by the containing asynchronous method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <remarks>If an exception occurs, it will be logged out as a hidden exception</remarks>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="logger">The ILogger implementation</param>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions ignored</param>
        /// <param name="defaultValue">
        /// If an exception is thrown, this will be returned instead.
        /// Default = default(t). Eg. null for most classes
        /// </param>
        /// <returns>The return value from the calling method (or defaultValue)</returns>
        public static async Task<T> LogException<T>(ILogger logger, Func<Task<T>> method, T defaultValue = default)
            => await OnException(
                method,
                async ex =>
                {
                    await logger.HiddenException(ex);
                    return defaultValue;
                });

        /// <summary>
        /// Ensures that any exceptions thrown by the containing method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <remarks>If an exception occurs, it will be logged out as a hidden exception</remarks>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="logger">The ILogger implementation</param>
        /// <param name="method">The method to execute which will have it's exceptions ignored</param>
        /// <param name="defaultValue">
        /// If an exception is thrown, this will be returned instead.
        /// Default = default(t). Eg. null for most classes
        /// </param>
        /// <returns>The return value from the calling method (or defaultValue)</returns>
        public static async Task<T> LogException<T>(ILogger logger, Func<T> method, T defaultValue = default)
            => await OnException(
                method,
                async ex =>
                {
                    await logger.HiddenException(ex);
                    return defaultValue;
                });

        /// <summary>
        /// Ensures that any exceptions thrown by the containing asynchronous method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <remarks>If an exception occurs, it will be logged out as a hidden exception</remarks>
        /// <param name="logger">The implementation of the ILogger interface</param>
        /// <param name="method">The asynchronous method to execute which will have it's exceptions ignored</param>
        /// <returns>False if an exception was thrown, True if everything went off without exception</returns>
        public static async Task<bool> LogException(ILogger logger, Func<Task> method)
            => await OnException(
                method,
                async ex =>
                {
                    await logger.HiddenException(ex);
                    return false;
                });

        /// <summary>
        /// Ensures that any exceptions thrown by the containing method will be handled gracefully and code will resume past this block
        /// </summary>
        /// <remarks>If an exception occurs, it will be logged out as a hidden exception</remarks>
        /// <param name="logger">The implementation of the ILogger interface</param>
        /// <param name="method">The method to execute which will have it's exceptions ignored</param>
        /// <returns>False if an exception was thrown, True if everything went off without exception</returns>
        public static async Task<bool> LogException(ILogger logger, Action method)
            => await OnException(
                method,
                async ex =>
                {
                    await logger.HiddenException(ex);
                    return false;
                });
        #endregion
    }
}
