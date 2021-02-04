using System.Collections.Generic;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// Represents a table/columns. Use this class directly for the primary table, and the JoinTable class for additional tables
    /// </summary>
    public class Table
    {
        /// <summary>
        /// SQL name of the table
        /// </summary>
        public string Name { get; set; }

        internal string Alias { get; set; }

        /// <summary>
        /// Collection of columns for the table (should contain all columns used for select/conditions/grouping/etc)
        /// </summary>
        public List<Column> Columns { get; set; } = new List<Column>();
    }
}