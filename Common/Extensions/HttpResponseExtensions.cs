using System.IO.Pipelines;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sphyrnidae.Common.Api.Responses;
using Sphyrnidae.Common.Serialize;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Extension methods for HttpRequest
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Adds a header to the HttpResponse message
        /// </summary>
        /// <param name="response">The current http response</param>
        /// <param name="key">The name of the header</param>
        /// <param name="value">The value of the header</param>
        public static void SetHeader(this HttpResponse response, string key, string value)
            => response.Headers.Add(key, value);

        /// <summary>
        /// Retrieves the body of a request as a string
        /// </summary>
        /// <remarks>Can't read a PipeWriter, so continue to do this one as a stream</remarks>
        /// <param name="response">The current http response</param>
        /// <returns>The full body of the request</returns>
        //public static string GetBody(this HttpResponse response) => response.BodyWriter.AsStream(true).AsString();
        public static async Task<string> GetBodyAsync(this HttpResponse response) => await response.Body.AsStringAsync();

        /// <summary>
        /// Retrieves the body of a response as a string
        /// </summary>
        /// <param name="response">The current http response</param>
        /// <param name="str">The string value of the body (usually object serialized to json)</param>
        public static async Task ReplaceBodyAsync(this HttpResponse response, string str)
        {
            var writer = response.BodyWriter;
            var workspace = writer.GetMemory();
            var bytes = Encoding.ASCII.GetBytes(str, workspace.Span);
            writer.Advance(bytes);
            await Flush(writer);
        }
        private static async Task Flush(PipeWriter writer) => await writer.FlushAsync();

        /// <summary>
        /// Writes out a custom response
        /// </summary>
        /// <param name="response">The current http response</param>
        /// <param name="obj">The object to write out</param>
        /// <param name="json">Json serialization settings</param>
        /// <returns></returns>
        public static async Task WriteResponseAsync(this HttpResponse response, IApiResponse obj, JsonSerializerSettings json)
        {
            response.ContentType = MediaTypeNames.Application.Json;
            var body = obj.ToResponseBody();
            response.StatusCode = obj.Code; // Do this after in case ToResponse() altered the Code
            await response.WriteAsync(body.SerializeJson(json));
        }
    }
}