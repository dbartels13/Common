using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Sphyrnidae.Common.Cache.Models;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Cache
{
    /// <inheritdoc />
    /// <summary>
    /// Accessing local cache only
    /// </summary>
    public class CacheLocal : ICache
    {
        public CacheOptions Options { get; }
        protected IMemoryCache L1Cache { get; }

        public CacheLocal(DefaultCacheOptions options, IMemoryCache l1Cache)
        {
            Options = options.Item;
            L1Cache = l1Cache;
        }

        /// <inheritdoc />
        /// <summary>
        /// Retrieves an item from the Cache
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="item">If the object is not found in the Cache, this will be populated</param>
        /// <returns>True if the object was found, False if not found</returns>
        public virtual bool Get<T>(string key, out T item)
        {
            // If in cache, just return it
            if (L1Cache.TryGetValue(key, out item))
                return true;

            item = default;
            return false;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets an item into cache
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="item">The item being set into cache</param>
        public virtual void Set<T>(string key, T item)
        {
            var expiration = DateTimeOffset.Now.AddSeconds(Options.Seconds);
            var l1Options = new MemoryCacheEntryOptions {AbsoluteExpiration = expiration, Priority = Options.Priority};
            L1Cache.Set(key, item, l1Options);
        }

        /// <inheritdoc />
        /// <summary>
        /// Attempts to retrieve an item from cache.
        /// If not found in cache, will make callback to obtain the item from the caller which will be stored for later use
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="method">The callback function returning the item if not initially found in cache</param>
        /// <returns>The object from cache/callback function</returns>
        public virtual T Get<T>(string key, Func<T> method)
        {
            if (Get(key, out T item))
                return item;

            return NamedLocker.Lock(key, num =>
            {
                if (Get(key, out item))
                    return item;

                item = method();

                if (Options.DoNotCacheDefault && item.IsDefault())
                    return item;
                // Also handles nulls, in case DoNotCacheDefault = false
                if (Options.DoNotCacheEmptyList && item is IEnumerable e && !e.GetEnumerator().MoveNext())
                    return item;

                Set(key, item);
                return item;
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Attempts to retrieve an item from cache.
        /// If not found in cache, will make callback to obtain the item from the caller which will be stored for later use
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="method">The callback function returning the item if not initially found in cache</param>
        /// <returns>The object from cache/callback function</returns>
        public virtual async Task<T> GetAsync<T>(string key, Func<Task<T>> method)
        {
            if (Get(key, out T item))
                return item;

            return await NamedLocker.LockAsync(key, async () =>
            {
                if (Get(key, out item))
                    return item;

                item = await method();

                if (Options.DoNotCacheDefault && item.IsDefault())
                    return item;
                // Also handles nulls, in case DoNotCacheDefault = false
                if (Options.DoNotCacheEmptyList && item is IEnumerable e && !e.GetEnumerator().MoveNext())
                    return item;

                Set(key, item);
                return item;
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Removes the given object from cache
        /// </summary>
        /// <remarks>
        /// Note that this removes from local cache, distributed cache, and via SignalR, all other local cache as well
        /// </remarks>
        /// <param name="key">Name of the item in cache</param>
        /// <returns>
        /// If an exception was thrown and the item was not fully removed, this exception will be returned.
        /// If everything succeeded, this will be null.
        /// </returns>
        public virtual Exception Remove(string key)
        {
            L1Cache.Remove(key);
            return default;
        }
    }
}
