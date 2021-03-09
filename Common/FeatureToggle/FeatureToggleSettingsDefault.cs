using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.FeatureToggle
{
    /// <inheritdoc />
    public class FeatureToggleSettingsDefault : FeatureToggleSettings
    {
        public void Setup() { }

        public override Task<IEnumerable<FeatureToggleSetting>> GetAll()
            => new Task<IEnumerable<FeatureToggleSetting>>(() => new List<FeatureToggleSetting>());
        public override FeatureToggleSetting GetItem(CaseInsensitiveBinaryList<FeatureToggleSetting> settingsCollection, string key) => new FeatureToggleSetting();
        public override string GetValue(FeatureToggleSetting setting) => setting.Value;


        public int RecheckSeconds => CachingSeconds;
        public bool EnableRecheck => false;
    }
}