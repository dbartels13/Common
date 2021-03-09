using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Sphyrnidae.Common.Dal;
using Sphyrnidae.Common.DynamicSql.Interfaces;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.DynamicSql
{
    public class DynamicMySqlRepo : MySqlRepo, IDynamicMySqlRepo
    {
        private string _cnnStr;
        public override string CnnStr => _cnnStr;
        protected override string CnnName => "Dynamic MySql";

        public DynamicMySqlRepo(ILogger logger) : base(logger) { }

        public async Task<IEnumerable<T>> List<T>(string cnnStr, string sql, List<KeyValuePair<string, object>> parameters)
        {
            _cnnStr = cnnStr;
            return await GetListSQLAsync<T>(sql, GetParameters(parameters));
        }

        public async Task<T> Get<T>(string cnnStr, string sql, List<KeyValuePair<string, object>> parameters)
        {
            _cnnStr = cnnStr;
            return await GetSQLAsync<T>(sql, GetParameters(parameters));
        }

        public async Task<T> Scalar<T>(string cnnStr, string sql, List<KeyValuePair<string, object>> parameters)
        {
            _cnnStr = cnnStr;
            return await ScalarSQLAsync<T>(sql, GetParameters(parameters));
        }

        private static object GetParameters(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var result = new ExpandoObject();
            foreach (var (key, value) in parameters)
                result.TryAdd(key, value);
            return result;
        }
    }
}