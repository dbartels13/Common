// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.DynamicSql.Enums
{
    /// <summary>
    /// Type of table join condition
    /// </summary>
    public enum JoinType
    {
        /// <summary>
        /// INNER JOIN
        /// </summary>
        Inner,

        /// <summary>
        /// LEFT OUTER JOIN
        /// </summary>
        Left,

        /// <summary>
        /// RIGHT OUTER JOIN
        /// </summary>
        Right,

        /// <summary>
        /// FULL OUTER JOIN
        /// </summary>
        Full
    }
}