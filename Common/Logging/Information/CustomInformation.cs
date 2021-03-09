using System.Collections.Generic;
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
    public abstract class CustomInformation<T> : BaseLogInformation
    {
        /// <summary>
        /// Any generic item that the user would like to have in the log
        /// </summary>
        public T Obj { get; private set; }

        /// <summary>
        /// Your custom object will need to have a defined severity
        /// </summary>
        protected abstract TraceEventType CustomSeverity { get; }

        /// <summary>
        /// Your custom object can either use the "Type" as the category, or define it's own
        /// </summary>
        protected virtual string CustomCategory { get; } = null;

        /// <summary>
        /// Your custom object can either hard-code in a message, or rely on this to be passed in
        /// </summary>
        protected virtual string CustomMessage { get; } = null;

        public CustomInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(T obj, string message = "")
        {
            InitializeBase(CustomSeverity);
            Category = CustomCategory ?? Type;
            Message = CustomMessage ?? message;
            Obj = obj;
        }

        protected abstract void SetHighProperties(Dictionary<string, string> highProperties, T obj);
        protected abstract void SetMedProperties(Dictionary<string, string> medProperties, T obj);
        protected abstract void SetLowProperties(Dictionary<string, string> lowProperties, T obj);

        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);

            SetHighProperties(HighProperties, Obj);
            SetMedProperties(MedProperties, Obj);
            SetLowProperties(LowProperties, Obj);
        }
    }
}