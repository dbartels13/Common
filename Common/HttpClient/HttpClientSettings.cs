using System;
using System.Linq;
using System.Net.Mime;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.RequestData;

namespace Sphyrnidae.Common.HttpClient
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of IHttpClientSettings which is set via:
    /// 1) Hard-Coded strings
    /// 2) Http Request Headers
    /// 3) Request Data (also read from headers)
    /// </summary>
    /// <remarks>This will read from the incoming HTTP request, and if the header/field exists there, will use that value</remarks>
    public class HttpClientSettings : IHttpClientSettings
    {
        protected IRequestData RequestData { get; }
        protected IEnvironmentSettings EnvSettings { get; }
        public HttpClientSettings(IRequestData requestData, IEnvironmentSettings envSettings)
        {
            RequestData = requestData;
            EnvSettings = envSettings;
        }

        public virtual string ContentType => MediaTypeNames.Application.Json;

        public virtual string JwtHeader => "Authorization";

        public virtual string Jwt
        {
            get
            {
                // First, try the authorization header
                var token = RequestData.GetHeader(JwtHeader);

                // Querystring access_token is also valid
                if (string.IsNullOrWhiteSpace(token))
                    token = RequestData.QueryString?.GetValues("access_token")?.FirstOrDefault();

                // Ensure you have received something
                if (string.IsNullOrWhiteSpace(token))
                    return token;

                // Optionally strip out "bearer" (with or without space)
                if (token.StartsWith("bearer ", BearerAndLocalhostComparison))
                    token = token.Substring(7);
                if (token.StartsWith("bearer", BearerAndLocalhostComparison))
                    token = token.Substring(6);
                return token;
            }
        }

        public virtual string RequestIdHeader => "V-Correlation-Id"; // X-Request-ID
        public virtual string RequestId
        {
            get
            {
                var requestId = RequestData.GetHeader(RequestIdHeader);
                return !string.IsNullOrWhiteSpace(requestId) ? requestId : RequestData.Id.ToString();
            }
        }

        public virtual string SessionIdHeader => "X-Tracking-Id"; // Completely made up :)
        public virtual string SessionId => RequestData.GetHeader(SessionIdHeader);

        public virtual string IpAddressHeader => "X-Forwarded-For";
        public virtual string IpAddress
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RequestData.IpAddress))
                    return RequestData.IpAddress;

                var ipHeader = RequestData.GetHeader(IpAddressHeader);
                if (!string.IsNullOrWhiteSpace(ipHeader))
                {
                    RequestData.IpAddress = CleanIpAddress(ipHeader);
                    return RequestData.IpAddress;
                }

                var ipRemote = RequestData.RemoteIpAddress;
                if (!string.IsNullOrWhiteSpace(ipRemote))
                {
                    RequestData.IpAddress = CleanIpAddress(ipRemote);
                    return RequestData.IpAddress;
                }

                RequestData.IpAddress = "unknown";
                return RequestData.IpAddress;
            }
        }
        protected string CleanIpAddress(string ipAddress)
        {
            var ip = ipAddress.Replace("\"", "");
            // ReSharper disable once InvertIf
            if (ip.Equals("::1"))
            {
                var env = SettingsEnvironmental.Get(EnvSettings, "ASPNETCORE_ENVIRONMENT");
                if (env.Equals("localhost", BearerAndLocalhostComparison))
                    return "127.0.0.1";
            }
            return ipAddress.KeepBefore(':').KeepBefore(',');
        }

        public virtual string LogOrderHeader => "X-Logging-Order"; // Completely made up :)
        public virtual string LogOrder => RequestData.GetHeader(LogOrderHeader);

        public virtual StringComparison BearerAndLocalhostComparison => StringComparison.CurrentCultureIgnoreCase;
    }
}
