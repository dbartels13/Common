# Alerts {#AlertsMd}

## Overview {#AlertsOverviewMd}
An alert is a piece of functionality that is built into the default [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger): [Logger](@ref Sphyrnidae.Common.Logging.Logger).
If you are not using the default logger, than it will be up to you to generate alerts in the system.

An alert will be triggered whenever:
1. Something takes too long - MaxMilliseconds()
2. An invalid HTTP response comes back (either from your own API, or from a WebService call) - HttpResponseAlert()

Using the default [Logger](@Sphyrnidae.Common.Logging.Logger), you should be sure to properly configure how Alerts will be handled in the system (eg. Do you log them, send e-mails, etc?).
This is done via the [LoggerConfiguration](@Sphyrnidae.Common.Logging.LoggerConfiguration)

Interface: [IAlert](@ref Sphyrnidae.Common.Alerts.IAlert)

Mock: [AlertNone](@ref Sphyrnidae.Common.Alerts.AlertNone)

Default Implementation: [Alert](@ref Sphyrnidae.Common.Alerts.Alert)

## Where Used {#AlertsWhereUsedMd}
1. [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger)

## Examples {#AlertsExampleMd}
<pre>
using Sphyrnidae.Common.Logging.Models;
namespace MyApplication.Alerts
{
    public class MyAlert : IAlert
    {
        public virtual long MaxMilliseconds(string name)
            => name switch
            {
                "API-MyApplicationName-GET-/widgets" => 100,
                "Database-MyApplicationName-RepoConnectionName-StoredProcedure" => 200,
                "WebService-MyApplicationName-POST-NameOfWebServiceEndpoint" => 300,
                _ => 0,
            };

        public virtual bool HttpResponseAlert(HttpResponseInfo responseInfo)
        {
            if (!responseInfo.HttpCode.HasValue || responseInfo.HttpCode.Value >= 200 || responseInfo.HttpCode.Value < 300)
                return false;
            if (responseInfo.Type == "API" && responseInfo.Route == "/widgets" && responseInfo.HttpMethod == "GET")
                return true;
            if (responseInfo.Type == "Web Service" && responseInfo.Route == "NameOfWebServiceEndpoint" && responseInfo.HttpMethod == "POST")
                return true;
            return false;
        }
    }
}
</pre>