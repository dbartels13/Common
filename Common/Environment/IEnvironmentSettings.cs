// ReSharper disable CommentTypo
namespace Sphyrnidae.Common.Environment
{
    /// <summary>
    /// Interface definition for reading environmental settings
    /// </summary>
    /// <remarks>
    /// An environmental setting is anything that is static per application/environment.
    /// It is either read from the actual environment (eg. AWS), or from appsettings.json (others as well).
    /// The main entry point to obtain an environmental setting is via SettingsEnvironmental class.
    /// </remarks>
    public interface IEnvironmentSettings
    {
        /// <summary>
        /// Obtains the environmental setting and returns the value
        /// </summary>
        /// <param name="name">The name of the environmental setting</param>
        /// <returns>The value of the environmental setting</returns>
        string Get(string name);
    }
}