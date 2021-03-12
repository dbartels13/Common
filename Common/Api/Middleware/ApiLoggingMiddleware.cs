using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Middleware
{
    /// <summary>
    /// Middleware which logs the API information
    /// </summary>
    public class ApiLoggingMiddleware
    {
        private RequestDelegate Next { get; }

        public ApiLoggingMiddleware(RequestDelegate next) => Next = next;

        public async Task Invoke(HttpContext context, ILoggerConfiguration config, ILogger logger)
        {
            // If not doing any logging
            if (!config.Enabled)
            {
                await Next(context);
                return;
            }

            var middlewareInfo = logger.MiddlewareEntry("ApiLogging");
            var apiInfo = await logger.ApiEntry();

            // Save off response body (must be done so the ApiExit call has access to the HttpResponse object)
            var response = context.Response;
            var originalBody = response.Body;
            var newBody = new MemoryStream();
            response.Body = newBody;

            try
            {
                await Next(context);
                await logger.ApiExit(apiInfo, response);
            }
            finally
            {
                // Replace response body
                var newBodyStr = await newBody.AsStringAsync();
                response.Body = originalBody;
                await response.WriteAsync(newBodyStr);

                logger.MiddlewareExit(middlewareInfo);
            }
        }
    }
}