using Sphyrnidae.Common.Lookup;

namespace Sphyrnidae.Common.UserPreference.Interfaces
{
    /// <summary>
    /// Services required for executing a user preference lookup
    /// </summary>
    public interface IUserPreferenceServices : ILookupServices<IUserPreferenceSettings, UserPreferenceSetting>
    {
    }
}
