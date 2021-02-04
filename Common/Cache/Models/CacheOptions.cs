using Microsoft.Extensions.Caching.Memory;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Cache.Models
{
    /// <summary>
    /// Options available for setting an item into cache
    /// </summary>
    public class CacheOptions
    {
        public static int Minute => 60;
        public static int Hour => 3600;
        public static int Day => 86400;
        public static int Month => 2592000;
        public static int Year => 31536000;

        /// <summary>
        /// Default = true (set in DefaultCacheOptions to true as well)
        /// If true, this entry will reside in local cache
        /// </summary>
        public bool UseLocalCache { get; set; } = true;

        /// <summary>
        /// Default = false (Actually set in DefaultCacheOptions to true)
        /// If true, this entry will reside in distributed cache
        /// </summary>
        public bool UseDistributedCache { get; set; } = false;

        /// <summary>
        /// Default = 1200s (20 minutes).
        /// How long will the item remain in cache
        /// </summary>
        public int Seconds { get; set; } = 1200;

        /// <summary>
        /// Default = Normal
        /// What is the policy for removal of cached items?
        /// </summary>
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;

        /// <summary>
        /// Default = true
        /// If true, and using the Get method with setter function, this will NOT cache if the value retrieved from the setter function is default
        /// </summary>
        public bool DoNotCacheDefault { get; set; } = true;

        /// <summary>
        /// Default = true
        /// If true, and using the Get method with setter function, this will NOT cache if the value is IEnumerable and contains no items
        /// Use this with caution though as string (and possibly others) do inherit from IEnumerable
        /// </summary>
        public bool DoNotCacheEmptyList { get; set; } = true;
    }
}
