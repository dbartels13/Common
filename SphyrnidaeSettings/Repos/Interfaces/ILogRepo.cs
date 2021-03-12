using System.Data;
using System.Threading.Tasks;
using Sphyrnidae.Common.Logging.Loggers.Models;

namespace Sphyrnidae.Settings.Repos.Interfaces
{
    public interface ILogRepo
    {
        string CnnStr { get; }
        Task<ulong?> InsertHeader(LogInsert model, IDbTransaction trans);
        Task<int> InsertApi(ulong id, string headers, string querystring, string form, string browser, IDbTransaction trans);
        Task<int> InsertDatabase(ulong id, string connection, string sqlParams, IDbTransaction trans);
        Task<int> InsertException(ulong id, string stackTrace, string source, string title, IDbTransaction trans);
        Task<int> InsertRequest(ulong id, string route, string method, string data, IDbTransaction trans);
        Task<int> InsertMisc(ulong id, string key, string value, IDbTransaction trans);
        Task<int> InsertTiming(ulong id, long milliseconds, IDbTransaction trans);
        Task<int> InsertResult(ulong id, string result, int statusCode, IDbTransaction trans);
    }
}