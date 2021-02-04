using Microsoft.AspNetCore.Http;

namespace Sphyrnidae.Common.HttpData
{
    /// <summary>
    /// Stores data that is unique per request (Lifestyle.Scoped = Request)
    /// </summary>
    public interface IHttpData
    {
        /// <summary>
        /// Gets the HttpRequest used in this API call
        /// </summary>
        /// <remarks>This should be "set" early in the middleware, as a "get" without first being "set" should result in an exception</remarks>
        HttpRequest Request { get; set; }

        /// <summary>
        /// Gets the HttpContext used in this API call
        /// </summary>
        /// <remarks>This should be "set" early in the middleware, as a "get" without first being "set" should result in an exception</remarks>
        HttpContext Context { get; set; }
    }
}