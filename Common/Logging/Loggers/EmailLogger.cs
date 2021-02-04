using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Sphyrnidae.Common.EmailUtilities;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Loggers.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Logging.Loggers
{
    /// <inheritdoc />
    public class EmailLogger : BaseLogger
    {
        public override string Name => "Email";
        public override bool IncludeHigh => true;
        public override bool IncludeMed => true;
        public override bool IncludeLow => false;

        private IWebHostEnvironment Host { get; }
        private IEmailServices Services { get; }
        public EmailLogger(IWebHostEnvironment host, IEmailServices services)
        {
            Host = host;
            Services = services;
        }

        protected override async Task DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
        {
            var subject =
                $"[{model.Identifier}] Log Insert for: {model.Application ?? "Unknown"} ({Host.EnvironmentName})";

            var body = $@"
{info.ApplicationStr}
Environment: {Host.EnvironmentName}
{info.MachineStr}

{info.TypeStr}
{info.IdentifierStr}
{info.TimestampStr}
{info.SeverityStr}
{info.RequestStr}
{info.OrderStr}
{info.SessionStr()}
{info.UserStr}
{info.CustomerStr}
{info.MessageStr.ShortenWithEllipses(maxLength)}
{info.CategoryStr.ShortenWithEllipses(maxLength)}
{string.Join("\r\n", model.Other)}
";

            await Email.SendAsync(Services, EmailType.Logging, subject, body);
        }

        protected override async Task DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
        {
            var subject = $"[{model.Identifier}] Completed";
            var body = $@"
{info.Type} Complete!
{info.IdentifierStr}
{info.GetElapsedStr()}
{string.Join("\r\n", model.Other)}
";

            await Email.SendAsync(Services, EmailType.Logging, subject, body);
        }
    }
}
