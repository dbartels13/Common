using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.UserPreference;
using Sphyrnidae.Common.Utilities;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Settings.UserPreference
{
    /// <inheritdoc />
    public class SphyrnidaeUserPreferenceSettings : UserPreferenceSettings
    {
        #region Properties
        protected IEmail EmailImpl { get; }
        protected ILogger Logger { get; }
        protected IUserPreferenceWebService Service { get; }
        protected IApplicationSettings App { get; }
        protected IIdentityHelper Identity { get; }

        protected int UserId => Identity.Current.Id;
        #endregion

        #region Constructor
        public SphyrnidaeUserPreferenceSettings(
            IEmail email,
            ILogger logger,
            IUserPreferenceWebService service,
            IApplicationSettings app,
            IIdentityHelper identity)
        {
            EmailImpl = email;
            Logger = logger;
            Service = service;
            App = app;
            Identity = identity;
        }
        #endregion

        #region Abstract Implementations
        public override string Key => $"SphyrnidaeUserPreferences_{App.Name}_{UserId}";

        public override async Task<IEnumerable<Common.UserPreference.UserPreferenceSetting>> GetAll()
            => await SafeTry.EmailException(
                EmailImpl,
                App,
                async () => await Service.GetAll(App.Name, UserId)
            );
        #endregion

        public override async Task<bool> Create(string key, string value)
            => await SafeTry.LogException(
                Logger,
                async () => await Service.Create(App.Name, UserId, key, value)
            );

        public override async Task<bool> Update(string key, string value)
            => await SafeTry.LogException(
                Logger,
                async () => await Service.Update(App.Name, UserId, key, value)
            );
    }
}