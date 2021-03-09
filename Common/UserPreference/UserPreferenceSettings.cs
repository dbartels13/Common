using System.Threading.Tasks;
using Sphyrnidae.Common.Lookup;
using Sphyrnidae.Common.UserPreference.Interfaces;

namespace Sphyrnidae.Common.UserPreference
{
    public abstract class UserPreferenceSettings : BaseLookupSetting<UserPreferenceSetting>, IUserPreferenceSettings
    {
        public override string Key => "UserPreferenceSettings";

        public override int CachingSeconds => 1200; // 20 minutes

        public abstract Task<bool> Create(string key, string value);
        public abstract Task<bool> Update(string key, string value);
    }
}