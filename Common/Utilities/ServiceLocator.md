# Service Locator {#ServiceLocatorMd}

## Overview {#ServiceLocatorOverviewMd}
Service locator is considered by many to be an anti-pattern.
However, it still provides <a href="https://en.wikipedia.org/wiki/Inversion_of_control" target="blank">Inversion of Control (IoC)</a> - just not using direct <a href="https://en.wikipedia.org/wiki/Dependency_injection" target="blank">Dependency Injection (DI)</a>.
The benefit to Service Location is the same benefit as Dependency Injection - you can code off an abstraction, and the actual implementation of that abstraction will be supplied at run-time.
Therefore, you are not "dependent" on the actual implementation, but rather the abstraction of it.

Why this is considered an anti-pattern is because it does not allow you to easily see all the dependencies within the actual implementation.
I personally do not see the value of easily seeing the dependencies - the service container will take care of all of that for you.
As for testing, you will be working off of Mocks, so the actual implementation (and it's dependencies) is irrelevant.

Nevertheless, I would recommend using DI wherever possible.
However, I wouldn't make this an absolute requirement.
There are certain cases where I've come to appreciate the usefulness of the service locator pattern:
1. Static Methods
2. Many optional dependencies

## Static Methods {#ServiceLocatorStaticMd}
Imagine you have the following extension method:
<pre>
public static bool IsSomething(this string str, IFoo foo) => foo.IsSomething(str);
</pre>

As you can see, that extension method had a dependency on IFoo.
Because this is a static method, there is no class generated which will have all of the dependencies injected.
It will be up to the consumer of this extension method to inject IFoo, and pass along IFoo to this method.

Now imagine that the extension method logic needs to change.
It will now work against a different interface to pull the information it needs:
<pre>
public static bool IsSomething(this string str, IWidget widget) => widget.IsSomething(str);
</pre>

This is now a breaking change, and because you changed the implementation of your extension method, all consumers must update their usage.
This is where the service locator pattern could be useful.
The writer of the extension method could now update it to be as follows:
<pre>
public static bool IsSomething(this string str, IServiceProvider sp) {
	var myDependency = ServiceLocator.Get<IMyDependency>(sp);
	return myDependency.IsSomething(str);
}
</pre>

The consumer of this extension method will now be shielded from future internal changes to this method.
They need only inject into their code the IServiceProvider, and that is the sole dependency.

Caution: This could cause run-time exceptions, which is why this is considered bad practice by so many.
If you have not registered an implementation of IMyDependency, then this will return null and any usage will result in an "Object Reference Not Set..." exception.
However, if you know that the consumer will always have IMyDependency registered, then you are in the clear.
And I should note that even if you are using DI instead of Service Locator, the injection of an unregistewred IMyDependency will also cause a runtime exception.

## Multiple Dependencies {#ServiceLocatorMultipleMd}
The [Logger](@ref Sphyrnidae.Common.Logging.Logger) implementation of the [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger) interface is a great example of this issue.
The implementation has a number of well-defined dependencies, and those are all directly injected.
However, there are an even larger number of dependencies that are only used in certain situations.
Injecting this complete list is certainly an option, but newing up this many objects that will never be used, just feels like a waste of resources.
Instead, the IServiceProvider is injected, and the proper service is retrieved upon actual need.

There could also be a case to be made for a long list of dependencies to a static method.
For example, EmailException() on [SafeTry](@ref Sphyrnidae.Common.Utilities.SafeTry).
This method takes 2 dependencies, so it will be up to the consumer to inject both dependencies.
It could be simpler from the consumer perspective to just inject at most a single dependency (IServiceProvider).


## Where Used {#ServiceLocatorWhereUsedMd}
1. [PipelineHelper](@ref Sphyrnidae.Common.Api.ServiceRegistration.PipelineHelper): Multiple Dependencies in a static method
2. [SignalRHelper](@ref Sphyrnidae.Common.Api.ServiceRegistration.SignalRHelper): Multiple Dependencies in a static method
3. [Logger](@ref Sphyrnidae.Common.Logging.Logger): Multiple optional dependencies

## Examples {#ServiceLocatorExampleMd}
<pre>
	IServiceProvider sp; // Should be injected
	var myService = ServiceLocator.Get<IMyService>(sp); // Will throw an exception if SP is null, or IMyService is not found
	myService.DoSomething(); // Safe to use (non-null)
</pre>