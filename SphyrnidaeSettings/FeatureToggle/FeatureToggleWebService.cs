using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sphyrnidae.Common.Api;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.FeatureToggle;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.WebServices;

namespace Sphyrnidae.Settings.FeatureToggle
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
            IIdentityHelper identity,
            ILogger logger,

            IEnvironmentSettings env,
            IApplicationSettings app
        ) : base(factory, settings, identity, logger)
        {
            Env = env;
            App = app;
        }

        public async Task<IEnumerable<FeatureToggleSetting>> GetAll(string application, string customerId)
        {
            const string name = "FeatureToggle_Get";
            var path = new UrlBuilder(Url)
                .AddPathSegment(application)
                .AddPathSegment(customerId)
                .Build();
            var response = await GetAsync(name, path);
            return await GetResult<IEnumerable<FeatureToggleSetting>>(response, name);
        }

        protected override void AlterHeaders(HttpHeaders headers)
        {
            headers.Add(Constants.ApiToApi.Application, App.Name);
            headers.Add(Constants.ApiToApi.Token, Env.Get("ApiAuthorization:FeatureToggle"));
        }
    }
}