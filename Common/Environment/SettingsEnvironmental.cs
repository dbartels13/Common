namespace Sphyrnidae.Common.Environment
{
    /// <summary>
    /// Wrapper/Helper class around retrieving environmental variables
    /// </summary>
    public static class SettingsEnvironmental
    {
        /// <summary>
        /// Retrieves an environmental variable value
        /// </summary>
        /// <param name="settings">The instance of the IEnvironmentSettings interface</param>
        /// <param name="name">The name of the environmental variable</param>
        /// <param name="defaultValue">If the environmental variable is not found in the collection from IEnvironmentSettings.Get(), this will be returned instead</param>
        /// <returns>The string value of the environmental variable (If you need to convert to something else, that will be done outside this call)</returns>
        public static string Get(IEnvironmentSettings settings, string name, string defaultValue = null) => settings.Get(name) ?? defaultValue;
    }
}