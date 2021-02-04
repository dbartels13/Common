using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomInformation3 : CustomInformation
    {
        /// <summary>
        /// The type/category (allows this to be dis/enabled)
        /// </summary>
        public abstract string CustomType { get; }
        public override string Type => CustomType;

        /// <summary>
        /// The "Severity"
        /// </summary>
        public abstract TraceEventType LogSeverity { get; }

        protected CustomInformation3(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(string message, object o) => base.Initialize(LogSeverity, Type, message, o);
    }
}