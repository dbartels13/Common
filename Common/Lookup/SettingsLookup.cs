using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Utilities;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.Lookup
{
    /// <summary>
    /// Retrieves a setting that can be dynamically set
    /// </summary>
    /// <remarks>
    /// The actual retrieval of the setting is retrieved from cache.
    /// The setting of cache, and the time to cache is implemented by ILookupSettings, so this interface must be implemented
    /// </remarks>
    public abstract class SettingsLookup<T, TS> where T : ILookupSettings<TS> where TS : LookupSetting
    {
        /// <summary>
        /// There must be a minimum level of caching, otherwise it will be a recursive loop
        /// </summary>
        private const int MinimumCacheSeconds = 30;

        #region Get

        /// <summary>
        /// Retrieves a setting
        /// </summary>
        /// <param name="services">The collection of lookup services</param>
        /// <param name="name">The name of the setting to retrieve</param>
        /// <param name="defaultValue">If the setting is not found, or there is an error retrieving the setting, this will be returned instead</param>
        /// <returns>The string setting (If you need to convert to something else, that will be done outside this call)</returns>
        public static string Get(ILookupServices<T, TS> services, string name, string defaultValue)
            => SafeTry.IgnoreException(
                () =>
                {
                    var service = services.Service;
                    if (service.Key == null)
                        return defaultValue;

                    var cachingSeconds = CachingSeconds(service);

                    // Get the collection of settings inside a Lock (this is possibly recursive, so can't use semaphore locking)
                    var settingsCollection = NamedLocker.Lock($"{service.Key}_Locker",
                        num => num > 1
                            ? null // If recursive (will be allowed in lock again), the default value will be used instead of a real lookup
                            : GetItems(cachingSeconds, service, services.Cache));

                    // If the above failed to find anything (eg. no settings, or a recursive call)
                    if (settingsCollection.IsDefault())
                        return defaultValue;

                    // Return the setting value (or default if no setting exists)
                    var setting = service.GetItem(settingsCollection, name);
                    return setting.IsDefault() ? defaultValue : service.GetValue(setting);
                },
                defaultValue
            );
        #endregion

        #region Helper Methods
        private static int CachingSeconds(T service)
        {
            var cachingSeconds = service.CachingSeconds;
            return cachingSeconds < MinimumCacheSeconds ? MinimumCacheSeconds : cachingSeconds;
        }

        private static CaseInsensitiveBinaryList<TS> GetItems(int cachingSeconds, T service, ICache cache)
        {
            // Possibly populate the cache and pull from that cache
            // If this fails for whatever reason, we still want to cache an empty list (or null)
            // This is done so that the consuming API will not repeatedly make calls that will fail
            // It will be up to all consumers to likely specify a default value to properly handle this
            cache.Options.Seconds = cachingSeconds;
            return Caching.Get(
                cache,
                service.Key,
                () => service
                    .GetAll()
                    .Result // We are inside a possibly recursive lock, so cannot await this with semaphore locking
                    .ToCaseInsensitiveBinaryList(x => x.Key)
            );
        }
        #endregion
    }
}
