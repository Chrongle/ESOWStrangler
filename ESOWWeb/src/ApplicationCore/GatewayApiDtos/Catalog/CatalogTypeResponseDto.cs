using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiResponseDtos.Catalog;

public class CatalogTypesResponseDto
{
    public IEnumerable<CatalogTypeDto>? Types { get; set; }
}

public class CatalogTypeDto
{
    public int Id { get; set; }

    public string? TypeName { get; set; }
}
