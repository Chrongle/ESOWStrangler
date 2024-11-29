using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Core.Services;
public class GetCatalogBrandService(ILogger<GetCatalogBrandService> logger, 
    IRepository<CatalogBrand> repository,
    ICacheService cache) : IGetCatalogService<CatalogBrand>
{
    private readonly string cacheKey = "CatalogBrands";
    public async Task<CatalogBrand> GetAsync(int id)
    {
        logger.LogInformation("Get brand from the cache.");
        var brands = await cache.GetCachedDataAsync<IEnumerable<CatalogBrand>>(cacheKey);
        if (brands == null)
        {
            logger.LogInformation("Cache is empty. Fetching data from the repository.");
            // TODO: Implement caching for single item?
            try
            {
                brands = await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching data from the repository.");
                throw;
            }
            try
            {
                await cache.SetCachedDataAsync(cacheKey, brands, TimeSpan.FromMinutes(3));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while caching data.");
                throw;
            }
        }
        return brands.FirstOrDefault(x => x.Id == id) ?? throw new ArgumentNullException();
    }

    public async Task<IEnumerable<CatalogBrand>> GetAllAsync()
    {
        logger.LogInformation("Get all brands from the cache.");
        var brands = await cache.GetCachedDataAsync<IEnumerable<CatalogBrand>>(cacheKey);
        if (brands == null)
        {
            logger.LogInformation("Cache is empty. Fetching data from the repository.");
            try
            {
                brands = await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching data from the repository.");
                throw;
            }
            try
            {
                await cache.SetCachedDataAsync(cacheKey, brands, TimeSpan.FromMinutes(3));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while caching data.");
                throw;
            }
        }
        return brands;
    }
}
