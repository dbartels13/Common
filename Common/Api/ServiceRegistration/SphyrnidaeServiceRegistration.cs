using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sphyrnidae.Common.Alerts;
using Sphyrnidae.Common.Api.Responses;
using Sphyrnidae.Common.Api.ServiceRegistration.Models;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Authentication.Identity;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Cache.Models;
using Sphyrnidae.Common.EmailUtilities;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Encryption.Algorithms;
using Sphyrnidae.Common.Encryption.KeyManager;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.FeatureToggle;
using Sphyrnidae.Common.FeatureToggle.Interfaces;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.HttpData;
using Sphyrnidae.Common.Logging;
using Sphyrnidae.Common.Logging.Configuration;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Logging.Loggers;
using Sphyrnidae.Common.RequestData;
using Sphyrnidae.Common.SignalR;
using Sphyrnidae.Common.UserPreference;
using Sphyrnidae.Common.UserPreference.Interfaces;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.Variable.Interfaces;
using Sphyrnidae.Common.WebServices.ApiAuthentication;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    /// <summary>
    /// Extension methods for use in Startup.cs for dealing with all the services
    /// </summary>
    public static class SphyrnidaeServiceRegistration
    {
        /// <summary>
        /// Adds in all the services that an application needs 
        /// </summary>
        /// <param name="services">The existing services collection</param>
        /// <param name="config">The configuration object for services</param>
        /// <param name="app">The implementation of IApplicationSettings (It can't be resolved here, so must be passed in to match)</param>
        /// <param name="env">The implementation of IEnvironmentSettings (It can't be resolved here, so must be passed in to match)</param>
        /// <param name="http">The implementation of IHttpClientSettings (It can't be resolved here, so must be passed in to match)</param>
        public static IMvcBuilder AddCommonServices(
            this IServiceCollection services,
            ServiceConfiguration config,
            IApplicationSettings app,
            IEnvironmentSettings env,
            IHttpClientSettings http)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Cors
            if (!string.IsNullOrWhiteSpace(config.CorsPolicyName))
                services.AddCors(config.Cors);

            // Swagger
            if (config.SwaggerEnabled)
            {
                services.AddSwaggerGen(c =>
                {
                    if (!string.IsNullOrWhiteSpace(config.SwaggerVersion) && config.SwaggerInfo != null)
                        c.SwaggerDoc(config.SwaggerVersion, config.SwaggerInfo(app));

                    if (config.SwaggerXmlCommentsLocation != null)
                        c.IncludeXmlComments(config.SwaggerXmlCommentsLocation(app));

                    // ReSharper disable once InvertIf
                    if (!string.IsNullOrWhiteSpace(config.SwaggerSecurityPolicyName)
                        && config.SwaggerSecurityDefinition != null
                        && config.SwaggerSecurityRequirement != null)
                    {
                        c.AddSecurityDefinition(config.SwaggerSecurityPolicyName, config.SwaggerSecurityDefinition(http));
                        c.AddSecurityRequirement(config.SwaggerSecurityRequirement());
                    }
                });
            }

            // Health Check
            if (!string.IsNullOrWhiteSpace(config.HealthCheckEndpoint))
                services.AddHealthChecks();

            // DI Mapping Registrations
            services.RegisterCommonServices(config, env); // How to make this abstract/override???

            // Web Services
            if (config.WebServicesEnabled)
                services.AddHttpClient();

            // API
            if (!config.ApiStandardControllerConfiguration)
                return null;

            // Controllers
            var builder = services.AddControllers();
            if (config.ApiControllerConfiguration != null)
                builder.ConfigureApiBehaviorOptions(config.ApiControllerConfiguration);
            if (config.ApiEnableNewtonsoftJson)
            {
                if (config.ApiNewtonsoftConfiguration != null)
                    builder.AddNewtonsoftJson(config.ApiNewtonsoftConfiguration);
                else
                    builder.AddNewtonsoftJson(); // Unsure if we can send null here
            }
            if (config.ApiEnableJsonNet && config.ApiJsonNetConfiguration != null)
                builder.AddJsonOptions(config.ApiJsonNetConfiguration);

            return builder;
        }

        private static void RegisterCommonServices(this IServiceCollection services, ServiceConfiguration config, IEnvironmentSettings env)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Request items
            services.TryAddScoped<IHttpData, HttpData.HttpData>();
            services.TryAddScoped<IRequestData, RequestData.RequestData>();

            // Authorization
            services.TryAddScoped<IIdentityHelper, IdentityHelper<BasicIdentity>>();

            // Caching
            services.AddMemoryCache();
            services.TryAddTransient<DefaultCacheOptions, DefaultCacheOptions>();
            var url = config.CacheRedisUrl(env);
            if (!string.IsNullOrWhiteSpace(url))
            {
                services.AddStackExchangeRedisCache(options => { options.Configuration = url; });
                services.TryAddTransient<ICache, CacheLocalAndDistributed>();
            }
            else
                services.TryAddTransient<ICache, CacheLocal>();

            // Email
            services.TryAddTransient<IEmail, EmailMock>();

            // Encryption
            services.TryAddTransient<IEncryption, EncryptionDispatcher>();
            services.TryAddTransient<IEncryptionAlgorithms, SphyrnidaeEncryptionAlgorithms>();
            services.TryAddTransient<IEncryptionKeyManager, EncryptionKeyManager>();

            // Feature Toggle
            services.TryAddTransient<IFeatureToggleSettings, FeatureToggleSettingsDefault>();
            services.TryAddTransient<IFeatureToggleServices, FeatureToggleServices>();

            // Logging
            services.TryAddTransient<ILogger, Logger>();
            services.TryAddScoped<ILoggerConfiguration, MockLoggerConfiguration>();
            services.TryAddTransient<ILoggerInformation, LoggerInformation>();
            services.TryAddTransient<ILoggers, LoggersNone>();
            services.TryAddTransient<IAlert, Alert>();
            services.TryAddTransient<ApiInformation, ApiInformation>();
            services.TryAddTransient<AttributeInformation, AttributeInformation>();
            //services.TryAddTransient<CustomInformation1, YOUR_CUSTOM_CLASS>(); // Will need to specify implementation if you wish to use
            //services.TryAddTransient<CustomInformation2, YOUR_CUSTOM_CLASS>(); // Will need to specify implementation if you wish to use
            //services.TryAddTransient<CustomInformation3, YOUR_CUSTOM_CLASS>(); // Will need to specify implementation if you wish to use
            //services.TryAddTransient<CustomTimerInformation1, YOUR_CUSTOM_CLASS>(); // Will need to specify implementation if you wish to use
            //services.TryAddTransient<CustomTimerInformation2, YOUR_CUSTOM_CLASS>(); // Will need to specify implementation if you wish to use
            //services.TryAddTransient<CustomTimerInformation3, YOUR_CUSTOM_CLASS>(); // Will need to specify implementation if you wish to use
            services.TryAddTransient<DatabaseInformation, DatabaseInformation>();
            services.TryAddTransient<ExceptionInformation, ExceptionInformation>();
            services.TryAddTransient<HttpResponseInformation, HttpResponseInformation>();
            services.TryAddTransient<LongRunningInformation, LongRunningInformation>();
            services.TryAddTransient<MessageInformation, MessageInformation>();
            services.TryAddTransient<MiddlewareInformation, MiddlewareInformation>();
            services.TryAddTransient<TimerInformation, TimerInformation>();
            services.TryAddTransient<UnauthorizedInformation, UnauthorizedInformation>();
            services.TryAddTransient<WebServiceInformation, WebServiceInformation>();

            // User Preferences
            services.TryAddTransient<IUserPreferenceSettings, UserPreferenceSettingsDefault>();
            services.TryAddTransient<IUserPreferenceServices, UserPreferenceServices>();

            // Variables
            services.TryAddTransient<IVariableSettings, VariableSettingsDefault>();
            services.TryAddTransient<IVariableServices, VariableServices>();

            // Web Services
            services.TryAddTransient<IApiAuthenticationWebService, ApiAuthenticationWebServiceMock>();

            // Transient Helpers
            //services.TryAddTransient<IApplicationSettings, YOUR_APP_SETTINGS_CLASS>(); // Caller must do this
            services.TryAddTransient<IEnvironmentSettings, EnvironmentalSettings>();
            services.TryAddTransient<IHttpClientSettings, HttpClientSettings>();
            services.TryAddTransient<ISignalR, SignalR.SignalR>();
            services.TryAddTransient<IApiResponse, ApiResponseStandard>();
        }

        /// <summary>
        /// Configures the application: Pipeline, Loggers, Encryption Implementations
        /// </summary>
        /// <param name="app">The primary application builder</param>
        /// <param name="config">The configuration object for services</param>
        /// <param name="sp">The service provider that will allow service location calls internal to this (as opposed to DI everything)</param>
        /// <returns></returns>
        public static IApplicationBuilder UseServices(this IApplicationBuilder app, ServiceConfiguration config, IServiceProvider sp)
        {
            config.ApiPipeline.ForEach(item => PipelineHelper.AddMiddlewareToPipeline(app, item, sp, config));

            // Configuration - SignalR clear cache automation - goes last
            config.SignalRLogger?.Invoke(sp);
            config.SignalRCacheInvalidation?.Invoke(sp);

            // All Done
            return app;
        }
    }
}
