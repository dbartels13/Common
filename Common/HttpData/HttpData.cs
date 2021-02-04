using Microsoft.AspNetCore.Http;

namespace Sphyrnidae.Common.HttpData
{
    /// <summary>
    /// Http Data (Request/Context) - to be set 1-time in the middleware
    /// </summary>
    public class HttpData : IHttpData
    {
        /// <summary>
        /// This could be null if not set by the middleware
        /// </summary>
        public virtual HttpRequest Request { get; set; }

        /// <summary>
        /// This could be null if not set by the middleware
        /// </summary>
        public virtual HttpContext Context { get; set; }
    }
}