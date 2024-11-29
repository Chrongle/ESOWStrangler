using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiResponseDtos.Catalog;

public class CatalogItemsResponseDto
{
    public IEnumerable<CatalogItemDto>? Items { get; set; }
}

public class CatalogItemDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? PictureUri { get; set; }

    public int? CatalogTypeId { get; set; }

    public int? CatalogBrandId { get; set; }
}
