# Caching {#CacheMd}

## Overview {#CacheOverviewMd}
Caching allows you to quickly retrieve saved data/objects instead of always having to lookup data.
Data/objects from cache are NOT real-time objects, so please be aware of this if you are using frequently-changing data.
Caching is most useful for slowly changing data (or best for static-data), as this avoids a typically to a database or other data source.

The [ICache](@ref Sphyrnidae.Common.Cache.ICache) interface has the following methods:
1. Get: Retrieves an item from cache via output variable (returns true/false on if it found the object)
2. Set: Saves an item into cache
3. Get<T>: If the item exists in cache, immediately returns that item. Otherwise, will execute your function to get the item, store it into cache, and then return the item.
4. GetAsync: Same as Get<T>, but is asynchronous allowing your function call to be asynchronous.

Interface: [ICache](@ref Sphyrnidae.Common.Cache.ICache)

Mock: [CacheNone](@ref Sphyrnidae.Common.Cache.CacheNone)

Implementations:
1. [CacheLocal](@ref Sphyrnidae.Common.Cache.CacheLocal): Uses local/memory cache only. If you have multiple web servers, they will each have their own cache (which could contain different objects based on when they were set with real-time data)
2. [CacheDistributed](@ref Sphyrnidae.Common.Cache.CacheDistributed): Distributed caching only (eg. <a href="https://redis.io/" target="blank">Redis</a>). All calls will go to an external caching system. Eg. Does have the overhead of a network call, but all servers share the exact same cache.
3. [CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed): This will use both local and distributed, based on the [Environmental](@ref EnvironmentMd) settings of Cache.Distributed and Cache:Local. If the setting is found locally, it will use that. Otherwise, it will use the distributed cache.

Wrapper: [Caching](@ref Sphyrnidae.Common.Cache.Caching) - currently contains no additional capability, you can generally just directly call the interface methods.

## Cache Invalidation {#CacheInvalidationMd}
If an object has been updated after it was placed in cache, the cached instance will no longer be accurate.
In some business scenarios, that is acceptable.
However, you should utilize the following techniques to help ensure the cached object is accurate:
1. Expiration: All of the implementations have an "Options" property. You can update the [Seconds](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Seconds) property on the Options object to specify how long this should be cached for (Default = 1200; 20 minutes). Note there are helper static properties on the [CacheOptions](@ref Sphyrnidae.Common.Cache.Models.CacheOptions) object (You can use these, or specify your own value): [Minute](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Minute), [Hour](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Hour), [Day](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Day), [Month](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Month), [Year](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Year)
2. [Priority](@ref Sphyrnidae.Common.Cache.Models.CacheOptions.Priority): If you are heavily caching, you could run into memory problems and items will be dropped from the cache. You should ensure your most-used, costliest, and/or static cache items have a higher priority than your least-used, cheap, and/or near real-time cache items.
3. Remove: This is a method on the [ICache](@ref Sphyrnidae.Common.Cache.ICache) interface which will remove the item from cache. This can be done whenever you know a cached object is no longer valid.

For the [CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed) implementation, the Remove method
will not only remove it from the local cache that happens to be running, but also distributed cache,
AND will send out a [SignalR](@ref SignalRMd) request to other servers to also clear their local cache.
The [API](@ref ApiMd) default setup for SignalR is to subscribe and handle these caching removal requests based on the [Environmental](@ref EnvironmentMd) variable: URL.Hub.Cache

## Where Used {#CacheWhereUsedMd}
<table>
    <tr>
        <th>Class
        <th>Key
        <th>Seconds
        <th>Description
    <tr>
        <td>[AuthenticationMiddleware](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware)
        <td>$"ApiAuth_{app.Name}_{application}_{token}"
        <td>30
        <td>Specifies if an application is authenticated for API<=>API communication
    <tr>
        <td>[EncryptionKeyManager](@ref Sphyrnidae.Common.Encryption.KeyManager.EncryptionKeyManager)
        <td>EncryptionKeys
        <td>Default
        <td>Retrieves information related to encryption keys
    <tr>
        <td>[IFeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleSettings)
        <td>FeatureToggleSettings
        <td>600
        <td>Stores feature toggle settings for the application
    <tr>
        <td>[IUserPreferenceSettings](@ref Sphyrnidae.Common.UserPreference.Interfaces.IUserPreferenceSettings)
        <td>UserPreferenceSettings
        <td>1200
        <td>Stores user preferences for a given user
    <tr>
        <td>[IVariableSettings](@ref Sphyrnidae.Common.Variable.Interfaces.IVariableSettings)
        <td>VariableSettings
        <td>Minute
        <td>Stores variables/configurations for the application
    <tr>
        <td>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
        <td>Logging_Enabled_Types
        <td>1200
        <td>Which types of loggers are enabled
    <tr>
        <td>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
        <td>Logging_Includes
        <td>1200
        <td>Optional attributes to log for a given type
    <tr>
        <td>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
        <td>Logging_Enabled_Loggers
        <td>1200
        <td>Which loggers are enabled
    <tr>
        <td>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
        <td>$"Logging_Enabled_{name}_Types"
        <td>1200
        <td>For a given logger, what types are enabled for that logger
    <tr>
        <td>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
        <td>Logging_HideKeys
        <td>1200
        <td>Listing of logging keys where the values will be obfuscated in the log
</table>

## Examples {#CacheExampleMd}
<pre>
    // Injected: ICache cache
    const string Key = "MyKey";

    cache.Options.Seconds = CacheOptions.Day;
    cache.Options.Priority = CacheItemPriority.NeverRemove;
    if (!cache.Get(Key, out string foo))
        cache.Set(Key, "MyValue");

    cache.Options.Seconds = CacheOptions.Hour;
    cache.Options.Priority = CacheItemPriority.Low;
    foo = cache.Get(Key, () => "MyValue");

    foo = await Caching.GetAsync(cache, key, async () => await MyAsyncMethod());

    var ex = Caching.Remove(cache, key);
</pre>