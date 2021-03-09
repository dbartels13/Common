# Dynamic SQL {#DynamicSqlMd}

## Overview {#DynamicSqlOverviewMd}
If you don't want to fully create your own sql statement, or your sql will be somewhat dynamic in nature, this class can help.
There are 2 main components to this:
1. [Dynamic Sql Generation](@ref DynamicSqlBuilderMd): This class utilizes the builder pattern to generate the complete SQL statement
2. [Executing Sql](@ref DynamicSqlExecuteMd): Allows for generic execution of the generated SQL

If the SQL you wish to generate is known at design-time, then you should just directly generate that SQL (possibly as a Stored Procedure).
However, if you are dynamically building the SQL with various tables and columns that could change, then this is the tool for now.
A common scenario for this would be in report building where the user can specify which tables, columns, aggregates, ordering, etc.

## Generating Dynamic Sql Object {#DynamicSqlBuilderMd}
<h2>Getting Started</h2>
You will begin by specifing the [Primary Table](@ref Sphyrnidae.Common.DynamicSql.Models.Table).
All you need to specify initially is the [Name](@ref Sphyrnidae.Common.DynamicSql.Models.Table.Name) of the table.
From there, you can utilize the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder).
The constructor will take the primary table you've created as the sole argument.

From here, you can either add [columns](@ref Sphyrnidae.Common.DynamicSql.Models.Column) to the table directly,
or you can utilize the AddColumn() method on the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder).
You must at a minimum specify the [Name](@ref Sphyrnidae.Common.DynamicSql.Models.Column.Name).
You will need to specify at least 1 column as [IsSelect](@ref Sphyrnidae.Common.DynamicSql.Models.Column.IsSelect).
On the column itself, you can also specify this column as [grouping](@ref Sphyrnidae.Common.DynamicSql.Models.Column.IsGrouping),
as an [aggregate](@ref Sphyrnidae.Common.DynamicSql.Models.AggregateSpecification) (it can actually be aggregated multiple ways),
or if it is involved in [ordering](@ref Sphyrnidae.Common.DynamicSql.Models.Column.OrderBy).
If you have multiple [ordering](@ref Sphyrnidae.Common.DynamicSql.Models.Column.OrderBy) columns,
you should also specify the [priority](@ref Sphyrnidae.Common.DynamicSql.Models.Column.OrderPriority).

<h2>Join Tables</h2>
To have multiple tables in your SQL, you will create another table as a [Join Table](@ref Sphyrnidae.Common.DynamicSql.Models.JoinTable).
This table is just like your primary table - so you need only specify the [Name](@ref Sphyrnidae.Common.DynamicSql.Models.Table.Name) of the table.
By default, this table will be [joined](@ref Sphyrnidae.Common.DynamicSql.Enums.JoinType) by an [inner join](@ref Sphyrnidae.Common.DynamicSql.Enums.JoinType.Inner).
You can update this join to be other types.
You can then call the AddTable() method on the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder).
You can add columns the same way to this table as you did for the primary table (any calls to AddColumn() will be against the last table specified in the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder)).

In order to specify the [join condition](@ref Sphyrnidae.Common.DynamicSql.Models.Condition) for this table,
you should create a condition (see below), and then call AddCondition() method on the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder).
As with columns, the conditions will apply to the latest table specified.

<h2>Conditions</h2>
A [Condition](@ref Sphyrnidae.Common.DynamicSql.Models.Condition) can be added to the 'Where' clause to filter results.
These same conditions are also used for join conditions when joining multiple tables together in your 'From' clause.
To add conditions to your 'Where' clause, you should first call the BeginWhere() method on the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder).
From there, you can begin adding conditions.
The first condition you add will appear immediately after the 'Where' clause.
For subsequent conditions, these will be siblings of the previous condition, and you must specify how the conditions are [joined](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.Join) together.
Default join condition is [And](@ref Sphyrnidae.Common.DynamicSql.Enums.ConditionJoin.And).

A condition will have the following properties:
1. Left operand: The [Left Column](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.LeftColumn) will be one of the [Columns](@ref Sphyrnidae.Common.DynamicSql.Models.Column) that belongs to a table
2. Operator: The [Operator](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.Operator) will be one of the [Compare Operators](@ref Sphyrnidae.Common.DynamicSql.Enums.CompareOperator)
3. Right operand: This will either a [Right Column](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.RightColumn) or a [Right Value](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.RightValue)

If you are specifying a non-column, this will be of type [Right Value](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.RightValue).
The value you specify will be parameterized in your SQL.
Note that some [Compare Operators](@ref Sphyrnidae.Common.DynamicSql.Enums.CompareOperator) take multiple values.
For these types, you should specify the 2nd (or any subsequent) parameters in [AdditionalParameterValues](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.AdditionalParameterValues).
Here is the listing of [Compare Operators](@ref Sphyrnidae.Common.DynamicSql.Enums.CompareOperator) where this is required:
1. [In](@ref Sphyrnidae.Common.DynamicSql.Enums.CompareOperator.In): You will not specify a [Right Value](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.RightValue) at all. All possible values should be added to [AdditionalParameterValues](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.AdditionalParameterValues)
2. [DateRange](@ref Sphyrnidae.Common.DynamicSql.Enums.CompareOperator.DateRange): You will specify the first date as the [Right Value](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.RightValue). The 2nd date shoule be added to [AdditionalParameterValues](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.AdditionalParameterValues)

If you need to group conditions together - eg. to specify AND (A OR B) - you can utilize the
BeginGroup() method on the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder) which will essentially write out "AND (".
You can then add additional conditions to the grouping.
The first condition added will not have the [join](@ref Sphyrnidae.Common.DynamicSql.Models.Condition.Join) condition specified.
When you are done adding conditions to the group, you will call the EndGroup() method on the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder) which will essentially write out "AND (",
which will essentially write out the ")".
You can also nest groupings (eg. a grouping within a grouping).

<h2>Limitations</h2>
This class is not meant to be all-inclusive of all possible SQL statements and structures.
Only the methods documented are currently supported.
If you need other functionality, a change request can be made to add that piece of functionality.

<h2>Build</h2>
When you are done adding everything to the [DynamicSqlBuilder](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlBuilder),
you will call the Build() method which will validate everything and return a [DynamicSql](@ref Sphyrnidae.Common.DynamicSql.Models.DynamicSql) object.
If the [Error](@ref Sphyrnidae.Common.DynamicSql.Models.DynamicSql.Error) property is populated, then validation failed with the specified message.
Otherwise, the [generated SQL](@ref Sphyrnidae.Common.DynamicSql.Models.DynamicSql.Sql) and [parameters](@ref Sphyrnidae.Common.DynamicSql.Models.DynamicSql.Parameters) will be populated.

## Executing Sql {#DynamicSqlExecuteMd}
Once you have built the sql object (see above), you now will likely want to execute that SQL.
There is no need to build your own repository (see [Data Access Layer](@ref DalMd)), as a generic repository is available for use:
1. [DynamicSqlServerRepo](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlServerRepo) for executing against a SQL Server database
2. [DynamicMySqlRepo](@ref Sphyrnidae.Common.DynamicSql.DynamicMySqlRepo)

To use these classes, use dependency injection to inject the instances, and then call whatever method you would like:
1. List: Retrieves multiple rows
2. Get: Retreives a single row
3. Scalar: Retrieves a single column from a single row

The parameters to these functions are:
1. The Connection String
2. SQL: The SQL to execute (from [sql](@ref Sphyrnidae.Common.DynamicSql.Models.DynamicSql.Sql))
3. Parameters: The parameters needed for the query (from [parameters](@ref Sphyrnidae.Common.DynamicSql.Models.DynamicSql.Parameters))

## Where Used {#DynamicSqlWhereUsedMd}
None

## Examples {#DynamicSqlExampleMd}
<pre>
To generate the following SQL:
	SELECT
		Count([tbl1].[RoleId]) AS [NumRoles],
		[tbl0].[UserId] AS [UserId]
	FROM
		[Users] AS [tbl0]
		Left JOIN [UserRoles] AS [tbl1]
			ON [tbl0].[UserId] = [tbl1].[UserId]
	WHERE
		[tbl0].[UserId] <= @Param0 And
		(
			[tbl0].[Name] = @Param1
			Or [tbl0].[Name] = @Param2
		)
	GROUP BY
		[tbl0].[UserId]
	ORDER BY
		[tbl0].[Name] Asc

With the following parameters:
	[
		{"Key":"Param0","Value":100},
		{"Key":"Param1","Value":"me"},
		{"Key":"Param2","Value":"you"}
	]

You can write the following code:
    var usersTable = new Table { Name = "Users" };
    var usersUserId = new Column
    {
        Name = "UserId",
        IsSelect = true,
        IsGrouping = true
    };
    var usersName = new Column
    {
        Name = "Name",
        OrderBy = OrderDirection.Asc
    };

    var userRolesTable = new JoinTable { Name = "UserRoles", Type = JoinType.Left };
    var userRolesUserId = new Column { Name = "UserId" };
    var userRolesRoleId = new Column { Name = "RoleId"};
    userRolesRoleId.Aggregates.Add(
        new AggregateSpecification
        {
            Aggregate = Aggregate.Count,
            Alias = "NumRoles"
        });


    var sqlBuilder = new DynamicSqlBuilder(usersTable);
    sqlBuilder.AddColumn(usersUserId);
    sqlBuilder.AddColumn(usersName);

    sqlBuilder.AddTable(userRolesTable);
    sqlBuilder.AddColumn(userRolesUserId);
    sqlBuilder.AddColumn(userRolesRoleId);
    sqlBuilder.AddCondition(new Condition
    {
        LeftColumn = usersUserId,
        Operator = CompareOperator.Equals,
        RightColumn = userRolesUserId
    });
    sqlBuilder.BeginWhere();
    sqlBuilder.AddCondition(new Condition
    {
        LeftColumn = usersUserId,
        Operator = CompareOperator.LessThanOrEqual,
        RightValue = 100
    });
    sqlBuilder.BeginGroup(ConditionJoin.And);
    sqlBuilder.AddCondition(new Condition
    {
        LeftColumn = usersName,
        Operator = CompareOperator.Equals,
        RightValue = "me"
    });
    sqlBuilder.AddCondition(new Condition
    {
        Join = ConditionJoin.Or,
        LeftColumn = usersName,
        Operator = CompareOperator.Equals,
        RightValue = "you"
    });
    sqlBuilder.EndGroup();

    var sql = sqlBuilder.Build(SqlType.SqlServer);
</pre>