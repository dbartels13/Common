using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    public abstract class CustomTimerInformation3 : CustomTimerInformation
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

        protected CustomTimerInformation3(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(string name, object o)
        {
            SetType(Type); // Sets the category
            base.Initialize(LogSeverity, name, o);
        }
    }
}