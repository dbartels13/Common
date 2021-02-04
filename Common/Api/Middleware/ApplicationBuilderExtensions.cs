using Microsoft.AspNetCore.Builder;

namespace Sphyrnidae.Common.Api.Middleware
{
    /// <summary>
    /// Extension methods for registering middleware components
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpDataMiddleware(this IApplicationBuilder app) => app.UseMiddleware<HttpDataMiddleware>();
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder app) => app.UseMiddleware<AuthenticationMiddleware>();
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder app) => app.UseMiddleware<JwtMiddleware>();
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app) => app.UseMiddleware<ApiLoggingMiddleware>();
    }
}