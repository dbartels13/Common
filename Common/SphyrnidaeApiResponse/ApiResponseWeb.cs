namespace Sphyrnidae.Common.SphyrnidaeApiResponse
{
    /// <summary>
    /// The same response, but cast-able to a defined type instead of object
    /// </summary>
    /// <typeparam name="T">The type of object that will actually be returned</typeparam>
    public class ApiResponseWeb<T> : ApiResponseBase
    {
        public T Body { get; set; }
    }
}