# Http Client {#HttpClientMd}

## Overview {#HttpClientOverviewMd}
The [IHttpClientSettings](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings) is meant to abstract out communication with an HTTP Request.
This interface basically works by allowing other abstractions to retrieve data from the HTTP Request via this abstraction.
If you'd like to change the location of information in a HTTP Request, you can do so by updating this class.
Most of the properties on this interface are the name of an HTTP Header, and a Get property to retrieve information from that property.
Other components will use this interface to set these values in outgoing HTTP Responses.

The [IHttpClientSettings](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings) interface has the following properties.
The table below shows all properties, and what the [default implementation](@ref Sphyrnidae.Common.HttpClient.HttpClientSettings) does to implement.
<table>
    <tr>
        <th>Property
        <th>Default Implementation
        <th>Where used
    <tr>
        <td>[ContentType](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.ContentType)
        <td>MediaTypeNames.Application.Json
        <td>None - this has been moved to [ServiceConfiguration](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration) which is defaulted to JSON responses.
    <tr>
        <td>[JwtHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader)
        <td>"Authorization"
        <td>[JwtMiddleware](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware): Sets the refreshed JWT into this header
        <br />[Api Logging](@ref LoggingMd): When logging the API request headers, this header will always be obfuscated
        <br />[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Sets the JWT into this header for the outgoing HTTP Request
    <tr>
        <td>[Jwt](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.Jwt)
        <td>
            1. Reads from [JwtHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader)
            2. Reads from Querystring "access_token"
            3. Removes "bearer" (with or without spaces)
        <td>[IdentityHelper](@ref Sphyrnidae.Common.Authentication.IdentityHelper): Obtains this JWT to convert to Identity
    <tr>
        <td>[RequestIdHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.RequestIdHeader)
        <td>"V-Correlation-Id"
        <td>[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Where the requestId goes in the header for the outgoing HTTP Request
    <tr>
        <td>[RequestId](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.RequestId)
        <td>
            1. Reads from [RequestIdHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.RequestIdHeader)
            2. Obtains this from [Request Data](@ref Sphyrnidae.Common.RequestData.IRequestData.Id)
        <td>[Logging](@ref LoggingMd): This value is always logged
        <br />[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Retrieves this to forward into outgoing HTTP Request
    <tr>
        <td>[SessionIdHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.SessionIdHeader)
        <td>"X-Tracking-Id"
        <td>[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Where the sessionId goes in the header for the outgoing HTTP Request
    <tr>
        <td>[SessionId](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.SessionId)
        <td>Reads from [SessionIdHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.SessionIdHeader)
        <td>[Logging](@ref LoggingMd): This value is always logged
        <br />[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Retrieves this to forward into outgoing HTTP Request
    <tr>
        <td>[IpAddressHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.IpAddressHeader)
        <td>"X-Forwarded-For"
        <td>[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Where the IP Address goes in the header for the outgoing HTTP Request
    <tr>
        <td>[IpAddress](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.IpAddress)
        <td>
            1. Pulls from [Request Data (IpAddress)](@ref Sphyrnidae.Common.RequestData.IRequestData.IpAddress)
            2. Reads from [IpAddressHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.IpAddressHeader)
            3. Pulls from [Request Data (RemoteIpAddress)](@ref Sphyrnidae.Common.RequestData.IRequestData.RemoteIpAddress)
            4. "unknown"
            5. For whatever value is pulled back, this will be "cleaned": Localhost check, strips out the domain
        <td>[WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): Retrieves this to forward into outgoing HTTP Request
    <tr>
        <td>[LogOrderHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.LogOrderHeader)
        <td>"X-Logging-Order"
        <td>[Logging](@ref LoggingMd): This value is always logged.
        The logger will also be responsible for placing the LogOrder into this header for web service requests
    <tr>
        <td>[LogOrder](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.LogOrder)
        <td>Reads from [LogOrderHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.LogOrderHeader)
        <td>[Logging](@ref LoggingMd): The existing value will be the prefix to the new value set into the header of outgoing HTTP requests.
        This value is also always logged.
    <tr>
        <td>[BearerAndLocalhostComparison](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.BearerAndLocalhostComparison)
        <td>StringComparison.CurrentCultureIgnoreCase
        <td>[Jwt](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.Jwt): To strip out "bearer"
        <br />[IpAddress](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.IpAddress): To check for "localhost"
</table>

Interface: [IHttpClientSettings](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings)

Mock: [HttpClientSettingsMock](@ref Sphyrnidae.Common.HttpClient.HttpClientSettingsMock)

Implementation: [HttpClientSettings](@ref Sphyrnidae.Common.HttpClient.HttpClientSettings)

## Examples {#HttpClientExampleMd}
<pre>
    IHttpClient http; // Should be injected
    var ipAddress = http.IpAddress;

    HttpHeaders headers; // Should be retrieved from the new HttpRequest object
    headers.Add(http.RequestIdHeader, http.RequestId);
</pre>