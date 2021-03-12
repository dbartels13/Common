# User Preferences {#UserPreferencesMd}

## Overview {#UserPreferenceOverviewMd}
A user preference is a [setting](#ref SettingsMd) which is a collection user-specific settings.
This allows each user in your system to have a unique batch of settings to customize your website to their desires.
For example, default number of rows to show in a particular grid.
Perhaps display properties, custom ordering, etc.

To understand how this is designed and how you can customize this solution, please refer to [setting](#ref SettingsMd).

Interfaces:
1. [IUserPreferenceSettings](@ref Sphyrnidae.Common.UserPreference.Interfaces.IUserPreferenceSettings)
2. [IUserPreferenceServices](@ref Sphyrnidae.Common.UserPreference.Interfaces.IUserPreferenceServices)

Implementations:
1. [UserPreferenceSettings](@ref Sphyrnidae.Common.UserPreference.UserPreferenceSettings): This is an abstract class. Default inherited class is [UserPreferenceSettingsDefault](@ref Sphyrnidae.Common.UserPreference.UserPreferenceSettingsDefault). You may wish to inherit and implement
2. [UserPreferenceServices](@ref Sphyrnidae.Common.UserPreference.UserPreferenceServices)

Other:
1. You can view an alternative implementation which actually uses a webservice to lookup these settings: <a href="https://github.com/dbartels13/Common/blob/main/SphyrnidaeSettings/UserPreference/SphyrnidaeUserPreferenceSettings.cs" target="blank">SphyrnidaeUserPreferenceSettings</a>. Note this implementation gathers customer-specific features and will email out any exceptions that occurred
2. [SettingsUserPreference](@ref Sphyrnidae.Common.UserPreference.SettingsUserPreference): Static class which you should use to gather any user preferences.

## Where Used {#UserPreferenceWhereUsedMd}
None

## Examples {#UserPreferenceExampleMd}
Please reference [Settings Example](@ref SettingsExampleMd) for a detailed review of all customizations.
