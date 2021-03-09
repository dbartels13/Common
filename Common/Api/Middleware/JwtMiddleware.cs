using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Middleware
{
    /// <summary>
    /// Replaces the JWT with a refreshed token in the HTTP response
    /// </summary>
    public class JwtMiddleware
    {
        private RequestDelegate Next { get; }

        public JwtMiddleware(RequestDelegate next) => Next = next;

        public async Task Invoke(HttpContext context, ILogger logger, IIdentityHelper identity, IHttpClientSettings http, IEncryption encrypt)
        {
            var info = await logger.MiddlewareEntry("Jwt");

            // We need to ensure a jwt always exists
            if (identity.Current.IsDefault())
                identity.Current = await identity.GetDefaultIdentity();

            // Replace the out-going JWT with a new one
            context.Response.OnStarting(() =>
            {
                //var httpContext = (HttpContext) state;
                context.Response.SetHeader(http.JwtHeader, identity.ToJwt(identity.Current));
                return Task.CompletedTask;
            });

            await Next(context);

            await logger.MiddlewareExit(info);
        }
    }
}