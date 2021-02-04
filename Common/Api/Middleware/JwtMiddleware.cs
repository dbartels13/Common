using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Api.DefaultIdentity;
using Sphyrnidae.Common.Authentication;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Middleware
{
    public class JwtMiddleware
    {
        private RequestDelegate Next { get; }

        public JwtMiddleware(RequestDelegate next) => Next = next;

        public async Task Invoke(HttpContext context, ILogger logger, IIdentityWrapper identity, IDefaultIdentity defaultIdentity, IHttpClientSettings http, ITokenSettings token, IEncryption encrypt)
        {
            var info = await logger.MiddlewareEntry("Jwt");

            // We need to ensure a jwt always exists
            if (identity.Current.IsDefault())
                identity.Current = defaultIdentity.Get;

            // Replace the out-going JWT with a new one
            context.Response.OnStarting(() =>
            {
                //var httpContext = (HttpContext) state;
                context.Response.SetHeader(http.JwtHeader, identity.Current.ToJwt(token, encrypt));
                return Task.CompletedTask;
            });

            await Next(context);

            await logger.MiddlewareExit(info);
        }
    }
}