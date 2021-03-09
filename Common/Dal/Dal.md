﻿# Data Access Layer {#DalMd}

## Overview {#DalOverviewMd}
The Data Access Layer (DAL) is how you go about retrieving data from a repository.
For the purposes of these classes, a DAL is how you interact with a database.
This will commonly be referred to as the repository/repo layer.

All methods that will interact with a database will inherit from the [BaseRepo](@ref Sphyrnidae.Common.Dal.BaseRepo) class.
This class provides all the abstraction you will need to perform almost any database operation.
Behind the covers, the class utilizes <a href="https://github.com/StackExchange/Dapper" target="blank">Dapper</a> to actually interact with the database.
Additionally, this class handles [logging](@ref LogMd) and connection management.

Base class: [BaseRepo](@ref Sphyrnidae.Common.Dal.BaseRepo)

Inherited classes:
1. [SqlServerRepo](@ref Sphyrnidae.Common.Dal.SqlServerRepo)
2. [MySqlRepo](@ref Sphyrnidae.Common.Dal.MySqlRepo)

Where used:
1. [DynamicSqlServerRepo](@ref Sphyrnidae.Common.DynamicSql.DynamicSqlServerRepo)
2. [DatabaseLogger](@ref Sphyrnidae.Common.Logging.Loggers.DatabaseLogger)

## Methods {#DalMethodsMd}
All methods are asynchronous (no synchronous option).
All methods can take either direct SQL, or a stored procedure.
All methods have a "TrappingExceptions" option (see [below](@ref DalImplementationMd) for more information)
1. WriteAsync: Performs any operation on the database that does not return a result set. This will return the number of records affected by the command. Optionally, this could return bool to specify if any records were affected.
2. Scalar: Will execute your operation, which should always return a single record with a single field. This field will be returned.
3. Insert: This should perform a SQL insert into a database table, and will return the identity of the newly created record.
4. Get (Single): This will query the database and return a single record. This record will be mapped to the generic entity you specify.
5. GetList: This will query the database and return 0, 1, or multiple records. These records will all be mapped to the generic entity you specify. This will return an IEnumerable<T>.
6. Exists: You can build multiple "conditions" - Eg. ExistsCondition() and pass those conditions into the Exists() method. This will return true if any of the conditions match a record.

## Implementing a Repo Class {#DalImplementationMd}
It is a best practice to have a repository class that roughly maps to a single database table.
As such, your repo class should inherit from either [SqlServerRepo](@ref Sphyrnidae.Common.Dal.SqlServerRepo) or [MySqlRepo](@ref Sphyrnidae.Common.DalMySqlRepo).
Your inherited class must implement the following abstract methods:
1. [CnnStr](@ref Sphyrnidae.Common.Dal.BaseRepo.CnnStr): The connection string to your database.
2. [CnnName](@ref Sphyrnidae.Common.Dal.BaseRepo.CnnName): The name of the object your repository is linked to (eg. the name of the table). This is only used for logging.

Optionally, you can override PreCall() or PostCall() methods.
The PreCall() is where the database connection is initiated (base class does this).
If there is anything you might wish to alter in your command or the connection, this is where you can do that.
The PostCall() is where you can setup additional handling for after the database call has completed.

A common scenario is where you are performing a non-Idempotent call (eg. Insert/Update/Delete).
These calls might fail for a variety of reasons (eg. Foreign Key constraint, other contraints, unique index, etc.)
When these calls fail, they will throw a SQLException.
You may wish to "Trap" these exceptions instead of letting your repository layer throw the exception out to other layers in your architecture.
You do this by utilizing the "TrapExceptions" method available for all of the calls.
If you have trapped an exception, you can directly access this protected member in your repo class (Ex).
Additionally, you may wish to implement the "HandleException" method.
By implementing this method, this allows the layer in your code which made the repo call to retrieve the exception as a result object.
Eg. Can return an HTTP 404 if it wasn't found, or a 409 if this is in conflict/duplicate.

## Transactions {#DalTransactionsMd}
The [Transaction](@ref Sphyrnidae.Common.Dal.Transaction) class provides the ability to execute multiple SQL statements within a transaction.
If any of the statements fail, the transaction will be rolled back.
If all of the statements complete successfully, the transaction will commit.
So this is an all-or-nothing scenario.

Most transactions will be against a single database.
If this is your scenario, then you will execute the "Run" static method.
This wrapper method will call your method with the newly created transaction which you should pass along in all repository calls.
There is no need for the internal sql statements to run synchronously... they can be all be run at once (asynchronously).
This is accomplished by altering the connection string to include ";MultipleActiveResultSets=True" (a performance hit is incurred).

If your transaction is going to span multiple databases, you should utilize the "Distributed" static method.
This method is similar, except that you don't pass in the connection string, and you don't receive a transaction to your method.
Instead, it creates a <a href="https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope?view=net-5.0" target="blank">TransactionScope</a> for all repository calls within your method.
Inside your transactional method, you will need to specify the connection string instead of the transaction.
Please note that your transactional method will need to handle the possibly of multiple active result sets.

Your transactional method will return a [TransactionResponse](@ref Sphyrnidae.Common.Dal.Models.TransactionResponse) - either "Commit()" or "Rollback()".
If your method throws an exception, the wrapper will catch this, call your exception handling method, and rollback the transaction.

Because the [Transaction](@ref Sphyrnidae.Common.Dal.Transaction) is generic, you can call this with any type that inherits from
<a href="https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbconnection?view=net-5.0" target="blank">DbConnection</a>.
If you are connecting to SqlServer, then you can instead use the static class [SqlTransaction](@ref Sphyrnidae.Common.Dal.SqlTransaction).
If you are connection to MySql, then you can instead use the static class [MySqlTransaction](@ref Sphyrnidae.Common.Dal.MySqlTransaction).

## Where Used {#DalWhereUsedMd}
<table>
    <tr>
        <th>Class
        <th>Description
    <tr>
        <td>[DatabaseLogger](@ref Sphyrnidae.Common.Logging.Loggers.DatabaseLogger)
        <td>If you are using the built-in database logger, this will connect to your logging database for all logging calls.
</table>

## Examples {#DalExampleMd}
<h2>Repository Implementation</h2>
<pre>
    public class MyRepo : SqlServerRepo, IMyRepo
    {
        public MyRepo(ILogger logger) : base(logger) { }
        public override string CnnStr => "My Connection String - possibly lookup from Environmental Settings";
        protected override string CnnName => "Widgets";

        public async Task<int?> InsertWidget(string name, double cost)
        {
            var parameters = new
            {
                Name = name,
                Cost = cost
            };
            return await InsertTrappingExceptionsAsync("insert into Widgets([Name], [Cost]) values (@Name, @Cost)", parameters);
        }
        public override ApiResponseStandard HandleException()
        {
            var msg = Ex.GetFullMessage().ToLower();
            if (msg.Contains("unique constraint"))
                return ApiResponse.Duplicate();
            return ApiResponse.InternalServerError(Ex);
        }
        public async Task<bool> TransactionTestWidget(int id, string name, double cost)
        {
            var parameters = new
            {
                Id = id,
                Name = name,
                Cost = cost
            };
            return await WriteSQLAsBoolAsync("update Widgets set [Name]=@Name, [Cost]=@Cost where [Id]=@Id", parameters);
        }
    }
</pre>

<h2>Consuming a Repository</h2>
<pre>
    var id = await myRepo.InsertWidget("widget1", 0.99);
    if (!id.HasValue)
        return myRepo.HandleException();
    return ApiResponse.Success(id.Value);
</pre>

<h2>Transaction</h2>
<pre>
    var success = await SqlTransaction.Run(
        logger,
        myRepo.CnnStr,
        async transaction =>
        {
            // 1st call
            if (!await myRepo.TransactionTestWidget(id.Value, "widget1a", 1.99))
                return TransactionResponse.Rollback(); // Could optionally return a value

            // 2nd call
            if (!await myRepo.TransactionTestWidget(id.Value, "widget1b", 2.99))
                return TransactionResponse.Rollback(); // Could optionally return a value

            return TransactionResponse.Commit(); // Could optionally return a value
        });

    // You may wish to handle failures/exceptions differently
    return success ? ApiResponse.Success(id.Value) : ApiResponse.NotFound("Widget", id.Value.ToString());
</pre>