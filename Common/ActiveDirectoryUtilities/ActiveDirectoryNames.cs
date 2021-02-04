namespace Sphyrnidae.Common.ActiveDirectoryUtilities
{
    /// <summary>
    /// Active Directory Constants
    /// </summary>
    public static class ActiveDirectoryNames
    {
        /// <summary>
        /// This gets all, but should not specify specific ones below
        /// </summary>
        public const string All = "*";

        /// <summary>
        /// This gets 'operational' properties
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        public const string Operational = "+";

        /// <summary>
        /// Sid, eg. dbartels
        /// </summary>
        public const string Sid = "samaccountname";

        /// <summary>
        /// Name, eg. Bartels, Doug
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// Firstname, eg. Doug
        /// </summary>
        public const string FirstName = "givenname";

        /// <summary>
        /// Lastname, eg. Bartels
        /// </summary>
        public const string LastName = "sn";

        /// <summary>
        /// Email, eg. doug.bartels@intel.com
        /// </summary>
        public const string Email = "mail";

        /// <summary>
        /// Roles (a possibly long list of all roles you have in active directory, eg. EAM + groups)
        /// </summary>
        public const string Roles = "memberOf"; // CN=ServiceNOW Techs,CN=Users,DC=fs,DC=local

        /// <summary>
        /// Department, eg. Engineering
        /// </summary>
        public const string Department = "department";

        /// <summary>
        /// Company , eg. Google
        /// </summary>
        public const string Company = "company";

        /// <summary>
        /// The distinguished name of the person being retrieved, can be used in direct query in active directory
        /// </summary>
        public const string ActiveDirectoryEntry = "distinguishedname"; // CN=Doug Bartels,OU=Programmers,OU=FS,DC=fs,DC=local

        /// <summary>
        /// Manager
        /// </summary>
        public const string ManagerActiveDirectoryEntry = "manager";
    }
}
