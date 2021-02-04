using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class ResultBaseInformation : TimerBaseInformation
    {
        public static string StatusCodeKey => "Http Response";
        public static string ResultKey => "Http Result";

        // Unused in this class, but child class do use these, so keeping it consistent
        public static string RouteKey => "Route";
        public static string MethodKey => "Http Method";
        public static string RequestDataKey => "Request Data";

        protected int StatusCode;
        private string _result;

        protected NameValueCollection Response { get; set; }

        protected ResultBaseInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        protected void InitializeResult(TraceEventType severity, string message)
            => InitializeTimer(severity, message);

        public void SaveResult(int statusCode, string result)
        {
            StatusCode = statusCode;
            _result = result;
        }

        public async Task SaveResponse(HttpResponse response)
        {
            SaveResult(
                response.StatusCode,
                await SafeTry.IgnoreException(async () => await response.GetBodyAsync())
            );
        }

        public async Task SaveResponse(HttpResponseMessage response)
        {
            SaveResult(
                (int)response.StatusCode,
                await SafeTry.IgnoreException(async () => await response.GetBodyAsync()));
        }

        public override void UpdateProperties(ILoggerConfiguration config)
        {
            base.UpdateProperties(config);

            // Use the internal object if it's in that format
            var apiResponse = SafeTry.IgnoreException(() => _result.DeserializeJson<ApiResponseObject>());
            if (apiResponse.IsPopulated() && apiResponse.Code != 0)
            {
                StatusCode = apiResponse.Code;

                // We could have both errors and a response object. We will just pick 1 (error first)
                _result = apiResponse.Error.IsPopulated()
                    ? apiResponse.Error.SerializeJson()
                    : apiResponse.Body?.SerializeJson();
            }

            if (StatusCode == 0)
                return;

            HighProperties.Add(StatusCodeKey, StatusCode.ToString());

            if (!config.Include(ResultKey))
                return;

            SetResponse();
            var hideKeys = config.HideKeys();
            HighProperties.Add(ResultKey, Response.AsString(hideKeys));
        }

        private void SetResponse() => Response ??= _result.ToNameValueCollection();
    }
}
