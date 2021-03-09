using System.Threading.Tasks;
using Sphyrnidae.Common.Lookup;

namespace Sphyrnidae.Common.UserPreference.Interfaces
{
    /// <summary>
    /// User Preferences
    /// </summary>
    public interface IUserPreferenceSettings : ILookupSettings<UserPreferenceSetting>
    {
        /// <summary>
        /// Creates a new user preference
        /// </summary>
        /// <remarks>
        /// This *should* clear out the user cache
        /// </remarks>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        /// <returns>True/False for if this was successfully created</returns>
        Task<bool> Create(string key, string value);

        /// <summary>
        /// Updates an existing user preference
        /// </summary>
        /// <remarks>
        /// This *should* clear out the user cache
        /// </remarks>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        /// <returns>True/False for if this was successfully updated</returns>
        Task<bool> Update(string key, string value);
    }
}