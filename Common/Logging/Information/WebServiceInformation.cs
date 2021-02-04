using System.Collections.Specialized;
using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Logging.Models;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for Web Service calls
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class OTHER THAN: SaveResult</remarks>
    public class WebServiceInformation : ResultBaseInformation
    {
        public override string LongRunningName => $"WebService-{AppSettings.Name}-{HttpMethod ?? ""}-{Route ?? ""}";
        public override HttpResponseInfo GetResponseInfo() => new HttpResponseInfo(Type, Route, HttpMethod, StatusCode);
        public override string GetResponse(CaseInsensitiveBinaryList<string> hideKeys)
            => HighProperties.ContainsKey(ResultKey)
                ? HighProperties[ResultKey]
                : Response.AsString(hideKeys);

        private string Route { get; set; }
        private string HttpMethod { get; set; }
        private object Data { get; set; }

        private NameValueCollection DataNvc { get; set; }

        public WebServiceInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings)
        {
            Category = "Web Service";
        }

        public virtual void Initialize(string route, string url, string httpMethod, object data)
        {
            InitializeResult(TraceEventType.Verbose, $"{httpMethod} {url}");
            Route = route;
            HttpMethod = httpMethod;
            Data = data;
        }

        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);

            if (!string.IsNullOrWhiteSpace(Route))
                HighProperties.Add(RouteKey, Route);

            if (!string.IsNullOrWhiteSpace(HttpMethod))
                HighProperties.Add(MethodKey, HttpMethod);

            if (!config.Include(RequestDataKey))
                return;

            SetData();
            var hideKeys = config.HideKeys();
            MedProperties.Add(RequestDataKey, DataNvc.AsString(hideKeys));
        }

        private void SetData() => DataNvc ??= Data.ToNameValueCollection();
    }
}
