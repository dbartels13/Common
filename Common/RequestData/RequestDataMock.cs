using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.RequestData
{
    /// <inheritdoc />
    public class RequestDataMock : IRequestData
    {
        public T GetEndpointObject<T>() => default;
        public virtual string GetHeader(string name) => default;
        public virtual StringValues GetHeaders(string name) => default;
        public Guid Id => Guid.NewGuid();
        public char LoggingOrder { get; set; } = (char)33;
        public string IpAddress { get; set; } = "localhost";
        public string RemoteIpAddress => "localhost";
        public string DisplayUrl => "localhost";
        public string Route => string.Empty;
        public Task<string> ContentData() => new Task<string>(() => string.Empty);
        public string HttpVerb => "TEST";
        public NameValueCollection Headers => new NameValueCollection();
        public NameValueCollection QueryString => new NameValueCollection();
        public NameValueCollection FormData => new NameValueCollection();
        public string Browser => "None";
    }
}