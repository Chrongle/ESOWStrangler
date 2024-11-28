using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces;
public interface IGetCatalogItemService
{
    Task<CatalogItem> GetAsync(int id);
    Task<IEnumerable<CatalogItem>> GetAllAsync(int? brandId, int? typeId);
}
