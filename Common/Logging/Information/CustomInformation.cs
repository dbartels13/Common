using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Serialize;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for general logging
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class CustomInformation : MessageInformation
    {
        public override string Type => Category;

        /// <summary>
        /// Any misc items (as any object) that the user would like to have in the log
        /// </summary>
        public object Misc { get; private set; }

        public CustomInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public void SetType(string type) => Category = type;

        public virtual void Initialize(TraceEventType severity, string category, string message, object o)
        {
            base.Initialize(severity, message, category);
            Misc = o;
        }

        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);

            if (Misc.IsPopulated())
                HighProperties.Add("Misc", Misc.SerializeJson());
        }
    }
}