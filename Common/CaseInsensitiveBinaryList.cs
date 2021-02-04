using System;
using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common
{
    /// <summary>
    /// A class that allows binary operations based on a case-insensitive property on the object
    /// </summary>
    /// <typeparam name="T">Any object type</typeparam>
    public class CaseInsensitiveBinaryList<T> : BinaryList<T, string>
    {
        protected internal CaseInsensitiveBinaryList(IEnumerable<T> items, Func<T, string> keySelector)
            : base(items, keySelector) { }

        #region Search Methods
        /// <summary>
        /// Retrieves a subset of the list matching the key
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>The subset of the list (or empty list if none match)</returns>
        public override IList<T> FindBinaryList(string key) => base.FindBinaryList(key.ToLower());

        /// <summary>
        /// Retrieves a single entry from the list matching the key
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>The single item if found, otherwise the default value (eg. null, or "", or 0... etc)</returns>
        public override T FindBinary(string key) => base.FindBinary(key.ToLower());

        /// <summary>
        /// Specifies if the BinaryList has the given key (similar to Contains or Any)
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>True if the key was found in the list, false otherwise</returns>
        public override bool Has(string key) => base.Has(key.ToLower());

        /// <summary>
        /// Returns the index of the element (so that you know for sure if you found something)
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>The index in the list, or -1 if not found</returns>
        public override int BinarySearch(string key) => base.BinarySearch(key.ToLower());
        #endregion
    }
}
