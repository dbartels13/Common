using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// List (IEnumerable or collection) custom methods
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Takes the list and converts it into a readonly BinaryList which can perform fast search/merge/etc operations
        /// </summary>
        /// <typeparam name="T">Any object type</typeparam>
        /// <typeparam name="TKey">The type of object (eg. string) that the list is ordered by (the key) - it must be IComparable</typeparam>
        /// <param name="items">The current IEnumerable</param>
        /// <param name="keySelector">How the resulting BinaryList is ordered and operated against</param>
        /// <returns>The BinaryList object you can operate against</returns>
        public static BinaryList<T, TKey> ToBinaryList<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
            => new BinaryList<T, TKey>(items, keySelector);

        /// <summary>
        /// Takes the list and converts it into a readonly BinaryList which can perform fast search/merge/etc operations
        /// </summary>
        /// <remarks>For an IEnumerable&lt;string&gt;, you should use the method without the keySelectorExpression</remarks>
        /// <typeparam name="T">Any object type - must be a class, but NOT a string (not compiler enforced)</typeparam>
        /// <param name="items">The current IEnumerable</param>
        /// <param name="keySelectorExpression">How the resulting CaseInsensitiveBinaryList is ordered and operated against</param>
        /// <returns>The CaseInsensitiveBinaryList object you can operate against</returns>
        public static CaseInsensitiveBinaryList<T> ToCaseInsensitiveBinaryList<T>(this IEnumerable<T> items,
            Expression<Func<T, string>> keySelectorExpression) where T : class
        {
            // Ensure the expression is valid
            if (!(keySelectorExpression.Body is MemberExpression memberSelectorExpression))
                throw new Exception("Unable to generate CaseInsensitiveBinaryList due to invalid keySelectorExpression MemberExpression");
            if (!(memberSelectorExpression.Member is PropertyInfo property))
                throw new Exception("Unable to generate CaseInsensitiveBinaryList due to invalid keySelectorExpression PropertyInfo");

            // Update the keys to be lowercase
            var keySelector = keySelectorExpression.Compile();
            var list = items.ToList(); // Avoid multiple enumerations
            foreach (var item in list)
                property.SetValue(item, keySelector(item).ToLower());

            // Create the CaseInsensitiveBinaryList
            return new CaseInsensitiveBinaryList<T>(list, keySelector);
        }

        /// <summary>
        /// Takes the list and converts it into a readonly BinaryList which can perform fast search/merge/etc operations
        /// </summary>
        /// <param name="items">The current IEnumerable</param>
        /// <returns>The CaseInsensitiveBinaryList object you can operate against</returns>
        public static CaseInsensitiveBinaryList<string> ToCaseInsensitiveBinaryList(this IEnumerable<string> items)
        {
            var list = items
                .ToList()
                .ConvertAll(x => x.ToLower());
            return new CaseInsensitiveBinaryList<string>(list, x => x);
        }

        /// <summary>
        /// Returns a distinct collection of items matching the selector
        /// </summary>
        /// <typeparam name="T">Any object type</typeparam>
        /// <typeparam name="TKey">The selector(s) for which properties define the distinct properties</typeparam>
        /// <param name="list">The enumeration with possibly duplicate key entries</param>
        /// <param name="keySelector">The field in the list which is the key</param>
        /// <example>myList.DistinctBy(x => x.Name); myList.DistinctBy(x => new { x.Name, x.OtherVal } );</example>
        /// <returns>The distinct list</returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            var known = new HashSet<TKey>();
            return list.Where(element => known.Add(keySelector(element)));
        }

        /// <summary>
        /// Converts a list of a derived type to a list of it's base class type
        /// </summary>
        /// <remarks>Supposedly Covariance has been introduced to IEnumerable, but I've never tested. Lists are also NOT contra-variant</remarks>
        /// <typeparam name="TDerived">The derived class type</typeparam>
        /// <typeparam name="TBase">The base class type of which the derived class inherits</typeparam>
        /// <param name="list">The original enumeration</param>
        /// <returns>The converted list</returns>
        public static IEnumerable<TBase> ToBaseList<TDerived, TBase>(this IEnumerable<TDerived> list)
            where TDerived : TBase
            => list.Cast<TBase>();

        /// <summary>
        /// Inserts an item into the list before the matching item
        /// </summary>
        /// <remarks>If the item to insert before is not found, it will be inserted at the end of the list</remarks>
        /// <typeparam name="T">Any object type</typeparam>
        /// <param name="list">The original list</param>
        /// <param name="matchingItem">Predicate for finding the matching item in the list</param>
        /// <param name="item">The item to insert into the list</param>
        /// <returns>The original list with the item added</returns>
        public static List<T> InsertBefore<T>(this List<T> list, Predicate<T> matchingItem, T item)
        {
            var idx = list.FindIndex(matchingItem);
            if (idx < 0)
                list.Add(item);
            else
                list.Insert(idx, item);
            return list;
        }

        /// <summary>
        /// Inserts an item into the list after the matching item
        /// </summary>
        /// <remarks>If the item to insert after is not found, it will be inserted at the end of the list</remarks>
        /// <typeparam name="T">Any object type</typeparam>
        /// <param name="list">The original list</param>
        /// <param name="matchingItem">Predicate for finding the matching item in the list</param>
        /// <param name="item">The item to insert into the list</param>
        /// <returns>The original list with the item added</returns>
        public static List<T> InsertAfter<T>(this List<T> list, Predicate<T> matchingItem, T item)
        {
            var idx = list.FindIndex(matchingItem);
            if (idx < 0)
                list.Add(item);
            else
                list.Insert(idx + 1, item);
            return list;
        }
    }
}
