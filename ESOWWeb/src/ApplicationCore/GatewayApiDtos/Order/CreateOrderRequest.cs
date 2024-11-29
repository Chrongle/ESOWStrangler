using System;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
namespace Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Order;

public class CreateOrderRequest
{
    public string BuyerId { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public Address ShipToAddress { get; set; }
}
