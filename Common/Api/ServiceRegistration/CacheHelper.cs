using Sphyrnidae.Common.Environment;

namespace Sphyrnidae.Common.Api.ServiceRegistration
{
    public static class CacheHelper
    {
        public static string RedisUrl(IEnvironmentSettings env) => SettingsEnvironmental.Get(env, "URL:Redis");
    }
}