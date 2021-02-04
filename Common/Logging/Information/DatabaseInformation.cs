using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Diagnostics;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Logging.Information
{
    /// <inheritdoc />
    /// <summary>
    /// Collection of information used for database calls
    /// </summary>
    /// <remarks>This is only used internally by the loggers. You should have no interaction with this class</remarks>
    public class DatabaseInformation : TimerBaseInformation
    {
        public static string ConnectionKey => "Connection";
        public static string ParametersKey => "SQL Parameters";

        public override string LongRunningName => $"Database-{AppSettings.Name}-{CnnName}-{Message}";

        private string CnnName { get; set; }
        private object Args { get; set; }
        private NameValueCollection SqlParams { get; set; }

        public DatabaseInformation(ILoggerInformation info, IApplicationSettings appSettings)
            : base(info, appSettings)
        {
            Category = "Database";
        }

        public virtual void Initialize(string cnnName, string command, object args)
        {
            InitializeTimer(TraceEventType.Verbose, command);
            CnnName = cnnName;
            Args = args;
        }

        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);

            HighProperties.Add(ConnectionKey, CnnName);

            if (!config.Include(ParametersKey))
                return;

            SetSqlParams();
            var hideKeys = config.HideKeys();
            MedProperties.Add(ParametersKey, SqlParams.AsString(hideKeys));
        }

        private void SetSqlParams()
        {
            SqlParams ??= SafeTry.IgnoreException(() =>
            {
                var items = new NameValueCollection();

                if (Args is DbParameterCollection parameterCollection)
                {
                    foreach (DbParameter p in parameterCollection)
                        items.Add(p.ParameterName, p.Value.ToString());
                }
                else
                {
                    var s = Args.SerializeJson(); // This basically converts it
                    var dict = s.DeserializeJson<Dictionary<string, object>>();
                    if (dict == null)
                        items.Add("Raw", s);
                    else
                    {
                        foreach (var (key, value) in dict)
                            items.Add(key, value.SerializeJson());
                    }
                }

                return items;
            }, new NameValueCollection { { "Parameters", "Unknown" } });
        }
    }
}
