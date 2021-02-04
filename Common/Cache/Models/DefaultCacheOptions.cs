using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.Cache.Models
{
    public class DefaultCacheOptions
    {
        public CacheOptions Item { get; }
        public DefaultCacheOptions(IEnvironmentSettings env)
        {
            Item ??= new CacheOptions
            {
                UseDistributedCache = SettingsEnvironmental.Get(env, "Cache:Distributed", "true").ToBool(true),
                UseLocalCache = SettingsEnvironmental.Get(env, "Cache:Local", "true").ToBool(true)
            };
        }
    }
}