namespace Sphyrnidae.Common.Api.Responses
{
    /// <summary>
    /// Handles HTTP response information
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>
        /// The actual http response code
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// Any errors encountered during the request that need to be returned to the user
        /// </summary>
        object Error { get; set; }

        /// <summary>
        /// The main body
        /// </summary>
        object Body { get; set; }

        /// <summary>
        /// Converts the object into a formatted HTTP response object
        /// </summary>
        /// <returns>The actual HTTP response body (possibly formatted)</returns>
        object ToResponseBody();
    }
}
