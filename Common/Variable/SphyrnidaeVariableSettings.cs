using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.Utilities;
using Sphyrnidae.Common.WebServices.Interfaces;

namespace Sphyrnidae.Common.Variable
{
    /// <inheritdoc />
    public class SphyrnidaeVariableSettings : VariableSettings
    {
        #region Properties
        protected IVariableWebService Service { get; }
        protected IApplicationSettings App { get; }
        protected IIdentityWrapper Identity { get; }

        protected int CustomerId => Identity.Current?.CustomerId ?? 0;
        #endregion

        #region Constructor
        public SphyrnidaeVariableSettings(IVariableWebService service, IApplicationSettings app, IIdentityWrapper identity)
        {
            Service = service;
            App = app;
            Identity = identity;
        }
        #endregion

        #region Abstract Implementations
        public override string Key => $"SphyrnidaeVariables_{App.Name}_{CustomerId}";

        // Can not use logging or email exception handling, since that would circular reference back to getting a variable
        public override async Task<IEnumerable<SphyrnidaeVariable>> GetAll()
            => await SafeTry.IgnoreException(async () => await Service.GetAll(App.Name, CustomerId));
        #endregion
    }
}