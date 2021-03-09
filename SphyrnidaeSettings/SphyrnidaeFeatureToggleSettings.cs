using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.FeatureToggle;
using Sphyrnidae.Common.Utilities;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Settings
{
    /// <inheritdoc />
    public class SphyrnidaeFeatureToggleSettings : FeatureToggleSettings
    {
        #region Properties
        protected IEmail EmailImpl { get; }
        protected IFeatureToggleWebService Service { get; }
        protected IApplicationSettings App { get; }
        protected IIdentityHelper Identity { get; }

        protected string CustomerId => ((SphyrnidaeIdentity)Identity.Current)?.CustomerId ?? "0";
        #endregion

        #region Constructor
        public SphyrnidaeFeatureToggleSettings(IEmail email, IFeatureToggleWebService service, IApplicationSettings app, IIdentityHelper identity)
        {
            EmailImpl = email;
            Service = service;
            App = app;
            Identity = identity;
        }
        #endregion

        #region Abstract Implementations
        public override string Key => $"SphyrnidaeFeatureToggle_{App.Name}_{CustomerId}";

        public override async Task<IEnumerable<FeatureToggleSetting>> GetAll()
            => await SafeTry.EmailException(
                EmailImpl,
                App,
                async () => await Service.GetAll(App.Name, CustomerId)
            );

        #endregion
    }
}