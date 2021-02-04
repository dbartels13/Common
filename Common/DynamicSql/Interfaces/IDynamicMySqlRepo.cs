//using System.Collections.Generic;
//using System.Threading.Tasks;
//// ReSharper disable UnusedMember.Global

//namespace Sphyrnidae.Common.DynamicSql.Interfaces
//{
//    public interface IDynamicMySqlRepo
//    {
//        Task<IEnumerable<T>> List<T>(string cnnStr, string sql, List<KeyValuePair<string, object>> parameters);
//        Task<T> Get<T>(string cnnStr, string sql, List<KeyValuePair<string, object>> parameters);
//        Task<T> Scalar<T>(string cnnStr, string sql, List<KeyValuePair<string, object>> parameters);
//    }
//}