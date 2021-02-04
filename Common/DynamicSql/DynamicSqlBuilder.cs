using Sphyrnidae.Common.DynamicSql.Enums;
using Sphyrnidae.Common.DynamicSql.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.DynamicSql
{
    /// <summary>
    /// Builder pattern for putting together the SqlBuilderObject and generating a DynamicSql response
    /// </summary>
    public class DynamicSqlBuilder
    {
        private readonly SqlBuilderObject _builderContents;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The already fully built SqlBuilderObject</param>
        public DynamicSqlBuilder(SqlBuilderObject obj) => _builderContents = obj;


        // Builder
        private Table _currentTable;
        private JoinTable _currentJoinTable;
        private Condition _currentCondition;
        private bool _addConditionAsGroup;
        /// <summary>
        /// Constructor for use with building
        /// </summary>
        /// <param name="primaryTable">You must always specify the primary table</param>
        public DynamicSqlBuilder(Table primaryTable)
        {
            _builderContents = new SqlBuilderObject { PrimaryTable = primaryTable };
            _currentTable = primaryTable;
        }

        /// <summary>
        /// Builder for adding a column to the last table added to the builder
        /// </summary>
        /// <param name="col">The Column to add</param>
        /// <returns>"this" for chaining builder functions</returns>
        public DynamicSqlBuilder AddColumn(Column col)
        {
            if (_currentTable != null)
                _currentTable.Columns.Add(col);
            else
                _currentJoinTable.Columns.Add(col);
            return this;
        }

        /// <summary>
        /// Builder for adding a 2nd (or more) joined table
        /// </summary>
        /// <param name="table">The JoinTable to add</param>
        /// <returns>"this" for chaining builder functions</returns>
        public DynamicSqlBuilder AddTable(JoinTable table)
        {
            _builderContents.Joins.Add(table);
            _currentTable = null;
            _currentJoinTable = table;
            _currentCondition = null;
            return this;
        }

        /// <summary>
        /// Ends the table conditions and starts the 'Where' conditions
        /// </summary>
        /// <returns>"this" for chaining builder functions</returns>
        public DynamicSqlBuilder BeginWhere()
        {
            _currentCondition = null;
            _currentJoinTable = null;
            return this;
        }

        /// <summary>
        /// Adds a condition to either a joined table or the where clause
        /// </summary>
        /// <param name="condition">The condition to be added</param>
        /// <returns>"this" for chaining builder functions</returns>
        public DynamicSqlBuilder AddCondition(Condition condition)
        {
            if (_currentCondition == null)
            {
                if (_currentJoinTable == null)
                    _builderContents.Where = condition;
                else
                    _currentJoinTable.Condition = condition;
            }

            else
            {
                if (_addConditionAsGroup)
                {
                    condition.Parent = _currentCondition;
                    _currentCondition.ConditionGroup = condition;
                }
                else
                {
                    condition.Parent = _currentCondition.Parent;
                    _currentCondition.Sibling = condition;
                }
            }

            _currentCondition = condition;
            _addConditionAsGroup = false;
            return this;
        }

        /// <summary>
        /// Starts a condition grouping - ()
        /// </summary>
        /// <param name="join">Optional - default = AND. If this is the first condition (or first in a group), this will be ignored</param>
        /// <returns>"this" for chaining builder functions</returns>
        public DynamicSqlBuilder BeginGroup(ConditionJoin join = ConditionJoin.And)
        {
            _addConditionAsGroup = true;

            if (_currentCondition == null)
            {
                _currentCondition = new Condition();
                if (_currentJoinTable == null)
                    _builderContents.Where = _currentCondition;
                else
                    _currentJoinTable.Condition = _currentCondition;
            }

            else
            {
                var condition = new Condition { Join = join, Parent = _currentCondition.Parent };
                _currentCondition.Sibling = condition;
                _currentCondition = condition;
            }

            return this;
        }
        /// <summary>
        /// Builder which allows you to specify the group has ended
        /// </summary>
        /// <remarks>
        /// This effectively puts the closing parenthesis ")" on the SQL
        /// and moves the pointer back to the condition parent so you can continue adding on siblings using the builder
        /// </remarks>
        /// <returns>"this" for chaining builder functions</returns>
        public DynamicSqlBuilder EndGroup()
        {
            _currentCondition = _currentCondition.Parent;
            return this;
        }

        /// <summary>
        /// When all builder calls have completed (or SqlBuilderObject provided), call this to complete the process
        /// </summary>
        /// <param name="t">Specify the database/sql type so that SQL can be properly formatted (eg. T-SQL vs MySQL)</param>
        /// <returns>The DynamicSql response object</returns>
        public Models.DynamicSql Build(SqlType t)
        {
            var response = new Models.DynamicSql();

            var engine = new DynamicSqlEngine(_builderContents);
            var validation = engine.Validate(t);
            if (!string.IsNullOrWhiteSpace(validation))
            {
                response.Error = validation;
                return response;
            }

            response.Parameters = engine.GetParameters(); // must be called first (before sql)
            response.Sql = engine.GetSql(t);

            return response;
        }
    }
}
