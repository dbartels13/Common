using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Api.Models;
using Sphyrnidae.Common.Api.Responses;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Serialize;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Middleware
{
    /// <summary>
    /// Middleware which ensures all exceptions are caught/handled
    /// </summary>
    public class ExceptionMiddleware
    {
        private RequestDelegate Next { get; }

        public ExceptionMiddleware(RequestDelegate next) => Next = next;

        public async Task Invoke(HttpContext context, ILogger logger, IApiResponse apiResponse)
        {
            var info = await logger.MiddlewareEntry("Exception");

            // Save off response body
            var response = context.Response;
            var originalBody = response.Body;
            var newBody = new MemoryStream();
            response.Body = newBody;

            try
            {
                await Next(context);
            }
            catch (UserException ex)
            {
                await ExceptionResponse(
                    response,
                    originalBody,
                    ApiResponse.InternalServerError(ex).ConvertToOther(apiResponse)
                );

                await logger.MiddlewareExit(info);
                return;
            }
            catch (Exception ex)
            {
                var guid = await logger.Exception(ex);
                await ExceptionResponse(
                    response,
                    originalBody,
                    ApiResponse.InternalServerError(guid).ConvertToOther(apiResponse)
                );

                await logger.MiddlewareExit(info);
                return;
            }

            // Replace response body
            var newBodyStr = await newBody.AsStringAsync();
            response.Body = originalBody;
            await response.WriteAsync(newBodyStr);

            await logger.MiddlewareExit(info);
        }

        private static async Task ExceptionResponse(HttpResponse response, Stream originalBody, IApiResponse responseObj)
        {
            response.Body = originalBody;
            await response.WriteResponseAsync(responseObj, SerializationSettings.Default);
        }
    }
}
