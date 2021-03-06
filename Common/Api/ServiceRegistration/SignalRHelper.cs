﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.SignalR;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class SignalRHelper
    {
        public static void Logger(IServiceProvider sp)
        {
            var email = ServiceLocator.Get<IEmail>(sp);
            var app = ServiceLocator.Get<IApplicationSettings>(sp);
            var logger = ServiceLocator.Get<ILogger>(sp);
            // Not at all sure how to get this to log out issues... this happens outside the scope of HttpRequest, meaning that the logger will fail :(
            SignalR.SignalR.SetLogger(
                async msg => await SafeTry.EmailException(
                    email,
                    app,
                    () => logger.Log(TraceEventType.Suspend, msg, "SignalR")
                ),
                async ex => await SafeTry.EmailException(
                    email,
                    app,
                    () => logger.Exception(ex)
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
            _ = SafeTry.LogException(logger, async () => await SignalRHub.Receive<string>(signalR, url, "Invalidate Cache", memory.Remove));
        }
    }
}