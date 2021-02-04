using Sphyrnidae.Common.Lookup;

namespace Sphyrnidae.Common.FeatureToggle.Interfaces
{
    /// <summary>
    /// Services required for executing a feature toggle lookup
    /// </summary>
    public interface IFeatureToggleServices : ILookupServices<IFeatureToggleSettings, SphyrnidaeFeatureToggle>
    {
    }
}
