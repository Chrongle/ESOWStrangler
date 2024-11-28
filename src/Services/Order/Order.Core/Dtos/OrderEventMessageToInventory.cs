using Order.Core.Entities;

namespace Order.Core.Dtos;

public class OrderEventMessageToInventory
{
  public required string OrderNumber { get; set; }
  public required DateTime OrderDate { get; set; }
  public required List<OrderItem> OrderItems { get; set; }
}