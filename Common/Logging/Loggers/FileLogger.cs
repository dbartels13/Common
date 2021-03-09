using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Loggers.Models;
using Sphyrnidae.Common.Paths;
using Sphyrnidae.Common.Utilities;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.Variable.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging.Loggers
{
    /// <inheritdoc />
    public class FileLogger : BaseLogger
    {
        public override string Name => "File";
        public override bool IncludeIdentity => true;
        public override bool IncludeStatic => true;
        public override bool IncludeHigh => true;
        public override bool IncludeMed => true;
        public override bool IncludeLow => false;

        private IVariableServices Variable { get; }
        public FileLogger(IVariableServices variable) => Variable = variable;

        protected override async Task DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
        {
            var isHighlighted = info.Severity == TraceEventType.Warning || info.Severity == TraceEventType.Error ||
                                info.Severity == TraceEventType.Critical;
            var logItems = new List<string>
            {
                info.TimestampStr,
                isHighlighted ? $"***** {info.Type} *****" : info.TypeStr,
                info.IdentifierStr,
                info.TimestampStr,
                info.SeverityStr,
                info.RequestStr,
                info.OrderStr,
                info.SessionStr(),
                info.UserStr,
                info.MessageStr.ShortenWithEllipses(maxLength),
                info.CategoryStr.ShortenWithEllipses(maxLength)
            };
            foreach (var (key, value) in model.Other)
                logItems.Add($"{key}: {value}");

            logItems.Add(""); // Blank line for spacing
            logItems.Add(""); // Blank line for spacing

            // Always log to regular file
            var filename = GetFileName(false);
            await NamedLocker.LockAsync(filename, async () => await File.AppendAllLinesAsync(filename, logItems));

            // If highlighted, log again in a special file
            if (!isHighlighted)
                return;

            var filenameHighlight = GetFileName(true);
            await NamedLocker.LockAsync(filenameHighlight, async () => await File.AppendAllLinesAsync(filenameHighlight, logItems));
        }

        protected override async Task DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
        {
            var logItems = new List<string>
            {
                info.TimestampStr,
                $"{info.Type} complete.",
                info.IdentifierStr,
                info.GetElapsedStr()
            };
            foreach (var (key, value) in model.Other)
                logItems.Add($"{key}: {value}");

            logItems.Add(""); // Blank line for spacing
            logItems.Add(""); // Blank line for spacing

            var filename = GetFileName(false);
            await NamedLocker.LockAsync(filename, async () => await File.AppendAllLinesAsync(filename, logItems));
        }

        private string GetFileName(bool highlight)
        {
            // If we have a log folder specified, use that, otherwise the server path
            var logPath = SettingsVariable.Get(Variable, "Logging_FilePath", "");
            var builder = new RelativePathBuilder(string.IsNullOrWhiteSpace(logPath) ? "Logs" : logPath);

            if (highlight)
                builder = builder.AddPathSegment("Highlights");
            logPath = builder.Build();

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            var dt = DateTime.Now;

            builder = builder.AddPathSegment(
                highlight
                    ? $"log_highlights_{dt.Year}_{dt:MMM}.log"
                    : $"log_{dt.DayOfYear}.log");

            return builder.Build();
        }
    }
}
