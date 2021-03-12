# Web Services {#WebServiceMd}
A Web Service is a call made from your server (business logic code) to another server's API endpoint.
This could be SOA, microservices, or an external system.
This is usually accomplished using [API-to-API](@ref AuthenticationMiddlewareMd) communication (eg. A token is provided for authentication).
This allows you to design a microservice/SOA type solution instead of retrieving all information from the [Data Access Layer](@ref DalMd).

The [WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase) class allows you to quickly and easily create multiple web service classes.
It is recommended that you create a class per route type (eg. All actions against a controller).
This is similar methodology to creation of your Data Access Layer, Services, etc.

The important things that [WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase) does for you:
1. Fully encapsulates the <a href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0" target="blank">IHttpClientFactory</a> implementation
2. Modifies HttpRequest headers to forward on:
	1. [RequestId](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.RequestIdHeader)
	2. [SessionId](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.SessionIdHeader)
	3. [IpAddress](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.IpAddressHeader)
	4. [Jwt](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.JwtHeader)
	5. [LogOrder](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.LogOrderHeader): The [logging](@ref LoggingMd) of the web service call actually does this
	6. User-Agent: Hard-coded to "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0"
3. Wrapper methods to handle the full POST/PUT/GET/etc calls with logging enabled
4. Provides parsing methods to handle the HttpResponse
5. Allows you to customize the headers, or anything else in the message, without having to deal with Http/Web Service implementation details

## Where Used {#WebServiceWhereUsedMd}
None in this package, however it is recommended the following interface implementations utilize web services (despite the name provided, they don't actually have to perform web service calls)
1. [IApiAuthenticationWebService](@ref Sphyrnidae.Common.WebServices.ApiAuthentication.IApiAuthenticationWebService)
2. <a href="https://github.com/dbartels13/Common/blob/main/SphyrnidaeSettings/FeatureToggle/IFeatureToggleWebService.cs" target="blank">IFeatureToggleWebService</a>
3. <a href="https://github.com/dbartels13/Common/blob/main/SphyrnidaeSettings/UserPreference/IUserPreferenceWebService.cs" target="blank">IUserPreferenceWebService</a>
4. <a href="https://github.com/dbartels13/Common/blob/main/SphyrnidaeSettings/Variable/IVariableWebService.cs" target="blank">IVariableWebService</a>

These are all called by their respective [settings](@ref SettingsMd).

## Examples {#WebServiceExampleMd}
<pre>
    // Create the model which will be returned by the web service call
    public class Widget {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }

    // Create the web service class
    public class WidgetWebService : WebServiceBase, IWidgetWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:Widget");

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }
        public WidgetWebService(
            IHttpClientFactory factory,
            IHttpClientSettings settings,
            IIdentityHelper identity,
            ILogger logger,

            IEnvironmentSettings env,
            IApplicationSettings app
        ) : base(factory, settings, identity, logger)
        {
            Env = env;
            App = app;
        }

        public async Task<IEnumerable<Widget>> Get(int customerId)
        {
            const string name = "Widget_Get";
            var url = new UrlBuilder(Url)
                .AddPathSegment(customerId.ToString()) // Could be path segment, querystring, etc
                .Build();
            var response = await GetAsync(name, url);
            return await GetResult<IEnumerable<Widget>>(response, name);
        }

        // API-to-API communication. These headers need to be set
        protected override void AlterHeaders(HttpHeaders headers)
        {
            headers.Add(Constants.ApiToApi.Application, App.Name);
            headers.Add(Constants.ApiToApi.Token, Env.Get("ApiAuthorization:Widget"));
        }
    }

    // Call this to get widgets
    IWidgetWebService Widgets; // Should be injected
    var widgets = await Widgets.Get(1);
</pre> 
