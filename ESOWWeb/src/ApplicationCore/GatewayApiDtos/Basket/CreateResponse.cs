using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;

public class CreateBasketResponse
{
    public required CustomerBasket Basket { get; set; }
}

public class CustomerBasket
{
  public int Id { get; set; }
  public required string UserName { get; set; }
  public required List<BasketItem> Items { get; set; }
}
