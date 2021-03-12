using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Sphyrnidae.Common.Dal;
using Sphyrnidae.Common.Dal.Models;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Loggers;
using Sphyrnidae.Common.Logging.Loggers.Models;
using Sphyrnidae.Settings.Repos.Interfaces;

namespace Sphyrnidae.Settings.Loggers
{
    /// <inheritdoc />
    public class DatabaseLogger : BaseLogger
    {
        public override string Name => "Database";
        public override bool IncludeIdentity => true;
        public override bool IncludeStatic => true;
        public override bool IncludeHigh => true;
        public override bool IncludeMed => true;
        public override bool IncludeLow => true;

        private static string DatabaseKey => "Database_Identity";

        private ILogRepo Repo { get; }
        public DatabaseLogger(ILogRepo repo) => Repo = repo;

        protected override Task DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
            => DoInsertAsync(model, info);

        private Task DoInsertAsync(LogInsert model, BaseLogInformation info)
        {
            return Transaction<SqlConnection>.Run(
                null,
                Repo.CnnStr,
                async trans =>
            {
                // Header
                var id = (await Repo.InsertHeader(model, trans)).Value;
                info.NotResetProperties.Add(DatabaseKey, id.ToString()); // Save this off for the update

                var inserts = new List<Task>();

                // Api
                if (model.Other.ContainsKey(ApiInformation.HeadersKey) ||
                    model.Other.ContainsKey(ApiInformation.QueryStringKey) ||
                    model.Other.ContainsKey(ApiInformation.FormKey) ||
                    model.Other.ContainsKey(ApiInformation.BrowserKey))
                {
                    var headers = model.Other.ContainsKey(ApiInformation.HeadersKey)
                        ? model.Other[ApiInformation.HeadersKey]
                        : null;
                    var querystring = model.Other.ContainsKey(ApiInformation.QueryStringKey)
                        ? model.Other[ApiInformation.QueryStringKey]
                        : null;
                    var form = model.Other.ContainsKey(ApiInformation.FormKey)
                        ? model.Other[ApiInformation.FormKey]
                        : null;
                    var browser = model.Other.ContainsKey(ApiInformation.BrowserKey)
                        ? model.Other[ApiInformation.BrowserKey]
                        : null;

                    inserts.Add(Repo.InsertApi(id, headers, querystring, form, browser, trans));
                }

                // Database
                if (model.Other.ContainsKey(DatabaseInformation.ConnectionKey))
                {
                    var connection = model.Other[DatabaseInformation.ConnectionKey];
                    var parameters = model.Other.ContainsKey(DatabaseInformation.ParametersKey)
                        ? model.Other[DatabaseInformation.ParametersKey]
                        : null;

                    inserts.Add(Repo.InsertDatabase(id, connection, parameters, trans));
                }

                // Exceptions
                if (model.Other.ContainsKey(ExceptionInformation.SourceKey))
                {
                    var stackTrace = model.Other[ExceptionInformation.StackTraceKey];
                    var source = model.Other[ExceptionInformation.SourceKey];
                    var title = model.Other[ExceptionInformation.TitleKey];

                    inserts.Add(Repo.InsertException(id, stackTrace, source, title, trans));
                }

                // Request
                if (model.Other.ContainsKey(ResultBaseInformation.RouteKey) ||
                    model.Other.ContainsKey(ResultBaseInformation.MethodKey) ||
                    model.Other.ContainsKey(ResultBaseInformation.RequestDataKey))
                {
                    var route = model.Other.ContainsKey(ResultBaseInformation.RouteKey)
                        ? model.Other[ResultBaseInformation.RouteKey]
                        : null;
                    var method = model.Other.ContainsKey(ResultBaseInformation.MethodKey)
                        ? model.Other[ResultBaseInformation.MethodKey]
                        : null;
                    var data = model.Other.ContainsKey(ResultBaseInformation.RequestDataKey)
                        ? model.Other[ResultBaseInformation.RequestDataKey]
                        : null;

                    inserts.Add(Repo.InsertRequest(id, route, method, data, trans));
                }

                // Misc
                foreach (var (key, value) in model.Other.Where(x =>
                    !x.Key.Equals(ApiInformation.HeadersKey) &&
                    !x.Key.Equals(ApiInformation.QueryStringKey) &&
                    !x.Key.Equals(ApiInformation.FormKey) &&
                    !x.Key.Equals(ApiInformation.BrowserKey) &&
                    !x.Key.Equals(DatabaseInformation.ConnectionKey) &&
                    !x.Key.Equals(DatabaseInformation.ParametersKey) &&
                    !x.Key.Equals(ExceptionInformation.StackTraceKey) &&
                    !x.Key.Equals(ExceptionInformation.SourceKey) &&
                    !x.Key.Equals(ExceptionInformation.TitleKey) &&
                    !x.Key.Equals(ResultBaseInformation.RouteKey) &&
                    !x.Key.Equals(ResultBaseInformation.MethodKey) &&
                    !x.Key.Equals(ResultBaseInformation.RequestDataKey)))
                    inserts.Add(Repo.InsertMisc(id, key, value, trans));

                await Task.WhenAll(inserts);
                return TransactionResponse.Commit();
            });
        }

        protected override Task DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
            => DoUpdateAsync(model, info);

        private Task DoUpdateAsync(LogUpdate model, TimerBaseInformation info)
        {
            var id = info.NotResetProperties[DatabaseKey].ToULong("Database Header Record Id");
            return Transaction<SqlConnection>.Run(
                null,
                Repo.CnnStr,
                async trans =>
            {
                var updates = new List<Task> { Repo.InsertTiming(id, info.GetElapsed() ?? 0, trans)};

                if (model.Other.ContainsKey(ResultBaseInformation.ResultKey) ||
                    model.Other.ContainsKey(ResultBaseInformation.StatusCodeKey))
                {
                    var result = model.Other.ContainsKey(ResultBaseInformation.ResultKey)
                        ? model.Other[ResultBaseInformation.ResultKey]
                        : null;
                    var code = model.Other.ContainsKey(ResultBaseInformation.StatusCodeKey)
                        ? model.Other[ResultBaseInformation.StatusCodeKey]
                        : "0";

                    updates.Add(Repo.InsertResult(id, result, code.ToInt(0), trans));
                }

                await Task.WhenAll(updates);
                return TransactionResponse.Commit();
            });
        }
    }
}
