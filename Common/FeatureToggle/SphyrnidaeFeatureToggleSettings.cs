using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Utilities;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Common.FeatureToggle
{
    /// <inheritdoc />
    public class SphyrnidaeFeatureToggleSettings : FeatureToggleSettings
    {
        #region Properties
        protected IEmailServices EmailServices { get; }
        protected IFeatureToggleWebService Service { get; }
        protected IApplicationSettings App { get; }
        protected IIdentityWrapper Identity { get; }

        protected int CustomerId => Identity.Current?.CustomerId ?? 0;
        #endregion

        #region Constructor
        public SphyrnidaeFeatureToggleSettings(IEmailServices emailServices, IFeatureToggleWebService service, IApplicationSettings app, IIdentityWrapper identity)
        {
            EmailServices = emailServices;
            Service = service;
            App = app;
            Identity = identity;
        }
        #endregion

        #region Abstract Implementations
        public override string Key => $"SphyrnidaeFeatureToggle_{App.Name}_{CustomerId}";

        public override async Task<IEnumerable<SphyrnidaeFeatureToggle>> GetAll()
            => await SafeTry.EmailException(
                EmailServices,
                async () => await Service.GetAll(App.Name, CustomerId)
            );

        #endregion
    }
}