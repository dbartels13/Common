// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Api
{
    /// <summary>
    /// Constant values
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Values used in Api to Api communication
        /// </summary>
        public static class ApiToApi
        {
            /// <summary>
            /// Name of the parameter containing the name of the granting application
            /// </summary>
            public const string Application = "RequestingApplication";

            /// <summary>
            /// Name of the parameter containing the authorization token for the application
            /// </summary>
            public const string Token = "Api2ApiToken";

            /// <summary>
            /// Name of the parameter containing the name of the application that owns the token
            /// </summary>
            public const string Owner = "AuthorizingApplication";
        }

        /// <summary>
        /// Id's of pre-defined customers
        /// </summary>
        public static class Customers
        {
            /// <summary>
            /// The un-authenticated public user
            /// </summary>
            public const string Public = "public";

            /// <summary>
            /// The main sphyrnidae customer
            /// </summary>
            public const string Sphyrnidae = "sphyrnidae";
        }

        /// <summary>
        /// Names of roles
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// Developer role
            /// </summary>
            public const string Developer = "Developer";
        }
    }
}