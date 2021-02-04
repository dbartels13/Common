using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Serialize;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for custom timing calls
    /// </summary>
    public class CustomTimerInformation : TimerBaseInformation
    {
        /// <summary>
        /// Any misc items (as any object) that the user would like to have in the log
        /// </summary>
        public object MiscStart { get; private set; }

        /// <summary>
        /// Any misc items (as any object) that the user would like to capture on the update
        /// </summary>
        public object MiscEnd { get; set; }

        public CustomTimerInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public void SetType(string type) => Category = type;

        public virtual void Initialize(TraceEventType severity, string name, object o)
        {
            InitializeTimer(severity, name);
            MiscStart = o;
        }

        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);

            if (MiscStart.IsPopulated())
                HighProperties.Add("Misc", MiscStart.SerializeJson());
        }

        public override void UpdateProperties(ILoggerConfiguration config)
        {
            base.UpdateProperties(config);

            if (MiscEnd.IsPopulated())
                HighProperties.Add(ResultBaseInformation.ResultKey, MiscEnd.SerializeJson());
        }
    }
}