using Sphyrnidae.Common.Logging.Models;

namespace Sphyrnidae.Common.Alerts
{
    /// <summary>
    /// If you don't wish to have alerts in the system
    /// </summary>
    public class AlertNone : IAlert
    {
        /// <summary>
        /// Always responds with 'false'
        /// </summary>
        /// <param name="responseInfo">The collected information about the Http Response</param>
        /// <returns>false</returns>
        public bool HttpResponseAlert(HttpResponseInfo responseInfo) => false;

        /// <summary>
        /// Always responds with 0 (only a value of 1 or more will trigger an alert)
        /// </summary>
        /// <param name="name">The name of the item being looked up</param>
        /// <returns>0</returns>
        public long MaxMilliseconds(string name) => 0;
    }
}
