using AuditSystem.Contract.Interfaces.Cache;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AuditSystem.Host.Caching;

/// <summary>
/// A caching service that combines IOutputCacheStore for tag-based eviction and IDistributedCache for key-value operations.
/// </summary>
internal class CacheService : ICacheService
{
    private readonly IOutputCacheStore _outputCache;
    private readonly IDistributedCache _distributedCache;
    private const string TagPrefix = "tag:"; // Prefix for tag-related keys in distributed cache

    public CacheService(IOutputCacheStore outputCache, IDistributedCache distributedCache)
    {
        _outputCache = outputCache ?? throw new ArgumentNullException(nameof(outputCache));
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
    }

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        return cachedValue == null ? default : JsonSerializer.Deserialize<T>(cachedValue);
    }

    /// <inheritdoc />
    public async Task SetAsync<T>(string key, T value, ICollection<string>? tags = null,
        TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var serializedValue = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        // Store the value in distributed cache
        await _distributedCache.SetStringAsync(key, serializedValue, options, cancellationToken);

        // Associate tags if provided
        if (tags != null && tags.Any())
        {
            foreach (var tag in tags)
            {
                var tagKey = $"{TagPrefix}{tag}";
                var taggedKeys = await GetTaggedKeysAsync(tagKey, cancellationToken) ?? new HashSet<string>();
                taggedKeys.Add(key);

                var tagSerialized = JsonSerializer.Serialize(taggedKeys);
                await _distributedCache.SetStringAsync(tagKey, tagSerialized, cancellationToken);
            }
        }
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        await _distributedCache.RemoveAsync(key, cancellationToken);
        await _outputCache.EvictByTagAsync(key, cancellationToken); // Also evict from output cache if applicable
    }

    /// <inheritdoc />
    public async Task RemoveCacheByTagAsync(ICollection<string> tags, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tags, nameof(tags));

        foreach (var tag in tags)
        {
            // Evict from output cache
            await _outputCache.EvictByTagAsync(tag, cancellationToken);

            // Remove tagged keys from distributed cache
            var tagKey = $"{TagPrefix}{tag}";
            var taggedKeys = await GetTaggedKeysAsync(tagKey, cancellationToken);
            if (taggedKeys != null && taggedKeys.Any())
            {
                foreach (var key in taggedKeys)
                {
                    await _distributedCache.RemoveAsync(key, cancellationToken);
                }
                await _distributedCache.RemoveAsync(tagKey, cancellationToken);
            }
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        var value = await _distributedCache.GetAsync(key, cancellationToken);
        return value != null;
    }

    /// <summary>
    /// Helper method to retrieve the set of keys associated with a tag from distributed cache.
    /// </summary>
    private async Task<HashSet<string>?> GetTaggedKeysAsync(string tagKey, CancellationToken cancellationToken)
    {
        var taggedKeysJson = await _distributedCache.GetStringAsync(tagKey, cancellationToken);
        return taggedKeysJson == null ? null : JsonSerializer.Deserialize<HashSet<string>>(taggedKeysJson);
    }
}