using Microsoft.AspNetCore.Http;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.HttpData
{
    /// <inheritdoc />
    public class HttpDataMock : IHttpData
    {
        public HttpRequest Request { get; set; }
        public HttpContext Context { get; set; }
    }
}