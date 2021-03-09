# Feature Toggles {#FeatureToggleMd}

## Overview {#FeatureToggleOverviewMd}
A feature toggle is a [setting](#ref SettingsMd) which is a collection of features, and the state of those features.
This allows you to develop code, have it be tested and released, but not actually enabled.
This could also be used to enable custom features for a customer based on what they have paid for.

It will be up to your development practices, but you may wish to make every feature you develop condition.
There is overhead in doing this, but this will give you the most robust system possible.
Perhaps you can "retire" some feature toggle checks in the future and always have a certain feature enabled.

To understand how this is designed and how you can customize this solution, please refer to [setting](#ref SettingsMd).

Interfaces:
1. [IFeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleSettings)
2. [IFeatureToggleServices](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleServices)

Implementations:
1. [FeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.FeatureToggleSettings): This is an abstract class. Default inherited class is [FeatureToggleSettingsDefault](@ref Sphyrnidae.Common.FeatureToggle.FeatureToggleSettingsDefault). You may wish to inherit and implement
2. [FeatureToggleServices](@ref Sphyrnidae.Common.FeatureToggle.FeatureToggleServices)

Other:
1. You can view an alternative implementation which actually uses a webservice to lookup these settings: [SphyrnidaeFeatureToggleSettings](@ref Sphyrnidae.Settings.SphyrnidaeFeatureToggleSettings). Note this implementation gathers customer-specific features and will email out any exceptions that occurred
2. [SettingsFeatureToggle](@ref Sphyrnidae.Common.FeatureToggle.SettingsFeatureToggle): Static class which you should use to gather any feature toggles.

## Where Used {#FeatureToggleWhereUsedMd}
None

## Examples {#FeatureToggleExampleMd}
Please reference [Settings Example](@ref SettingsExampleMd) for a detailed review of all customizations.
