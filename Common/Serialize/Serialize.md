# Serialize {#SerializeMd}
The [Serialization](@ref Sphyrnidae.Common.Serialize.Serialization) class is a collection of extension methods that allow you to de/serialize JSON or XML.
The JSON methods take an optional parameter of JsonSerializerSettings - [default](@ref Sphyrnidae.Common.Serialize.SerializationSettings.Default) is to specify DefaultValueHandling.IgnoreAndPopulate.
You can specify any of the pre-configured [SerializationSettings](@ref Sphyrnidae.Common.Serialize.SerializationSettings), or you can specify your own custom JsonSerializerSettings.

## Where Used {#SerializeWhereUsedMd}
1. [HealthCheck](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration.HealthCheckOptions): Output of the health check is basically the serialized HealthReport
2. [Identity](@ref AuthenticationMd): Conversion between the [identity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity) and JWT
3. Middleware exceptions will write out a custom response that is a serialized object
4. [Logging](@ref LoggingMd): Utilization of extension method [ToNameValueCollection](@ref Sphyrnidae.Common.Extensions.NameValueCollectionExtensions) to basically convert a generic response object to something that is loggable
5. [DatabaseInformation](@ref Sphyrnidae.Common.Logging.Information.DatabaseInformation) (see [Logging](@ref LoggingInformationMd)): Conversion of generic database parameters to something that can be logged
6. [Log4NetLogger](@ref Sphyrnidae.Common.Logging.Loggers.Log4NetLogger) (see [Loggers](@ref LoggingLoggersMd)): Log4Net only logs strings, so the object to be logged needs to be serialized
7. [WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase) (see [Web Services](@ref WebServiceMd)): Any data that is POST/PUT/etc needs to be serialized to the request body

## Examples {#SerializeExampleMd}
<pre>
	var obj = new Foo();
	var serialized = obj.SerializeJson();
	obj = serialized.DeserializeJson<Foo>();

	serialized = obj.SerializeXml();
	obj = serialized.DeserializeXml<Foo>();
</pre> 
