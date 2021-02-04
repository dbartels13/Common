//using System;
//using System.Collections.Generic;
//using System.Linq;
//// ReSharper disable UnusedMember.Global

//namespace Sphyrnidae.Utilities.ActiveDirectoryUtilities
//{
//    /// <summary>
//    /// This is used by the Active Directory call to retrieve a user from Active Directory
//    /// </summary>
//    public class ActiveDirectoryUser
//    {
//        #region Properties
//        /// <summary>
//        /// Sid of the user (without domain)
//        /// </summary>
//        public string Sid { get; internal set; }

//        /// <summary>
//        /// Full name (distinguished name) of the user
//        /// </summary>
//        public string Name { get; internal set; }

//        /// <summary>
//        /// First name of the user
//        /// </summary>
//        public string FirstName { get; internal set; }

//        /// <summary>
//        /// Last name of the user
//        /// </summary>
//        public string LastName { get; internal set; }

//        /// <summary>
//        /// E-mail address of the user
//        /// </summary>
//        public string Email { get; internal set; }

//        /// <summary>
//        /// The groups which the user belongs to
//        /// </summary>
//        public List<string> Groups { get; internal set; }

//        /// <summary>
//        /// The entry of this person in active directory
//        /// </summary>
//        internal string DistinguishedName { get; set; }

//        /// <summary>
//        /// The manager entry in active directory
//        /// </summary>
//        internal string ManagerDistinguishedName { get; set; }

//        private ActiveDirectoryUser _manager;
//        private List<ActiveDirectoryUser> _employees;
//        private bool _employeesRecursiveRetrieved;

//        public int NumEmployees => GetEmployees().Count;
//        #endregion

//        #region Constructor
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        internal ActiveDirectoryUser()
//        {
//            Groups = new List<string>();
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Retrieves listing of all those reporting to the current person.
//        /// </summary>
//        /// <param name="recursive">If false (default), it only gets the direct reports. If true, gets everyone below (expensive/performance)</param>
//        /// <returns>The collection of employees</returns>
//        public List<ActiveDirectoryUser> GetEmployees(bool recursive = false)
//        {
//            if (string.IsNullOrWhiteSpace(DistinguishedName))
//                return _employees; // Could be null, or already somehow populated

//            if (_employees == null)
//                _employees = ActiveDirectory.GetDirectReports(DistinguishedName).ToList();

//            if (!recursive || _employeesRecursiveRetrieved)
//                return _employees;

//            foreach (var emp in _employees)
//                emp.GetEmployees(true);

//            _employeesRecursiveRetrieved = true;
//            return _employees;
//        }

//        /// <summary>
//        /// Retrieves the manager object of the current person
//        /// </summary>
//        /// <returns>The manager object</returns>
//        public ActiveDirectoryUser GetManager()
//        {
//            if (_manager == null && !string.IsNullOrWhiteSpace(ManagerDistinguishedName))
//                _manager = ActiveDirectory.GetUser(ManagerDistinguishedName, ActiveDirectory.IdType.DistinguishedName, false, false);
//            return _manager;
//        }

//        /// <summary>
//        /// Does the user have a given role/group
//        /// </summary>
//        /// <param name="role">The role in active directory/EAM</param>
//        /// <param name="allowAccessWhenNoRolesRequired">Default = true, If true, and roles are blank, then the user has access. If false, then the user must explicitly have a given role</param>
//        /// <returns>True if exists, false otherwise</returns>
//        public bool HasRole(string role, bool allowAccessWhenNoRolesRequired = true) => HasRole(new List<string> { role }, allowAccessWhenNoRolesRequired);

//        /// <summary>
//        /// Does the user have a given role/group
//        /// </summary>
//        /// <param name="names">The name of the role/groups required in active directory/EAM (as a list)</param>
//        /// <param name="allowAccessWhenNoRolesRequired">Default = true, If true, and roles are blank, then the user has access. If false, then the user must explicitly have a given role</param>
//        /// <returns>True if exists, false otherwise</returns>
//        public bool HasRole(List<string> names, bool allowAccessWhenNoRolesRequired = true)
//        {
//            return names.Count == 0 ? allowAccessWhenNoRolesRequired : names.Any(name => name.Equals(Sid, StringComparison.CurrentCultureIgnoreCase) || !string.IsNullOrEmpty(Groups.FindBinary(x => x, name.ToLower())));
//        }
//        #endregion
//    }
//}
