using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Loggers.Models;

namespace Sphyrnidae.Common.Logging.Loggers
{
    /// <inheritdoc />
    public class DebugLogger : BaseLogger
    {
        public override string Name => "Debug";
        public override bool IncludeIdentity => false;
        public override bool IncludeStatic => false;
        public override bool IncludeHigh => true;
        public override bool IncludeMed => false;
        public override bool IncludeLow => false;

        protected override Task DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
        {
            Debug.WriteLine(model.Message, info.CategoryStr);
            WriteOther(model.Other);
            return Task.CompletedTask;
        }

        protected override Task DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
        {
            Debug.WriteLine(info.Message, $"{info.CategoryStr} {info.GetElapsedStr()}");
            WriteOther(model.Other);
            return Task.CompletedTask;
        }

        private static void WriteOther(Dictionary<string, string> items)
        {
            foreach (var (key, value) in items)
                Debug.WriteLine(key, value);
        }
    }
}