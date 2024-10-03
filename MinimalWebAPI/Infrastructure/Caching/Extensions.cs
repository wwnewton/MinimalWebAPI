// <copyright file="Extensions.cs" company="Newton Software">
// Copyright (c) Newton Software. All rights reserved.
// </copyright>

namespace MinimalWebAPI.Infrastructure.Caching;

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// IDistributedCache extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Get or add an item to the cache.
    /// </summary>
    /// <typeparam name="T">Type of item to add to cache.</typeparam>
    /// <param name="cache">Cache.</param>
    /// <param name="key">Key.</param>
    /// <param name="factory">Factory to get item if it does not exist.</param>
    /// <param name="options">Distributed cache options.</param>
    /// <returns>Cache item of type T.</returns>
    public static async Task<T?> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> factory, DistributedCacheEntryOptions? options = null)
    {
        var value = await cache.GetStringAsync(key);
        if (value is not null)
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        var result = await factory();
        var cacheOptions = options ?? new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTime.Now.AddSeconds(30), };
        await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(result), cacheOptions);

        return result;
    }
}
