using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sphyrnidae.Common.Api.Middleware;
using Sphyrnidae.Common.Api.ServiceRegistration.Models;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Utilities;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class PipelineHelper
    {
        internal static List<Pipeline> DefaultPipeline =>
            new List<Pipeline>
            {
                Pipeline.DevelopmentHsts, // Should go first
                Pipeline.HttpsRedirect,
                Pipeline.Swagger,
                Pipeline.Routing, // Should go early before anything route related
                Pipeline.Cors,
                Pipeline.HealthCheck, // Do before custom authentication middleware component
                Pipeline.HttpData, // Must go before anything using Http Request utilities (IRequestData, IHttpData)
                Pipeline.Authentication,
                Pipeline.Jwt, // After authentication
                Pipeline.Exceptions, // Do before logging as we don't want to time this, and an exception would be logged double too
                Pipeline.Logging, // Goes right before API calls
                Pipeline.ControllerAction // Goes last
            };

        /// <summary>
        /// Helper method to add a middleware item to the pipeline
        /// </summary>
        /// <remarks>You will also need to add implementation hooks by setting ApiCustomMiddlewareX</remarks>
        /// <param name="pipeline">The existing pipeline</param>
        /// <param name="item">The middleware item to insert</param>
        /// <param name="beforeItem">The location in the pipeline where the 'item' will get inserted before</param>
        /// <returns>The pipeline with the item added</returns>
        public static List<Pipeline> AddToPipeline(this List<Pipeline> pipeline, Pipeline item, Pipeline beforeItem)
            => pipeline.InsertBefore(x => x == beforeItem, item);

        internal static void AddMiddlewareToPipeline(IApplicationBuilder app, Pipeline item, IServiceProvider sp, ServiceConfiguration config)
        {
            // TODO: This could be a preferable method to us explicitly setting this header and reading from it
            //app.UseForwardedHeaders(new ForwardedHeadersOptions {ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto})

            switch (item)
            {
                case Pipeline.DevelopmentHsts:
                    {
                        var env = ServiceLocator.Get<IWebHostEnvironment>(sp);
                        if (env.IsDevelopment() || env.IsEnvironment("localhost"))
                            app.UseDeveloperExceptionPage();
                        else
                            app.UseHsts();
                        break;
                    }

                case Pipeline.HttpsRedirect:
                    app.UseHttpsRedirection();
                    break;

                case Pipeline.Swagger:
                    {
                        if (config.SwaggerEnabled)
                        {
                            app.UseSwagger();
                            var appSettings = ServiceLocator.Get<IApplicationSettings>(sp);
                            app.UseSwaggerUI(c =>
                            {
                                c.SwaggerEndpoint(config.SwaggerEndpoint, appSettings.Name + " " + config.SwaggerVersion);
                                c.DocExpansion(DocExpansion.List);
                            });
                        }
                        break;
                    }

                case Pipeline.Routing:
                    app.UseRouting();
                    break;

                case Pipeline.Cors:
                    if (!string.IsNullOrWhiteSpace(config.CorsPolicyName))
                        app.UseCors(config.CorsPolicyName);
                    break;

                case Pipeline.HealthCheck:
                    var envSettings = ServiceLocator.Get<IEnvironmentSettings>(sp);
                    if (!string.IsNullOrWhiteSpace(config.HealthCheckEndpoint))
                        app.UseHealthChecks(config.HealthCheckEndpoint, config.HealthCheckOptions(envSettings));
                    break;

                case Pipeline.HttpData:
                    app.UseHttpDataMiddleware();
                    break;

                case Pipeline.Authentication:
                    app.UseAuthenticationMiddleware();
                    break;

                case Pipeline.Jwt:
                    app.UseJwtMiddleware();
                    break;

                case Pipeline.Exceptions:
                    app.UseExceptionMiddleware();
                    break;

                case Pipeline.Logging:
                    app.UseLoggingMiddleware();
                    break;

                case Pipeline.ControllerAction:
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                    break;

                case Pipeline.Custom1:
                    config.ApiCustomMiddleware1?.Invoke(app);
                    break;

                case Pipeline.Custom2:
                    config.ApiCustomMiddleware2?.Invoke(app);
                    break;

                case Pipeline.Custom3:
                    config.ApiCustomMiddleware3?.Invoke(app);
                    break;

                case Pipeline.Custom4:
                    config.ApiCustomMiddleware4?.Invoke(app);
                    break;

                case Pipeline.Custom5:
                    config.ApiCustomMiddleware5?.Invoke(app);
                    break;

                case Pipeline.Custom6:
                    config.ApiCustomMiddleware6?.Invoke(app);
                    break;

                case Pipeline.Custom7:
                    config.ApiCustomMiddleware7?.Invoke(app);
                    break;

                case Pipeline.Custom8:
                    config.ApiCustomMiddleware8?.Invoke(app);
                    break;

                case Pipeline.Custom9:
                    config.ApiCustomMiddleware9?.Invoke(app);
                    break;

                case Pipeline.Custom10:
                    config.ApiCustomMiddleware10?.Invoke(app);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }
    }
}
