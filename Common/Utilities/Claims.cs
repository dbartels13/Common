using System;
using System.Linq;
using System.Security.Claims;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Utilities
{
    /// <summary>
    /// Helper methods for working with the IPrincipal (Logged in User)
    /// </summary>
    public static class Claims
    {
        #region Add

        /// <summary>
        /// Adds a claim to the identity
        /// </summary>
        /// <param name="claim">The name of the claim being added</param>
        /// <param name="value">The value of the claim (must be a string - if an object, you must serialize it first)</param>
        /// <param name="identity">The already retrieved claims identity (optional, default=null and will be retrieved)</param>
        public static void Add(string claim, string value, ClaimsIdentity identity = null)
            => (identity ?? GetIdentity()).AddClaim(new Claim(claim, value));
        #endregion

        #region Check for existence

        /// <summary>
        /// Specifies if the user has the given claim or not
        /// </summary>
        /// <param name="claim">The name of the claim being checked</param>
        /// <param name="identity">The already retrieved claims identity (optional, default=null and will be retrieved)</param>
        /// <param name="comparison">The string comparison to use (optional, default = CurrentCultureIgnoreCase</param>
        /// <returns>True if the claim exists, false otherwise</returns>
        public static bool Has(string claim, ClaimsIdentity identity = null,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => GetRawClaim(claim, identity, comparison) != null;
        #endregion

        #region Get

        /// <summary>
        /// Returns the value of the claim from the current user
        /// </summary>
        /// <param name="claim">The name of the claim being retrieved</param>
        /// <param name="identity">The already retrieved claims identity (optional, default=null and will be retrieved)</param>
        /// <param name="comparison">The string comparison to use (optional, default = CurrentCultureIgnoreCase</param>
        /// <returns>The value of the claim. If claim not found, or retrieving identity that is unauthenticated, an exception will be raised</returns>
        public static string Get(string claim, ClaimsIdentity identity,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => Get(claim, null, true, identity, comparison);

        /// <summary>
        /// Returns the value of the claim from the current user
        /// </summary>
        /// <param name="claim">The name of the claim being retrieved</param>
        /// <param name="throwIfNotExists">
        /// Optional: Default=true
        /// If the claim can not be obtained, this will throw an exception (true), or return blank ("") (false)
        /// </param>
        /// <param name="identity">The already retrieved claims identity (optional, default=null and will be retrieved)</param>
        /// <param name="comparison">The string comparison to use (optional, default = CurrentCultureIgnoreCase</param>
        /// <returns>The value of the claim. If retrieving identity that is unauthenticated, an exception will be raised</returns>
        public static string Get(string claim, bool throwIfNotExists = true, ClaimsIdentity identity = null,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => Get(claim, null, throwIfNotExists, identity, comparison);

        /// <summary>
        /// Returns the value of the claim from the current user
        /// </summary>
        /// <param name="claim">The name of the claim being retrieved</param>
        /// <param name="defaultValue">If the claim is not found, this will be returned instead</param>
        /// <param name="identity">The already retrieved claims identity (optional, default=null and will be retrieved)</param>
        /// <param name="comparison">The string comparison to use (optional, default = CurrentCultureIgnoreCase</param>
        /// <returns>The value of the claim (or defaultValue). If retrieving identity that is unauthenticated, an exception will be raised</returns>
        public static string Get(string claim, string defaultValue, ClaimsIdentity identity = null,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            => Get(claim, defaultValue, false, identity, comparison);
        // ReSharper disable once UnusedParameter.Local
        private static string Get(string claim, string defaultValue, bool throwIfNotExists, ClaimsIdentity identity,
            StringComparison comparison)
        {
            var c = GetRawClaim(claim, identity, comparison);
            if (c != null)
                return c.Value;
            if (throwIfNotExists)
                throw new Exception("IPrincipal user does not have the following claim: " + claim);
            return defaultValue;
        }

        private static Claim GetRawClaim(string t, ClaimsIdentity identity, StringComparison comparison)
            => (identity ?? GetIdentity())
                .Claims
                .FirstOrDefault(x => x.Type.Equals(t, comparison));
        #endregion

        #region Helpers
        /// <summary>
        /// Retrieves the identity of the current user
        /// </summary>
        /// <param name="requireAuthentication">
        /// Optional: Default = true
        /// If the user is not authenticated, this specifies whether to throw an exception (true), or to return the unauthenticated user (false)</param>
        /// <returns></returns>
        public static ClaimsIdentity GetIdentity(bool requireAuthentication = true)
        {
            var user = ClaimsPrincipal.Current.Identities.First();
            if (user.IsAuthenticated)
                return user;
            if (requireAuthentication)
                throw new Exception("Unable to obtain the IPrincipal user");
            return null;
        }
        #endregion
    }
}
