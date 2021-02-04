//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Sphyrnidae.Utilities.ActiveDirectoryUtilities
//{
//    /// <summary>
//    /// Helper class to retrieve all things Active Directory (EAM). Note: This does not cache any results, always does the real lookup every time
//    /// </summary>
//    public static class ActiveDirectory
//    {
//        #region AD Queries
//        //private const string PreQuery = "(&(objectCategory=user)(objectClass=user)(!(name=ad_*))";
//        //private const string PreQuery = "(&(" + AdNames.BadgeType + "=*)(objectCategory=person)";
//        private const string PreQuery = "(&(objectCategory=person)";
//        private const string PostQuery = ")";
//        #endregion

//        #region Get all active directory attributes
//        /// <summary>
//        /// Debugging to retrieve all main active roles for a user (does not get roles/groups)
//        /// </summary>
//        /// <param name="sid">The sid of the person</param>
//        /// <returns>The collection</returns>
//        public static List<KeyValuePair<string, string>> GetAllActiveDirectoryAttributes(string sid)
//        {
//            return Caching.Get("All Active Directory Attributes_" + sid, () =>
//            {
//                var searchResults = GetRawAllResults(sid).ToList();
//                if (searchResults.Count != 1)
//                    throw new Exception("User is not found in active directory");
//                var entry = searchResults.First();
//                var props = entry.Properties;
//                return (from string propName in props.PropertyNames
//                        select new KeyValuePair<string, string>(propName, entry.Properties[propName][0].ToString()))
//                    .ToList();
//            }, 300);
//        }

//        private static IEnumerable<SearchResult> GetRawAllResults(string sid)
//        {
//            using (var searcher = new DirectorySearcher(GetRootEntry()))
//            {
//                searcher.CacheResults = false;
//                searcher.PageSize = 1000;
//                searcher.PropertiesToLoad.Add(AdNames.All);
//                searcher.Filter = PreQuery + "(" + AdNames.Sid + $"={sid})" + PostQuery;
//                return SafeGetResults(searcher);
//            }
//        }
//        #endregion

//        #region Get single user
//        /// <summary>
//        /// Retrieves a single user from active directory
//        /// </summary>
//        /// <param name="id">The search string for the user</param>
//        /// <param name="t">The type that ID is</param>
//        /// <param name="includeRoles">
//        /// If true, this will include all roles that the user belongs to (takes longer to retrieve)
//        /// Default = false
//        /// </param>
//        /// <param name="throwOnNotFound">
//        /// If the single user is not found, this specifies whether an exception is raised, or if null will be returned
//        /// Default = true (exception will be raised)
//        /// </param>
//        /// <returns>The found active directory user (or null if not found and not throwing)</returns>
//        public static ActiveDirectoryUser GetUser(string id, IdType t, bool includeRoles = false, bool throwOnNotFound = true)
//        {
//            return Caching.Get(
//                "Get Active Directory_" + id + "_t" + (includeRoles ? "yes" : "no")
//                , () => SetUserObject(GetUserSearchResults(id, t, includeRoles, throwOnNotFound), includeRoles)
//                , 300);
//        }

//        private static SearchResult GetUserSearchResults(string id, IdType t, bool includeRoles, bool throwOnNotFound)
//        {
//            var searchResults = new List<SearchResult>();
//            if (t == IdType.Sid || t == IdType.Any)
//                searchResults.AddRange(GetRawIdResults(id, AdNames.Sid, includeRoles).ToList());
//            if (t == IdType.DistinguishedName || t == IdType.Any)
//                searchResults.AddRange(GetRawIdResults(id, AdNames.ActiveDirectoryEntry, includeRoles)
//                    .ToList());
//            if (searchResults.Count == 1)
//                return searchResults.First();
//            if (throwOnNotFound)
//                throw new Exception("User is not found in active directory: " + id);
//            return null;
//        }

//        private static IEnumerable<SearchResult> GetRawIdResults(string id, string adName, bool includeRoles)
//        {
//            //using (var searcher = new DirectorySearcher("GC://DC=CORP,DC=INTEL,DC=COM"))
//            using (var searcher = new DirectorySearcher(GetRootEntry()))
//            {
//                FillInSearcher(searcher, includeRoles);
//                searcher.Filter = string.Format(PreQuery + "(" + adName + "={0})" + PostQuery, id);
//                return SafeGetResults(searcher);
//            }
//        }
//        #endregion

//        #region Search for users
//        /// <summary>
//        /// Retrieves records from active directory matching the search text (non-cached)
//        /// </summary>
//        /// <param name="searchText">The text to look for as SID, Name</param>
//        /// <param name="size">How many results to return (for quicker querying/return)</param>
//        /// <returns>The collection of users matching</returns>
//        public static IEnumerable<ActiveDirectoryUser> Search(string searchText, int size = 10)
//        {
//            var searchResults = GetRawSearchResults(searchText, size);
//            return searchResults.Select(user => SetUserObject(user, false))
//                .Where(user => !string.IsNullOrWhiteSpace(user.Sid));
//        }

//        private static IEnumerable<SearchResult> GetRawSearchResults(string searchText, int size)
//        {
//            string filter;

//            // Example Names:
//            //  Bartels, Doug           (Last, First)
//            //  Reza, Juan D            (Last, First MI) - Middle Initial is part of the first name in active directory
//            //  Jimenez Campos, Roy A   (Last1 Last2, First MI) - Last1/2 is the last name
//            //  Doug Bartels            (First Last)
//            //  Juan D Reza             (First MI Last) - Middle Initial is part of the first name in active directory
//            //  Roy A Jimenez Campos    (First MI Last1 Last2) - Last1/2 is the last name
//            //  Roy Jimenez Campos      (First Last1 Last2) - Last1/2 is the last name

//            // If a comma, assume this is "Last, First" notation
//            if (searchText.Contains(','))
//            {
//                var names = searchText.Split(new[] { ',' }, 2);
//                filter = string.Format(
//                    PreQuery + "(" + AdNames.FirstName + "={1}*)(" + AdNames.LastName + "={0}*)" + PostQuery, names[0],
//                    names[1]);
//            }

//            // If a space (no comma), assume this is "First Last"
//            else if (searchText.Contains(' '))
//            {
//                var names = searchText.Split(' ');
//                var firstNameIndex = 0;
//                if (names.Length > 2)
//                {
//                    for (var i = 1; i < names.Length - 1; i++)
//                    {
//                        if (names[i].Trim().Length != 1)
//                            continue;
//                        firstNameIndex = i;
//                        break;
//                    }
//                }

//                var firstname = "";
//                for (var i = 0; i <= firstNameIndex; i++)
//                {
//                    if (!string.IsNullOrWhiteSpace(firstname))
//                        firstname += " ";
//                    firstname += names[i].Trim();
//                }

//                var lastname = "";
//                for (var i = firstNameIndex + 1; i < names.Length; i++)
//                {
//                    if (!string.IsNullOrWhiteSpace(lastname))
//                        lastname += " ";
//                    lastname += names[i].Trim();
//                }

//                filter = PreQuery + "(" + AdNames.FirstName + $"={firstname}*)(" + AdNames.LastName + $"={lastname}*)" +
//                         PostQuery;
//            }

//            // default query to catch whatever it is hopefully
//            else
//                filter = string.Format(
//                    PreQuery + "(|(" + AdNames.Name + "={0}*)(" + AdNames.Email + "={0}*)(" + AdNames.Sid + "={0}*)(" +
//                    AdNames.FirstName + "={0}*)(" + AdNames.LastName + "={0}*))" + PostQuery, searchText);

//            using (var searcher = new DirectorySearcher(GetRootEntry()))
//            {
//                FillInSearcher(searcher, false);
//                searcher.Filter = filter;
//                searcher.SearchScope = SearchScope.Subtree;
//                searcher.Sort = new SortOption("Name", SortDirection.Ascending);
//                searcher.ReferralChasing = ReferralChasingOption.None;
//                searcher.PropertyNamesOnly = false;
//                searcher.PageSize = 0;
//                searcher.SizeLimit = size;
//                return SafeGetResults(searcher);
//            }
//        }
//        #endregion

//        #region Getting associated people in management hierarchy
//        /// <summary>
//        /// Retrieves all of your co-workers (those that share the same manager) (possibly cached)
//        /// </summary>
//        /// <param name="user">The active directory user requesting teammates</param>
//        /// <param name="includeRoles">If the users should be populated with roles (costly operation)</param>
//        /// <returns>List of all people sharing the same manager</returns>
//        public static IEnumerable<ActiveDirectoryUser> GetTeammates(ActiveDirectoryUser user, bool includeRoles = false) => GetDirectReports(user.ManagerDistinguishedName, includeRoles);

//        /// <summary>
//        /// Retrieves those people reporting to the given manager (possibly cached)
//        /// </summary>
//        /// <param name="user">The active directory user to retrieve employees for</param>
//        /// <param name="includeRoles">If the users should be populated with roles (costly operation)</param>
//        /// <returns>The collection of users reporting to the manager (or empty collection if none)</returns>
//        internal static IEnumerable<ActiveDirectoryUser> GetDirectReports(ActiveDirectoryUser user, bool includeRoles = false) => GetDirectReports(user.DistinguishedName, includeRoles);

//        /// <summary>
//        /// Retrieves those people reporting to the given manager
//        /// </summary>
//        /// <param name="distinguishedName">The distinguished name in active directory for the manager</param>
//        /// <param name="includeRoles">If the users should be populated with roles (costly operation)</param>
//        /// <returns>The collection of users reporting to the manager (or empty collection if none)</returns>
//        internal static IEnumerable<ActiveDirectoryUser> GetDirectReports(string distinguishedName, bool includeRoles = false)
//        {
//            return Caching.Get(
//                "Get Active Directory Reports" + distinguishedName + (includeRoles ? "yes" : "no")
//                , () => GetRawDirectReportsResults(distinguishedName, includeRoles).Select(x => SetUserObject(x, includeRoles))
//                , 300
//            );
//        }

//        private static IEnumerable<SearchResult> GetRawDirectReportsResults(string managerDn, bool includeRoles)
//        {
//            using (var searcher = new DirectorySearcher(GetRootEntry()))
//            {
//                FillInSearcher(searcher, includeRoles);
//                searcher.Filter = PreQuery + "(" + AdNames.ManagerActiveDirectoryEntry + $"={managerDn})" + PostQuery;
//                return SafeGetResults(searcher);
//            }
//        }
//        #endregion

//        #region Conversion from Search Result to Objects
//        private static ActiveDirectoryUser SetUserObject(SearchResult user, bool includeGroups)
//        {
//            if (ReferenceEquals(user, null))
//                return null;
//            var adUser = new ActiveDirectoryUser
//            {
//                Sid = GetActiveDirectoryProperty(user, AdNames.Sid),
//                Name = GetActiveDirectoryProperty(user, AdNames.Name),
//                FirstName = GetActiveDirectoryProperty(user, AdNames.FirstName),
//                LastName = GetActiveDirectoryProperty(user, AdNames.LastName),
//                Email = GetActiveDirectoryProperty(user, AdNames.Email),
//                DistinguishedName = GetActiveDirectoryProperty(user, AdNames.ActiveDirectoryEntry),
//            };
//            if (includeGroups)
//                adUser.Groups =
//                    (from object role in user.Properties[AdNames.Roles] select GetGroupName(role.ToString()))
//                    .Select(x => x.ToLower())
//                    .OrderBy(x => x)
//                    .ToList();
//            return adUser;
//        }
//        #endregion

//        #region Common Private Methods
//        private static DirectoryEntry GetRootEntry()
//        {
//            var root = new DirectoryEntry("GC:");
//            var list = root.Children.GetEnumerator();
//            list.MoveNext();
//            return (DirectoryEntry)list.Current;
//        }

//        private static void FillInSearcher(DirectorySearcher searcher, bool includeGroups)
//        {
//            searcher.CacheResults = false;
//            searcher.PageSize = 1000;
//            searcher.PropertiesToLoad.Add(AdNames.Sid);
//            searcher.PropertiesToLoad.Add(AdNames.Name);
//            searcher.PropertiesToLoad.Add(AdNames.FirstName);
//            searcher.PropertiesToLoad.Add(AdNames.LastName);
//            searcher.PropertiesToLoad.Add(AdNames.Email);
//            searcher.PropertiesToLoad.Add(AdNames.ActiveDirectoryEntry);
//            if (includeGroups)
//                searcher.PropertiesToLoad.Add(AdNames.Roles);
//        }

//        private static IEnumerable<SearchResult> SafeGetResults(DirectorySearcher searcher)
//        {
//            using (var results = searcher.FindAll())
//            {
//                foreach (SearchResult result in results)
//                {
//                    yield return result;
//                }
//            } // SearchResultCollection will be disposed here
//        }

//        private static string GetActiveDirectoryProperty(SearchResult user, string property)
//        {
//            if (user != null && user.Properties[property].Count == 1)
//                return user.Properties[property][0].ToString();
//            return null;
//        }

//        private static string GetGroupName(string origGroupName)
//        {
//            var groupName = origGroupName.Replace("\\,", "\\&c");
//            groupName = groupName.Substring(0, groupName.IndexOf(",", 0, StringComparison.Ordinal));
//            groupName = groupName.Replace("CN=", "").Trim();
//            return groupName.Replace("\\&c", ",");
//        }
//        #endregion
//    }
//}
