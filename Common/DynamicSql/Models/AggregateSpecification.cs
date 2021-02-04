using Sphyrnidae.Common.DynamicSql.Enums;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// A column can be specified to be aggregated
    /// </summary>
    public class AggregateSpecification
    {
        /// <summary>
        /// Aggregation type
        /// </summary>
        public Aggregate Aggregate { get; set; }

        /// <summary>
        /// The name of the output column
        /// </summary>
        public string Alias { get; set; }
    }
}