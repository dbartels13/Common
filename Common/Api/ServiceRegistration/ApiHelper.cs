using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class ApiHelper
    {
        public static void ControllerConfiguration(ApiBehaviorOptions options) =>
            options.InvalidModelStateResponseFactory = context =>
            {
                //var dict = context.ModelState;
                //var errors = new List<string>();
                //foreach (var key in dict.Keys)
                //    errors.AddRange(dict[key].Errors.Select(x => x.ErrorMessage));
                //return new BadRequestObjectResult(ApiResponse.ModelStateErrors(errors));
                return new BadRequestObjectResult(context.ModelState);
            };

        public static void NewtonsoftConfiguration(MvcNewtonsoftJsonOptions options) => options.SerializerSettings.ContractResolver = NewtonsoftContractResolver;

        public static IContractResolver NewtonsoftContractResolver { get; set; } = new CamelCasePropertyNamesContractResolver();

        public static void JsonNetConfiguration(JsonOptions options) => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    }
}
