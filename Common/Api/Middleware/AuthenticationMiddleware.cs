using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Api.Attributes;
using Sphyrnidae.Common.Api.Models;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.RequestData;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.Variable.Interfaces;
using Sphyrnidae.Common.WebServices.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Middleware
{
    /// <summary>
    /// Middleware which ensures all exceptions are caught/handled
    /// </summary>
    public class AuthenticationMiddleware
    {
        private RequestDelegate Next { get; }

        public AuthenticationMiddleware(RequestDelegate next) => Next = next;

        public async Task Invoke(
            HttpContext context,
            IRequestData request,
            IIdentityWrapper identity,
            ILogger logger,
            IHttpClientSettings http,
            IEncryption encryption,
            IApplicationSettings app,
            IVariableServices variable,
            ICache cache,
            IApiAuthenticationWebService service)
        {
            var info = await logger.MiddlewareEntry("Authentication");

            var authentication = request.GetEndpointObject<AuthenticationAttribute>();
            var type = authentication?.Type ?? AuthenticationType.Jwt;

            // Jwt authentication
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (type == AuthenticationType.Jwt)
            {
                // Must have Jwt
                if (identity.Current.IsDefault())
                {
                    await NotAuthenticated(context);
                    await Log(
                        logger,
                        () => IdentityHelper.RetrieveIdentityError(http, encryption),
                        info);
                    return;
                }

                // Authorization role check
                var role = authentication?.Role;
                if (!string.IsNullOrWhiteSpace(role) && !identity.Current.SearchableRoles.Has(role))
                {
                    await context.Response.WriteResponseAsync(ApiResponse.NotAuthorized(), SerializationSettings.Default);
                    await Log(
                        logger,
                        () => $"Not authorized for role '{role}'",
                        info);
                    return;
                }
            }

            else if (type == AuthenticationType.ApiToApi)
            {
                var application = request.GetHeader(Constants.ApiToApi.Application);
                if (string.IsNullOrWhiteSpace(application))
                {
                    await NotAuthenticated(context);
                    await Log(
                        logger,
                        () => $"{Constants.ApiToApi.Application} was not provided for Api to Api communication",
                        info);
                    return;
                }

                var token = request.GetHeader(Constants.ApiToApi.Token);
                if (string.IsNullOrWhiteSpace(token))
                {
                    await NotAuthenticated(context);
                    await Log(
                        logger,
                        () => $"{Constants.ApiToApi.Token} was not provided for Api to Api communication",
                        info);
                    return;
                }

                var key = $"ApiAuth_{app.Name}_{application}_{token}";
                cache.Options.Seconds = SettingsVariable.Get(variable, "CachingSeconds_ApiAuth", "30").ToInt(30);
                if (!await Caching.GetAsync(cache, key, async () => await service.IsAuthenticated(application, token)))
                {
                    await NotAuthenticated(context);
                    await Log(
                        logger,
                        () => $"Invalid token from {application} for Api to Api communication",
                        info);
                    return;
                }
            }

            // Authentication Passed
            await Next(context);
            await logger.MiddlewareExit(info);
        }

        private static async Task NotAuthenticated(HttpContext context)
            => await context.Response.WriteResponseAsync(ApiResponse.NotAuthenticated(), SerializationSettings.Default);

        private static async Task Log(ILogger logger, Func<string> reason, MiddlewareInformation info)
        {
            // Some hidden exceptions will occur here since user/etc is not provided and these are always logged... but it is all handled.
            await logger.Unauthorized(reason());
            await logger.MiddlewareExit(info);
        }
    }
}
