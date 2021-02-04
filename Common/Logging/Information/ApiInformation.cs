using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Logging.Models;
using Sphyrnidae.Common.RequestData;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for API calls
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class OTHER THAN: SaveResult</remarks>
    public class ApiInformation : ResultBaseInformation
    {
        public static string HeadersKey => "Http Headers";
        public static string QueryStringKey => "Query String";
        public static string FormKey => "Form Data";
        public static string BrowserKey => "Browser";

        protected IRequestData RequestData { get; }
        protected IHttpClientSettings HttpSettings { get; }

        #region Alerts
        public override string LongRunningName => $"API-{AppSettings.Name}-{HttpMethod ?? ""}-{Route ?? ""}";
        public override HttpResponseInfo GetResponseInfo() => new HttpResponseInfo(Type, Route, HttpMethod, StatusCode);
        public override string GetResponse(CaseInsensitiveBinaryList<string> hideKeys)
            => HighProperties.ContainsKey(ResultKey)
                ? HighProperties[ResultKey]
                : Response.AsString(hideKeys);
        #endregion

        #region Properties
        private string ContentData { get; set; }
        private string Route { get; set; }
        private string HttpMethod { get; set; }
        private NameValueCollection Headers { get; set; }
        private NameValueCollection QueryString { get; set; }
        private NameValueCollection FormData { get; set; }
        private NameValueCollection RequestUserData { get; set; }
        private string Browser { get; set; }
        #endregion

        #region Constructor

        public ApiInformation(ILoggerInformation info, IApplicationSettings appSettings,
            IRequestData requestData, IHttpClientSettings httpSettings)
            : base(info, appSettings)
        {
            RequestData = requestData;
            HttpSettings = httpSettings;
            Category = "API";
        }

        public virtual async Task Initialize(ILoggerConfiguration config)
        {
            InitializeResult(TraceEventType.Information, RequestData.DisplayUrl);
            // Need to save off anything from the request/context objects here in the main thread
            SetRoute();
            SetMethod();
            SetHeaders();
            SetQueryString();
            SetFormData();
            if (config.Include(RequestDataKey))
                ContentData = await RequestData.ContentData();
            SetBrowser();
        }
        #endregion

        #region Overrides
        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);
            var hideKeys = config.HideKeys();

            // Most are set in Initialize
            if (!string.IsNullOrWhiteSpace(Route))
                HighProperties.Add(RouteKey, Route);
            if (!string.IsNullOrWhiteSpace(HttpMethod))
                HighProperties.Add(MethodKey, HttpMethod);
            if (config.Include(HeadersKey))
                MedProperties.Add(HeadersKey, GetHeaders());
            if (config.Include(QueryStringKey))
                MedProperties.Add(QueryStringKey, QueryString.AsString(hideKeys));
            if (config.Include(FormKey))
                MedProperties.Add(FormKey, FormData.AsString(hideKeys));
            if (config.Include(RequestDataKey))
            {
                SetRequestData(); // Raw info was saved in Initialize
                MedProperties.Add(RequestDataKey, RequestUserData.AsString(hideKeys));
            }
            if (!string.IsNullOrWhiteSpace(Browser) && config.Include(BrowserKey))
                LowProperties.Add(BrowserKey, Browser);
        }

        #region High
        private void SetRoute() => Route ??= RequestData.Route;
        private void SetMethod() => HttpMethod ??= RequestData.HttpVerb;
        #endregion

        #region Medium
        private void SetHeaders() => Headers ??= RequestData.Headers;
        protected string GetHeaders()
        {
            var authToken = HttpSettings.JwtHeader;
            var hideKeys = new[] { authToken }.ToCaseInsensitiveBinaryList();
            return Headers.AsString(hideKeys);
        }

        private void SetQueryString() => QueryString ??= RequestData.QueryString;

        private void SetFormData() => FormData ??= RequestData.FormData;

        private void SetRequestData() => RequestUserData ??= ContentData.ToNameValueCollection();
        #endregion

        #region Low
        private void SetBrowser() => Browser ??= RequestData.Browser;
        #endregion
        #endregion
    }
}
