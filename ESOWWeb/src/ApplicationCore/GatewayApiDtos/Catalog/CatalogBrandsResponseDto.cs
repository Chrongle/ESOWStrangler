using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiResponseDtos.Catalog;

public class CatalogBrandsResponseDto
{
    public IEnumerable<CatalogBrandDto>? Brands { get; set; }
}

public class CatalogBrandDto
{
    public int Id { get; set; }

    public string BrandName { get; set; }
}
