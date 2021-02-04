using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Alerts;
using Sphyrnidae.Common.EmailUtilities;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Logging.Loggers;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Logging
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of logging
    /// </summary>
    /// <remarks>All calls will be handled asynchronously and the actual logging occurs in this class</remarks>
    public class Logger : ILogger
    {
        #region Properties
        protected ILoggerConfiguration Config { get; }
        protected ILoggers Loggers { get; }
        protected IServiceProvider Provider { get; }
        protected IEmailServices EmailServices { get; }
        protected IAlert Alert { get; }
        protected LongRunningInformation LongRunningInfo { get; }
        protected HttpResponseInformation HttpResponseInfo { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Note that I'm only injecting ILoggerConfiguration, and all the others are being looked up via Service Locator.
        /// This seemed like an easier pattern to use instead of having a large interface listing into constructor
        /// </summary>
        public Logger(
            ILoggerConfiguration config,
            ILoggers loggers,
            IServiceProvider provider,
            IEmailServices emailServices,
            IAlert alert,
            LongRunningInformation longRunningInfo,
            HttpResponseInformation httpResponseInfo)
        {
            Config = config;
            Loggers = loggers;
            Provider = provider;
            EmailServices = emailServices;
            Alert = alert;
            LongRunningInfo = longRunningInfo;
            HttpResponseInfo = httpResponseInfo;
        }
        #endregion

        #region Main Calls
        #region Generic
        public virtual async Task Entry(BaseLogInformation info, Action errorAction = null)
        {
            if (!EnabledPreCheck(info))
                return;
            await DoLog(info, errorAction);
        }

        public virtual async Task Exit(TimerBaseInformation info, Action errorAction = null) => await DoUpdate(info, errorAction);
        #endregion

        #region Misc
        /// <inheritdoc />
        public virtual async Task Log(TraceEventType severity, string message, string category = "")
        {
            var info = ServiceLocator.Get<MessageInformation>(Provider);
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(severity, message, category);
            await DoLog(info);
        }

        public virtual async Task Unauthorized(string message)
        {
            var info = ServiceLocator.Get<UnauthorizedInformation>(Provider);
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(message);
            await DoLog(info);
        }
        #endregion

        #region Timers
        public virtual async Task<TimerInformation> TimerStart(string name)
        {
            var info = ServiceLocator.Get<TimerInformation>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(name);
            await DoLog(info);
            return info;
        }
        public virtual async Task TimerEnd(TimerInformation info) => await DoUpdate(info);
        #endregion

        #region Custom
        public virtual async Task Custom(TraceEventType severity, string type, string message, object o = null)
        {
            var info = ServiceLocator.Get<CustomInformation>(Provider);
            info.SetType(type);
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(severity, type, message, o);
            await DoLog(info);
        }
        public virtual async Task Custom1(string message, object o = null)
        {
            var info = ServiceLocator.Get<CustomInformation1>(Provider);
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(message, o);
            await DoLog(info);
        }
        public virtual async Task Custom2(string message, object o = null)
        {
            var info = ServiceLocator.Get<CustomInformation2>(Provider);
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(message, o);
            await DoLog(info);
        }
        public virtual async Task Custom3(string message, object o = null)
        {
            var info = ServiceLocator.Get<CustomInformation3>(Provider);
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(message, o);
            await DoLog(info);
        }

        public virtual async Task<CustomTimerInformation> CustomTimerStart(TraceEventType severity, string type, string name, object o = null)
        {
            var info = ServiceLocator.Get<CustomTimerInformation>(Provider);
            info.SetType(type);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(severity, name, o);
            await DoLog(info);
            return info;
        }
        public virtual async Task<CustomTimerInformation1> CustomTimer1Start(string name, object o = null)
        {
            var info = ServiceLocator.Get<CustomTimerInformation1>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(name, o);
            await DoLog(info);
            return info;
        }
        public virtual async Task<CustomTimerInformation2> CustomTimer2Start(string name, object o = null)
        {
            var info = ServiceLocator.Get<CustomTimerInformation2>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(name, o);
            await DoLog(info);
            return info;
        }
        public virtual async Task<CustomTimerInformation3> CustomTimer3Start(string name, object o = null)
        {
            var info = ServiceLocator.Get<CustomTimerInformation3>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(name, o);
            await DoLog(info);
            return info;
        }

        public virtual async Task CustomTimerEnd(CustomTimerInformation info, object o = null)
        {
            if (o != null && info != null)
                info.MiscEnd = o;
            await DoUpdate(info);
        }
        #endregion

        #region Attributes
        public virtual async Task<AttributeInformation> AttributeEntry(string attributeName, Dictionary<string, string> parameters)
        {
            var info = ServiceLocator.Get<AttributeInformation>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(attributeName, parameters);
            await DoLog(info);
            return info;
        }
        public virtual async Task AttributeExit(AttributeInformation info) => await DoUpdate(info);
        #endregion

        #region Api
        public virtual async Task<ApiInformation> ApiEntry()
        {
            var info = ServiceLocator.Get<ApiInformation>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            await info.Initialize(Config);
            await DoLog(info);
            return info;
        }
        public virtual async Task ApiExit(ApiInformation info, int statusCode, string result)
        {
            info?.SaveResult(statusCode, result);
            await DoUpdate(info);
        }

        public virtual async Task ApiExit(ApiInformation info, HttpResponse response)
        {
            if (info == null)
                return;

            await info.SaveResponse(response);
            await DoUpdate(info);
        }
        #endregion

        #region Database
        public virtual async Task<DatabaseInformation> DatabaseEntry(string cnnName, string command, object args = null)
        {
            var info = ServiceLocator.Get<DatabaseInformation>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(cnnName, command, args);
            await DoLog(info);
            return info;
        }

        public virtual async Task DatabaseExit(DatabaseInformation info) => await DoUpdate(info);
        #endregion

        #region Web Services
        public virtual async Task<WebServiceInformation> WebServiceEntry(HttpHeaders headers, string route, string url, string method, object data = null)
        {
            // Always do initialization even if not logging since the Order is needed
            var info = ServiceLocator.Get<WebServiceInformation>(Provider);
            info.Initialize(route, url, method, data);

            // Set the order in the request headers
            var httpSettings = ServiceLocator.Get<IHttpClientSettings>(Provider);
            headers.Add(httpSettings.LogOrderHeader, info.Order);

            if (!EnabledPreCheck(info))
                return null;

            await DoLog(info);
            return info;
        }

        public virtual async Task WebServiceExit(WebServiceInformation info, int statusCode, string result)
        {
            info?.SaveResult(statusCode, result);
            await DoUpdate(info);
        }
        public virtual async Task WebServiceExit(WebServiceInformation info, HttpResponseMessage response)
        {
            if (info == null)
                return;

            await info.SaveResponse(response);
            await DoUpdate(info);
        }
        #endregion

        #region Middleware
        public virtual async Task<MiddlewareInformation> MiddlewareEntry(string name)
        {
            var info = ServiceLocator.Get<MiddlewareInformation>(Provider);
            if (!EnabledPreCheck(info))
                return null;

            info.Initialize(name);
            await DoLog(info);
            return info;
        }

        public virtual async Task MiddlewareExit(MiddlewareInformation info) => await DoUpdate(info);
        #endregion

        #region Hidden Exceptions
        public virtual async Task HiddenException(Exception e, bool messageOnly) => await HiddenException(e, "", messageOnly);
        public virtual async Task HiddenException(Exception e, string title = "", bool messageOnly = true)
        {
            var info = ServiceLocator.Get<ExceptionInformation>(Provider);
            info.SetHidden();
            if (!EnabledPreCheck(info))
                return;

            info.Initialize(e, title, messageOnly);
            await DoLog(info);
        }
        #endregion

        #region Exceptions
        public virtual async Task<Guid> Exception(Exception e, bool messageOnly) => await Exception(e, "", messageOnly);
        public virtual async Task<Guid> Exception(Exception e, string title = "", bool messageOnly = false)
        {
            var info = ServiceLocator.Get<ExceptionInformation>(Provider);
            if (!EnabledPreCheck(info))
                return info.Identifier;

            info.Initialize(e, title, messageOnly);
            var ex = e;

            await DoLog(info, () =>
            {
                // We were unable to record the exception
                // Eg. "SetConfiguration" failed to make the call or get the info
                // Send out the real exception that occurred
                var subject = $"{GetEmailSubjectPrefix(info)}Exception Recording Failed: {ex.GetFullMessage()}";
                var body = $@"Source: {ex.Source ?? "unknown"}
Message:
{ex.GetFullMessage()}

Stack Trace:
  {ex.GetStackTrace()}
";
#pragma warning disable 4014
                SendEmail(subject, body);
#pragma warning restore 4014
            });

            return info.Identifier;
        }
        #endregion
        #endregion

        #region Private Worker Calls

        private bool EnabledPreCheck(BaseLogInformation info)
            => info != null && Config.Enabled && Config.TypeEnabled(info.Type);

        private string GetEmailSubjectPrefix(BaseLogInformation info)
        {
            return $"{System.Environment.MachineName} ({EmailServices.WebHost.EnvironmentName}): {info.Application}; ";
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private async Task<bool> SendEmail(string subject, string body)
            => await Email.SendAsync(EmailServices, EmailType.Exception, subject, body);

        #region Inserts

        protected Task DoLog(BaseLogInformation info, Action errorAction = null)
        {
            var t = Task.CompletedTask;
            t.ContinueWith(
                task =>
                    SafeTry.EmailException(
                        EmailServices,
                        async () =>
                        {
                            // Set properties on the object (ones that aren't set on the constructor that may take a bit)
                            info.SetProperties(Config);

                            // Log it (Insert)
                            await DoAllLoggerInserts(info, errorAction);

                            // Waited for complete above so that this can be set (free it up for updates)
                            if (info is TimerBaseInformation timer)
                                timer.SetComplete = true;
                        }
                    )
            );
            return t;
        }

        private async Task DoAllLoggerInserts(BaseLogInformation info, Action errorAction = null)
            => await Task.WhenAll(Loggers.All
                .Where(logger => Config.LoggerEnabled(logger.Name, info.Type))
                .Select(logger => DoLoggerInsert(logger, info, errorAction)));

        private Task DoLoggerInsert(BaseLogger logger, BaseLogInformation info, Action errorAction)
            => SafeTry.IgnoreException(
                async () =>
                {
                    var done = await SafeTry.EmailException(
                        EmailServices,
                        async () => await logger.Insert(info, Config.MaxLength(logger.Name))
                    );
                    if (!done)
                        errorAction?.Invoke();
                });

        #endregion

        #region Updates
        protected Task DoUpdate(TimerBaseInformation info, Action errorAction = null)
        {
            var t = Task.CompletedTask;
            if (info == null)
                return t;

            info.Stop();

            t.ContinueWith(
                task => SafeTry.EmailException(
                    EmailServices,
                    async () =>
                    {
                        // Only proceed when the initial logging call has completed as everything below requires information in the set call to be present/done
                        for (var i = 0; i < 600; i++) // 1 minute max wait
                        {
                            if (info.SetComplete)
                                break;
                            await Task.Delay(100);
                        }

                        // If this failed, email out (all we can do, kinda)
                        if (!info.SetComplete)
                        {
                            var subject = $"{GetEmailSubjectPrefix(info)}Unable to Record Logging Update";
                            var body = $@"{info.IdentifierStr}
{info.GetElapsedStr()}";
#pragma warning disable 4014
                            SendEmail(subject, body);
#pragma warning restore 4014
                            return;
                        }

                        // Set update properties now that may take a while
                        info.UpdateProperties(Config);

                        // Long running determination
                        if (info.LongRunningName != null)
                        {
                            // Enabled?
                            if (Config.TypeEnabled(LongRunningInformation.StaticType))
                            {
                                // Check
                                var maxMilliseconds = Alert.MaxMilliseconds(info.LongRunningName);
                                if (info.IsLongRunning(maxMilliseconds) ?? false)
                                {
                                    LongRunningInfo.Initialize(info);
                                    LongRunningInfo.SetProperties(Config);
                                    await DoAllLoggerInserts(LongRunningInfo);
                                }
                            }
                        }

                        // HTTP response determination
                        var responseInfo = info.GetResponseInfo();
                        if (responseInfo != null)
                        {
                            // Enabled?
                            if (Config.TypeEnabled(HttpResponseInformation.StaticType))
                            {
                                // Check
                                if (Alert.HttpResponseAlert(responseInfo))
                                {
                                    var result = info.GetResponse(Config.HideKeys());
                                    HttpResponseInfo.Initialize(info, responseInfo.Type, responseInfo.HttpMethod,
                                        responseInfo.HttpCode, result);
                                    HttpResponseInfo.SetProperties(Config);
                                    await DoAllLoggerInserts(HttpResponseInfo);
                                }
                            }
                        }

                        // Log it (Update)
                        await Task.WhenAll(Loggers.All
                            .Where(logger => Config.LoggerEnabled(logger.Name, info.Type))
                            .Select(logger => DoLoggerUpdate(logger, info, errorAction)));
                    }
                )
            );
            return t;
        }

        private Task DoLoggerUpdate(BaseLogger logger, TimerBaseInformation info, Action errorAction)
            => SafeTry.IgnoreException(async () =>
            {
                var done = await SafeTry.EmailException(
                    EmailServices,
                    async () => await logger.Update(info, Config.MaxLength(logger.Name))
                );
                if (!done)
                    errorAction?.Invoke();
            });

        #endregion

        #endregion
    }
}
