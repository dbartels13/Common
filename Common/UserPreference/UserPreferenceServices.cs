using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.UserPreference.Interfaces;

namespace Sphyrnidae.Common.UserPreference
{
    public class UserPreferenceServices : IUserPreferenceServices
    {
        public ICache Cache { get; }
        public IUserPreferenceSettings Service { get; }

        public UserPreferenceServices(ICache cache, IUserPreferenceSettings service)
        {
            Cache = cache;
            Service = service;
        }
    }
}
