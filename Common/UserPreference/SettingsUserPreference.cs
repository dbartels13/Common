using System.Threading.Tasks;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.Extensions;
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
        /// <param name="service">The user preferences settings</param>
        /// <param name="email">The implementation of the IEmail interface</param>
        /// <param name="app">The implementation of the IApplicationSettings interface</param>
        /// <param name="cache">The ICache implementation</param>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        /// <returns>True/False for if this was successfully created</returns>
        public static async Task<bool> Create(
            IUserPreferenceSettings service,
            IEmail email,
            IApplicationSettings app,
            ICache cache,
            string key,
            string value)
        {
            var success = await SafeTry.EmailException(
                email,
                app,
                async () => await service.Create(key, value)
            );

            return Caching.Remove(cache, service.Key).IsDefault() && success;
        }

        /// <summary>
        /// Updates the user preference and clears cache so that it can be retrieved next time
        /// </summary>
        /// <param name="service">The user preferences settings</param>
        /// <param name="email">The implementation of the IEmail interface</param>
        /// <param name="app">The implementation of the IApplicationSettings interface</param>
        /// <param name="cache">The ICache implementation</param>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        /// <returns>True/False for if this was successfully updated</returns>
        public static async Task<bool> Update(
            IUserPreferenceSettings service,
            IEmail email,
            IApplicationSettings app,
            ICache cache,
            string key,
            string value)
        {
            var success = await SafeTry.EmailException(
                email,
                app,
                async () => await service.Update(key, value)
            );

            return Caching.Remove(cache, service.Key).IsDefault() && success;
        }
    }
}
