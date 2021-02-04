namespace Sphyrnidae.Common.SphyrnidaeApiResponse
{
    /// <summary>
    /// A helper class that is easier to manage than an IActionResult
    /// </summary>
    public abstract class ApiResponseBase
    {
        /// <summary>
        /// The actual http response code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// If there is an error, the body will contain the text, and this property will contain an object with relevant information
        /// </summary>
        public object Error { get; set; }
    }
}
