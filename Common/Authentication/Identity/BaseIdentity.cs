using System;
using System.Collections.Generic;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.Authentication.Identity
{
    /// <summary>
    /// The main identity object of the authenticated user
    /// </summary>
    /// <remarks>
    /// This object may be passed around (encrypted) between applications.
    /// The IdentityHelper class will be used for decryption/caching of the object.
    /// I wish I didn't have to declare this class in Utilities (eg. in Implementations instead)
    /// </remarks>
    public abstract class BaseIdentity
    {
        /// <summary>
        /// The Id of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Logon username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The E-mail address of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// In UTC, when does the identity/token expire?
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// All roles that the user has (possibly customer-specific)
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();

        private CaseInsensitiveBinaryList<string> SavedSearchableRoles { get; set; }
        /// <summary>
        /// Searchable collection wrapper around Roles
        /// </summary>
        public CaseInsensitiveBinaryList<string> SearchableRoles
        {
            get
            {
                if (SavedSearchableRoles.IsDefault())
                    SavedSearchableRoles = Roles.ToCaseInsensitiveBinaryList();
                return SavedSearchableRoles;
            }
        }

        /// <summary>
        /// This will send to the logger any additional "custom" properties in your inherited class
        /// </summary>
        /// <returns>Collection of items to be always logged</returns>
        public abstract Dictionary<string, string> GetCustomLoggingProperties();

        /// <summary>
        /// Allows an inherited class to set properties on a default/public identity
        /// </summary>
        public abstract void SetDefaultProperties();
    }
}