using System;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.SignalR;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    internal class SignalRHelper
    {
        public static void Logger(IServiceProvider sp)
        {
            var emailServices = ServiceLocator.Get<IEmailServices>(sp);
            var logger = ServiceLocator.Get<ILogger>(sp);
            // Not at all sure how to get this to log out issues... this happens outside the scope of HttpRequest, meaning that the logger will fail :(
            SignalR.SignalR.SetLogger(
                async msg => await SafeTry.EmailException(
                    emailServices,
                    async () => await logger.Log(TraceEventType.Suspend, msg, "SignalR")
                ),
                async ex => await SafeTry.EmailException(
                    emailServices,
                    async () => await logger.Exception(ex)
                )
            );
        }

        public static void CacheInvalidation(IServiceProvider sp)
        {
            var env = ServiceLocator.Get<IEnvironmentSettings>(sp);
            var url = SettingsEnvironmental.Get(env, "URL:Hub:Cache");
            var logger = ServiceLocator.Get<ILogger>(sp);
            var signalR = ServiceLocator.Get<ISignalR>(sp);
            var memory = ServiceLocator.Get<IMemoryCache>(sp);
            SafeTry.LogException(logger, () => SignalRHub.Receive<string>(signalR, url, "Invalidate Cache", memory.Remove));
        }
    }
}