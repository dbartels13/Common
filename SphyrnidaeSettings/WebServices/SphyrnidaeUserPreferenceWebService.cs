using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Sphyrnidae.Common.Api;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.UserPreference;
using Sphyrnidae.Common.WebServices;
using Sphyrnidae.Common.WebServices.Interfaces;
using Sphyrnidae.Settings.WebServices.Models;

namespace Sphyrnidae.Settings.WebServices
{
    public class SphyrnidaeUserPreferenceWebService : WebServiceBase, IUserPreferenceWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:UserPreferences");
        private const string Prefix = "UserPreferences_";

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }
        public SphyrnidaeUserPreferenceWebService(
            IHttpClientFactory factory,
            IHttpClientSettings settings,
            IIdentityHelper identity,
            IEncryption encryption,
            ILogger logger,

            IEnvironmentSettings env,
            IApplicationSettings app
            ) : base(factory, settings, identity, encryption, logger)
        {
            Env = env;
            App = app;
        }

        public async Task<IEnumerable<UserPreferenceSetting>> GetAll(string application, int userId)
        {
            var name = $"{Prefix}Get";
            var path = new UrlBuilder(Url)
                .AddPathSegment(application)
                .AddPathSegment(userId.ToString())
                .Build();
            var response = await GetAsync(name, path);
            return await GetResult<IEnumerable<UserPreferenceSetting>>(response, name);
        }

        public async Task<bool> Create(string application, int userId, string key, string value)
        {
            var name = $"{Prefix}Create";
            var model = new UserPreferencesRequest
            {
                Application = application,
                UserId = userId,
                Key = key,
                Value = value
            };
            var response = await PostAsync(name, Url, model);
            return await GetResult(response, false);
        }

        public async Task<bool> Update(string application, int userId, string key, string value)
        {
            var name = $"{Prefix}Update";
            var model = new UserPreferencesRequest
            {
                Application = application,
                UserId = userId,
                Key = key,
                Value = value
            };
            var response = await PatchAsync(name, Url, model);
            return await GetResult(response, false);
        }

        protected override void AlterHeaders(HttpHeaders headers)
        {
            headers.Add(Constants.ApiToApi.Application, App.Name);
            headers.Add(Constants.ApiToApi.Token, Env.Get("ApiAuthorization:UserPreferences"));
        }
    }
}
