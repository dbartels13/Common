using Sphyrnidae.Common.Logging.Models;

namespace Sphyrnidae.Common.Alerts
{
    /// <summary>
    /// This will retrieve information necessary for sending alerts
    /// </summary>
    public interface IAlert
    {
        /// <summary>
        /// How to determine how long something is considered to be "long running"
        /// </summary>
        /// <param name="name">The name of the item being looked up</param>
        /// <returns>The number of milliseconds something is considered to be long running</returns>
        long MaxMilliseconds(string name);

        /// <summary>
        /// Application specific request on if this should cause an alert
        /// </summary>
        /// <param name="responseInfo">The collected information about the Http Response</param>
        /// <returns>True if this should cause an alert</returns>
        bool HttpResponseAlert(HttpResponseInfo responseInfo);
    }
}