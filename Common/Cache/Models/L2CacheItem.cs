using System;
using Microsoft.Extensions.Caching.Memory;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.Cache.Models
{
    [Serializable]
    public class L2CacheItem<T>
    {
        public DateTime Expiration { get; set; }
        public CacheItemPriority Priority { get; set; }
        public T Item { get; set; }

        // Must provide default constructor so it can be serialized
        public L2CacheItem() { }

        internal L2CacheItem(int seconds, CacheItemPriority priority, T item)
        {
            Expiration = DateTime.Now.ToUniversalTime().AddSeconds(seconds);
            Priority = priority;
            Item = item;
        }

        internal TimeSpan? RemainingCacheTime()
        {
            var expires = Expiration.AsUtc();
            var now = DateTime.Now.ToUniversalTime();
            if (now >= expires)
                return null;
            return expires - now;
        }
    }
}