using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Loggers.Models;
using Sphyrnidae.Common.Serialize;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging.Loggers
{
    /// <inheritdoc />
    public class Log4NetLogger : BaseLogger
    {
        public override string Name => "Log4Net";
        public override bool IncludeIdentity => true;
        public override bool IncludeStatic => true;
        public override bool IncludeHigh => true;
        public override bool IncludeMed => true;
        public override bool IncludeLow => true;

        private ILogger Logger { get; }
        public Log4NetLogger(ILogger logger) => Logger = logger;

        protected override Task DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
        {
            var obj = model.SerializeJson();
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (model.Severity)
            {
                case TraceEventType.Critical:
                    Logger.LogCritical(obj);
                    break;
                case TraceEventType.Error:
                    Logger.LogError(obj);
                    break;
                case TraceEventType.Warning:
                    Logger.LogWarning(obj);
                    break;
                case TraceEventType.Information:
                    Logger.LogInformation(obj);
                    break;
                default:
                    Logger.LogDebug(obj);
                    break;
            }

            return Task.CompletedTask;
        }

        protected override Task DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
        {
            var obj = model.SerializeJson();
            Logger.LogTrace(obj);
            return Task.CompletedTask;
        }
    }
}
