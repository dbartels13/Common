using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.UserPreference.Interfaces;

// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.UserPreference
{
    /// <inheritdoc />
    public class UserPreferenceSettingsMock : IUserPreferenceSettings
    {
        public void Setup() { }

        public virtual async Task<IEnumerable<SphyrnidaeUserPreference>> GetAll()
            => await new Task<List<SphyrnidaeUserPreference>>(() => new List<SphyrnidaeUserPreference>());
        public SphyrnidaeUserPreference GetItem(CaseInsensitiveBinaryList<SphyrnidaeUserPreference> settingsCollection, string key) => new SphyrnidaeUserPreference();
        public string GetValue(SphyrnidaeUserPreference setting) => setting.Value;

        public string Key => "UserPreferences";
        public int CachingSeconds => 600;
        public int RecheckSeconds => CachingSeconds;
        public bool EnableRecheck => false;

        public Task<bool> Create(string key, string value) => Task.FromResult(true);
        public Task<bool> Update(string key, string value) => Task.FromResult(true);
    }
}