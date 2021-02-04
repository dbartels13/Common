using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for logging unauthorized requests
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class UnauthorizedInformation : BaseLogInformation
    {
        public override string Type => "Unauthorized";

        public UnauthorizedInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(string message)
        {
            InitializeBase(TraceEventType.Warning);

            Message = message;
            Category = Info.Method;
        }
    }
}