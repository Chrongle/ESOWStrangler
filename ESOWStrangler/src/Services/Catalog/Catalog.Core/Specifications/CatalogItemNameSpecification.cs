using Ardalis.Specification;
using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Specifications
{
  public class CatalogItemNameSpecification : Specification<CatalogItem>
  {
    public CatalogItemNameSpecification(string catalogItemName)
    {
      Query.Where(item => catalogItemName == item.Name);
    }
  }
}
