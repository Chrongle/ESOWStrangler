using System.ComponentModel.DataAnnotations;

namespace Basket.Core.Entities;

public class BasketItem
{
  [Key]
  public int Id { get; set; }
  public required int CatalogItemId { get; set; }
  public required string ProductName { get; set; }
  public required decimal Price { get; set; }
  public required int Quantity { get; set; }
}