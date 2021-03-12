using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Sphyrnidae.Common.Logging.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Dal
{
    /// <summary>
    /// Inherit from this class to talk to a MySql database
    /// </summary>
    public abstract class MySqlRepo : BaseRepo
    {
        protected MySqlRepo(ILogger logger) : base(logger) { }

        protected override IDbConnection GetConnection => new MySqlConnection(CnnStr);

        protected override Task PreCall(IDbConnection cnn, IDbTransaction trans)
            => cnn.State == ConnectionState.Open
                ? Task.CompletedTask
                : ((MySqlConnection)cnn).OpenAsync();
    }
}