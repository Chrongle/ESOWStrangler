using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Core.Services;
public class GetCatalogTypeService(ILogger<GetCatalogTypeService> logger, 
    IRepository<CatalogType> repository,
    ICacheService cache) : IGetCatalogService<CatalogType>
{
    private readonly string cacheKey = "CatalogTypes";
    public async Task<CatalogType> GetAsync(int id)
    {
        logger.LogInformation("Get type from the cache.");
        var types = await cache.GetCachedDataAsync<IEnumerable<CatalogType>>(cacheKey);
        if (types == null)
        {
            logger.LogInformation("Cache is empty. Fetching data from the repository.");
            // TODO: Implement caching for single item?
            types = await repository.GetAllAsync();
            await cache.SetCachedDataAsync(cacheKey, types, TimeSpan.FromMinutes(3));
        }
        return types.FirstOrDefault(x => x.Id == id) ?? throw new ArgumentNullException();
    }

    public async Task<IEnumerable<CatalogType>> GetAllAsync()
    {
        logger.LogInformation("Get all types from the cache.");
        var types = await cache.GetCachedDataAsync<IEnumerable<CatalogType>>(cacheKey);
        if (types == null)
        {
            logger.LogInformation("Cache is empty. Fetching data from the repository.");
            try
            {
                types = await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching data from the repository.");
                throw;
            }
            try
            {
                await cache.SetCachedDataAsync(cacheKey, types, TimeSpan.FromMinutes(3));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while caching data.");
                throw;
            }
        }
        return types;
    }
}
