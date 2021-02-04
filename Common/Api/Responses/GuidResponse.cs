using System;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Responses
{
    /// <summary>
    /// A response from an API that only contains a guid
    /// </summary>
    public class GuidResponse
    {
        /// <summary>
        /// The guid being returned
        /// </summary>
        public Guid Id { get; set; }
    }
}