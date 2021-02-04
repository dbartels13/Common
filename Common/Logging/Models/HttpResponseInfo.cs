namespace Sphyrnidae.Common.Logging.Models
{
    /// <summary>
    /// The collected information about the Http Response
    /// </summary>
    public class HttpResponseInfo
    {
        /// <summary>
        /// The type of HttpRequest (usually either API/WebService)
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The unique route which identifies the exact request. Note this could be null/blank
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Post/Get/etc. Note this could be null/blank
        /// </summary>
        public string HttpMethod { get; }

        /// <summary>
        /// The HTTP Response Code (eg. 200, 404, etc). Note this could be null/blank
        /// </summary>
        public int? HttpCode { get; }

        /// <summary>
        /// Saves off information about the Http Response
        /// </summary>
        /// <param name="type">The type of HttpRequest (usually either API/WebService)</param>
        /// <param name="route">The unique route which identifies the exact request. Note this could be null/blank</param>
        /// <param name="httpMethod">Post/Get/etc. Note this could be null/blank</param>
        /// <param name="httpCode">The HTTP Response Code (eg. 200, 404, etc). Note this could be null/blank</param>
        public HttpResponseInfo(string type, string route, string httpMethod, int? httpCode)
        {
            Type = type;
            Route = route;
            HttpMethod = httpMethod;
            HttpCode = httpCode;
        }
    }
}