using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sphyrnidae.Common.Api;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.FeatureToggle;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Common.WebServices
{
    public class FeatureToggleWebService : WebServiceBase, IFeatureToggleWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:FeatureToggle");

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }
        public FeatureToggleWebService(
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

        public async Task<IEnumerable<SphyrnidaeFeatureToggle>> GetAll(string application, int customerId)
        {
            const string name = "FeatureToggle_Get";
            var path = new UrlBuilder(Url)
                .AddPathSegment(application)
                .AddPathSegment(customerId.ToString())
                .Build();
            var response = await GetAsync(name, path);
            return await response.GetSphyrnidaeResult<IEnumerable<SphyrnidaeFeatureToggle>>(name);
        }

        protected override void AlterHeaders(HttpHeaders headers)
        {
            headers.Add(Constants.ApiToApi.Application, App.Name);
            headers.Add(Constants.ApiToApi.Token, Env.Get("ApiAuthorization:FeatureToggle"));
        }
    }
}