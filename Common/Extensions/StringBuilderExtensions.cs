using System.Text;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// StringBuilder custom methods
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Conditionally appends content to the StringBuilder instance
        /// </summary>
        /// <typeparam name="T">The type of object being added</typeparam>
        /// <param name="sb">The StringBuilder</param>
        /// <param name="o">The object being added</param>
        /// <param name="condition">The result of the condition (if null or false, this will not append the object)</param>
        /// <returns>True if the object was appended, false otherwise</returns>
        public static bool AppendIf<T>(this StringBuilder sb, T o, bool? condition = null)
        {
            // If condition is false
            if (condition != null)
            {
                if (!condition.Value)
                    return false;
            }

            // Or if condition nos supplied and this is a default object (null)
            else if (o.IsDefault())
                return false;

            // Valid, append
            sb.Append(o);
            return true;
        }

        /// <summary>
        /// Conditionally appends content and a newline to the StringBuilder instance
        /// </summary>
        /// <typeparam name="T">The type of object being added</typeparam>
        /// <param name="sb">The StringBuilder</param>
        /// <param name="o">The object being added</param>
        /// <param name="condition">The result of the condition (if null or false, this will not append the object)</param>
        public static void AppendLineIf<T>(this StringBuilder sb, T o, bool? condition = null)
        {
            if (sb.AppendIf(o, condition))
                sb.AppendLine();
        }
    }
}