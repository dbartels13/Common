using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Api.Attributes;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpData;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.RequestData;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Middleware
{
    /// <summary>
    /// Middleware which captures Request Data (HttpRequest)
    /// </summary>
    public class HttpDataMiddleware
    {
        private RequestDelegate Next { get; }

        public HttpDataMiddleware(RequestDelegate next) => Next = next;

        public async Task Invoke(
            HttpContext context,
            IHttpData httpData,
            IRequestData request,
            ILoggerConfiguration config,
            IEnvironmentSettings env,
            ILogger logger)
        {
            // No logging in this one
            /*
            var syncIOFeature = context.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
                syncIOFeature.AllowSynchronousIO = true;
             */

            httpData.Request = context.Request;
            httpData.Context = context;
            config.Enabled = request.GetEndpointObject<SkipLogAttribute>().IsDefault();

            var requireHttps = SettingsEnvironmental.Get(env, "Require_Https", "false").ToBool(false);
            if (requireHttps && !context.Request.IsHttps)
            {
                await logger.Log(TraceEventType.Warning, "Non-Https request received", "HTTPS");
                await context.Response.WriteResponseAsync(ApiResponse.HttpsRequired(), SerializationSettings.Default);
                return;
            }

            await Next(context);
        }
    }
}