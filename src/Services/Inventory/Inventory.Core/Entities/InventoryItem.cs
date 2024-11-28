using System.ComponentModel.DataAnnotations;

namespace Inventory.Core.Entities;

public class InventoryItem
{
  [Key]
  public int Id { get; set; }
  public int CatalogItemId { get; set; }
  public int StockCount { get; set; }
  public int ReservedCount { get; set; }
  public int ShippedCount { get; set; }

  public int AvailableStock => StockCount - ReservedCount - ShippedCount;
}