using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Models;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.Variable.Interfaces;

namespace Sphyrnidae.Common.Alerts
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of IAlert which retrieves values from Variables
    /// </summary>
    public class Alert : IAlert
    {
        protected IVariableServices Variable { get; }
        public Alert(IVariableServices variable) => Variable = variable;

        /// <summary>
        /// Looks up the variable by "name" and determines how long is considered long running (variable value)
        /// </summary>
        /// <param name="name">The name of the item being looked up</param>
        /// <returns>If variable found, the value of that variable. Otherwise, 0</returns>
        public virtual long MaxMilliseconds(string name)
        {
            var max = SettingsVariable.Get(Variable, name, "0");
            return max.ToLong(0);
        }

        /// <summary>
        /// Always responds with 'false' (Will be up to each application/developer to override to customize this)
        /// </summary>
        /// <param name="responseInfo">The collected information about the Http Response</param>
        /// <returns>false</returns>
        public virtual bool HttpResponseAlert(HttpResponseInfo responseInfo) => false;
    }
}