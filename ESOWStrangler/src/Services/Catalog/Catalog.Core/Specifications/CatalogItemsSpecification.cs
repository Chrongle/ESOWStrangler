using Catalog.Core.Entities;
using Ardalis.Specification;
using System.Linq;

namespace Catalog.Core.Specifications
{
  public class CatalogItemsSpecification : Specification<CatalogItem>
  {
    public CatalogItemsSpecification(params int[] ids)
    {
     // Query.Where(c => ids.Contains(c.Id));
    }
  }
}
