namespace Catalog.Core.Interfaces;
public interface ICacheService
{
  public Task<T> GetCachedDataAsync<T>(string key);
  public Task SetCachedDataAsync<T>(string key, T data, TimeSpan expiration);
}