using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Sphyrnidae.Common.UserPreference;
using Sphyrnidae.Common.WebServices.Interfaces;
using Sphyrnidae.Common.WebServices.Models;

namespace Sphyrnidae.Common.WebServices
{
    public class UserPreferenceWebService : WebServiceBase, IUserPreferenceWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:UserPreferences");
        private const string Prefix = "UserPreferences_";

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }
        public UserPreferenceWebService(
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

        public async Task<IEnumerable<SphyrnidaeUserPreference>> GetAll(string application, int userId)
        {
            var name = $"{Prefix}Get";
            var path = new UrlBuilder(Url)
                .AddPathSegment(application)
                .AddPathSegment(userId.ToString())
                .Build();
            var response = await GetAsync(name, path);
            return await response.GetSphyrnidaeResult<IEnumerable<SphyrnidaeUserPreference>>(name);
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
            return await response.GetSphyrnidaeResult(false);
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
            return await response.GetSphyrnidaeResult(false);
        }

        protected override void AlterHeaders(HttpHeaders headers)
        {
            headers.Add(Constants.ApiToApi.Application, App.Name);
            headers.Add(Constants.ApiToApi.Token, Env.Get("ApiAuthorization:UserPreferences"));
        }
    }
}
