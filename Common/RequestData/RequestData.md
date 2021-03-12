# Request Data {#RequestDataMd}

## Overview {#RequestDataOverviewMd}
The [IRequestData](@ref Sphyrnidae.Common.RequestData.IRequestData) interface provides a number of methods for accessing HttpRequest data.
The [implementation](@ref Sphyrnidae.Common.RequestData.RequestData) is registered as request-scoped, and pulls everything it needs from [IHttpData](@ref Sphyrnidae.Common.HttpData.IHttpData).
Because of this dependency, many of these methods are not thread-safe (eg. will cause exceptions if the main thread has returned and the context has been disposed).
Note that all of these properties do directly access the HttpRequest object, which does not account for these values being passed along in headers (see [Http Client](@ref HttpClientMd)).

Interface: [IRequestData](@ref Sphyrnidae.Common.RequestData.IRequestData)

Mock: [RequestDataMock](@ref Sphyrnidae.Common.RequestData.RequestDataMock)

Implementation: [RequestData](@ref Sphyrnidae.Common.RequestData.RequestData)

<table>
    <tr>
        <th>Property
        <th>Description
        <th>Implementation
    <tr>
        <td>[Id](@ref Sphyrnidae.Common.RequestData.IRequestData.Id)
        <td>Gets the unique ID for the request (Typically called a CorrelationId)
        <td>Guid.NewGuid()
    <tr>
        <td>[LoggingOrder](@ref Sphyrnidae.Common.RequestData.IRequestData.LoggingOrder)
        <td>Order of things being logged
        <td>(char)33 - will be incremented every time logging occurs
    <tr>
        <td>[IpAddress](@ref Sphyrnidae.Common.RequestData.IRequestData.IpAddress)
        <td>Ip Address of the end user/client
        <td>None - just a place to set/get this property
    <tr>
        <td>[RemoteIpAddress](@ref Sphyrnidae.Common.RequestData.IRequestData.RemoteIpAddress)
        <td>IP Address of the machine making this request (may not be the original)
        <td>[IpAddress](@ref Sphyrnidae.Common.Extensions.HttpContextExtensions)
    <tr>
        <td>[DisplayUrl](@ref Sphyrnidae.Common.RequestData.IRequestData.DisplayUrl)
        <td>The base URL of the request
        <td>HttpRequest.GetDisplayUrl()
    <tr>
        <td>[Route](@ref Sphyrnidae.Common.RequestData.IRequestData.Route)
        <td>Route of an API Request
        <td>Route template for a controller action
    <tr>
        <td>ContentData()
        <td>The raw content data of the request
        <td>[GetBodyAsync](@ref Sphyrnidae.Common.Extensions.HttpRequestExtensions)
    <tr>
        <td>[HttpVerb](@ref Sphyrnidae.Common.RequestData.IRequestData.HttpVerb)
        <td>The Http Verb for the request
        <td>HttpRequest.Method
    <tr>
        <td>[Headers](@ref Sphyrnidae.Common.RequestData.IRequestData.Headers)
        <td>Collection of Http Headers
        <td>HttpRequest.Headers.ToNameValueCollection()
    <tr>
        <td>[QueryString](@ref Sphyrnidae.Common.RequestData.IRequestData.QueryString)
        <td>Collection of QueryString variables
        <td>HttpRequest.Query
    <tr>
        <td>[FormData](@ref Sphyrnidae.Common.RequestData.IRequestData.FormData)
        <td>Collection of Form variables
        <td>HttpRequest.Form
    <tr>
        <td>[Browser](@ref Sphyrnidae.Common.RequestData.IRequestData.Browser)
        <td>Name/Description of the client browser
        <td>HttpRequest.Headers["User-Agent"]
    <tr>
        <td>T GetEndpointObject<T>()
        <td>Retrieves some object about the actual endpoint
        <td>HttpContext.Features.Get<IEndpointFeature>().Endpoint.Metadata - locate T within collection
    <tr>
        <td>GetHeader()
        <td>Retrieves an HTTP Header from the request
        <td>[GetHeader](@ref Sphyrnidae.Common.Extensions.HttpRequestExtensions)
    <tr>
        <td>GetHeaders()
        <td>The collection of HTTP Headers with the given name
        <td>HttpRequest.Headers[name] ?? default
</table>

## Where Used {#RequestDataWhereUsedMd}
1. [AuthenticationMiddleware](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware): Retrieving authentication headers and the [AuthenticationAttribute](@ref Sphyrnidae.Common.Api.Attributes.AuthenticationAttribute)
2. [HttpDataMiddleware](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware): Retrieval of [SkipLogAttribute](@ref Sphyrnidae.Common.Api.Attributes.SkipLogAttribute)
3. [HttpClientSettings](@ref Sphyrnidae.Common.HttpClient.HttpClientSettings): The implementation of [IHttpClientSettings](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings) will check against provided values in the HTTP headers, or against the request object
4. [ApiInformation](@ref Sphyrnidae.Common.Logging.Information.ApiInformation) (see [Logging](@ref LoggingInformationMd)): Obtains a number of items to be logged
5. [LoggerInformation](@ref Sphyrnidae.Common.Logging.LoggerInformation) (see [Logging](@ref LoggingInformationMd)): Also gets a number of things to be logged, and also updates [LoggingOrder](@ref Sphyrnidae.Common.RequestData.IRequestData.LoggingOrder)

## Examples {#RequestDataExampleMd}
Compare this to [Http Data](@ref HttpDataExampleMd)
<pre>
    IRequestData request; // Should be injected
    var httpVerb = request.HttpVerb;
</pre>