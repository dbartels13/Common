using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Extension methods for HttpRequest
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Obtains an item from the HTTP header
        /// </summary>
        /// <param name="request">The current http request</param>
        /// <param name="name">The name of the HTTP header to retrieve</param>
        /// <returns>The value of the HTTP header</returns>
        public static string GetHeader(this HttpRequest request, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var values = request.Headers[name];
            return values == StringValues.Empty ? null : values.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the body of a request as a string
        /// </summary>
        /// <param name="request">The current http request</param>
        /// <returns>The full body of the request</returns>
        //public static string GetBody(this HttpRequest request) => request.BodyReader.AsStream(true).AsString();
        public static Task<string> GetBodyAsync(this HttpRequest request)
        {
            request.EnableBuffering();
            return request.Body.AsStringAsync();
        }
        /*
        {
            var cancellationToken = request.HttpContext.RequestAborted;
            var reader = request.BodyReader;
            while (!cancellationToken.IsCancellationRequested)
            {
                var readResult = GetReadResult(reader, cancellationToken).Result;
                var buffer = readResult.Buffer;
                reader.AdvanceTo(buffer.Start, buffer.End);

                if (readResult.IsCompleted)
                {
                    if (buffer.IsSingleSegment)
                        return Encoding.ASCII.GetString(buffer.FirstSpan);

                    return string.Create((int)buffer.Length, buffer, (span, sequence) =>
                    {
                        foreach (var segment in sequence)
                        {
                            Encoding.ASCII.GetChars(segment.Span, span);
                            span = span.Slice(segment.Length);
                        }
                    });
                }
            }

            return null;
        }
        private static Task<ReadResult> GetReadResult(PipeReader reader, CancellationToken cancellationToken) => reader.ReadAsync(cancellationToken);
        */
    }
}