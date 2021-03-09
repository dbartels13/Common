# Settings {#SettingsMd}

## Overview {#SettingsOverviewMd}
A [Setting](@ref Sphyrnidae.Common.Lookup) is a certain type of object that has the following properties:
1. Inherits from [ILookupSettings](@ref Sphyrnidae.Common.Lookup.ILookupSettings)
2. Contains a collection of objects that inherit from [LookupSetting](@ref Sphyrnidae.Common.Lookup.LookupSetting)
3. Retrieval of these objects (and settings for caching, etc) are implemented by [ILookupServices](@ref Sphyrnidae.Common.Lookup.ILookupServices)

The implementation of a setting requires you to implement the following:
1. Implementation of [LookupSetting](@ref Sphyrnidae.Common.Lookup.LookupSetting) - can be extended
2. Custom interface inheriting from [ILookupSettings](@ref Sphyrnidae.Common.Lookup.ILookupSettings)
3. Implementation of your custom interface (inherited from [ILookupSettings](@ref Sphyrnidae.Common.Lookup.ILookupSettings) and [BaseLookupSetting](@ref Sphyrnidae.Common.Lookup.BaseLookupSetting))
4. Custom interface inheriting from [ILookupServices](@ref Sphyrnidae.Common.Lookup.ILookupServices)
5. Implementation of your custom interface (inherited from [ILookupServices](@ref Sphyrnidae.Common.Lookup.ILookupServices))
6. Static method implementation class that inherits from [SettingsLookup](@ref Sphyrnidae.Common.FeatureToggle.SettingsLookup)
7. Register your interface implementations (startup.cs)

Yes, this might seem like a lot of setup to essentially have a Dictionary you can query.
However, this can be configured and extended to handle any scenario you can imagine,
and uses the best available design pattern to do so without much custom coding.

Why this is preferable:
1. Thread-safe: If you make this call multiple times before the data has been successfully looked up, the raw lookup of the data will occur only once
2. Things like User Preferences will be a user-specific listing
3. Caching is automatically a part of this
4. Allows you to map the data from any data source (eg. web service call to retrieve data in your [ILookupSettings](@ref Sphyrnidae.Common.Lookup.ILookupSettings) implementation)
5. Fast query times using [CaseInsensitiveBinaryList](@ref Sphyrnidae.Common.CaseInsensitiveBinaryList)
6. Fault-tolerant: If a lookup fails, you can specify a default value
7. End result is a simple static method that will, almost magically, return a value for the given key

## Where Used {#SettingsWhereUsedMd}
1. [Variables](@ref VariableMd)
2. [User Preferences](@ref UserPreferencesMd)
3. [Feature Toggle](@ref FeatureToggleMd)

## Examples {#SettingsExampleMd}
<pre>
	// Create your object type by inheriting from LookupSetting and extending for additional properties
    public class Widget : LookupSetting { }

	// Create a custom interface derived by ILookupSettings
    public interface IWidgetSettings : ILookupSettings<Widget> { }

	// Implement your custom interface derived from ILookupSettings and BaseLookupSetting
    // Can optionally override virtual methods for additional behavior
    public class WidgetSettings : BaseLookupSetting<Widget>, IWidgetSettings
    {
        public override string Key => "Widget";
        public override int CachingSeconds => 600;
        public override Task<IEnumerable<Widget>> GetAll()
        {
            var widgets = new List<Widget>
            {
                new Widget { Key = "widget1", Value = "foo1" },
                new Widget { Key = "widget2", Value = "foo2" },
                new Widget { Key = "widget3", Value = "foo3" }
            };
            return Task.FromResult(widgets.AsEnumerable());
        }
    }

	// Create a custom interface derived by ILookupServices
    public interface IWidgetServices : ILookupServices<IWidgetSettings, Widget> { }

	// Implement your custom interface derived from IWidgetServices
    public class WidgetServices : IWidgetServices
    {
        public ICache Cache { get; }
        public IWidgetSettings Service { get; }
        public WidgetServices(ICache cache, IWidgetSettings service)
        {
            Cache = cache;
            Service = service;
        }
    }

    // Static method implementation class that inherits from SettingsLookup
    // Can place any additional methods here for different lookups
    public class SettingsWidget : SettingsLookup<IWidgetSettings, Widget> { }

    // Register your implementations in startup.cs
    services.TryAddTransient<IWidgetSettings, WidgetSettings>();
    services.TryAddTransient<IWidgetServices, WidgetServices>();

    // Test everything out
    IWidgetServices service; // Should be injected
    var val = SettingsWidget.Get(service, "widget1", "default value"); // foo1
    val = SettingsWidget.Get(service, "widget18", "default value"); // default value
</pre> 
