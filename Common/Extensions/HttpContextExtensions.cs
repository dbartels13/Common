using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Extension methods for HttpContext
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieves the actual IP address from the request
        /// </summary>
        /// <param name="context">The HttpContext</param>
        /// <returns>The actual/full IP Address (could be null)</returns>
        public static string IpAddress(this HttpContext context) => SafeTry.IgnoreException(() => context.Connection.RemoteIpAddress.ToString());

    }
}