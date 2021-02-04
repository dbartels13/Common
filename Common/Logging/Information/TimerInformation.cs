using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for custom timing calls
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class TimerInformation : TimerBaseInformation
    {
        public TimerInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings)
        {
            Category = "Timer";
        }

        public virtual void Initialize(string name) => InitializeTimer(TraceEventType.Information, name);
    }
}