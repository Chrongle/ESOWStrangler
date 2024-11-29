using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces;

public interface IUpdateCatalogService
{
    Task UpdateItemAsync(CatalogItem item);
}
