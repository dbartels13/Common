using System.Net.Http;
using System.Threading.Tasks;
using Sphyrnidae.Common.Api;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.WebServices;
using Sphyrnidae.Common.WebServices.ApiAuthentication;

namespace Sphyrnidae.Settings
{
    public class SphyrnidaeApiAuthenticationWebService : WebServiceBase, IApiAuthenticationWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:ApiAuthentication");

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }
        public SphyrnidaeApiAuthenticationWebService(
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

        public async Task<bool> IsAuthenticated(string application, string token)
        {
            const string name = "ApiAuthentication_IsAuthenticated";
            var path = new UrlBuilder(Url)
                .AddQueryString(Constants.ApiToApi.Owner, App.Name)
                .AddQueryString(Constants.ApiToApi.Application, application)
                .AddQueryString(Constants.ApiToApi.Token, token)
                .Build();
            var response = await GetAsync(name, path);
            return await GetResult(response, false);
        }
    }
}