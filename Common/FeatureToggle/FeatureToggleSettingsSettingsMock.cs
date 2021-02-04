using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.FeatureToggle.Interfaces;

// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.FeatureToggle
{
    /// <inheritdoc />
    public class FeatureToggleSettingsSettingsMock : IFeatureToggleSettings
    {
        public void Setup() { }

        public virtual Task<IEnumerable<SphyrnidaeFeatureToggle>> GetAll()
            => new Task<IEnumerable<SphyrnidaeFeatureToggle>>(() => new List<SphyrnidaeFeatureToggle>());
        public SphyrnidaeFeatureToggle GetItem(CaseInsensitiveBinaryList<SphyrnidaeFeatureToggle> settingsCollection, string key) => new SphyrnidaeFeatureToggle();
        public string GetValue(SphyrnidaeFeatureToggle setting) => setting.Value;

        public string Key => "FeatureToggleSettings";
        public int CachingSeconds => 600;
        public int RecheckSeconds => CachingSeconds;
        public bool EnableRecheck => false;
    }
}