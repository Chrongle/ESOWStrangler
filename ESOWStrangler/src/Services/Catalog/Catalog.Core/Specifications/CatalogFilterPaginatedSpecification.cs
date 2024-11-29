using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
  public class CatalogFilterPaginatedSpecification : Specification<CatalogItem>
  {
    public CatalogFilterPaginatedSpecification(int skip, int take, int? brandId, int? typeId)
      : base()
    {
      if (take == 0)
      {
        take = int.MaxValue;
      }
      Query
        .Where(i => (!brandId.HasValue || i.CatalogBrandId == brandId) &&
                    (!typeId.HasValue || i.CatalogTypeId == typeId))
        .Skip(skip).Take(take);
    }
  }
}
