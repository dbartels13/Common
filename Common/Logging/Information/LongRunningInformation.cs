using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for a long running timer
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class LongRunningInformation : BaseLogInformation
    {
        public static string StaticType = "Long Running";
        public override string Type => StaticType;

        public LongRunningInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings) { }

        public virtual void Initialize(TimerBaseInformation prevInfo)
        {
            InitializeBase(TraceEventType.Warning, prevInfo);

            Message = $"LONG RUNNING {prevInfo.GetElapsedStr()}; {prevInfo.Message}";
            Category = $"{prevInfo.Category} Long Running";
        }
    }
}