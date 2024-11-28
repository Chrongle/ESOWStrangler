using System.Text.Json;
using Catalog.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Infrastructure.Cache;
public class RedisCacheService(IDistributedCache cache) : ICacheService
{
  public async Task<T> GetCachedDataAsync<T>(string key)
  {
    var jsonData = await cache.GetStringAsync(key);
    return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
  }

  public Task SetCachedDataAsync<T>(string key, T data, TimeSpan expiration)
  {
    var jsonData = JsonSerializer.Serialize(data);
    return cache.SetStringAsync(key, jsonData, 
      new DistributedCacheEntryOptions 
      { 
        AbsoluteExpirationRelativeToNow = expiration 
      });
  }
}