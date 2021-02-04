using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
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
        protected static JsonResult GetWithDefaultsRemoved(ApiResponseObject response)
            => new JsonResult(response, SerializationSettings.Minimal);
    }
}