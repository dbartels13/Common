# Safe Try {#SafeTryMd}

## Overview {#SafeTryOverviewMd}
The [SafeTry](@ref Sphyrnidae.Common.Utilities.SafeTry) static class methods are basically syntatic replacements for the try/catch block.
The method that is passed to this call will be executed inside the 'try'.
Depending on the static method that is called, the 'catch' will do various things:

<table>
	<tr>
		<th>Method
		<th>Catch Handling
	<tr>
		<td>IgnoreException
		<td>Debug.WriteLine() will be called with the exception information
	<tr>
		<td>EmailException
		<td>An [Email](@ref EmailMd) of type [HiddenException](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.HiddenException) will be sent containing the exception information.
		Note that the generation of the subject, body, and sending of the Email will all have exceptions handled using IgnoreException
	<tr>
		<td>LogException
		<td>The exception will be sent to the [logger](@ref LoggingMd) as a [HiddenException](@ref LoggingStatementsMd)
	<tr>
		<td>OnException
		<td>User-supplied handling method
</table>

There are various overloads on all methods to work with basically any return type:
1. Action: returns boolean for success (eg. false if exception was thrown)
2. Func<T>: returns the object, or default value on exception
3. Func<Task>: Same as Action, but is awaitable
4. Func<Task<T>>: Safe as Func<T>, but is awaitable

## Performance Concerns {#SafeTryPerformanceMd}
The usage of this class introduces some overhead.
First, it takes what was in a single method, and puts that method as a sub-method.
So this adds another layer to the call stack.
Additionally, all of the state/binding must now occur.
Variables used by the internal method now must be "captured" and scope created.
If you are using the asynchronous methods (inner method returns a Task), then another layer of state management must be created.

All of this is fairly small in scope, but this could add up depending on the complexity of your application.
If you are seeing performance concerns related to this increased call stack,
you can always revert back to the standard try/catch notation.
You can, however, utilize the Exception handling method in your catch block.

## Where Used {#SafeTryWhereUsedMd}
1. IgnoreException:
	1. Get [Variable Error](@ref VariableMd)
	2. Getting [settings error](@ref SettingsMd)
	3. Distributed [Cache](@ref CacheMd) interaction
	4. [Logging](@ref LoggingMd) as a catch-all around EmailException handling
	5. [RegExAttribute](@ref Sphyrnidae.Common.Api.Attributes.RegExAttribute)
	6. [HealthCheckOptions](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration.HealthCheckOptions)
	7. [JWT](@ref AuthenticationMd) parsing
	8. [IpAddress](@ref Sphyrnidae.Common.Extensions.HttpContextExtensions)
	9. [NameValueCollectionExtensions](@ref Sphyrnidae.Common.Extensions.NameValueCollectionExtensions)
	10. Setting of [Logging information](@ref LoggingInformationMd)
	11. Getting most of [Request Data](@ref RequestDataMd) properties
	12. EmailException (on exception catch)
	13. Capturing [logging update information](@ref LoggingUpdateInfoMd)
2. EmailException:
	1. [SignalR](@ref SignalRMd) connection issues and logging
	2. [Logging](@ref LoggingMd) issues
	3. Get [User Preference Error](@ref UserPreferencesMd)
	4. Get [Feature Toggle Error](@ref FeatureToggleMd)
3. LogException:
	1. [SignalR](@ref SignalRMd) Cache Invalidation Receive Registration
	2. Create/Update [User Preference Error](@ref UserPreferencesMd)

## Examples {#SafeTryExampleMd}
<pre>
	// "Standard" way of doing things
	string val;
	try {
		// Do lots of stuff
		val = "value";
	}
	catch (Exception ex) {
		// Ignore, Email(), Log(), Other
		val = "error happened";
	}

	// Using SafeTry
	var val = SafeTry.IgnoreException(() => {
		// Do lots of stuff
		return "value";
	}, "error happened");
</pre>