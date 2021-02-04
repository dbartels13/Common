using System;
using System.Collections.Generic;

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Exception custom methods
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Retrieves the message (and all inner exception messages)
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The fully formatted message of the exception and all inner exceptions</returns>
        public static string GetFullMessage(this Exception ex)
        {
            if (ex == null)
                return "";

            var innerException = ex.InnerException.GetFullMessage();
            return string.IsNullOrWhiteSpace(innerException)
                ? ex.Message
                : $"{ex.Message} - {innerException}";
        }

        /// <summary>
        /// Retrieves the stack trace formatted
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>The fully formatted stack trace</returns>
        public static string GetStackTrace(this Exception ex)
        {
            if (ex == null)
                return "";

            var stackTraceItems = ex.StackTrace.SafeSplit("\r\n") ?? new List<string>();
            // There are 2 spaces in the string literal with the newline - this is intentional for formatting
            return string.Join(@"  
", stackTraceItems);
        }
    }
}