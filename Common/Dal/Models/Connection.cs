using System.Data;

namespace Sphyrnidae.Common.Dal.Models
{
    /// <summary>
    /// A database connection
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// The connection string
        /// </summary>
        public string CnnStr { get; set; }

        /// <summary>
        /// The actual connection
        /// </summary>
        public IDbConnection Cnn { get; set; }
    }
}
