using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Sphyrnidae.Common.RequestData
{
    /// <summary>
    /// Stores data that is unique per request (Lifestyle.Scoped = Request)
    /// </summary>
    public interface IRequestData
    {
        /// <summary>
        /// Gets the unique ID for the request
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Order of things being logged
        /// </summary>
        char LoggingOrder { get; set; }

        /// <summary>
        /// Ip Address of the end user/client
        /// </summary>
        string IpAddress { get; set; }

        /// <summary>
        /// IP Address of the machine making this request (may not be the original)
        /// </summary>
        string RemoteIpAddress { get; }

        /// <summary>
        /// The base URL of the request
        /// </summary>
        string DisplayUrl { get; }

        /// <summary>
        /// Route of an API Request
        /// </summary>
        string Route { get; }

        /// <summary>
        /// The raw content data of the request
        /// </summary>
        Task<string> ContentData();

        /// <summary>
        /// The Http Verb for the request
        /// </summary>
        string HttpVerb { get; }

        /// <summary>
        /// Collection of Http Headers
        /// </summary>
        NameValueCollection Headers { get; }

        /// <summary>
        /// Collection of QueryString variables
        /// </summary>
        NameValueCollection QueryString { get; }

        /// <summary>
        /// Collection of Form variables
        /// </summary>
        NameValueCollection FormData { get; }

        /// <summary>
        /// Name/Description of the client browser
        /// </summary>
        string Browser { get; }

        /// <summary>
        /// Retrieves some object about the actual endpoint
        /// </summary>
        /// <typeparam name="T">The type of object being retrieved</typeparam>
        /// <returns>The object (if found on the endpoint), default (null) otherwise</returns>
        T GetEndpointObject<T>();

        /// <summary>
        /// Retrieves an HTTP Header from the request
        /// </summary>
        /// <param name="name">Name of the HTTP header</param>
        /// <returns>The first value of the header (or null if no header)</returns>
        string GetHeader(string name);

        /// <summary>
        /// The collection of HTTP Headers with the given name
        /// </summary>
        /// <param name="name">Name of the HTTP header</param>
        /// <returns>The collection of values</returns>
        StringValues GetHeaders(string name);
    }
}
