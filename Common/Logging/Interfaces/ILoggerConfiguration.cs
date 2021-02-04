namespace Sphyrnidae.Common.Logging.Interfaces
{
    /// <summary>
    /// All configurations needed for the default logging implementation
    /// </summary>
    /// <remarks>None of these are of type 'Task' because they use SettingsLookup which is synchronous</remarks>
    public interface ILoggerConfiguration
    {
        /// <summary>
        /// Global flag to be set to possibly disable logging completely (scope=request)
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Specifies if a logging type is enabled
        /// </summary>
        /// <param name="type">The "Type" of the BaseLogInformation</param>
        /// <returns>True/False</returns>
        bool TypeEnabled(string type);

        /// <summary>
        /// Is the optional item supposed to be included in the log
        /// </summary>
        /// <param name="item">The name of the item to include</param>
        /// <returns>True/False</returns>
        bool Include(string item);

        /// <summary>
        /// Specifies if a logger (eg. file, database, etc) is enabled for a certain log type
        /// </summary>
        /// <param name="name">The "Name" of the BaseLogger</param>
        /// <param name="type">The "Type" of the BaseLogInformation</param>
        /// <returns>True/False</returns>
        bool LoggerEnabled(string name, string type);

        /// <summary>
        /// Listing of "keys" that when logged will have it's values obscured
        /// </summary>
        /// <returns>Collection of keys to obscure</returns>
        CaseInsensitiveBinaryList<string> HideKeys();

        /// <summary>
        /// The maximum length of something being logged (to prevent huge things such as images or large collections)
        /// </summary>
        /// <param name="name">The "Name" of the BaseLogger</param>
        /// <returns>The maximum length for something to log</returns>
        int MaxLength(string name);
    }
}