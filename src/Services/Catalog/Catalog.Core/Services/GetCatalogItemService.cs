using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Core.Services;
public class GetCatalogItemService(ILogger<GetCatalogItemService> logger,
    IRepository<CatalogItem> repository,
    ICacheService cache) : IGetCatalogItemService
{
    private readonly string cacheKey = "CatalogItems";
    public async Task<CatalogItem> GetAsync(int id)
    {
        logger.LogInformation("Get item from the cache.");
        var catalogItems = await cache.GetCachedDataAsync<IEnumerable<CatalogItem>>(cacheKey);
        if (catalogItems == null)
        {
            logger.LogInformation("Cache is empty. Fetching data from the repository.");
            // TODO: Implement caching for single item?
            catalogItems = await repository.GetAllAsync();
            await cache.SetCachedDataAsync(cacheKey, catalogItems, TimeSpan.FromMinutes(3));
        }
        return catalogItems.FirstOrDefault(x => x.Id == id) ?? throw new ArgumentNullException();
    }

    public async Task<IEnumerable<CatalogItem>> GetAllAsync(int? brandId, int? typeId)
    {
        logger.LogInformation("Get all items from the cache.");
        var catalogItems = await cache.GetCachedDataAsync<IEnumerable<CatalogItem>>(cacheKey);
        if (catalogItems == null)
        {
            logger.LogInformation("Cache is empty. Fetching data from the repository.");
            try
            {
                catalogItems = await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching data from the repository.");
                throw;
            }
            try
            {
                await cache.SetCachedDataAsync(cacheKey, catalogItems, TimeSpan.FromMinutes(3));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while caching data.");
                throw;
            }
        }
        return catalogItems.Where(i =>
            (!brandId.HasValue || i.CatalogBrandId == brandId) &&
            (!typeId.HasValue || i.CatalogTypeId == typeId));
    }
}
