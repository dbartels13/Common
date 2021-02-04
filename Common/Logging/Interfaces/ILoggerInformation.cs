using Sphyrnidae.Common.Authentication;

namespace Sphyrnidae.Common.Logging.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoggerInformation
    {
        /// <summary>
        /// This prefix will help you so that you can order all of your log statements across multiple applications
        /// </summary>
        string LogOrderPrefix { get; }

        /// <summary>
        /// The ID for the request (or run-time execution)
        /// </summary>
        string RequestId { get; }

        /// <summary>
        /// The current logging order for the current application
        /// </summary>
        char LoggingOrder { get; set; }

        /// <summary>
        /// The identity of the user executing the call (can be null, but certain logging properties will also be null)
        /// </summary>
        SphyrnidaeIdentity Identity { get; }

        /// <summary>
        /// The ID associated with a group of calls
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// The method being executed (Eg. POST /api/v1/MyRoute)
        /// </summary>
        string Method { get; }
    }
}