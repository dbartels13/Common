// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Api.Responses.Wrappers
{
    /// <summary>
    /// A response from an API that only contains a string
    /// </summary>
    public class StringResponse
    {
        /// <summary>
        /// The guid being returned
        /// </summary>
        public string Str { get; set; }
    }
}