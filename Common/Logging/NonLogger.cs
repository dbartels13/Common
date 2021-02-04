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
        public Task Entry(BaseLogInformation info, Action errorAction = null) => Task.CompletedTask;

        public Task Exit(TimerBaseInformation info, Action errorAction = null) => Task.CompletedTask;

        public Task Log(TraceEventType severity, string message, string category = "") => Task.CompletedTask;
        public Task Unauthorized(string message) => Task.CompletedTask;

        public Task<TimerInformation> TimerStart(string name) => Task.FromResult(default(TimerInformation));
        public Task TimerEnd(TimerInformation info) => Task.CompletedTask;

        public Task Custom(TraceEventType severity, string type, string message, object o = null) => Task.CompletedTask;
        public Task Custom1(string message, object o = null) => Task.CompletedTask;
        public Task Custom2(string message, object o = null) => Task.CompletedTask;
        public Task Custom3(string message, object o = null) => Task.CompletedTask;

        public Task<CustomTimerInformation> CustomTimerStart(TraceEventType severity, string type, string name, object o = null) => Task.FromResult(default(CustomTimerInformation));
        public Task<CustomTimerInformation1> CustomTimer1Start(string name, object o = null) => Task.FromResult(default(CustomTimerInformation1));
        public Task<CustomTimerInformation2> CustomTimer2Start(string name, object o = null) => Task.FromResult(default(CustomTimerInformation2));
        public Task<CustomTimerInformation3> CustomTimer3Start(string name, object o = null) => Task.FromResult(default(CustomTimerInformation3));
        public Task CustomTimerEnd(CustomTimerInformation info, object o = null) => Task.CompletedTask;

        public Task<AttributeInformation> AttributeEntry(string attributeName, Dictionary<string, string> parameters) => Task.FromResult(default(AttributeInformation));
        public Task AttributeExit(AttributeInformation info) => Task.CompletedTask;

        public Task<ApiInformation> ApiEntry() => Task.FromResult(default(ApiInformation));
        public Task ApiExit(ApiInformation info, int statusCode, string result) => Task.CompletedTask;
        public Task ApiExit(ApiInformation info, HttpResponse response) => Task.CompletedTask;

        public Task<DatabaseInformation> DatabaseEntry(string cnnName, string command, object args = null) => Task.FromResult(default(DatabaseInformation));
        public Task DatabaseExit(DatabaseInformation info) => Task.CompletedTask;

        public Task<WebServiceInformation> WebServiceEntry(HttpHeaders headers, string route, string url, string method, object data = null) => Task.FromResult(default(WebServiceInformation));
        public Task WebServiceExit(WebServiceInformation info, int statusCode, string result) => Task.CompletedTask;
        public Task WebServiceExit(WebServiceInformation info, HttpResponseMessage response) => Task.CompletedTask;

        public Task<MiddlewareInformation> MiddlewareEntry(string name) => Task.FromResult(default(MiddlewareInformation));
        public Task MiddlewareExit(MiddlewareInformation info) => Task.CompletedTask;

        public Task HiddenException(Exception ex, bool messageOnly) => Task.CompletedTask;
        public Task HiddenException(Exception ex, string title = "", bool messageOnly = true) => Task.CompletedTask;

        public Task<Guid> Exception(Exception e, bool messageOnly) => Task.FromResult(Guid.Empty);
        public Task<Guid> Exception(Exception e, string title = "", bool messageOnly = false) => Task.FromResult(Guid.Empty);
    }
}
