using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Api.ServiceRegistration.Models;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Serialize;

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class HealthCheckHelper
    {
        public static string Url { get; set; } = "/hc";

        public static HealthCheckOptions Options(IEnvironmentSettings env) =>
            new HealthCheckOptions
            {
                ResponseWriter = async (c, r) =>
                {
                    c.Response.ContentType = MediaTypeNames.Application.Json;
                    var result = new HealthCheck(r, env).SerializeJson();
                    await c.Response.WriteAsync(result);
                }
            };
    }
}