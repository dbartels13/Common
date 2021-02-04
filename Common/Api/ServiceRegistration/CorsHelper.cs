using Microsoft.AspNetCore.Cors.Infrastructure;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class CorsHelper
    {
        public static string CorsPolicyName { get; set; } = "Cors All";

        public static void CorsAll(CorsOptions options)
        {
            options.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        }

        public static void CorsAllWithCredentials(CorsOptions options)
        {
            options.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(s => true)
                    .AllowCredentials();
            });
        }
    }
}