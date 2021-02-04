using Sphyrnidae.Common.DynamicSql.Enums;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// A 2nd (or more) additional table joined to the primary table
    /// </summary>
    public class JoinTable : Table
    {
        /// <summary>
        /// The type of joining (eg. inner, left, right, outer)
        /// </summary>
        public JoinType Type { get; set; }

        /// <summary>
        /// How the table should be joined (you can build a complex join using condition groups/siblings)
        /// </summary>
        public Condition Condition { get; set; }
    }
}