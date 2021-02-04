//using System;
//using System.Collections.Generic;
//using System.Linq;
//// ReSharper disable UnusedMember.Global

//namespace Sphyrnidae.Utilities
//{
//    /// <summary>
//    /// Allows you to quickly save/retrieve data
//    /// </summary>
//    /// <typeparam name="TKey">The key that identifies the record</typeparam>
//    /// <typeparam name="TValue">The record value</typeparam>
//    /// <remarks>This is NOT thread safe</remarks>
//    public class FastLookup<TKey, TValue> where TKey : IComparable<TKey>
//    {
//        private List<KeyValuePair<TKey, TValue>> _list;
//        private readonly Dictionary<TKey, TValue> _lookups;
//        private readonly TKey[] _keys;
//        private readonly TValue[] _values;
//        private int _count;
//        private readonly int _max;
//        private readonly int _sortAfter;

//        public FastLookup(int recent = 3, int sortAfter = 5)
//        {
//            _lookups = new Dictionary<TKey, TValue>();
//            _keys = new TKey[recent];
//            _values = new TValue[recent];
//            _count = 0;
//            _max = recent;
//            _sortAfter = sortAfter;
//        }

//        /// <summary>
//        /// Attempts to get a saved record. If not found, retrieves that value and saves the record
//        /// </summary>
//        /// <param name="key">The key that identifies the record</param>
//        /// <param name="func">If not currently found, a function that will set the value</param>
//        /// <returns>The record value</returns>
//        public TValue Get(TKey key, Func<TValue> func)
//        {
//            // First time, we can't do any lookups
//            if (_count == 0)
//            {
//                var val = func();
//                _keys[0] = key;
//                _values[0] = val;
//                _count++;
//                _lookups.Add(key, val);
//                return val;
//            }

//            // See if we have this in our recent array
//            if (_keys[0].Equals(key))
//                return _values[0];

//            for (var i = 1; i < _count; i++)
//            {
//                if (!_keys[i].Equals(key))
//                    continue;

//                // Found it, move that one to the top of the queue/array
//                var val = _values[i];
//                UpdateRecent(key, val, i);
//                return val;
//            }

//            // If not in non-full recent array, we don't have it
//            if (_count < _max)
//            {
//                _count++;
//                var val = func();
//                _lookups.Add(key, val);
//                UpdateRecent(key, val, _count - 1);
//                return val;
//            }

//            // Do we have it in the sorted list?
//            if (_list != null)
//            {
//                var idx = _list.BinarySearch(x => x.Key, key);
//                if (idx >= 0)
//                {
//                    var val = _list[idx].Value;
//                    UpdateRecent(key, val, _count - 1);
//                    return val;
//                }
//            }

//            // Do we currently have the value in lookups?
//            if (_lookups.TryGetValue(key, out var value))
//            {
//                UpdateRecent(key, value, _count - 1);
//                return value;
//            }

//            // Don't have it, so call the method
//            value = func();
//            _lookups.Add(key, value);
//            UpdateRecent(key, value, _count - 1);

//            // Possibly move dictionary into sorted list
//            if (_lookups.Count < _sortAfter)
//                return value;

//            _list ??= new List<KeyValuePair<TKey, TValue>>();
//            foreach (var item in _lookups)
//                _list.Add(new KeyValuePair<TKey, TValue>(item.Key, item.Value));
//            _list = _list.OrderBy(x => x.Key).ToList();
//            _lookups.Clear();
//            return value;
//        }

//        private void UpdateRecent(TKey key, TValue value, int num)
//        {
//            // Found it, move that one to the top of the queue/array
//            while (num > 0)
//            {
//                _keys[num] = _keys[num - 1];
//                _values[num] = _values[num - 1];
//                num--;
//            }
//            _keys[0] = key;
//            _values[0] = value;
//        }
//    }
//}
