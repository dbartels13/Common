# Overview
This Nuget package contains numerous helper methods and common functionality useful in any application.
To install, you can search Nuget for <a href="https://www.nuget.org/packages/Sphyrnidae.Common" target="blank">Sphyrnidae.Common</a> - (Gnu General Public License).
The source code is open-sourced, and is located in GitHub: https://github.com/dbartels13/Common
This documentation will guide you through all of the utilities available in this package.

## Quick Start {#OverviewQuickStartMd}
[API Setup](@ref SetupMd): How to setup your API project

## Authentication {#OverviewAuthenticationMd}
To learn about the Authentication and Authorization (JWT-based) go [here](@ref AuthenticationMd)

## Utility class types
All of the functionality in this library can be broken down into the following categories:
<ol>
	<li>[Stand-Alone Helper Methods](@ref OverviewHelperMethodsMd)
	<li>[Interface Driven Functionality](@ref OverviewInterfaceMethodsMd)
	<li>[Base Classes](@ref OverviewBaseClassesMd)
	<li>[Api-Specific Behavior](@ref ApiMd)

## Helper Methods {#OverviewHelperMethodsMd}
These methods can be directly called without any interface registration.
As such, all customization in the behavior of these methods is done via method overloads.
All calls to uses these classes/methods are either extension methods, or at static in nature.

[Active Directory](@ref Sphyrnidae.Common.ActiveDirectoryUtilities): Methods for accessing active directory (not currently supported)

[BinaryList](@ref BinaryListMd): Fast searching of a list

[Dynamic Sql](@ref DynamicSqlMd): Helper for building a complex sql statement

[Extension Methods](@ref Sphyrnidae.Common.Extensions): All kinds of extension methods

## Interface-Based Functionality {#OverviewInterfaceMethodsMd}
These pieces of functionality are abstracted into an interface.
From there, a default implementation is usually provided.
A mock implementation is also provided.
If the default implementation is abstract in nature, there may be a child class which does the rest of the implementation.
Lastly, there could be a wrapper class around the interface.
This wrapper class is sometimes done for ease-of-use,
but is also done if there is specific functionality that should be done for every possible implementation
(eg. it wraps the interface call in some helper code)
<table>
	<tr>
		<th>Functionality
		<th>Interface
		<th>Mock
		<th>Implementation
		<th>Wrapper Class
		<th>Notes
	<tr>
		<td>[Alerts](@ref AlertsMd)
		<td>[IAlert](@ref Sphyrnidae.Common.Alerts.IAlert)
		<td>[AlertNone](@ref Sphyrnidae.Common.Alerts.AlertNone)
		<td>[Alert](@ref Sphyrnidae.Common.Alerts.Alert)
		<td>
		<td>
	<tr>
		<td>[API Responses](@ref ApiResponseMd)
		<td>[IApiResponse](@ref Sphyrnidae.Common.Api.Responses.IApiResponse)
		<td>[ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard)
		<td>[ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard)
		<td>
		<td>
	<tr>
		<td>[Application Settings](@ref ApplicationMd)
		<td>[IApplicationSettings](@ref Sphyrnidae.Common.Application.IApplicationSettings)
		<td>[ApplicationSettingsMock](@ref Sphyrnidae.Common.Application.ApplicationSettingsMock)
		<td>None
		<td>
		<td>You must override in your own project
	<tr>
		<td>[Caching](@ref CacheMd)
		<td>[ICache](@ref Sphyrnidae.Common.Cache.ICache)
		<td>[CacheNone](@ref Sphyrnidae.Common.Cache.CacheNone)
		<td>[CacheLocal](@ref Sphyrnidae.Common.Cache.CacheLocal)
		<br />[CacheDistributed](@ref Sphyrnidae.Common.Cache.CacheDistributed)
		<br />[CacheLocalAndDistributed](@ref Sphyrnidae.Common.Cache.CacheLocalAndDistributed) (Default)
		<td>[Caching](@ref Sphyrnidae.Common.Cache.Caching)
		<td>
	<tr>
		<td>[Email](@ref EmailMd)
		<td>[IEmail](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmail)
		<td>[EmailMock](@ref Sphyrnidae.Common.EmailUtilities.EmailMock)
		<td>[DotNetEmail](@ref Sphyrnidae.Common.EmailUtilities.DotNetEmail)
		<td>[Email](@ref Sphyrnidae.Common.EmailUtilities.Email)
		<td>[EmailBase](@ref Sphyrnidae.Common.EmailUtilities.EmailBase): Content generation
	<tr>
		<td>[Encryption](@ref EncryptionMd)
		<td>[IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption)
		<td>[EncryptionNone](@ref Sphyrnidae.Common.Encryption.EncryptionNone)
		<td>[EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher) (Default)
		<br />[EncryptionStrong](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionStrong)
		<br />[EncryptionWeak](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionWeak)
		<br />[EncryptionNormal](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionNormal)
		<br />[EncryptionOld](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionOld)
		<td>[EncryptionExtensions](@ref Sphyrnidae.Common.Encryption.EncryptionExtensions)
		<td>Wrapper class is a string extension method
	<tr>
		<td>[Environmental Settings](@ref EnvironmentMd)
		<td>[IEnvironmentSettings](@ref Sphyrnidae.Common.Environment.IEnvironmentSettings)
		<td>[EnvironmentalSettingsMock](@ref Sphyrnidae.Common.Environment.EnvironmentalSettingsMock)
		<td>[EnvironmentalSettings](@ref Sphyrnidae.Common.Environment.EnvironmentalSettings)
		<td>[SettingsEnvironmental](@ref Sphyrnidae.Common.Environment.SettingsEnvironmental)
		<td>
	<tr>
		<td>[Feature Toggles](@ref FeatureToggleMd)
		<td>[IFeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleSettings)
		<td>[FeatureToggleSettingsDefault](@ref Sphyrnidae.Common.FeatureToggle.FeatureToggleSettingsDefault)
		<td>[FeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.FeatureToggleSettings)
		<td>[SettingsFeatureToggle](@ref Sphyrnidae.Common.FeatureToggle.SettingsFeatureToggle)
		<td>The implementation is an abstract class, so you should provide your own implementation
	<tr>
		<td>[Http Client](@ref HttpClientMd)
		<td>[IHttpClientSettings](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings)
		<td>[HttpClientSettingsMock](@ref Sphyrnidae.Common.HttpClient.HttpClientSettingsMock)
		<td>[HttpClientSettings](@ref Sphyrnidae.Common.HttpClient.HttpClientSettings)
		<td>
		<td>
	<tr>
		<td>[Http Data](@ref HttpDataMd)
		<td>[IHttpData](@ref Sphyrnidae.Common.HttpData.IHttpData)
		<td>[HttpDataMock](@ref Sphyrnidae.Common.HttpData.HttpDataMock)
		<td>[HttpData](@ref Sphyrnidae.Common.HttpData.HttpData)
		<td>
		<td>
	<tr>
		<td>[Identity / Authentication](@ref AuthenticationMd)
		<td>[IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper)
		<td>[BasicIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BasicIdentity)
		<td>[BasicIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BasicIdentity)
		<td>
		<td>
	<tr>
		<td>[Logging](@ref LoggingMd)
		<td>[ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger)
		<td>[NonLogger](@ref Sphyrnidae.Common.Logging.NonLogger)
		<td>[Logger](@ref Sphyrnidae.Common.Logging.Logger)
		<td>
		<td>
	<tr>
		<td>
		<td>
		<td>
		<td>
		<td>
		<td>
	<tr>
		<td>
		<td>
		<td>
		<td>
		<td>
		<td>
	<tr>
		<td>
		<td>
		<td>
		<td>
		<td>
		<td>
	<tr>
		<td>
		<td>
		<td>
		<td>
		<td>
		<td>
	<tr>
		<td>
		<td>
		<td>
		<td>
		<td>
		<td>
</table>


## Base Classes {#OverviewBaseClassesMd}
The following are all base classes available for your consumption.
These should always be inherited from because they provide some automatic "wiring" up of things like logging.

It's important to know the naming conventions used for designing your API's.
This basically assumes that you have a layered architecture with each layer having a defined responsibility and inputs/outputs.
Here are the layers that are portrayed by these base classes:
<ol>
	<li>[BaseApi](@ref Sphyrnidae.Common.Api.BaseClasses.BaseApi) [See [Api](@ref ApiMd) for more information): All controllers should inherit from this class.
	A controller is solely responsible for retrieval of request objects and populating the response.
	A controller may be versioned if the request/response structure changes for an endpoint.
	Any business logic should go in the "Engine" layer.
	<li>[BaseEngine](@ref Sphyrnidae.Common.Api.BaseClasses.BaseEngine): All engines should inherit from this class.
	An engine is responsible for performing the main business logic for the API endpoint.
	There should be a 1:1 mapping between a controller action and an engine method (this could be M:1 with versioning).
	The engine will work with all the other layers to get data, and then perform the proper business logic to ultimately return a business object back to the controller's action.
	<li>[BaseRepo](@ref Sphyrnidae.Common.Dal.BaseRepo) [See [Data Access Layer](@ref DalMd) for more information]: All database repositoris should inherit from this class.
	A repo is responsible for CRUD operations against a repository (eg. database, file sytem, etc).
	It is best practice to usually have a repository class per database table.
	<li>[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase) [See [Web Services](@ref WebServiceMd) for more information]: All web services should inherit from this class.
	A web service is a call to another system (could be SOA, microservices, or an external system).
	Each system/API that you need to access should be it's own web service class.
	<li>Service layer (no base class): A service layer is a class that does one of the following things:
		<ol>
			<li>Wrapper around a repository class (eg. for caching)
			<li>Wrapper around a web service class (eg. for caching or parsing of the response)
			<li>Performs common business logic (eg. can be shared across multiple engines)
		</ol>
	<li>[EmailBase](@ref Sphyrnidae.Common.EmailUtilities.EmailBase): You can inherit from this class to define a system email.
	This e-mail will be sent to the current user (logged in Identity).
	You will only need to override the [Subject](@ref Sphyrnidae.Common.EmailUtilities.EmailBase.Subject) and [Content](@ref Sphyrnidae.Common.EmailUtilities.EmailBase.Content).
	You may also wish to have inherit and create your own base class which replaces the [Shell](@ref Sphyrnidae.Common.EmailUtilities.EmailBase.Shell)
	<li>[EncryptionAlgorithm](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionAlgorithm): Any algorithms you wish to register for [encryption](@ref EncryptionMd) should inherit from this class.
	This is only used when using the [EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher) implementation of the [IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption) interface.
	Alternatively, if you develop your own implementation and wish to use this same logic, you could reuse this base class.
</ol>
