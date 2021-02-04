using System.Collections.Generic;
using Sphyrnidae.Common.DynamicSql.Enums;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// A table column
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Name of the column in sql
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alias name for the column in case the 'name' is used multiple times (on any tables/selects)
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// If the column value should appear in the select statement
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// Any aggregations to perform on the column values
        /// </summary>
        /// <remarks>If you are only doing aggregation and not selecting the column, you should leave IsSelect = false</remarks>
        public List<AggregateSpecification> Aggregates { get; set; } = new List<AggregateSpecification>();

        /// <summary>
        /// If the result set should be ordered by this column (Default = None)
        /// </summary>
        public OrderDirection OrderBy { get; set; } = OrderDirection.None;

        /// <summary>
        /// Sorting priority - only specify if OrderBy is provided. Will do priority in ascending order
        /// </summary>
        public int OrderPriority { get; set; }

        /// <summary>
        /// Specify this if this column should be grouped (GROUP BY)
        /// </summary>
        public bool IsGrouping { get; set; }

        internal Table Parent { get; set; }
        internal int UniqueNum { get; set; }
    }
}