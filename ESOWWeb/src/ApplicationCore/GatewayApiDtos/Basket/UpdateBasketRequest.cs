
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;

namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;

public class UpdateBasketRequest
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public required List<BasketItem> Items { get; set; }
}
