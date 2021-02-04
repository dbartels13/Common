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

        protected override async Task PreCall(IDbConnection cnn, IDbTransaction trans)
        {
            if (cnn.State != ConnectionState.Open)
                await ((MySqlConnection)cnn).OpenAsync();
        }
    }
}