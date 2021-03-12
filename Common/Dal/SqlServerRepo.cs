using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Dal
{
    /// <summary>
    /// Use this class to talk to a Sql Server database
    /// </summary>
    public abstract class SqlServerRepo : BaseRepo
    {
        protected SqlServerRepo(ILogger logger) : base(logger) { }

        protected override IDbConnection GetConnection => new SqlConnection(CnnStr);
        protected override Task PreCall(IDbConnection cnn, IDbTransaction trans)
            => cnn.State == ConnectionState.Open
                ? Task.CompletedTask
                : ((SqlConnection)cnn).OpenAsync();
    }
}