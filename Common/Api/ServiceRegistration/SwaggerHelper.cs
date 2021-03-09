using System;
using System.IO;
using Microsoft.OpenApi.Models;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.HttpClient;

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class SwaggerHelper
    {
        public static OpenApiInfo ApiInfo(IApplicationSettings app)
            => new OpenApiInfo
            {
                Title = app.Name,
                Contact = new OpenApiContact { Name = app.ContactName, Email = app.ContactEmail },
                Description = $@"{app.Description}
<p></p>
<b>Note: All responses will actually be Http 200 status code with the following wrapper:</b>
<br />{{
<br />&nbsp;&nbsp;&nbsp;""code"": The http response code you'd expect (int)
<br />&nbsp;&nbsp;&nbsp;""error"": For non-2xx codes, this will be a serialized object of the errors
<br />&nbsp;&nbsp;&nbsp;""body"": If there is a return object (eg. 2xx codes), this will be given with the object type defined below
<br />}}
"
            };

        public static string XmlComments(IApplicationSettings app)
        {
            var xmlFile = $"{app.Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }

        public static string SecurityPolicyName { get; set; } = "Sphyrnidae JWT";

        public static OpenApiSecurityScheme SecurityScheme(IHttpClientSettings http)
            => new OpenApiSecurityScheme
            {
                Description = "JWT Token",
                In = ParameterLocation.Header,
                Name = http.JwtHeader,
                Type = SecuritySchemeType.ApiKey
            };

        public static OpenApiSecurityRequirement SecurityRequirement() =>
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecurityPolicyName
                        }
                    },
                    new string[] { }
                }
            };
    }
}
