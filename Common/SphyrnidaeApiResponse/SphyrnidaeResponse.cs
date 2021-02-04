using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.SphyrnidaeApiResponse
{
    /// <summary>
    /// Extensions method for retrieving real response from a visual vault response message
    /// </summary>
    public static class SphyrnidaeResponse
    {
        /// <summary>
        /// Retrieves the result of a visual vault Json formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="name">For logging purposes only: This is an identifier for a thrown exception</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetSphyrnidaeResult<T>(this HttpResponseMessage result, string name,
            JsonSerializerSettings jsonSettings = null)
            => await result.GetSphyrnidaeResult(
                true,
                name,
                default,
                x => x.DeserializeJson<ApiResponseWeb<T>>(jsonSettings));

        /// <summary>
        /// Retrieves the result of a visual vault Json formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="defaultObject">This will be returned if any errors arise getting the result</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetSphyrnidaeResult<T>(this HttpResponseMessage result, T defaultObject,
            JsonSerializerSettings jsonSettings = null)
            => await result.GetSphyrnidaeResult(
                false,
                null,
                defaultObject,
                x => x.DeserializeJson<ApiResponseWeb<T>>(jsonSettings));

        /// <summary>
        /// Retrieves the result of a visual vault XML formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="name">For logging purposes only: This is an identifier for a thrown exception</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetSphyrnidaeXmlResult<T>(this HttpResponseMessage result, string name,
            XmlSerializer serializer = null)
            => await result.GetSphyrnidaeResult(
                true,
                name,
                default,
                x => x.DeserializeXml<ApiResponseWeb<T>>(serializer));

        /// <summary>
        /// Retrieves the result of a visual vault XML formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="defaultObject">This will be returned if any errors arise getting the result</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The object from the result</returns>
        public static async Task<T> GetSphyrnidaeXmlResult<T>(this HttpResponseMessage result, T defaultObject,
            XmlSerializer serializer = null)
            => await result.GetSphyrnidaeResult(
                false,
                null,
                defaultObject,
                x => x.DeserializeXml<ApiResponseWeb<T>>(serializer));

        /// <summary>
        /// Retrieves the result of a visual vault http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="result">The HttpResponseMessage</param>
        /// <param name="throwOnFailure">
        /// If false, the default object will be returned if any error is encountered
        /// If true and an error is encountered retrieving the result, an exception will be thrown
        /// </param>
        /// <param name="name">For logging purposes only: set this as an identifier for a thrown exception</param>
        /// <param name="defaultObject">If "throwOnFailure" is false, this will be returned instead</param>
        /// <param name="deserializer">The deserialization method</param>
        /// <returns>The object from the result</returns>
        private static async Task<T> GetSphyrnidaeResult<T>(this HttpResponseMessage result, bool throwOnFailure,
            string name, T defaultObject, Func<string, ApiResponseWeb<T>> deserializer)
            => await SafeTry.OnException(async () =>
                {
                    // The outer needs to always be success, otherwise there is some sort of larger issue
                    if (!result.IsSuccessStatusCode)
                        return Unsuccessful(throwOnFailure, name, defaultObject);

                    // Get result as string
                    // This will actually be done twice in web services... first one for logging, and 2nd one for the actual result.
                    var strResult = await result.GetBodyAsync();
                    if (strResult == null)
                        return Unsuccessful(throwOnFailure, name, defaultObject);

                    // Deserialize to complex outer object
                    var sphyrnidaeResult = deserializer(strResult);

                    // Check the real status code
                    var statusCode = sphyrnidaeResult?.Code ?? 0;
                    if (statusCode < 200 || statusCode >= 300)
                        return Unsuccessful(throwOnFailure, name, defaultObject);

                    // Check for errors
                    // ReSharper disable once PossibleNullReferenceException
                    return sphyrnidaeResult.Error.IsPopulated()
                        ? Unsuccessful(throwOnFailure, name, defaultObject)
                        : sphyrnidaeResult.Body;
                },
                ex =>
                {
                    if (throwOnFailure)
                        throw ex;
                    return defaultObject;
                });

        private static T Unsuccessful<T>(bool throwOnFailure, string name, T defaultObject)
        {
            // Unsuccessful
            if (!throwOnFailure)
                return defaultObject;

            if (name == null)
                throw new Exception("Unsuccessful call");
            throw new Exception("Unsuccessful call to: " + name);
        }
    }
}
