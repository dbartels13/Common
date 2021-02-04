using Sphyrnidae.Common.FeatureToggle.Interfaces;
using Sphyrnidae.Common.Lookup;

namespace Sphyrnidae.Common.FeatureToggle
{
    public abstract class FeatureToggleSettings : BaseLookupSetting<SphyrnidaeFeatureToggle>, IFeatureToggleSettings
    {
        public override string Key => "FeatureToggleSettings";

        public override int CachingSeconds => 600; // 10 minutes
    }
}