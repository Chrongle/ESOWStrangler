using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;
public class CreateBasketRequest
{
    public string UserName { get; set; } = string.Empty;
    public required List<BasketItem> Items { get; set; }
}

public class BasketItem
{
    public int Id { get; set; }
    public required int CatalogItemId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
}
