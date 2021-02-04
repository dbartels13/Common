using System.Net.Http;
using System.Threading.Tasks;
using Sphyrnidae.Common.Api;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Common.WebServices
{
    public class ApiAuthenticationWebService : WebServiceBase, IApiAuthenticationWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:ApiAuthentication");

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }
        public ApiAuthenticationWebService(
            IHttpClientFactory factory,
            IHttpClientSettings settings,
            IIdentityWrapper identity,
            ITokenSettings token,
            IEncryption encryption,
            ILogger logger,

            IEnvironmentSettings env,
            IApplicationSettings app
        ) : base(factory, settings, identity, token, encryption, logger)
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
            return await response.GetSphyrnidaeResult(false);
        }
    }
}