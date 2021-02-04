using System;
using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for exception logging
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class ExceptionInformation : BaseLogInformation
    {
        public static string StackTraceKey => "Stack Trace";
        public static string SourceKey => "Source";
        public static string TitleKey => "Title";

        public override string Type => Severity == TraceEventType.Warning ? "Hidden Exception" : "Exception";

        private Exception Ex { get; set; }
        private bool MessageOnly { get; set; }
        private string Title { get; set; }
        private string StackTrace { get; set; }

        public ExceptionInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings)
        {
            Severity = TraceEventType.Error;
        }

        public void SetHidden() => Severity = TraceEventType.Warning;

        public virtual void Initialize(Exception ex, string title, bool messageOnly)
        {
            InitializeBase(Severity);
            Ex = ex;
            MessageOnly = messageOnly;
            Title = string.IsNullOrWhiteSpace(title) ? ex.GetType().ToString() : title;
            Category = SafeTry.IgnoreException(() => $"{(Severity == TraceEventType.Warning ? "Hidden " : "")}{Title} Exception", "");
        }

        public override void SetProperties(ILoggerConfiguration config)
        {
            Message = Ex.GetFullMessage();
            base.SetProperties(config);

            if (MessageOnly)
                return;

            StackTrace = Ex.GetStackTrace();
            HighProperties.Add(StackTraceKey, StackTrace);
            MedProperties.Add(SourceKey, Ex.Source);
            LowProperties.Add(TitleKey, Title);
        }
    }
}
