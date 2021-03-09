using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Authentication.Identity;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.RequestData;
using System.Collections.Generic;

namespace Sphyrnidae.Common.Logging
{
    /// <summary>
    /// Retrieves logging information from the API request/http
    /// </summary>
    public class LoggerInformation : ILoggerInformation
    {
        protected IHttpClientSettings HttpSettings { get; }
        protected IIdentityHelper IdentityHelper { get; }
        protected IRequestData RequestData { get; }

        public LoggerInformation(IHttpClientSettings httpSettings, IIdentityHelper identity, IRequestData requestData)
        {
            HttpSettings = httpSettings;
            IdentityHelper = identity;
            RequestData = requestData;
        }

        /// <summary>
        /// If provided in the HTTP header
        /// </summary>
        public string LogOrderPrefix => HttpSettings.LogOrder;

        /// <summary>
        /// If provided in the HTTP header, otherwise will be a unique ID
        /// </summary>
        public string RequestId => HttpSettings.RequestId;

        /// <summary>
        /// Stores the order/sequence of logging statement within the current call
        /// </summary>
        /// <remarks>This class is transient, so this will forward it on to a scoped class</remarks>
        public char LoggingOrder
        {
            get => RequestData.LoggingOrder;
            set => RequestData.LoggingOrder = value;
        }

        /// <summary>
        /// The current identity on the request
        /// </summary>
        public BaseIdentity Identity => IdentityHelper.Current;

        /// <summary>
        /// If provided in the HTTP header
        /// </summary>
        public string SessionId => HttpSettings.SessionId;

        /// <summary>
        /// API endpoint information: &lt;verb&gt; &lt;route&gt;
        /// </summary>
        public string Method => RequestData.HttpVerb + " " + RequestData.Route;

        /// <summary>
        /// Can override in inherited class for any custom properties
        /// </summary>
        public virtual Dictionary<string, string> StaticProperties => new Dictionary<string, string>();
    }
}