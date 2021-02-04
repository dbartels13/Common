using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Utilities
{
    /// <summary>
    /// Performs a lock that is "name" specific
    /// </summary>
    public static class NamedLocker
    {
        private class LockObject
        {
            internal long LockCount;

            internal LockObject LockCountAdd(long i)
            {
                Interlocked.Add(ref LockCount, i);
                return this;
            }
        }

        private class SemaphoreObject : LockObject
        {
            internal readonly SemaphoreSlim Obj = new SemaphoreSlim(1, 1);

            internal SemaphoreObject SemaphoreCountAdd(long i)
            {
                LockCountAdd(i);
                return this;
            }
        }

        private static readonly ConcurrentDictionary<string, LockObject> LockDict =
            new ConcurrentDictionary<string, LockObject>();

        private static readonly ConcurrentDictionary<string, SemaphoreObject> SemaphoreDict =
            new ConcurrentDictionary<string, SemaphoreObject>();

        // ReSharper disable once InconsistentlySynchronizedField
        private static LockObject GetLock(string name)
            => LockDict
                .GetOrAdd(name, s => new LockObject())
                .LockCountAdd(1);

        private static SemaphoreObject GetSemaphore(string name)
            => SemaphoreDict
                .GetOrAdd(name, s => new SemaphoreObject())
                .SemaphoreCountAdd(1);

        private static void TryRemoveLock(string name, LockObject o)
        {
            if (o.LockCountAdd(-1).LockCount == 0)
                LockDict.TryRemove(name, out _);
        }

        private static void TryRemoveSemaphore(string name, SemaphoreObject o)
        {
            o.Obj.Release();
            if (o.SemaphoreCountAdd(-1).LockCount == 0)
                SemaphoreDict.TryRemove(name, out _);
        }

        /// <summary>
        /// Locks the method based on unique name
        /// </summary>
        /// <param name="name">Unique name which acts as a lock</param>
        /// <param name="method">The locked method that is locked by name</param>
        public static void Lock(string name, Action<long> method)
        {
            var o = GetLock(name);
            lock (o)
            {
                try
                {
                    method(o.LockCount);
                }
                finally
                {
                    TryRemoveLock(name, o);
                }
            }
        }


        /// <summary>
        /// Locks the method based on unique name
        /// </summary>
        /// <param name="name">Unique name which acts as a lock</param>
        /// <param name="method">The locked method that is locked by name</param>
        public static async Task LockAsync(string name, Func<Task> method)
        {
            var o = GetSemaphore(name);
            await o.Obj.WaitAsync();
            try
            {
                await method();
            }
            finally
            {
                TryRemoveSemaphore(name, o);
            }
        }

        /// <summary>
        /// Locks the method based on unique name
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="name">Unique name which acts as a lock</param>
        /// <param name="method">The locked method that is locked by name</param>
        /// <returns>Whatever the method returns</returns>
        public static T Lock<T>(string name, Func<long, T> method)
        {
            var o = GetLock(name);
            lock (o)
            {
                try
                {
                    return method(o.LockCount);
                }
                finally
                {
                    TryRemoveLock(name, o);
                }
            }
        }

        /// <summary>
        /// Locks the method based on unique name
        /// </summary>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <param name="name">Unique name which acts as a lock</param>
        /// <param name="method">The locked method that is locked by name</param>
        /// <returns>Whatever the method returns</returns>
        public static async Task<T> LockAsync<T>(string name, Func<Task<T>> method)
        {
            var o = GetSemaphore(name);
            await o.Obj.WaitAsync();
            try
            {
                return await method();
            }
            finally
            {
                TryRemoveSemaphore(name, o);
            }
        }
    }
}
