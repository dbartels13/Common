using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for middleware calls
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class MiddlewareInformation : TimerBaseInformation
    {
        public MiddlewareInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings)
        {
            Category = "Middleware";
        }

        public virtual void Initialize(string name) => InitializeTimer(TraceEventType.Information, name);
    }
}