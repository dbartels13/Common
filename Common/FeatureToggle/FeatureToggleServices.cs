using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.FeatureToggle.Interfaces;

namespace Sphyrnidae.Common.FeatureToggle
{
    public class FeatureToggleServices : IFeatureToggleServices
    {
        public ICache Cache { get; }
        public IFeatureToggleSettings Service { get; }

        public FeatureToggleServices(ICache cache, IFeatureToggleSettings service)
        {
            Cache = cache;
            Service = service;
        }
    }
}
