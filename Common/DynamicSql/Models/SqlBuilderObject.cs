using System.Collections.Generic;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// The raw object that stores all of the information to be 'built'
    /// </summary>
    public class SqlBuilderObject
    {
        /// <summary>
        /// Required: The primary table
        /// </summary>
        public Table PrimaryTable { get; set; }

        /// <summary>
        /// Optional: Any additional tables to be joined
        /// </summary>
        public List<JoinTable> Joins { get; set; } = new List<JoinTable>();

        /// <summary>
        /// Optional: SQL filtering (you can build a complex join using condition groups/siblings)
        /// </summary>
        public Condition Where { get; set; }
    }
}