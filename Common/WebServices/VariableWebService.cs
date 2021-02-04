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
using Sphyrnidae.Common.Logging;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.SphyrnidaeApiResponse;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Common.WebServices
{
    public class VariableWebService : WebServiceBase, IVariableWebService
    {
        private static string _url;
        private string Url => _url ??= SettingsEnvironmental.Get(Env, "URL:Variable");

        private IEnvironmentSettings Env { get; }
        private IApplicationSettings App { get; }

        public VariableWebService(
            IHttpClientFactory factory,
            IHttpClientSettings settings,
            IIdentityWrapper identity,
            ITokenSettings token,
            IEncryption encryption,

            IEnvironmentSettings env,
            IApplicationSettings app
        )
            : base(factory, settings, identity, token, encryption, new NonLogger())
        {
            Env = env;
            App = app;
        }

        // This call won't be logged...
        // We can't inject ILogger like other web services because this would be a circular reference:
        // ILogger => ILoggerConfiguration => IVariableSettings => this
        public async Task<IEnumerable<SphyrnidaeVariable>> GetAll(string application, int customerId)
        {
            const string name = "Variables_Get";
            var path = new UrlBuilder(Url)
                .AddPathSegment(application)
                .AddPathSegment(customerId.ToString())
                .Build();
            var response = await GetAsync(name, path);
            return await response.GetSphyrnidaeResult<IEnumerable<SphyrnidaeVariable>>(name);
        }

        protected override void AlterHeaders(HttpHeaders headers)
        {
            headers.Add(Constants.ApiToApi.Application, App.Name);
            headers.Add(Constants.ApiToApi.Token, Env.Get("ApiAuthorization:Variable"));
        }
    }
}