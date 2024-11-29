namespace Order.Core.Entities;

public class OrderItem
{
  public required int Id { get; set; }
  public required int CatalogId { get; set; }
  public required string CatalogName { get; set; }
  public required decimal UnitPrice { get; set; }
  public required int Units { get; set; }
  public required string PictureUrl { get; set; }
}