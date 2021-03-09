# Authentication {#AuthenticationMd}

## Overview {#AuthenticationOverviewMd}
Authentication is done without the need for an overly complex architecture containing OAuth.
Instead, this package relies on the presense of a <a href="https://en.wikipedia.org/wiki/JSON_Web_Token" target="blank">JWT</a>.
It will be up to your application to handle user authentication/registration, and that endpoint should return a JWT in it's response.
This JWT should be saved by the client and provided in every subsequent HTTP request.

Interface: [IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper)

Default Implementation / Mock: [IdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IdentityHelper)

## Setup {#AuthenticationSetupMd}
All methods needed to act on an [Identity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) object are provided in [IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper).
The registered implementation is to use [IdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IdentityHelper) with the type of [BasicIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BasicIdentity).
You can always extend the [Identity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) by inheriting and adding custom properties.
If you do extend the identity, you should register (in startup.cs) the new implementation.
You can additionally modify the default behaviors present in the [IdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IdentityHelper) implementation (or provide a different implementation class).

Please note that the [IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper) will generally work off a [BaseIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity).
You can send anything that inherits from [BaseIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) into this class,
and if your implementation returns an inherited class, you will need to cast the result to your inherited type.

## JWT Creation {#AuthenticationJwtCreateMd}
To generate a JWT, you should first generate an [Identity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) object.
Eg. new() up an inherited class from [BaseIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity)
You can ignore the 'Expires' attribute, as that will be set in the next step.
All roles that the user has will be populated into the [Roles](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity.Roles) property.

After you have created your identity object, you can convert this to a JWT via the ToJwt() method on the [IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper) interface.
This will set the [Expires](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity.Expires) property based on [ExpirationMinutes](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper.ExpirationMinutes).
The [standard implementation](@ref Sphyrnidae.Common.Authentication.Helper.IdentityHelper) utilizes [serialization](@ref SerializeMd) and [encryption](@ref EncryptionMd) to convert your identity object to a string.

The JWT can then be placed into your HTTP response.

## JWT Consumption {#AuthenticationJwtConsumeMd}
The JWT is a self-contained encrypted object which contains all identity information needed for Authentication and Authorization.
The client will be unable to parse/alter the JWT, as this is only a server-side ability.
When the client makes an HTTP request to your application, it should place the JWT in the [Authorization](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader) header.
If you need to return the actual identity object (or any of it's properties), you will need to convert the JWT back into the [Identity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) object.
This is done using the [Current](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper.Current) property on the [IIdentityHelper](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper) interface.
The interface implementation should be registered as request-scoped, meaning that the object will be cached for the entirety of the request.

There is also the GetIdentity() method, but this is not cached and will always do the full decryption/deserialization.
Behind the scenes, the [Current](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper.Current) property will call the GetIdentity() method if it does not already have the identity.
If you need to convert a JWT that is not the [Current](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper.Current) one,
you can pass that JWT directly to the GetIdentity() method.
By default, the retrieval of the JWT comes from the [JwtHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader).

## Authentication and Authorization {#AuthenticationAuthorizationMd}
By utilizing the [API Pipeline](@ref ApiMd), authentication and authorization will automatically occur in your API request.
There is nothing else you have to do - if you have guarded your API endpoint with an [Authentication](@ref Sphyrnidae.Common.Api.Attributes.AuthenticationAttribute) attribute,
then you can be assured that only requests that are authenticated and authroized make it through to your API.
For more information on this setup, please refer to the [Authentication](@ref AuthenticationMiddlewareMd) section of the [API](@ref ApiMd) documentation.

## Refresh Token {#AuthenticationRefreshTokenMd}
A common scenario is to give a JWT a short expiration time (eg. 20 minutes), but this time should be reset on every interaction with your application.
To help accomplish this, the [JwtMiddleware](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware) component will automatically put a refreshed JWT
(with a new expiration date) into the HTTP response (will utilize the same [Authorization](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader) header.)
This middleware component is included in the [default pipeline](@ref ApiMd), but can be removed or altered per [configuration](@ref ApiPipelineMd).
If you would like to change any properties of the JWT/Identity, this can be done by 'setting' a new [Current Identity](@ref Sphyrnidae.Common.Authentication.Helper.IIdentityHelper.Current).

Please note that this implementation does leave a security hole open in your application.
If an attacker is able to steal a JWT, it will be valid only until it expires.
However, if you are always sending a refresh token, the attacker can continue to use a refreshed token indefinitely.
It is our recommendation that you have a JWT invalidation scheme.
Eg. Update (replace) the [JwtMiddleware](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware) component with another component which will lookup validity of a user's logon.

## Examples {#AuthenticationExampleMd}
<pre>
	IIdentityHelper helper; // Should be injected

	// Building new identity and setting the JWT
	var identity = new BasicIdentity {
		Id = 1,
		Username = "MyUsername",
		FirstName = "First",
		LastName = "Last",
		Email = "me@foo.com",
		Roles = new List<string> {"role1","role2","role3"}
	};
	var jwt = helper.ToJwt(identity);

	// Take any jwt and convert it to an identity
	identity = helper.GetIdentity(jwt);

	// Getting the identity back from the request
	identity = helper.Current;

	// See if user has a given role
	var hasRole = identity.SearchableRoles.Has("role1"); // true
	hasRole = identity.SearchableRoles.Has("role18"); // false
</pre>