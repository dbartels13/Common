using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.UserPreference
{
    /// <inheritdoc />
    public class UserPreferenceSettingsDefault : UserPreferenceSettings
    {
        public void Setup() { }

        public override Task<IEnumerable<UserPreferenceSetting>> GetAll()
            => Task.FromResult(new List<UserPreferenceSetting>().AsEnumerable());
        public override UserPreferenceSetting GetItem(CaseInsensitiveBinaryList<UserPreferenceSetting> settingsCollection, string key) => new UserPreferenceSetting();
        public override string GetValue(UserPreferenceSetting setting) => setting.Value;

        public int RecheckSeconds => CachingSeconds;
        public bool EnableRecheck => false;

        public override Task<bool> Create(string key, string value) => Task.FromResult(true);
        public override Task<bool> Update(string key, string value) => Task.FromResult(true);
    }
}