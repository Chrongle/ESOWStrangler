using Inventory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.EFCore.Configurations;

public class IventoryConfiguration : IEntityTypeConfiguration<InventoryItem>
{
  public void Configure(EntityTypeBuilder<InventoryItem> builder)
  {
    builder.HasKey(x => x.Id);

    // Seed data
    builder.HasData(
      new InventoryItem { Id = 1, CatalogItemId = 1, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 2, CatalogItemId = 2, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 3, CatalogItemId = 3, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 4, CatalogItemId = 4, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 5, CatalogItemId = 5, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 6, CatalogItemId = 6, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 7, CatalogItemId = 7, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 8, CatalogItemId = 8, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 9, CatalogItemId = 9, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 10, CatalogItemId = 10, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 11, CatalogItemId = 11, StockCount = 10, ReservedCount = 0, ShippedCount = 0 },
      new InventoryItem { Id = 12, CatalogItemId = 12, StockCount = 10, ReservedCount = 0, ShippedCount = 0 }
    );
  }
}