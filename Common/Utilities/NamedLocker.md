# Named Locker {#NamedLockerMd}

## Overview {#NamedLockerOverviewMd}
The "lock" concept in c# is a way to ensure concurrency concerns are met when it comes to a critical section.
Eg. Only 1 thread is allowed into this locked area at a time.
There are caveats to this rule - such as if the locked area makes a call which ultimately makes it back to the locked area, it will be allowed in.
The problem with the standard method of locking is that it's all-or-nothing.
However, there are many situations where you might want to allow lots of different threads into the critical section, as long as they are doing different things.

For example: [Variable](@ref VariableMd) lookups.
There could be multiple threads all trying to get at the configuration set.
You don't want to have all of these threads all making calls to the database, so you place a lock around this call to ensure that only 1 database lookup happens.
All the other threads will then be able to use the same set of data (cached) simply by waiting for the first call to succeed.
Now let's assume that you are looking up your configuration set that is client/customer specific.
In this case, if you have multiple client/customers, each one of those will be allowed into the critical section to perform their lookup of variable/configuration data.
However, if you have multiple threads that are trying to get at the exact same data (eg. for the same client/customer),
then those can be blocked using the same "name".

The [NamedLocker](@ref Sphyrnidae.Common.Utilities.NamedLocker) class is what facilitates this ability.
It essentially has a Lock() method which takes the "name" to lock upon, and a method that will be locked on that name.

## Where Used {#NamedLockerWhereUsedMd}
1. [FileLogger](@ref Sphyrnidae.Common.Logging.Loggers.FileLogger) (see [Loggers](@ref LoggingLoggersMd)): Actual writing to a file is locked on the full filename (allows multiple files to all be written at once)
2. [Caching](@ref Sphyrnidae.Common.Cache.Caching) (see [Cache](@ref CacheMd)): The Get() method that optionally executes a method if not found and places the result in the cache... this utilizes the NamedLocker based on the CacheKey
3. [LoggingOrder](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.LoggingOrder): Setting of the logging order will be locked based on the [RequestId](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.RequestId)
4. [Settings](@ref SettingsMd): Retrieval of the collection of data/settings will be locked on the [Key](@ref Sphyrnidae.Common.Lookup.ILookupSettings.Key)

## Examples {#NamedLockerExampleMd}
<pre>
	var result = NamedLocker.Lock(customerId.ToString(), num => {
		// Do something that is customer-specific lookup
		return "my customer information";
	});
</pre>