# Environmental Settings {#EnvironmentMd}

## Overview {#EnvironmentOverviewMd}
Environmental settings are settings/configurations that are specific to an environment.
Typical environments are:
1. Dev
2. QA
3. Production

Any settings that will be unique to a certain environment should be stored as an environmental variable.
Examples:
1. URL's to other services/applications in that environment
2. [Caching](@ref CacheMd) settings
3. Connection strings

.Net Core has the <a href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0" target="blank">IConfiguration</a> interface.
The [IEnvironmentSettings](@ref Sphyrnidae.Common.Environment.IEnvironmentSettings) is the basis for this.
In fact the [implementation](@ref Sphyrnidae.Common.Environment.EnvironmentalSettings) merely calls this interface.
So if you'd prefer to just use the IConfiguration interface, then that is perfectly fine.

Interface: [IEnvironmentSettings](@ref Sphyrnidae.Common.Environment.IEnvironmentSettings)

Mock: [EnvironmentalSettingsMock](@ref Sphyrnidae.Common.Environment.EnvironmentalSettingsMock)

Implementation: [EnvironmentalSettings](@ref Sphyrnidae.Common.Environment.EnvironmentalSettings)

## Where Used {#EnvironmentWhereUsedMd}
<table>
    <tr>
        <th>Class
        <th>Name of Configuration
        <th>Default Value
        <th>Description
    <tr>
        <td>[IpAddress](@ref Sphyrnidae.Common.HttpClient.HttpClientSettings.IpAddress)
        <td>ASPNETCORE_ENVIRONMENT
        <td>
        <td>If "localhost", then use 127.0.0.1
    <tr>
        <td>[CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed)
        <td>Cache:Distributed
        <td>true
        <td>Purely for this caching implementation, specifies if distributed caching is enabled
    <tr>
        <td>[CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed)
        <td>Cache:Local
        <td>true
        <td>Purely for this caching implementation, specifies if local caching is enabled
    <tr>
        <td>[LogRepo.CnnStr](@ref Sphyrnidae.Common.Repos.LogRepo.CnnStr)
        <td>Cnn:Logging
        <td>
        <td>Encrypted connection string to the logging database
    <tr>
        <td>[EncryptionKeyManager](@ref Sphyrnidae.Common.Encryption.KeyManager.EncryptionKeyManager)
        <td>Encryption_Key
        <td>
        <td>current encryption key
    <tr>
        <td>[EncryptionKeyManager](@ref Sphyrnidae.Common.Encryption.KeyManager.EncryptionKeyManager)
        <td>Encryption_Key_Old
        <td>
        <td>Previous encryption key
    <tr>
        <td>[HttpDataMiddleware](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware)
        <td>Require_Https
        <td>false
        <td>If your application requires all requests be secure (HTTPS)
    <tr>
        <td>[ApiAuthenticationWebService.Url](@ref Sphyrnidae.Common.WebServices.ApiAuthenticationWebService)
        <td>URL:ApiAuthentication
        <td>
        <td>URL to the Authentication microservice (API<=>API)
    <tr>
        <td>[FeatureToggleWebService.Url](@ref Sphyrnidae.Common.WebServices.FeatureToggleWebService)
        <td>URL:FeatureToggle
        <td>
        <td>URL to the Feature Toggle microservice
    <tr>
        <td>[SignalR](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration.SignalRCacheInvalidation)
        <td>URL:Hub:Cache
        <td>
        <td>For [CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed), when you clear cache, this is the URL to SignalR which will then publish a clear cache message to other applications to clear their local cache
    <tr>
        <td>[CacheDistributed](@ref Sphyrnidae.Common.Cache.CacheDistributed) and [CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed)
        <td>URL:Redis
        <td>
        <td>Distributed caching URL
    <tr>
        <td>[UserPreferenceWebService.Url](@ref Sphyrnidae.Common.WebServices.UserPreferenceWebService)
        <td>URL:UserPreferences
        <td>
        <td>URL to the User Preferences microservice
    <tr>
        <td>[VariableWebService.Url](@ref Sphyrnidae.Common.WebServices.VariableWebService)
        <td>URL:Variable
        <td>
        <td>URL to the Variable microservice
    <tr>
        <td>[HealthCheck](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration.HealthCheckOptions)
        <td>version
        <td>Assembly.GetEntryAssembly()?.GetName().Version
        <td>For the health check endpoint (/hc) to list the current version of the application
</table>

## Examples {#EnvironmentExampleMd}
<pre>
    // Note that the nested structure in appsettings.json get converted to ":" at each level
    // &lt;a&gt;&lt;b&gt;&lt;c&gt;123&lt;/c&gt;&lt;/b&gt;&lt;/a&gt;
    IEnvironmentSettings env; // Should be injected
    SettingsEnvironmental.Get(env, "a:b:c", "default value"); // 123
</pre>