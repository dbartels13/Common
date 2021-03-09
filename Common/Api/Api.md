# API Methods {#ApiMd}

## Pipeline / Middleware {#ApiPipelineMd}
The Sphyrnidae.Common.Api namespace is mostly about building a robust pipeline for a modern WebAPI.
This namespace is driven by the [ServiceConfiguration](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration) class.
It comes pre-configured to do all of the following things:
<ul>
	<li>[Cors](@ref Sphyrnidae.Common.Api.ServiceRegistration.CorsHelper)
	<li>[Swagger](@ref Sphyrnidae.Common.Api.ServiceRegistration.SwaggerHelper)
	<li>[Health Checks](@ref Sphyrnidae.Common.Api.ServiceRegistration.HealthCheckHelper)
	<li>[Caching](@ref Sphyrnidae.Common.Api.ServiceRegistration.CacheHelper)
	<li>Web Services (enabled by default)
	<li>[API Configurations](@ref Sphyrnidae.Common.Api.ServiceRegistration.ApiHelper)
	<li>[SignalR](@ref Sphyrnidae.Common.Api.ServiceRegistration.SignalRHelper)
	<li>[Pipeline](@ref Sphyrnidae.Common.Api.ServiceRegistration.PipelineHelper)
</ul>

The default pipeline used in the configuration is:
<ol>
	<li>DevelopmentHsts - Dev/Localhost: app.UseDeveloperExceptionPage(); Other: app.UseHsts();
	<li>HttpsRedirect - app.UseHttpsRedirection();
	<li>Swagger - Enable swagger/UI (app.UseSwagger(); app.UseSwaggerUI();)
	<li>Routing - app.UseRouting();
	<li>Cors - app.UseCors(config.CorsPolicyName);
	<li>HealthCheck - app.UseHealthChecks(config.HealthCheckEndpoint, config.HealthCheckOptions(envSettings));
	<li>[HttpData](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware)
	<li>[Authentication](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware)
	<li>[JWT](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware)
	<li>[Exceptions](@ref Sphyrnidae.Common.Api.Middleware.ExceptionMiddleware)
	<li>[Logging](@ref Sphyrnidae.Common.Api.Middleware.ApiLoggingMiddleware)
	<li>Controller API Call - app.UseEndpoints(endpoints => {endpoints.MapControllers();});
</ol>

You can create/add any additional middleware components you want to this pipeline, or totally redo the pipeline.

## Service Registration {#ApiServiceRegistrationMd}
In your own application, you need only worry about things that are specific to your application.
However, you do need to wire in all of this configuration into your startup.cs class.
Information on doing this is given in [API Setup](@ref SetupMd).

## Attributes {#ApiAttributesMd}
There are a number of [Attributes](@ref Sphyrnidae.Common.Api.Attributes).
There are 2 types of attributes:
<ol>
	<li>Validation Attributes:
	<ol>
		<li>[EnumValidator](@ref Sphyrnidae.Common.Api.Attributes.EnumValidatorAttribute)
		<li>[NotDefault](@ref Sphyrnidae.Common.Api.Attributes.NotDefaultAttribute)
		<li>[RegEx](@ref Sphyrnidae.Common.Api.Attributes.RegExAttribute)
	</ol>
	<li>Pipeline configuration attributes:
	<ol>
		<li>[Authentication](@ref Sphyrnidae.Common.Api.Attributes.AuthenticationAttribute)
		<li>[SkipLog](@ref Sphyrnidae.Common.Api.Attributes.SkipLogAttribute)
	</ol>
</ol>

## Authentication {#AuthenticationMiddlewareMd}
The [Authentication](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware) middleware component take care of Authentication and Authorization checks.
If your API endpoint needs to be secured, you should put the [Authentication](@ref Sphyrnidae.Common.Api.Attributes.AuthenticationAttribute) attribute on it.
This attribute has one of the following [types](@ref Sphyrnidae.Common.Api.Models.AuthenticationType):
<ol>
	<li>None: Use this type if you have specified the entire controller should be secured, but not this particular endpoint.
	If an authenticated user accesses this endpoint, that will be allowed.
	If an unauthenticated user access this endpoint, they will be assigned the [default/public identity](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper): see GetDefaultIdentity() method
	<li>Jwt: This is the primary mechanism for authentication. To learn more about JWT's, go <a href="https://en.wikipedia.org/wiki/JSON_Web_Token" target="blank">here</a>
	<li>ApiToApi: Microservice calls will provide their authorization "key" to the next service. Using this type ensures that this endpoint is hit only by a validated microservice call.
</ol>

The JWT is basically a serialized and encrypted [Identity Object](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) - generated and decrypted using the [IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper).

The JWT contains a collection of roles (List<string>) that the user has access to.
By specifying a named role in the [Authentication](@ref Sphyrnidae.Common.Api.Attributes.AuthenticationAttribute) attribute,
this will first Authenticate via the JWT, and then Authorize based on the user having that named role in their role collection.

The	<li>[JWT](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware) middleware component does not play a direct role in Authentication/Authorization.
What this component does instead is places a [refreshed JWT](@ref AuthenticationRefreshTokenMd) into the [Authorization](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader) header of the HTTP Response.
If there were any updates to the [Identity Object](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity),
you will need to update the [Current](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper.Current) object with this updated/new identity.

For more information on Authentication and Authorization, go [here](@ref AuthenticationMd)

## Responses {#ApiResponseMd}
In your business logic, you may need to return a non-2xx response.
If you are in the controller, you have access to methods such as Ok() or NotFound().
However, in other layers, you don't have access to these methods.
Instead, you should be returning a business entity and relying on the controller to actually convert that entity into a response object.
I have created a class called [ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard) which serves that purpose.

[ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard) is the business entity that contains all the information needed for a controller to convert this to an HTTP response.
To create an [ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard), you can use the static methods contained in [ApiResponse](@ref Sphyrnidae.Common.Api.Responses.ApiResponse).

When this object is returned to the controller, you can execute one of the [base class](@ref Sphyrnidae.Common.Api.BaseClasses.BaseApi) methods.
The [BaseApi](@ref Sphyrnidae.Common.Api.BaseClasses.BaseApi) class takes as a constructor argument [IApiResponse](@ref Sphyrnidae.Common.Api.Responses.IApiResponse).
This class is injected, and by calling the methods FormatResponse() or GetWithDefaultsRemoved(), this will create the proper response from the [ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard) object.
By default, the registered implementation of [IApiResponse](@ref Sphyrnidae.Common.Api.Responses.IApiResponse) is [ApiResponseStandard](@ref Sphyrnidae.Common.Api.Responses.ApiResponseStandard).
If you'd like to format the HTTP response differently, you can register a new implementation.
The common scenario I've seen for this is when you're trying to differentiate between a network 404 and a server/application 404.

Additionally, if you have a customized HTTP response object (eg. not just the simple return object in the body of the response),
you will need to update ModelState validation.
This will be done by registering a new [ApiControllerConfiguration](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration.ApiControllerConfiguration).
Your custom implementation will be registered with the builder as builder.ConfigureApiBehaviorOptions(YOUR_METHOD),
and to override the ModelState validation error, you should specify a custom implementation for InvalidModelStateResponseFactory.

## Error Handling {#ApiExceptionsMd}
The [Exceptions](@ref Sphyrnidae.Common.Api.Middleware.ExceptionMiddleware) middleware component will trap and log all exceptions that occur during the API call.
There are 2 types of exceptions that will be handled slightly differently:
<ol>
	<li>[UserException](@ref Sphyrnidae.Common.Api.Models.UserException) will record the exception, and send the full message back in the HTTP response
	<li>All others will still record the exception, but will instead send back in the HTTP response a "nice" message:
	We're sorry, but something went wrong with your request.
	The details of this issue have been captured - please reference issue #{guid} if you need to contact the help desk.
</ol>

## Logging {#ApiLoggingMd}
The [Logging](@ref Sphyrnidae.Common.Api.Middleware.ApiLoggingMiddleware) middleware will ensure that all API calls are logged.
The [logging](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger) of these calls will contain both request and response information (eg. the response body and how long it took).

Additionally, the custom sphyrnidae middleware componets (
	[HttpData](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware),
	[Authentication](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware),
	[JWT](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware),
	[Exceptions](@ref Sphyrnidae.Common.Api.Middleware.ExceptionMiddleware),
	[Logging](@ref Sphyrnidae.Common.Api.Middleware.ApiLoggingMiddleware))
	will all log that the middleware component is executing (starting, and stopping with time elapsed).
