using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.WebServices
{
    /// <summary>
    /// Base class for all web service calls. You should inherit for each web service endpoint you will be using.
    /// </summary>
    public abstract class WebServiceBase
    {
        #region Constructor and Implementations
        protected IHttpClientFactory Factory { get; }
        protected IHttpClientSettings Settings { get; }
        protected IIdentityHelper Identity { get; }
        protected ILogger Logger { get; }
        protected WebServiceBase(IHttpClientFactory factory, IHttpClientSettings settings, IIdentityHelper identity, ILogger logger)
        {
            Factory = factory;
            Settings = settings;
            Identity = identity;
            Logger = logger;
        }
        #endregion

        #region Virtuals
        /// <summary>
        /// Override this in inherited class to further alter the headers to be sent in the request
        /// </summary>
        /// <param name="headers">HttpHeaders collection</param>
        protected virtual void AlterHeaders(HttpHeaders headers) { }

        /// <summary>
        /// HTML must accept JSON
        /// </summary>
        protected virtual string JsonContentType => MediaTypeNames.Application.Json;

        /// <summary>
        /// HTML must accept XML
        /// </summary>
        protected virtual string XmlContentType => MediaTypeNames.Text.Xml;
        #endregion

        #region Main Calls
        /// <summary>
        /// Makes a POST request to another API
        /// </summary>
        /// <typeparam name="T">The type of data being posted</typeparam>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <param name="data">The data to be posted</param>
        /// <returns>The HttpResponseMessage from the POST request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> PostAsync<T>(string name, string endpoint, T data)
        {
            var content = new StringContent(data.SerializeJson(), Encoding.UTF8, JsonContentType); // Hard-coding this so that it's all JSON

            AddHeaders(content.Headers);
            AlterHeaders(content.Headers);

            var client = GetClient();

            var info = Logger.WebServiceEntry(content.Headers, name, endpoint, "Post", data);
            var result = await client.PostAsync(endpoint, content);
            await Logger.WebServiceExit(info, result);

            return result;
        }

        /// <summary>
        /// Makes a PUT request to another API
        /// </summary>
        /// <typeparam name="T">The type of data being put</typeparam>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <param name="data">The data to be put</param>
        /// <returns>The HttpResponseMessage from the PUT request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> PutAsync<T>(string name, string endpoint, T data)
        {
            var content = new StringContent(data.SerializeJson(), Encoding.UTF8, JsonContentType);

            AddHeaders(content.Headers);
            AlterHeaders(content.Headers);

            var client = GetClient();

            var info = Logger.WebServiceEntry(content.Headers, name, endpoint, "Put", data);
            var result = await client.PutAsync(endpoint, content);
            await Logger.WebServiceExit(info, result);

            return result;
        }

        /// <summary>
        /// Makes a PATCH request to another API
        /// </summary>
        /// <typeparam name="T">The type of data being put</typeparam>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <param name="data">The data to be patched</param>
        /// <returns>The HttpResponseMessage from the PATCH request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> PatchAsync<T>(string name, string endpoint, T data)
        {
            var content = new StringContent(data.SerializeJson(), Encoding.UTF8, JsonContentType);

            AddHeaders(content.Headers);
            AlterHeaders(content.Headers);

            var client = GetClient();

            var info = Logger.WebServiceEntry(content.Headers, name, endpoint, "Patch", data);
            var result = await client.PatchAsync(endpoint, content);
            await Logger.WebServiceExit(info, result);

            return result;
        }

        /// <summary>
        /// Makes a GET request to another API
        /// </summary>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <returns>The HttpResponseMessage from the GET request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> GetAsync(string name, string endpoint)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint),
                Method = HttpMethod.Get
            };

            AddHeaders(request.Headers);
            AlterHeaders(request.Headers);

            var client = GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType)); // Not sure if this is needed?

            var info = Logger.WebServiceEntry(request.Headers, name, endpoint, "Get");
            var result = await client.SendAsync(request);
            await Logger.WebServiceExit(info, result);

            return result;
        }

        /// <summary>
        /// Makes a DELETE request to another API
        /// </summary>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <returns>The HttpResponseMessage from the DELETE request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> DeleteAsync(string name, string endpoint)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint),
                Method = HttpMethod.Delete
            };

            AddHeaders(request.Headers);
            AlterHeaders(request.Headers);

            var client = GetClient();

            var info = Logger.WebServiceEntry(request.Headers, name, endpoint, "Delete");
            var result = await client.SendAsync(request);
            await Logger.WebServiceExit(info, result);

            return result;
        }

        /// <summary>
        /// Sends an image to another API
        /// </summary>
        /// <remarks>This uses the form type multipart/form-content</remarks>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <param name="filename">The name of the file</param>
        /// <param name="img">The image to send (as a Stream)</param>
        /// <returns>The HttpResponseMessage from the image request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> ImageAsync(string name, string endpoint, string filename, Stream img)
        {
            var content = new MultipartFormDataContent
            {
                new StreamContent(img)
            };
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = filename
            };

            AddHeaders(content.Headers);
            AlterHeaders(content.Headers);

            var client = GetClient();

            var info = Logger.WebServiceEntry(content.Headers, name, endpoint, "Image", filename);
            var result = await client.PostAsync(endpoint, content);
            await Logger.WebServiceExit(info, result);

            return result;
        }

        /// <summary>
        /// Makes a POST request to another API with xml content
        /// </summary>
        /// <param name="name">A name per service/request method (Ignores unique parameters - eg. Service1_GetSomething)</param>
        /// <param name="endpoint">The full URL of the API endpoint to hit (possibly with querystring attached)</param>
        /// <param name="xml">The xml data to be posted (already serialized)</param>
        /// <returns>The HttpResponseMessage from the POST request (can use .GetResult()) extension method to get at the real return object</returns>
        protected async Task<HttpResponseMessage> XmlPostAsync(string name, string endpoint, string xml)
        {
            var content = new StringContent(xml, Encoding.UTF8, XmlContentType);

            AddHeaders(content.Headers);
            AlterHeaders(content.Headers);

            var client = GetClient();

            var info = Logger.WebServiceEntry(content.Headers, name, endpoint, "XML Post", xml);
            var result = await client.PostAsync(endpoint, content);
            await Logger.WebServiceExit(info, result);

            return result;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Obtains the HttpClient and adds in required DefaultRequestHeaders
        /// </summary>
        /// <returns>The HttpClient</returns>
        protected virtual System.Net.Http.HttpClient GetClient()
        {
            var client = Factory.CreateClient();

            //Client.DefaultRequestHeaders.Add("Connection", "close");
            //Client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //Client.DefaultRequestHeaders.Add("Connection", "3600");

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // Had this one previously
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            // Must specify this (using a real browser, also required, but this could be any text we want - eg. "WebServiceCall")
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");

            // Always include the token which has other custom properties like UserId/CustomerId
            var identity = Identity.Current;
            if (!identity.IsPopulated())
                return client;

            var jwt = Identity.ToJwt(identity);
            client.DefaultRequestHeaders.TryAddWithoutValidation(Settings.JwtHeader, jwt);
            return client;
        }

        /// <summary>
        /// Adds in common headers used by this framework
        /// </summary>
        /// <param name="headers">HttpHeaders to modify</param>
        protected virtual void AddHeaders(HttpHeaders headers)
        {
            // Request/CorrelationId
            var requestId = Settings.RequestId;
            if (!string.IsNullOrWhiteSpace(requestId))
                headers.Add(Settings.RequestIdHeader, requestId);

            // Session/TrackingId
            var trackingId = Settings.SessionId;
            if (!string.IsNullOrWhiteSpace(trackingId))
                headers.Add(Settings.SessionIdHeader, trackingId);

            // IP Address
            var ip = Settings.IpAddress;
            if (!string.IsNullOrWhiteSpace(ip))
                headers.Add(Settings.IpAddressHeader, ip);

            // LogOrder is not auto-set, will get appended with the Logger.WebServiceEntry() call instead (it must be there)
        }
        #endregion

        #region Response Parsing
        /// <summary>
        /// If the body is something other than the real type that you want, this will convert it
        /// </summary>
        /// <param name="body">The full body of the HttpResponseMessage</param>
        /// <returns>The part of the body that contains the realy object(s)</returns>
        protected virtual Tuple<int, string> ParseResponseBody(int statusCode, string body)
            => new Tuple<int, string>(statusCode, body);

        /// <summary>
        /// Retrieves the result of a Json formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="response">The HttpResponseMessage</param>
        /// <param name="name">For logging purposes only: This is an identifier for a thrown exception</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The object from the response</returns>
        protected Task<T> GetResult<T>(HttpResponseMessage response, string name,
            JsonSerializerSettings jsonSettings = null)
            => GetResult(
                response,
                true,
                name,
                default,
                x => x.DeserializeJson<T>(jsonSettings));

        /// <summary>
        /// Retrieves the result of a Json formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="response">The HttpResponseMessage</param>
        /// <param name="defaultObject">This will be returned if any errors arise getting the result</param>
        /// <param name="jsonSettings">Optional: The json settings to use</param>
        /// <returns>The object from the result</returns>
        protected Task<T> GetResult<T>(HttpResponseMessage response, T defaultObject,
            JsonSerializerSettings jsonSettings = null)
            => GetResult(
                response,
                false,
                null,
                defaultObject,
                x => x.DeserializeJson<T>(jsonSettings));

        /// <summary>
        /// Retrieves the result of a XML formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="response">The HttpResponseMessage</param>
        /// <param name="name">For logging purposes only: This is an identifier for a thrown exception</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The object from the result</returns>
        protected Task<T> GetXmlResult<T>(HttpResponseMessage response, string name,
            XmlSerializer serializer = null)
            => GetResult(
                response,
                true,
                name,
                default,
                x => x.DeserializeXml<T>(serializer));

        /// <summary>
        /// Retrieves the result of a XML formatted http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="response">The HttpResponseMessage</param>
        /// <param name="defaultObject">This will be returned if any errors arise getting the result</param>
        /// <param name="serializer">Optional: The XML Serializer to use</param>
        /// <returns>The object from the result</returns>
        protected Task<T> GetXmlResult<T>(HttpResponseMessage response, T defaultObject,
            XmlSerializer serializer = null)
            => GetResult(
                response,
                false,
                null,
                defaultObject,
                x => x.DeserializeXml<T>(serializer));

        /// <summary>
        /// Retrieves the result of a http response
        /// </summary>
        /// <typeparam name="T">Any object</typeparam>
        /// <param name="response">The HttpResponseMessage</param>
        /// <param name="throwOnFailure">
        /// If false, the default object will be returned if any error is encountered
        /// If true and an error is encountered retrieving the result, an exception will be thrown
        /// </param>
        /// <param name="name">For logging purposes only: set this as an identifier for a thrown exception</param>
        /// <param name="defaultObject">If "throwOnFailure" is false, this will be returned instead</param>
        /// <param name="deserializer">The deserialization method</param>
        /// <returns>The object from the result</returns>
        private async Task<T> GetResult<T>(HttpResponseMessage response, bool throwOnFailure,
            string name, T defaultObject, Func<string, T> deserializer)
        {
            try
            {
                // Make sure the call was successful
                if (!response.IsSuccessStatusCode)
                    return Unsuccessful(throwOnFailure, name, defaultObject);

                // Get body as string
                // This will actually be done twice in web services... first one for logging, and 2nd one for the actual result.
                var body = await response.GetBodyAsync();
                if (body == null)
                    return Unsuccessful(throwOnFailure, name, defaultObject);

                // Possibly do some parsing in case the structure is not of type T
                var parsedResponse = ParseResponseBody((int)response.StatusCode, body);

                // Secondary check in case the status code changed based on parsing
                var statusCode = parsedResponse.Item1;
                if (statusCode < 200 || statusCode >= 300)
                    return Unsuccessful(throwOnFailure, name, defaultObject);

                // Deserialize to complex outer object
                var result = deserializer(parsedResponse.Item2);
                return result;
            }
            catch (Exception ex)
            {
                if (throwOnFailure)
                    throw ex;
                return defaultObject;
            }
        }

        private static T Unsuccessful<T>(bool throwOnFailure, string name, T defaultObject)
        {
            // Unsuccessful
            if (!throwOnFailure)
                return defaultObject;

            if (name == null)
                throw new Exception("Unsuccessful call");
            throw new Exception("Unsuccessful call to: " + name);
        }
        #endregion
    }
}
