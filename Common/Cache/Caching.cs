using System;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.Cache
{
    /// <summary>
    /// Wrapper/Helper class around Caching
    /// </summary>
    public static class Caching
    {
        /// <summary>
        /// Retrieves an item from the Cache
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="cache">The instance of the ICache interface</param>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="item">If the object is found in the Cache, this will be populated</param>
        /// <returns>True if the object was found, False if not found</returns>
        public static bool Get<T>(ICache cache, string key, out T item) => cache.Get(key, out item);

        /// <summary>
        /// Sets an item into cache
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="cache">The instance of the ICache interface</param>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="item">The item being set into cache</param>
        public static void Set<T>(ICache cache, string key, T item) => cache.Set(key, item);

        /// <summary>
        /// Attempts to retrieve an item from cache.
        /// If not found in cache, will make callback to obtain the item from the caller which will be stored for later use
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="cache">The instance of the ICache interface</param>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="method">The callback function returning the item if not initially found in cache</param>
        /// <returns>The object from cache/callback function</returns>
        public static T Get<T>(ICache cache, string key, Func<T> method) => cache.Get(key, method);

        /// <summary>
        /// Attempts to retrieve an item from cache.
        /// If not found in cache, will make callback to obtain the item from the caller which will be stored for later use
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="cache">The instance of the ICache interface</param>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="method">The callback function returning the item if not initially found in cache</param>
        /// <returns>The object from cache/callback function</returns>
        public static Task<T> GetAsync<T>(ICache cache, string key, Func<Task<T>> method) => cache.GetAsync(key, method);

        /// <summary>
        /// Removes the given object from cache
        /// </summary>
        /// <remarks>
        /// Note that this removes from local cache, distributed cache, and via SignalR, all other local cache as well
        /// Any exceptions occurring during this process will be hidden
        /// </remarks>
        /// <param name="cache">The instance of the ICache interface</param>
        /// <param name="key">Name of the item in cache</param>
        public static void Remove(ICache cache, string key) => cache.Remove(key);

        /// <summary>
        /// Removes the given object from cache
        /// </summary>
        /// <remarks>
        /// Note that this removes from local cache, distributed cache, and via SignalR, all other local cache as well
        /// </remarks>
        /// <param name="cache">The instance of the ICache interface</param>
        /// <param name="key">Name of the item in cache</param>
        /// <returns>
        /// If an exception was thrown and the item was not fully removed, this exception will be returned.
        /// If everything succeeded, this will be null.
        /// </returns>
        public static Task<Exception> RemoveAsync(ICache cache, string key) => cache.RemoveAsync(key);
    }
}
