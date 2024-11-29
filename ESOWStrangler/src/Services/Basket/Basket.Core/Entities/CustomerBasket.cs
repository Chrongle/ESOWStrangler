using System.ComponentModel.DataAnnotations;

namespace Basket.Core.Entities;

public class CustomerBasket
{
  [Key]
  public int Id { get; set; }
  public required string UserName { get; set; }
  public required List<BasketItem> Items { get; set; }
}