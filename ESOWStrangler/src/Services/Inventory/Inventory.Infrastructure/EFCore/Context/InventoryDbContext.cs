using Inventory.Core.Entities;
using Inventory.Infrastructure.EFCore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.EFCore.Context;

public partial class InventoryDbContext : DbContext
{
  public InventoryDbContext()
  {
  }

  public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
  {
  }

  public virtual DbSet<InventoryItem> InventoryItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new IventoryConfiguration());

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}