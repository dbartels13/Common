// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Dal.Models
{
    /// <summary>
    /// Specifies the type of sql server identity that will be returned
    /// </summary>
    public enum DatabaseIdentity
    {
        /// <summary>
        /// @@IDENTITY
        /// Identity of the last row affected in SQL Server - eg. via nested queries, triggers, etc
        /// </summary>
        Identity,

        /// <summary>
        /// Scope_Identity()
        /// The identity that you just created in the stored procedure specified
        /// </summary>
        ScopeIdentity
    }
}