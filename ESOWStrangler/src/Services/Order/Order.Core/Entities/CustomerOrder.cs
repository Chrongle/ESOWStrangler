using System.ComponentModel.DataAnnotations;

namespace Order.Core.Entities;

public class CustomerOrder
{
  [Key]
  public int Id { get; set; }
  public required string OrderNumber { get; set; }
  public required int CustomerId { get; set; }
  public required DateTime OrderDate { get; set; }
  public required string OrderStatus { get; set; }
  public required List<OrderItem> OrderItems { get; set; }
  public required ShippingAddress ShippingAddress { get; set; }
}