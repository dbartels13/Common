using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common
{
    /// <summary>
    /// A class that allows binary operations
    /// </summary>
    /// <typeparam name="T">Any object type</typeparam>
    /// <typeparam name="TKey">The type of object (eg. string) that the list is ordered by (the key) - it must be IComparable</typeparam>
    public class BinaryList<T, TKey> : ReadOnlyCollection<T> where TKey : IComparable<TKey>
    {
        protected Func<T, TKey> KeySelector { get; }

        protected internal BinaryList(IEnumerable<T> items, Func<T, TKey> keySelector)
            : base(items.OrderBy(keySelector).ToList())
        {
            KeySelector = keySelector;
        }

        #region Search Methods
        /// <summary>
        /// Retrieves a subset of the list matching the key
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>The subset of the list (or empty list if none match)</returns>
        public virtual IList<T> FindBinaryList(TKey key)
        {
            var idx = BinarySearch(key);
            var myResults = new List<T>();
            if (idx < 0)
                return myResults;

            // might need to back up the index if the found number is in the middle somewhere
            while (idx > 0)
            {
                if (KeySelector(Items[idx - 1]).CompareTo(key) == 0)
                    idx--;
                else
                    break;
            }

            while (idx < Items.Count)
            {
                if (KeySelector(Items[idx]).CompareTo(key) == 0)
                {
                    myResults.Add(Items[idx]);
                    idx++;
                }
                else
                    break;
            }

            return myResults;
        }

        /// <summary>
        /// Retrieves a single entry from the list matching the key
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>The single item if found, otherwise the default value (eg. null, or "", or 0... etc)</returns>
        public virtual T FindBinary(TKey key)
        {
            var idx = BinarySearch(key);
            return idx >= 0 ? Items[idx] : default;
        }

        /// <summary>
        /// Specifies if the BinaryList has the given key (similar to Contains or Any)
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>True if the key was found in the list, false otherwise</returns>
        public virtual bool Has(TKey key) => BinarySearch(key) >= 0;

        /// <summary>
        /// Returns the index of the element (so that you know for sure if you found something)
        /// </summary>
        /// <param name="key">The value of the key</param>
        /// <returns>The index in the list, or -1 if not found</returns>
        public virtual int BinarySearch(TKey key)
        {
            var min = 0;
            var max = Items.Count - 1;

            while (min < max)
            {
                var mid = (max + min) / 2;
                var midItem = Items[mid];
                var midKey = KeySelector(midItem);
                var comp = midKey.CompareTo(key);
                if (comp < 0)
                    min = mid + 1;
                else if (comp > 0)
                    max = mid - 1;
                else
                    return mid;
            }

            if (min == max && KeySelector(Items[min]).CompareTo(key) == 0)
                return min;

            return -1;
        }

        #endregion

        #region Intersect
        /// <summary>
        /// Returns the items from the provided enumeration that match (intersect) on the provided key (Eg. Join where selecting the provided enumeration objects)
        /// </summary>
        /// <typeparam name="T2">Any object type</typeparam>
        /// <param name="list">The list containing those items which must be contained in the first list</param>
        /// <param name="keySelector">The key selector for the enumeration</param>
        /// <returns>Items from the provided enumeration that match (intersect) on the provided key</returns>
        public virtual IEnumerable<T2> Intersect<T2>(IEnumerable<T2> list, Func<T2, TKey> keySelector)
            => list.Where(x => Has(keySelector(x)));

        /// <summary>
        /// Returns the items from the provided enumeration that don't match (intersect) on the provided key (eg. left join where not exists; selecting the provided enumeration objects)
        /// </summary>
        /// <typeparam name="T2">Any object type</typeparam>
        /// <param name="list">The list containing those items which must not be contained in the first list</param>
        /// <param name="keySelector">The key selector for the enumeration</param>
        /// <returns>Items from the provided enumeration that don't match (intersect) on the provided key</returns>
        public virtual IEnumerable<T2> NonIntersect<T2>(IEnumerable<T2> list, Func<T2, TKey> keySelector)
            => list.Where(x => !Has(keySelector(x)));
        #endregion
    }
}
