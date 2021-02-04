using System;
using System.Net.Mime;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.HttpClient
{
    /// <inheritdoc />
    public class HttpClientSettingsMock : IHttpClientSettings
    {

        public virtual string ContentType => MediaTypeNames.Application.Json;

        public virtual string JwtHeader => "Authorization";
        public virtual string Jwt => "";

        public virtual string RequestIdHeader => "V-Correlation-Id"; // X-Request-ID
        public virtual string RequestId => "";

        public virtual string SessionIdHeader => "X-Tracking-Id"; // Completely made up :)
        public virtual string SessionId => "";

        public virtual string IpAddressHeader => "X-Forwarded-For";
        public virtual string IpAddress => "";
        protected string CleanIpAddress(string ipAddress) => ipAddress;

        public virtual string LogOrderHeader => "X-Logging-Order"; // Completely made up :)
        public virtual string LogOrder => "";

        public virtual StringComparison BearerAndLocalhostComparison => StringComparison.CurrentCultureIgnoreCase;
    }
}