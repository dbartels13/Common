﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sphyrnidae.Common.Api.ServiceRegistration;
using Sphyrnidae.Common.Api.ServiceRegistration.Models;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.EmailUtilities;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.FeatureToggle.Interfaces;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.UserPreference.Interfaces;
using Sphyrnidae.Common.Variable.Interfaces;
using Sphyrnidae.Common.WebServices.ApiAuthentication;
using Sphyrnidae.Settings.Email;
using Sphyrnidae.Settings.FeatureToggle;
using Sphyrnidae.Settings.Loggers;
using Sphyrnidae.Settings.Repos;
using Sphyrnidae.Settings.Repos.Interfaces;
using Sphyrnidae.Settings.UserPreference;
using Sphyrnidae.Settings.Variable;

namespace Sphyrnidae.Settings
{
    public static class SphyrnidaeRegisterServices
    {
        /// <summary>
        /// Registers implementations that are specific for SphyrnidaeTech.com
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="app">Your version of IApplicationSettings</param>
        /// <param name="env">Your specific IEnvironmentSettings</param>
        public static void RegisterSphyrnidaeServices(this IServiceCollection services, IApplicationSettings app, IEnvironmentSettings env)
        {
            var config = new ServiceConfiguration();
            var http = new HttpClientSettings(null, env);

            // Standard overrides
            services.TryAddScoped<IIdentityHelper, SphyrnidaeIdentityHelper>();
            services.TryAddScoped<ILoggerConfiguration, SphyrnidaeLoggerConfiguration>();
            services.TryAddSingleton<ILoggers, SphyrnidaeLoggers>();
            services.TryAddTransient<IFeatureToggleSettings, SphyrnidaeFeatureToggleSettings>();
            services.TryAddTransient<IVariableSettings, SphyrnidaeVariableSettings>();
            services.TryAddTransient<IUserPreferenceSettings, SphyrnidaeUserPreferenceSettings>();

            // Email
            services.TryAddTransient<IEmail, DotNetEmail>();
            services.TryAddTransient<IDotNetEmailSettings, SphyrnidaeDotNetEmailSettings>();
            services.TryAddTransient<IEmailDefaultSettings, SphyrnidaeEmailDefaultSettings>();
            services.TryAddTransient<IEmailSettings, SphyrnidaeEmailSettings>();

            // Repositories
            services.TryAddTransient<ILogRepo, LogRepo>();
            services.TryAddTransient<IDefaultUserRepo, DefaultUserRepo>();

            // Web Services
            services.TryAddTransient<IApiAuthenticationWebService, SphyrnidaeApiAuthenticationWebService>();
            services.TryAddTransient<IFeatureToggleWebService, FeatureToggleWebService>();
            services.TryAddTransient<IUserPreferenceWebService, UserPreferenceWebService>();
            services.TryAddTransient<IVariableWebService, VariableWebService>();

            SphyrnidaeServiceRegistration.AddCommonServices(services, config, app, env, http);
        }
    }
}
