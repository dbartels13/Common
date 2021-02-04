using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sphyrnidae.Common.DynamicSql.Enums;

namespace Sphyrnidae.Common.DynamicSql.Models
{
    /// <summary>
    /// A condition as used in table joins, or 'WHERE' clause
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// How the condition will be joined (AND/OR)
        /// </summary>
        public ConditionJoin Join { get; set; }
        
        /// <summary>
        /// If specified, this condition is not an actual condition, but rather a grouping - represented by () in sql
        /// </summary>
        /// <remarks>
        /// Note that the Condition is a basic condition here - not a condition sibling as it won't need and/or.
        /// Also, if this is specified, than left/right columns/values, operator, etc should not be specified.
        /// </remarks>
        public Condition ConditionGroup { get; set; }

        /// <summary>
        /// A condition can have additional clauses (and/or)
        /// </summary>
        public Condition Sibling { get; set; }

        /// <summary>
        /// The left operand column
        /// </summary>
        public Column LeftColumn { get; set; }

        /// <summary>
        /// Operator for comparison of the left/right values/columns
        /// </summary>
        public CompareOperator Operator { get; set; }

        /// <summary>
        /// The right operand column
        /// </summary>
        /// <remarks>if set, then value/parameters should not be set</remarks>
        public Column RightColumn { get; set; }

        /// <summary>
        /// The value comparison of the right hand operand
        /// </summary>
        /// <remarks>
        /// If set, RightColumn should not be set.
        /// The value can be anything, and is dependent on the Compare Operator.
        /// Eg. LIKE items should be a string;
        ///     DateOffset should be a TimeSpan;
        ///     DateRange should be a date;
        ///     IN won't have either RightColumn/Value set - will use AdditionalParameterValues
        /// </remarks>
        public object RightValue { get; set; }

        private string ParameterName { get; set; }

        /// <summary>
        /// If you need to have a 2nd (or more) values
        /// </summary>
        /// <remarks>
        /// DateRange should populate this with a single value - the 2nd date;
        /// IN will use this to populate all possible values
        /// </remarks>
        public List<object> AdditionalParameterValues { get; set; } = new List<object>();

        private List<KeyValuePair<string, object>> AdditionalParameters { get; } = new List<KeyValuePair<string, object>>();

        internal Condition Parent { get; set; }

        internal string Validate(SqlType t)
        {
            if (ConditionGroup != null && LeftColumn != null)
                return "You have specified a condition as both a grouping and a condition";

            if (ConditionGroup == null)
            {
                if (LeftColumn == null)
                    return "You must specify a left column for condition";
                if (RightColumn != null && RightValue != null)
                    return "You can not set a condition to be both column and literal based";
                if (Operator != CompareOperator.In && RightColumn == null && RightValue == null)
                    return "You must specify either a compare column or literal value";
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Operator)
                {
                    case CompareOperator.Contains:
                    case CompareOperator.NotContains:
                    case CompareOperator.BeginsWith:
                    case CompareOperator.EndsWith:
                        if (RightValue == null)
                            return "You must specify a literal value for a LIKE operation";
                        RightValue = RightValue
                            .ToString()
                            .Replace("|", "||")
                            .Replace("%", "|%")
                            .Replace("_", "|_");
                        if (t == SqlType.SqlServer)
                            RightValue = RightValue
                                .ToString()
                                .Replace("[", "|[")
                                .Replace("]", "|]");
                        break;

                    case CompareOperator.DateOffset:
                        if (!(RightValue is TimeSpan))
                            return "You must specify a time span for date offset";

                        var now = DateTime.UtcNow;
                        AdditionalParameterValues.Add(now.Add((TimeSpan)RightValue));
                        RightValue = now;
                        break;

                    case CompareOperator.DateRange:
                        if (!(RightValue is DateTime))
                            return "Date range must use dates (first date)";
                        if (!(AdditionalParameterValues.FirstOrDefault() is DateTime))
                            return "Date range must use dates (second date)";
                        break;

                    case CompareOperator.In:
                        if (RightValue != null)
                            return "For IN comparison, you must specify all values as AdditionalParameterValues";
                        if (!AdditionalParameterValues.Any())
                            return "For IN comparison, you must specify at least 1 AdditionalParameterValues";
                        break;

                }
            }

            else
            {
                var groupValidation = ConditionGroup.Validate(t);
                if (groupValidation != null)
                    return groupValidation;
            }

            return Sibling?.Validate(t);
        }

        internal List<KeyValuePair<string, object>> GetParameters(int num)
        {
            var parameters = new List<KeyValuePair<string, object>>();

            if (RightValue != null)
            {
                ParameterName = $"Param{num}";
                parameters.Add(new KeyValuePair<string, object>(ParameterName, RightValue));
            }
            foreach (var p in AdditionalParameterValues)
            {
                var paramName = $"Param{num + parameters.Count}";
                var kvp = new KeyValuePair<string, object>(paramName, p);
                parameters.Add(kvp);
                AdditionalParameters.Add(kvp);
            }

            if (ConditionGroup != null)
                parameters.AddRange(ConditionGroup.GetParameters(num + parameters.Count));
            if (Sibling != null)
                parameters.AddRange(Sibling.GetParameters(num + parameters.Count));
            return parameters;
        }

        internal string GetSql(SqlType t)
        {
            var sql = new StringBuilder();
            if (LeftColumn == null)
                sql.Append($" ( {ConditionGroup.GetSql(t)} ) ");

            else
            {
                sql.Append($" {DynamicSqlEngine.ColumnSql(t, LeftColumn.Parent.Alias, LeftColumn.Name)} ");

                switch (Operator)
                {
                    case CompareOperator.Contains:
                        sql.Append($"LIKE CONCAT('%', @{ParameterName}, '%') ESCAPE '|'");
                        break;
                    case CompareOperator.NotContains:
                        sql.Append($"NOT LIKE CONCAT('%', @{ParameterName}, '%') ESCAPE '|'");
                        break;
                    case CompareOperator.BeginsWith:
                        sql.Append($"LIKE CONCAT(@{ParameterName}, '%') ESCAPE '|'");
                        break;
                    case CompareOperator.EndsWith:
                        sql.Append($"LIKE CONCAT('%', @{ParameterName}) ESCAPE '|'");
                        break;
                    case CompareOperator.DateOffset:
                    case CompareOperator.DateRange:
                        sql.Append($"BETWEEN @{ParameterName} AND @{AdditionalParameters.First().Key}"); // Parameters must be built first - GetParameters()
                        break;
                    case CompareOperator.In:
                        sql.Append("IN (");
                        foreach (var p in AdditionalParameters)
                            sql.Append($"@{p.Key}, ");
                        sql.Length -= 2;// Remove the trailing ", "
                        sql.Append(")");
                        break;
                    default:
#pragma warning disable 8509
                        sql.Append(Operator switch
#pragma warning restore 8509
                        {
                            CompareOperator.Equals => "=",
                            CompareOperator.NotEquals => "<>",
                            CompareOperator.GreaterThan => ">",
                            CompareOperator.LessThan => "<",
                            CompareOperator.GreaterThanOrEqual => ">=",
                            CompareOperator.LessThanOrEqual => "<="
                        });

                        sql.Append(" ");

                        sql.Append(RightColumn != null
                            ? $" {DynamicSqlEngine.ColumnSql(t, RightColumn.Parent.Alias, RightColumn.Name)} "
                            : $"@{ParameterName}");
                        break;
                }

                sql.Append(" ");
            }

            if (Sibling != null)
                sql.Append($" {Enum.GetName(typeof(ConditionJoin), Sibling.Join)} {Sibling.GetSql(t)} ");
            return sql.ToString();
        }
    }
}
