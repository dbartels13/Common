# Variables {#VariableMd}

## Overview {#VariableOverviewMd}
A variable is a [setting](#ref SettingsMd) which is commonly used for dynamic run-time configurations.
Eg. You can change a configuration at any time, and it should take effect immediately.
Depending on your value for [CachingSeconds](@ref Sphyrnidae.Common.Lookup.BaseLookupSetting.CachingSeconds), this will not be immediate,
but rather will take effect when the caching period has expired.
This does leave a potential gap in the application where 1 server might have the latest variables/configurations, while another server has stale configurations.

It is recommended that you do either:
1. Set the [CachingSeconds](@ref Sphyrnidae.Common.Lookup.BaseLookupSetting.CachingSeconds) to a period that you can live with (Default is 1 minute)
2. Setup a cache invalidation policy whenever these variables/configurations change. Eg. Send() a message to [SignalR](@ref SignalR) that the cache item has been removed. If you are using distributed caching, you can simply call the [Remove()](@ref Sphyrnidae.Common.Cache.ICache) method (this would be a best practice to do even for a single server)

To understand how this is designed and how you can customize this solution, please refer to [setting](#ref SettingsMd).

Interfaces:
1. [IVariableSettings](@ref Sphyrnidae.Common.Variable.Interfaces.IVariableSettings)
2. [IVariableServices](@ref Sphyrnidae.Common.Variable.Interfaces.IVariableServices)

Implementations:
1. [VariableSettings](@ref Sphyrnidae.Common.Variable.VariableSettings): This is an abstract class. Default inherited class is [VariableSettingsDefault](@ref Sphyrnidae.Common.Variable.VariableSettingsDefault). You may wish to inherit and implement
2. [VariableServices](@ref Sphyrnidae.Common.Variable.VariableServices)

Other:
1. You can view an alternative implementation which actually uses a webservice to lookup these settings: <a href="https://github.com/dbartels13/Common/blob/main/SphyrnidaeSettings/Variable/SphyrnidaeVariableSettings.cs" target="blank">SphyrnidaeVariableSettings</a>. Note this implementation gathers customer-specific features and will email out any exceptions that occurred
2. [SettingsVariable](@ref Sphyrnidae.Common.Variable.SettingsVariable): Static class which you should use to gather any variables.

## Where Used {#VariableWhereUsedMd}
None

## Examples {#VariableExampleMd}
Please reference [Settings Example](@ref SettingsExampleMd) for a detailed review of all customizations.
