namespace Sphyrnidae.Common.Api.Models
{
    /// <summary>
    /// How the request should be authenticated
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// No authentication required at all
        /// </summary>
        None,

        /// <summary>
        /// Request should be fully authenticated (eg. authorization header with token parseable to identity)
        /// </summary>
        Jwt,

        /// <summary>
        /// This is an API to API internal request
        /// </summary>
        ApiToApi
    }
}