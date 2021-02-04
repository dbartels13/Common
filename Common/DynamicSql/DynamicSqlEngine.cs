using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sphyrnidae.Common.DynamicSql.Enums;
using Sphyrnidae.Common.DynamicSql.Models;

namespace Sphyrnidae.Common.DynamicSql
{
    internal class DynamicSqlEngine
    {
        private readonly SqlBuilderObject _obj;
        private List<Column> _selects;
        private List<Column> _aggregateColumns;
        private List<Column> _groupingColumns;
        internal DynamicSqlEngine(SqlBuilderObject obj) => _obj = obj;

        #region Prepare/Validate
        internal string Validate(SqlType t) => ValidateSelect() ?? ValidateFrom(t) ?? ValidateWhere(t);

        private string ValidateSelect()
        {
            // Ensure all columns are named
            if (_obj.PrimaryTable.Columns.Any(x => string.IsNullOrWhiteSpace(x.Name)))
                return "You have not specified a name for a table column on primary table";
            if (_obj.Joins.Any(table => table.Columns.Any(x => string.IsNullOrWhiteSpace(x.Name))))
                return "You have not specified a name for a table column on a joined table";

            // 1-time building of these
            BuildSelects(); // Goes first to set the parent
            BuildAggregates();
            BuildGroupings();

            // If there is aggregates or groupings, all select items must be contained in those
            if (_aggregateColumns.Any() || _groupingColumns.Any())
            {
                var errorCol = _selects.FirstOrDefault(col =>
                    _groupingColumns.All(x => x.UniqueNum != col.UniqueNum) &&
                    _aggregateColumns.All(x => x.UniqueNum != col.UniqueNum));
                if (errorCol != null)
                    return $"Column {errorCol.Alias ?? errorCol.Name} is invalid in the select list because it is not contained in an aggregate or grouping";
            }

            // Start a full select list
            var selectNames = _selects.Select(x => x.Alias ?? x.Name).ToList();

            // All aggregates will be part of the select list and must have an alias
            if (_aggregateColumns.Any())
                foreach (var col in _aggregateColumns)
                {
                    foreach (var aggregate in col.Aggregates)
                    {
                        if (string.IsNullOrWhiteSpace(aggregate.Alias))
                            return $"Aggregate column {col.Name} does not have an alias name for an aggregation";
                        selectNames.Add(aggregate.Alias);
                    }
                }

            // Ensure you have at least 1 item in the select list
            if (!selectNames.Any())
                return "You must specify at least 1 column as either aggregate or selectable";

            return selectNames.Count != selectNames.Distinct().Count()
                ? "Select fields do not have unique names"
                : null;
        }

        private void BuildSelects()
        {
            var i = 0;
            _obj.PrimaryTable.Columns.ForEach(x =>
            {
                x.Parent = _obj.PrimaryTable;
                x.UniqueNum = i;
                i++;
            });
            _selects = _obj.PrimaryTable.Columns.Where(x => x.IsSelect).ToList();

            foreach (var table in _obj.Joins)
            {
                table.Columns.ForEach(x =>
                {
                    x.Parent = table;
                    x.UniqueNum = i;
                    i++;
                });
                _selects.AddRange(table.Columns.Where(x => x.IsSelect));
            }
        }

        private void BuildAggregates()
        {
            _aggregateColumns = _obj.PrimaryTable.Columns.Where(col => col.Aggregates.Any()).ToList();
            _aggregateColumns.AddRange(_obj.Joins.SelectMany(table => table.Columns.Where(col => col.Aggregates.Any())));
        }

        private void BuildGroupings()
        {
            _groupingColumns = _obj.PrimaryTable.Columns.Where(x => x.IsGrouping).ToList();
            _groupingColumns.AddRange(_obj.Joins.SelectMany(table => table.Columns.Where(x => x.IsGrouping)));
        }

        private string ValidateFrom(SqlType t)
        {
            if (string.IsNullOrWhiteSpace(_obj.PrimaryTable.Name))
                return "You have not specified the name of the primary table";
            if (_obj.Joins.Any(x => string.IsNullOrWhiteSpace(x.Name)))
                return "You have not specified the name of a joined table";

            foreach (var table in _obj.Joins)
            {
                if (table.Condition == null)
                    return $"No join condition specified for table {table.Name}";
                var conditionValidation = table.Condition.Validate(t);
                if (conditionValidation != null)
                    return conditionValidation;
            }

            _obj.PrimaryTable.Alias = "tbl0";
            for (var i = 0; i < _obj.Joins.Count; i++)
                _obj.Joins[i].Alias = $"tbl{i + 1}";

            return null;
        }

        private string ValidateWhere(SqlType t) => _obj.Where?.Validate(t);

        private IEnumerable<Column> GetOrderBy()
        {
            var ordering = _obj.PrimaryTable.Columns.Where(x => x.OrderBy != OrderDirection.None).ToList();
            foreach (var table in _obj.Joins)
                ordering.AddRange(table.Columns.Where(x => x.OrderBy != OrderDirection.None));
            return ordering.OrderBy(x => x.OrderPriority);
        }
        #endregion

        #region Parameters
        internal List<KeyValuePair<string, object>> GetParameters()
        {
            var parameters = new List<KeyValuePair<string, object>>();

            foreach (var table in _obj.Joins)
                parameters.AddRange(table.Condition.GetParameters(parameters.Count));

            if (_obj.Where != null)
                parameters.AddRange(_obj.Where.GetParameters(parameters.Count));

            return parameters;
        }
        #endregion

        #region Sql
        internal string GetSql(SqlType t)
        {
            var sql = new StringBuilder("SELECT ");
            foreach (var col in _aggregateColumns)
                foreach (var aggregate in col.Aggregates)
                    sql.Append($"{Enum.GetName(typeof(Aggregate), aggregate.Aggregate)}({ColumnSql(t, col.Parent.Alias, col.Name)}) AS {BracketSql(t, aggregate.Alias)}, ");
            foreach (var col in _selects)
                sql.Append($"{ColumnSql(t, col.Parent.Alias, col.Name)} AS {BracketSql(t, col.Alias ?? col.Name)}, ");
            sql.Length -= 2; // Remove the trailing ", "

            sql.Append(" FROM ");
            sql.Append($"{BracketSql(t, _obj.PrimaryTable.Name)} AS {BracketSql(t, _obj.PrimaryTable.Alias)} ");

            foreach (var table in _obj.Joins)
                sql.Append($"{Enum.GetName(typeof(JoinType), table.Type)} JOIN {BracketSql(t, table.Name)} AS {BracketSql(t, table.Alias)} ON {table.Condition.GetSql(t)}");

            if (_obj.Where != null)
                sql.Append($" WHERE {_obj.Where.GetSql(t)}");

            if (_groupingColumns.Any())
            {
                sql.Append(" GROUP BY ");
                foreach (var col in _groupingColumns)
                    sql.Append($" {ColumnSql(t, col.Parent.Alias, col.Name)}, ");
                sql.Length -= 2; // Remove the trailing ", "
            }

            var ordering = GetOrderBy().ToList();
            if (ordering.Any())
            {
                sql.Append(" ORDER BY ");
                foreach (var col in ordering)
                    sql.Append($" {ColumnSql(t, col.Parent.Alias, col.Name)} {Enum.GetName(typeof(OrderDirection), col.OrderBy)}, ");
                sql.Length -= 2; // Remove the trailing ", "
            }

            return sql.ToString();
        }

        internal static string ColumnSql(SqlType t, string table, string column) =>
            $"{BracketSql(t, table)}.{BracketSql(t, column)}";

        internal static string BracketSql(SqlType t, string val) =>
            $"{(t == SqlType.SqlServer ? "[" : "`")}{val}{(t == SqlType.SqlServer ? "]" : "`")}";

        #endregion
    }
}
