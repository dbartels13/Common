using System.Linq;
using System.Net.Http.Headers;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// HttpHeaders custom methods
    /// </summary>
    public static class HttpHeadersExtensions
    {
        /// <summary>
        /// Obtains an item from the HTTP header
        /// </summary>
        /// <param name="headers">The current http headers</param>
        /// <param name="name">The name of the HTTP header to retrieve</param>
        /// <returns>The value of the HTTP header</returns>
        public static string Get(this HttpHeaders headers, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            string val = null;
            if (headers?.Contains(name) ?? false)
                val = headers.GetValues(name).FirstOrDefault();
            return string.IsNullOrWhiteSpace(val) ? default : val;
        }

        /// <summary>
        /// Ensures the header has the given value
        /// </summary>
        /// <param name="headers">The headers collection</param>
        /// <param name="name">Name of the header</param>
        /// <param name="value">Value to give the header</param>
        /// <remarks>This will insert the header if not exist, or update if already exists</remarks>
        public static void Alter(this HttpHeaders headers, string name, string value)
        {
            headers.SafeRemove(name);
            headers.Add(name, value);
        }

        /// <summary>
        /// Removes a header
        /// </summary>
        /// <param name="headers">The headers collection</param>
        /// <param name="name">Name of the header</param>
        /// <remarks>If the header does not exist, this does nothing</remarks>
        public static void SafeRemove(this HttpHeaders headers, string name)
        {
            if (headers.Contains(name))
                headers.Remove(name);
        }
    }
}