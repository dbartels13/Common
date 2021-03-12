using System.Data;
using System.Threading.Tasks;
using Sphyrnidae.Common.Dal;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging;
using Sphyrnidae.Common.Logging.Loggers.Models;
using Sphyrnidae.Settings.Repos.Interfaces;

namespace Sphyrnidae.Settings.Repos
{
    public class LogRepo : MySqlRepo, ILogRepo
    {
        protected IEnvironmentSettings Env { get; }
        protected IEncryption Encrypt { get; }
        public LogRepo(IEnvironmentSettings env, IEncryption encrypt) : base(new NonLogger()) {
            Env = env;
            Encrypt = encrypt;
        }

        protected override string CnnName => "Logging";
        protected override bool DoLog => false;
        private static string _cnn;
        public override string CnnStr => _cnn ??= SettingsEnvironmental.Get(Env, "Cnn:Logging").Decrypt(Encrypt).Value;

        public Task<ulong?> InsertHeader(LogInsert model, IDbTransaction trans)
        {
            var parameters = new
            {
                p_Type = model.Type,
                p_Identifier = model.Identifier,
                p_Severity = model.Severity,
                p_Order = model.Order,
                p_RequestId = model.RequestId ?? "Unknown",
                p_Session = model.Session ?? "Unknown",
                p_Message = model.Message.ShortenWithEllipses(4000) ?? "",
                p_Category = model.Category.ShortenWithEllipses(4000) ?? "Unknown",
                p_Machine = model.Machine ?? "Unknown",
                p_Application = model.Application,
                p_UserId = model.UserId,
                p_CustomerId = model.Other[SphyrnidaeIdentity.CustomerIdKey]
            };
            return ScalarSPAsync<ulong?>("LogHeader_Insert", parameters, trans);
        }

        public Task<int> InsertApi(ulong id, string headers, string querystring, string form, string browser, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                Headers = headers,
                QueryString = querystring,
                Form = form,
                Browser = browser
            };
            return WriteSPAsync("LogApi_Insert", parameters, trans);
        }

        public Task<int> InsertDatabase(ulong id, string connection, string sqlParams, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                Connection = connection,
                Parameters = sqlParams
            };
            return WriteSPAsync("LogDatabase_Insert", parameters, trans);
        }

        public Task<int> InsertException(ulong id, string stackTrace, string source, string title, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                StackTrace = stackTrace,
                Source = source,
                Title = title
            };
            return WriteSPAsync("LogException_Insert", parameters, trans);
        }

        public Task<int> InsertRequest(ulong id, string route, string method, string data, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                Route = route,
                Method = method,
                Data = data
            };
            return WriteSPAsync("LogRequest_Insert", parameters, trans);
        }

        public Task<int> InsertMisc(ulong id, string key, string value, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                Key = key,
                Value = value
            };
            return WriteSPAsync("LogMisc_Insert", parameters, trans);
        }

        public Task<int> InsertTiming(ulong id, long milliseconds, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                Milliseconds = milliseconds
            };
            return WriteSPAsync("LogTiming_Insert", parameters, trans);
        }

        public Task<int> InsertResult(ulong id, string result, int statusCode, IDbTransaction trans)
        {
            var parameters = new
            {
                LogHeaderId = id,
                Result = result,
                StatusCode = statusCode
            };
            return WriteSPAsync("LogResult_Insert", parameters, trans);
        }
    }
}
