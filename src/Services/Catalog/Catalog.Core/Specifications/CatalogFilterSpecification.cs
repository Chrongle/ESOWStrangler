using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
  public class CatalogFilterSpecification : Specification<CatalogItem>
  {
    public CatalogFilterSpecification(int? brandId, int? typeId)
    {
      Query.Where(i => (!brandId.HasValue || i.CatalogBrandId == brandId) &&
                       (!typeId.HasValue || i.CatalogTypeId == typeId));
    }
  }
}