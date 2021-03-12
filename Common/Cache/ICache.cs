using System;
using System.Threading.Tasks;
using Sphyrnidae.Common.Cache.Models;

namespace Sphyrnidae.Common.Cache
{
    /// <summary>
    /// Storing items in the cache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Retrieves an item from the Cache
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="item">If the object is not found in the Cache, this will be populated</param>
        /// <returns>True if the object was found, False if not found</returns>
        bool Get<T>(string key, out T item);

        /// <summary>
        /// Sets an item into cache
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="item">The item being set into cache</param>
        void Set<T>(string key, T item);

        /// <summary>
        /// Attempts to retrieve an item from cache.
        /// If not found in cache, will make callback to obtain the item from the caller which will be stored for later use
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="method">The callback function returning the item if not initially found in cache</param>
        /// <returns>The object from cache/callback function</returns>
        T Get<T>(string key, Func<T> method);

        /// <summary>
        /// Attempts to retrieve an item from cache.
        /// If not found in cache, will make callback to obtain the item from the caller which will be stored for later use
        /// </summary>
        /// <typeparam name="T">The type of item</typeparam>
        /// <param name="key">Name of the item in cache</param>
        /// <param name="method">The callback function returning the item if not initially found in cache</param>
        /// <returns>The object from cache/callback function</returns>
        Task<T> GetAsync<T>(string key, Func<Task<T>> method);

        /// <summary>
        /// Removes the given object from cache
        /// </summary>
        /// <remarks>
        /// Note that this removes from local cache, distributed cache, and via SignalR, all other local cache as well.
        /// Any errors in removal of the item will be hidden from the user, so it is preferred to use RemoveAsync if you need this exception handling.
        /// </remarks>
        /// <param name="key">Name of the item in cache</param>
        void Remove(string key);

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
        Task<Exception> RemoveAsync(string key);

        /// <summary>
        /// The options for storing items in cache
        /// </summary>
        CacheOptions Options { get; }
    }
}
