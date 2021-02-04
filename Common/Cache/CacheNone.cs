﻿using System;
using System.Threading.Tasks;
using Sphyrnidae.Common.Cache.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Cache
{
    /// <inheritdoc />
    /// <summary>
    /// Accessing cache (no cache actually used)
    /// </summary>
    public class CacheNone : ICache
    {
        public bool Get<T>(string key, out T item)
        {
            item = default;
            return false;
        }

        public CacheOptions Options => new CacheOptions();

        public void Set<T>(string key, T item) { }

        public T Get<T>(string key, Func<T> method) => method();

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> method) => await method();

        public Exception Remove(string key) => default;
    }
}