using System;

namespace Sphyrnidae.Common.HttpClient
{
    /// <summary>
    /// Interface definition for http client settings
    /// </summary>
    /// <remarks>
    /// You will need to implement this interface and have it dependency injected in order to use anything that consumes this interface.
    /// Eg. RequestManager class uses these settings to read from the http request and write to the http response
    /// </remarks>
    public interface IHttpClientSettings
    {
        /// <summary>
        /// The type of data/content being sent (eg. "application/json")
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The name of the http header which contains the authentication token (eg. "Authorization")
        /// </summary>
        string JwtHeader { get; }
        /// <summary>
        /// Retrieves the jwt from the http request
        /// </summary>
        string Jwt { get; }

        /// <summary>
        /// The name of the http header which contains the correlation/request id (eg. "V-Correlation-Id" or "X-Request-ID")
        /// </summary>
        string RequestIdHeader { get; }
        /// <summary>
        /// Retrieves the originating id of the request from the http request
        /// </summary>
        string RequestId { get; }

        /// <summary>
        /// The name of the http header which contains the same ID for all requests per session (eg. X-Tracking-ID)
        /// </summary>
        string SessionIdHeader { get; }
        /// <summary>
        /// Retrieves the unique tracking ID of the session for all requests
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// The name of the http header which contains the originating IP address (eg. X-Forwarded-For)
        /// </summary>
        string IpAddressHeader { get; }
        /// <summary>
        /// Retrieves the originating IP Address of the request
        /// </summary>
        string IpAddress { get; }

        /// <summary>
        /// The name of the http header which contains the ordering for logging (eg. X-Logging-Order)
        /// </summary>
        string LogOrderHeader { get; }
        /// <summary>
        /// Retrieves the ordering prefix for logging of the request
        /// </summary>
        string LogOrder { get; }

        /// <summary>
        /// The string comparison method for determining if a JWT starts with "bearer" or environment is localhost
        /// </summary>
        /// <remarks>Recommend using IgnoreCase (eg. CurrentCultureIgnoreCase)</remarks>
        StringComparison BearerAndLocalhostComparison { get; }
    }
}
