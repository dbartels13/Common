using System.Collections.Generic;
using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for custom timing calls
    /// </summary>
    public abstract class CustomTimerInformation<T1, T2> : TimerBaseInformation
    {
        /// <summary>
        /// The first object to log
        /// </summary>
        public T1 ObjStart { get; private set; }

        /// <summary>
        /// Any updated logic to log at the end (must be set prior to use)
        /// </summary>
        public T2 ObjEnd { get; set; }

        /// <summary>
        /// Your custom object will need to have a defined severity
        /// </summary>
        protected abstract TraceEventType CustomSeverity { get; }

        /// <summary>
        /// Your custom object will need to have a defined type (will also be the category)
        /// </summary>
        protected abstract string CustomType { get; }

        /// <summary>
        /// Your custom object can either hard-code in a message, or rely on this to be passed in
        /// </summary>
        protected virtual string CustomMessage { get; } = null;

        public CustomTimerInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(T1 obj, string message = "")
        {
            InitializeTimer(CustomSeverity, CustomMessage ?? message);
            Category = CustomType;
            ObjStart = obj;
        }

        protected abstract void SetHighProperties(Dictionary<string, string> highProperties, T1 obj);
        protected abstract void SetMedProperties(Dictionary<string, string> medProperties, T1 obj);
        protected abstract void SetLowProperties(Dictionary<string, string> lowProperties, T1 obj);
        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);

            SetHighProperties(HighProperties, ObjStart);
            SetMedProperties(MedProperties, ObjStart);
            SetLowProperties(LowProperties, ObjStart);
        }

        protected abstract void UpdateHighProperties(Dictionary<string, string> highProperties, T2 obj);
        protected abstract void UpdateMedProperties(Dictionary<string, string> medProperties, T2 obj);
        protected abstract void UpdateLowProperties(Dictionary<string, string> lowProperties, T2 obj);
        public override void UpdateProperties(ILoggerConfiguration config)
        {
            base.UpdateProperties(config);

            UpdateHighProperties(HighProperties, ObjEnd);
            UpdateMedProperties(MedProperties, ObjEnd);
            UpdateLowProperties(LowProperties, ObjEnd);
        }
    }
}