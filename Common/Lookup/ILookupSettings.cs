using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.Lookup
{
    /// <summary>
    /// Interface definition for looking up settings (Variables, features, user preferences, etc)
    /// </summary>
    /// <remarks>
    /// This interface is not meant to be directly inherited from.
    /// You should inherit your specific interface from this
    /// </remarks>
    public interface ILookupSettings<T> where T : LookupSetting
    {
        #region Setup
        /// <summary>
        /// Name of the unique key for this setting (can be sub-divided)
        /// </summary>
        string Key { get; }

        /// <summary>
        /// How long should the full collection of settings be cached
        /// </summary>
        /// <returns>A value in seconds</returns>
        int CachingSeconds { get; }
        #endregion

        #region Main Get
        /// <summary>
        /// Retrieves all of the settings at once so that these can be cached locally
        /// </summary>
        /// <returns>The full collection of settings</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Retrieves a single setting from the settingsCollection (populated from GetAll)
        /// </summary>
        /// <param name="settingsCollection">The full collection of settings</param>
        /// <param name="key">The key to match on</param>
        /// <returns>The setting (or default if not found)</returns>
        T GetItem(CaseInsensitiveBinaryList<T> settingsCollection, string key);

        /// <summary>
        /// Retrieves the value from the setting
        /// </summary>
        /// <param name="setting">The setting</param>
        /// <returns>The value</returns>
        string GetValue(T setting);
        #endregion
    }
}
