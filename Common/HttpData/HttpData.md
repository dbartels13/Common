# Http Data {#HttpDataMd}

## Overview {#HttpDataOverviewMd}
The [IHttpData](@ref Sphyrnidae.Common.HttpData.IHttpData) interface gives you access to the HttpRequest and HttpContext objects.
Registration of this interface is "scoped" - meaning for the life of the request.
The [HttpDataMiddleware](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware) is where these objects get "saved" into this scoped object.
From there, you can inject the instance of [IHttpData](@ref Sphyrnidae.Common.HttpData.IHttpData) anywhere to get access to these object.
Note: Just like the objects themselves, they are not thread-safe.
Meaning that once the main application thread has exited, these objects may become unreferenced/null.
Any thread that might live longer will be unable to access these objects (which is why [logging](@ref LoggingMd) pulls out any information it needs before running async).

Interface: [IHttpData](@ref Sphyrnidae.Common.HttpData.IHttpData)

Mock: [HttpDataMock](@ref Sphyrnidae.Common.HttpData.HttpDataMock)

Implementation: [HttpData](@ref Sphyrnidae.Common.HttpData.HttpData)

## Where Used {#HttpDataWhereUsedMd}
1. [HttpDataMiddleware](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware): Initial setting/saving the objects
2. [RequestData](@ref Sphyrnidae.Common.RequestData.RequestData): Obtaining items from these objects

## Examples {#HttpDataExampleMd}
<pre>
    IHttpData data; // Should be injected

    // Wrapped in try/catch in case the thread accessing the HttpRequest object outlives the primary thread
    var httpVerb = SafeTry.IgnoreException(() => Data.Request?.Method, "") ?? "";
</pre>