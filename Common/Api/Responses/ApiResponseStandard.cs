namespace Sphyrnidae.Common.Api.Responses
{
    /// <inheritdoc />
    /// <summary>
    /// Base class with common implementations
    /// </summary>
    public class ApiResponseStandard : IApiResponse
    {
        public int Code { get; set; }

        public object Error { get; set; }

        public object Body { get; set; }

        /// <summary>
        /// The response will not be a complicated object, will just be the body
        /// </summary>
        /// <returns>The HTTP response (just the body)</returns>
        public virtual object ToResponseBody() => Body;

        /// <summary>
        /// Generally, the business logic uses an ApiResponseStandard.
        /// However, the actual response from the controller might be formatted differently.
        /// This is a method that will do the conversion to this other type
        /// </summary>
        /// <typeparam name="T">The proper IApiResponse</typeparam>
        /// <param name="response">The injected proper IApiResponse</param>
        /// <returns>The proper IApiResponse</returns>
        public T ConvertToOther<T>(T response) where T : IApiResponse
        {
            response.Code = Code;
            response.Error = Error;
            response.Body = Body;
            return response;
        }
    }
}
