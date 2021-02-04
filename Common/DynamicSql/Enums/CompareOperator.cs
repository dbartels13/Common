// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.DynamicSql.Enums
{
    /// <summary>
    /// For a condition, how the left side compares to the right side
    /// </summary>
    public enum CompareOperator
    {
        /// <summary>
        /// =
        /// </summary>
        Equals,

        /// <summary>
        /// &lt;&gt;
        /// </summary>
        NotEquals,

        /// <summary>
        /// &gt;
        /// </summary>
        GreaterThan,

        /// <summary>
        /// &lt;
        /// </summary>
        LessThan,

        /// <summary>
        /// &gt;=
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// &lt;=
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// LIKE %{item}%
        /// </summary>
        Contains,

        /// <summary>
        /// NOT LIKE %{item}%
        /// </summary>
        NotContains,

        /// <summary>
        /// LIKE {item}%
        /// </summary>
        BeginsWith,

        /// <summary>
        /// LIKE %{item}
        /// </summary>
        EndsWith,

        /// <summary>
        /// IN ({list})
        /// </summary>
        In,

        /// <summary>
        /// BETWEEN now() AND offset()
        /// </summary>
        DateOffset,

        /// <summary>
        /// BETWEEN date1 AND date2
        /// </summary>
        DateRange
    }
}