using System.Threading.Tasks;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Lookup;
using Sphyrnidae.Common.UserPreference.Interfaces;
using Sphyrnidae.Common.Utilities;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.UserPreference
{
    /// <summary>
    /// Wrapper/Helper class around user preferences
    /// </summary>
    public class SettingsUserPreference : SettingsLookup<IUserPreferenceSettings, UserPreferenceSetting>
    {
        /// <summary>
        /// Creates the user preference and clears cache so that it can be retrieved next time
        /// </summary>
        /// <param name="logger">The ILogger implementation</param>
        /// <param name="service">The user preferences settings</param>
        /// <param name="cache">The ICache implementation</param>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        /// <returns>True/False for if this was successfully created</returns>
        public static async Task<bool> Create(
            ILogger logger,
            IUserPreferenceSettings service,
            ICache cache,
            string key,
            string value)
        {
            var createTask = SafeTry.LogException(
                logger,
                async () => await service.Create(key, value)
            );

            var removeTask = Caching.RemoveAsync(cache, service.Key);
            await Task.WhenAll(createTask, removeTask);
            return createTask.Result && removeTask.Result.IsDefault();
        }

        /// <summary>
        /// Updates the user preference and clears cache so that it can be retrieved next time
        /// </summary>
        /// <param name="logger">The ILogger implementation</param>
        /// <param name="service">The user preferences settings</param>
        /// <param name="cache">The ICache implementation</param>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        /// <returns>True/False for if this was successfully updated</returns>
        public static async Task<bool> Update(
            ILogger logger,
            IUserPreferenceSettings service,
            ICache cache,
            string key,
            string value)
        {
            var updateTask = SafeTry.LogException(
                logger,
                async () => await service.Update(key, value)
            );

            var removeTask = Caching.RemoveAsync(cache, service.Key);
            await Task.WhenAll(updateTask, removeTask);
            return updateTask.Result && removeTask.Result.IsDefault();
        }
    }
}
