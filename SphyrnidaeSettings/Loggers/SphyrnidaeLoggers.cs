using System.Collections.Generic;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Logging.Loggers;
using Sphyrnidae.Settings.Repos.Interfaces;

namespace Sphyrnidae.Settings.Loggers
{
    /// <inheritdoc />
    public class SphyrnidaeLoggers : ILoggers
    {
        private ILogRepo Repo { get; }
        public SphyrnidaeLoggers(ILogRepo repo) => Repo = repo;

        public List<BaseLogger> All => new List<BaseLogger>
        {
            new DatabaseLogger(Repo),
            new DebugLogger()
            //new EmailLogger(),
            //new FileLogger(),
            //new Log4NetLogger(),
            //new AwsLogger(),
            //new AzureLogger()
        };
        /*
        public static AwsLogger GetAwsLogger(IServiceProvider sp)
        {
            var env = ServiceLocator.Get<IEnvironmentSettings>(sp);
            var credentials = new BasicAWSCredentials(SettingsEnvironmental.Get(env, "AWS:Key"), SettingsEnvironmental.Get(env, "AWS:Secret"));
            var region = RegionEndpoint.GetBySystemName(SettingsEnvironmental.Get(env, "AWS:Region"));
            var aws = new AmazonCloudWatchLogsClient(credentials, region);

            var app = ServiceLocator.Get<IApplicationSettings>(sp);
            var lockSuffix = Environment.MachineName + "-" + app.Name;

            var serializer = ServiceLocator.Get<ISerializationSettings>(sp);
            return new AwsLogger(aws, serializer.JsonSettings, lockSuffix);
        }
        */

        /*
        public static Log4NetLogger GetLog4NetLogger(IServiceProvider sp)
        {
            var loggerFactory = ServiceLocator.Get<ILoggerFactory>(sp);
            loggerFactory.AddLog4Net();

            var serializer = ServiceLocator.Get<ISerializationSettings>(sp);
            return new Log4NetLogger(loggerFactory.CreateLogger(""), serializer.JsonSettings);
        }
        */
    }
}
