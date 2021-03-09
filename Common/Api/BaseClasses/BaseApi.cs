using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Sphyrnidae.Common.Api.Responses;
using Sphyrnidae.Common.Serialize;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.BaseClasses
{
    /// <inheritdoc />
    /// <summary>
    /// Base controller which all API controllers should inherit from
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public abstract class BaseApi : ControllerBase
    {
        protected IApiResponse ApiResponse { get; }
        public BaseApi(IApiResponse response) => ApiResponse = response;

        /// <summary>
        /// Converts the business entity into the desired HTTP response
        /// </summary>
        /// <param name="standardResponse">The business entity containing the response information</param>
        /// <returns>Properly formatted HTTP response</returns>
        protected ObjectResult FormatResponse(ApiResponseStandard standardResponse)
        {
            var apiResponse = standardResponse.ConvertToOther(ApiResponse);
            var result = new ObjectResult(apiResponse.ToResponseBody());
            result.StatusCode = apiResponse.Code; // Do this as 2nd step in case the ToResponseBody() changed the Code
            return result;
        }

        /// <summary>
        /// Converts the business entity into the desired HTTP response with minimal serialization properties
        /// </summary>
        /// <param name="standardResponse">The business entity containing the response information</param>
        /// <returns>Properly formatted HTTP response</returns>
        protected JsonResult GetWithDefaultsRemoved(ApiResponseStandard standardResponse)
        {
            var apiResponse = standardResponse.ConvertToOther(ApiResponse);
            var result = new JsonResult(apiResponse.ToResponseBody(), SerializationSettings.Minimal);
            result.StatusCode = apiResponse.Code; // Do this as 2nd step in case the ToResponseBody() changed the Code
            return result;
        }
    }
}