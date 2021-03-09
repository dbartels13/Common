using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.FeatureToggle.Interfaces;
using Sphyrnidae.Common.Lookup;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.FeatureToggle
{
    /// <summary>
    /// Wrapper/Helper class around feature toggles
    /// </summary>
    public class SettingsFeatureToggle : SettingsLookup<IFeatureToggleSettings, FeatureToggleSetting>
    {
        /// <summary>
        /// Retrieves a feature toggle value as a boolean - use this if you are doing multiple lookups to avoid doing ServiceLocation each time on the service/cache
        /// </summary>
        /// <remarks>
        /// All of the following values will return true (case insensitive):
        ///     TRUE
        ///     T
        ///     YES
        ///     Y
        ///     ON
        ///     1
        ///     CHECK
        ///     CHECKED
        /// 
        /// All of the following values will return false (case insensitive):
        ///     FALSE
        ///     F
        ///     NO
        ///     N
        ///     OFF
        ///     0
        ///     UNCHECK
        ///     UNCHECKED
        ///
        /// Anything else will also return the default value
        /// </remarks>
        /// <param name="services">The services needed for the actual lookup</param>
        /// <param name="name">The name of the feature toggle to retrieve</param>
        /// <param name="defaultValue">
        /// If the feature toggle is not found in the collection from IFeatureToggle.GetAll(), this will be returned instead
        /// Default: false
        /// </param>
        /// <returns>The bool value of the feature toggle</returns>
        public static bool IsEnabled(IFeatureToggleServices services, string name, bool defaultValue = false)
        {
            var enabled = Get(services, name, defaultValue.ToString());
            return enabled.ToBool(defaultValue);
        }
    }
}
