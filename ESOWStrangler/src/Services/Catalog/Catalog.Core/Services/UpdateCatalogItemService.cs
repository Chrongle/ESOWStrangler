using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Core.Services;

public class UpdateCatalogItemService(ILogger<UpdateCatalogItemService> logger, 
    IRepository<CatalogItem> repository) : IUpdateCatalogService
{
    public async Task UpdateItemAsync(CatalogItem item)
    {
        logger.LogInformation("Update item in the repository.");
        try
        {
            await repository.UpdateAsync(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating the item in the repository.");
            throw;
        }
    }
}
