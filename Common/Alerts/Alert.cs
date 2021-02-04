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

        public virtual long MaxMilliseconds(string name)
        {
            var max = SettingsVariable.Get(Variable, name, "0");
            return max.ToLong(0);
        }

        // I'll leave this up to each individual application to implement this
        public virtual bool HttpResponseAlert(HttpResponseInfo responseInfo) => false;
    }
}