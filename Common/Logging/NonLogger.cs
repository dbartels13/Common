using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging
{
    /// <inheritdoc />
    /// <summary>
    /// Logging implementation that does absolutely nothing
    /// </summary>
    public class NonLogger : ILogger
    {
        public void Generic(BaseLogInformation info, Action errorAction = null) { }
        public void Entry(TimerBaseInformation info, Action errorAction = null) { }
        public void Exit(TimerBaseInformation info, Action errorAction = null) { }

        public void Log(TraceEventType severity, string message, string category = "") { }
        public void Unauthorized(string message) { }

        public TimerInformation TimerStart(string name) => null;
        public void TimerEnd(TimerInformation info) { }

        public void Custom1<T>(T obj, string message = "") { }
        public void Custom2<T>(T obj, string message = "") { }
        public void Custom3<T>(T obj, string message = "") { }

        public CustomTimerInformation1<T1, T2> CustomTimer1Start<T1, T2>(T1 obj, string message = "") => null;
        public CustomTimerInformation2<T1, T2> CustomTimer2Start<T1, T2>(T1 obj, string message = "") => null;
        public CustomTimerInformation3<T1, T2> CustomTimer3Start<T1, T2>(T1 obj, string message = "") => null;
        public void CustomTimerEnd<T1, T2>(CustomTimerInformation<T1, T2> info, T2 obj) { }

        public AttributeInformation AttributeEntry(string attributeName, Dictionary<string, string> parameters) => null;
        public void AttributeExit(AttributeInformation info) { }

        public Task<ApiInformation> ApiEntry() => Task.FromResult(default(ApiInformation));
        public void ApiExit(ApiInformation info, int statusCode, string result) { }
        public Task ApiExit(ApiInformation info, HttpResponse response) => Task.CompletedTask;

        public DatabaseInformation DatabaseEntry(string cnnName, string command, object args = null) => null;
        public void DatabaseExit(DatabaseInformation info) { }

        public WebServiceInformation WebServiceEntry(HttpHeaders headers, string name, string url, string method, object data = null) => null;
        public void WebServiceExit(WebServiceInformation info, int statusCode, string result) { }
        public Task WebServiceExit(WebServiceInformation info, HttpResponseMessage response) => Task.CompletedTask;

        public MiddlewareInformation MiddlewareEntry(string name) => null;
        public void MiddlewareExit(MiddlewareInformation info) { }

        public void HiddenException(Exception ex, bool messageOnly) { }
        public void HiddenException(Exception ex, string title = "", bool messageOnly = true) { }

        public Guid Exception(Exception e, bool messageOnly) => Guid.Empty;
        public Guid Exception(Exception e, string title = "", bool messageOnly = false) => Guid.Empty;
    }
}
