using System.Collections.Generic;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// The response object to a "build" of the DynamicSqlBuilder object
    /// </summary>
    public class DynamicSql
    {
        /// <summary>
        /// If there is an error, this will be populated. If no error, this should be null
        /// </summary>
        public string Error { get; internal set; }

        /// <summary>
        /// If no error, this will contain the SQL to execute against the database
        /// </summary>
        public string Sql { get; internal set; }

        /// <summary>
        /// This is the parameters/values used in the SQL statement
        /// </summary>
        public List<KeyValuePair<string, object>> Parameters { get; internal set; }
    }
}