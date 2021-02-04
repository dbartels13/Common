using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.HttpClient;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.ServiceRegistration.Models
{
    /// <summary>
    /// All configurations for service registration
    /// </summary>
    public class ServiceConfiguration
    {
        #region Cors
        /// <summary>
        /// Name of the CORS policy. To not have a policy, leave this blank/null.
        /// </summary>
        /// <remarks>Default = "Cors All"</remarks>
        public string CorsPolicyName
        {
            get => CorsHelper.CorsPolicyName;
            set => CorsHelper.CorsPolicyName = value;
        }

        /// <summary>
        /// The implementation of the Cors policy/options (usually creating a policy and building out allowed origin/header/method/credentials/etc
        /// </summary>
        /// <remarks>Default = Accept all (non-credentialed)</remarks>
        public Action<CorsOptions> Cors { get; set; } = CorsHelper.CorsAll;
        #endregion

        #region Swagger
        /// <summary>
        /// Primary setting for if swagger is enabled or not
        /// </summary>
        /// <remarks>Default = true</remarks>
        public bool SwaggerEnabled { get; set; } = true;

        /// <summary>
        /// Version to display on the swagger document page
        /// </summary>
        /// <remarks>Default = "v1"</remarks>
        public string SwaggerVersion { get; set; } = "v1";

        /// <summary>
        /// Other information/configuration for the swagger document page
        /// </summary>
        /// <remarks>Default = Default setup with title, contact, description (noting http response type)</remarks>
        public Func<IApplicationSettings, OpenApiInfo> SwaggerInfo { get; set; } = SwaggerHelper.ApiInfo;

        /// <summary>
        /// Location of XML comments built by the project
        /// </summary>
        /// <remarks>Default = &lt;BaseDirectory&gt;&lt;ApplicationName&gt;.xml</remarks>
        public Func<IApplicationSettings, string> SwaggerXmlCommentsLocation { get; set; } = SwaggerHelper.XmlComments;

        /// <summary>
        /// Name of the authentication/security policy for swagger
        /// </summary>
        /// <remarks>Default = "Sphyrnidae JWT"</remarks>
        public string SwaggerSecurityPolicyName
        {
            get => SwaggerHelper.SecurityPolicyName;
            set => SwaggerHelper.SecurityPolicyName = value;
        }

        /// <summary>
        /// If you have a SwaggerSecurityPolicyName, this should be the security definitions for that policy 
        /// </summary>
        /// <remarks>Default = Accept JWT</remarks>
        public Func<IHttpClientSettings, OpenApiSecurityScheme> SwaggerSecurityDefinition { get; set; } =
            SwaggerHelper.SecurityScheme;

        /// <summary>
        /// If you have a SwaggerSecurityPolicyName, this should be the security requirement for that policy
        /// </summary>
        /// <remarks>Default = Require JWT</remarks>
        public Func<OpenApiSecurityRequirement> SwaggerSecurityRequirement { get; set; } = SwaggerHelper.SecurityRequirement;

        /// <summary>
        /// Endpoint for the swagger generated json file
        /// </summary>
        /// <remarks>Default = "/swagger/v1/swagger.json"</remarks>
        public string SwaggerEndpoint { get; set; } = "/swagger/v1/swagger.json";
        #endregion

        #region Health Check
        /// <summary>
        /// The endpoint/URL for health checks (must be provided to enable health checks)
        /// </summary>
        /// <remarks>Default = "/hc"</remarks>
        public string HealthCheckEndpoint
        {
            get => HealthCheckHelper.Url;
            set => HealthCheckHelper.Url = value;
        }

        /// <summary>
        /// Options for health checks
        /// </summary>
        /// <remarks>Default = Standard Health Report as JSON</remarks>
        public Func<IEnvironmentSettings, HealthCheckOptions> HealthCheckOptions { get; set; } =
            HealthCheckHelper.Options;
        #endregion

        #region Cache
        /// <summary>
        /// URL for Redis server. If not provided, distributed caching will be disabled
        /// </summary>
        /// <remarks>Default = Reads from environmental variable "URL:Redis"</remarks>
        public Func<IEnvironmentSettings, string> CacheRedisUrl { get; set; } = CacheHelper.RedisUrl;
        #endregion

        #region Web Services
        /// <summary>
        /// Can you make web service calls (IHttpClient enabled)
        /// </summary>
        /// <remarks>Default = true</remarks>
        public bool WebServicesEnabled { get; set; } = true;
        #endregion

        #region API
        /// <summary>
        /// Allows you to completely write your own "AddMVC" or "AddControllers"
        /// </summary>
        /// <remarks>Default = false</remarks>
        public bool ApiStandardControllerConfiguration { get; set; } = true;
        /// <summary>
        /// Any configurations to controllers (eg. changing return type, etc).
        /// </summary>
        /// <remarks>Default = Format ModelState errors as our standard response object</remarks>
        public Action<ApiBehaviorOptions> ApiControllerConfiguration { get; set; } = ApiHelper.ControllerConfiguration;

        /// <summary>
        /// Is newtonsoft enabled for web api de/serialization?
        /// </summary>
        /// <remarks>Default = false. Will use json.net instead</remarks>
        public bool ApiEnableNewtonsoftJson { get; set; } = false;

        /// <summary>
        /// If ApiEnableNewtonsoftJson = true, and using the default ApiNewtonsoftConfiguration, this is the resolver that is registered
        /// </summary>
        /// <remarks>Default = CamelCasePropertyNamesContractResolver</remarks>
        public IContractResolver ApiNewtonsoftContractResolver
        {
            get => ApiHelper.NewtonsoftContractResolver;
            set => ApiHelper.NewtonsoftContractResolver = value;
        }

        /// <summary>
        /// If ApiEnableNewtonsoftJson = true, Any configurations to newtonsoft json (eg. setting the default contract resolver)
        /// </summary>
        /// <remarks>Default = Set Contract Resolver to ApiNewtonsoftContractResolver</remarks>
        public Action<MvcNewtonsoftJsonOptions> ApiNewtonsoftConfiguration { get; set; } = ApiHelper.NewtonsoftConfiguration;

        /// <summary>
        /// Allows for custom enabling of Json.net
        /// </summary>
        /// <remarks>Default = true (only do this if ApiEnableNewtonsoftJson = true, OR ApiJsonNetConfiguration is non-null)</remarks>
        public bool ApiEnableJsonNet { get; set; } = true;

        /// <summary>
        /// If ApiEnableJsonNet = true, Any configurations to json.net
        /// </summary>
        /// <remarks>Default = Sets PropertyNameCaseInsensitive = true</remarks>
        public Action<JsonOptions> ApiJsonNetConfiguration { get; set; } = ApiHelper.JsonNetConfiguration;

        /// <summary>
        /// Collection of enumerated values representing the middleware components and order of the pipeline
        /// </summary>
        /// <remarks>
        /// Default = Standard pipeline
        /// Note: Alter with caution... you should generally only be adding your custom middleware at some point in this collection
        /// </remarks>
        public List<Pipeline> ApiPipeline { get; set; } = ApiHelper.DefaultPipeline;

        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom1
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware1 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom2
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware2 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom3
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware3 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom4
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware4 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom5
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware5 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom6
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware6 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom7
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware7 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom8
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware8 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom9
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware9 { get; set; } = null;
        /// <summary>
        /// If you have custom middleware, you can define it here, and then add it to the pipeline using ApiPipeline with value of Pipeline.Custom10
        /// </summary>
        /// <remarks>Default = not provided</remarks>
        public Action<IApplicationBuilder> ApiCustomMiddleware10 { get; set; } = null;
        #endregion
        
        #region SignalR
        /// <summary>
        /// This method will enable logging of SignalR calls and exceptions
        /// </summary>
        /// <remarks>Default = standard logging of messages and exceptions from SignalR</remarks>
        public Action<IServiceProvider> SignalRLogger { get; set; } = SignalRHelper.Logger;

        /// <summary>
        /// This method will receive signal R messages and activate cache invalidation
        /// </summary>
        /// <remarks>Default = connects to the hub url at environmental variable "URL:Hub:Cache", and will forward request on to IMemoryCache.Remove</remarks>
        public Action<IServiceProvider> SignalRCacheInvalidation { get; set; } = SignalRHelper.CacheInvalidation;
        #endregion
    }
}
