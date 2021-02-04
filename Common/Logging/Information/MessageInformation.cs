using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for general logging
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class MessageInformation : BaseLogInformation
    {
        public override string Type => "Message";

        public MessageInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(TraceEventType severity, string message, string category)
        {
            InitializeBase(severity);
            Message = message;
            Category = category;
        }
    }
}