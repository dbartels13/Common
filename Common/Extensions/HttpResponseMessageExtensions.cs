using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// HttpResponseMessage custom methods
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        #region Get Result
        /// <summary>
        /// Retrieves the result of the Json formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="endpoint">For logging purposes only: This is an identifier for a thrown exception</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetResult<T>(this HttpResponseMessage result, string endpoint, JsonSerializerSettings jsonSettings = null) => await result.GetResult(true, endpoint, default, x => x.DeserializeJson<T>(jsonSettings));
        /// <summary>
        /// Retrieves the result of the Json formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="defaultObject">This will be returned if any errors arise getting the result</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetResultAsync<T>(this HttpResponseMessage result, T defaultObject, JsonSerializerSettings jsonSettings = null) => await result.GetResult(false, null, defaultObject, x => x.DeserializeJson<T>(jsonSettings));
        /// <summary>
        /// Retrieves the result of the XML formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="endpoint">For logging purposes only: This is an identifier for a thrown exception</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetXmlResultAsync<T>(this HttpResponseMessage result, string endpoint, XmlSerializer serializer = null) => await result.GetResult(true, endpoint, default, x => x.DeserializeXml<T>(serializer));
        /// <summary>
        /// Retrieves the result of the XML formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="defaultObject">This will be returned if any errors arise getting the result</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetXmlResultAsync<T>(this HttpResponseMessage result, T defaultObject, XmlSerializer serializer = null) => await result.GetResult(false, null, defaultObject, x => x.DeserializeXml<T>(serializer));

        /// <summary>
        /// Retrieves the result of the http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="throwOnFailure">
        /// If false, the default object will be returned if any error is encountered
        /// If true and an error is encountered retrieving the result, an exception will be thrown
        /// </param>
        /// <param name="endpoint">For logging purposes only: set this as an identifier for a thrown exception</param>
        /// <param name="defaultObject">If "throwOnFailure" is false, this will be returned instead</param>
        /// <param name="deserializer">The deserialization method</param>
        /// <returns>The object from the result</returns>
        private static async Task<T> GetResult<T>(this HttpResponseMessage result, bool throwOnFailure, string endpoint, T defaultObject, Func<string, T> deserializer)
            => await SafeTry.OnException(
                async () =>
                {
                    // Successful, return the object
                    if (result.IsSuccessStatusCode)
                    {
                        // This will actually be done twice in web services... first one for logging, and 2nd one for the actual result (this).
                        var strResult = await result.GetBodyAsync();

                        // Since this is not an error, don't return "defaultObject" - "default" is correct
                        return strResult == null ? default : deserializer(strResult);
                    }

                    // Unsuccessful
                    if (!throwOnFailure)
                        return defaultObject;

                    if (endpoint == null)
                        throw new Exception("Unsuccessful call");
                    throw new Exception("Unsuccessful call to: " + endpoint);
                },
                ex =>
                {
                    if (throwOnFailure)
                        throw ex;
                    return defaultObject;
                });
        #endregion

        #region Helpers
        /// <summary>
        /// Obtains an item from the HTTP header
        /// </summary>
        /// <param name="result">The current http response</param>
        /// <param name="name">The name of the HTTP header to retrieve</param>
        /// <returns>The value of the HTTP header</returns>
        public static string GetHeader(this HttpResponseMessage result, string name) => string.IsNullOrWhiteSpace(name) ? null : result?.Headers.Get(name);

        /// <summary>
        /// Obtains the body as a string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static async Task<string> GetBodyAsync(this HttpResponseMessage result) => await result.Content.ReadAsStringAsync();
        #endregion
    }
}
