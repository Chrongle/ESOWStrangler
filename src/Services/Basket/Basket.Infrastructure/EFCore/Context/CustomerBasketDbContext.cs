using Basket.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Basket.Infrastructure.EFCore.Context;
public class CustomerBasketDbContext : DbContext
{
  public CustomerBasketDbContext()
  {
  }
  public CustomerBasketDbContext(DbContextOptions<CustomerBasketDbContext> options) 
    : base(options)
  {
  }

  public DbSet<CustomerBasket> CustomerBaskets { get; set; }
  public DbSet<BasketItem> BasketItems { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<CustomerBasket>(e =>
    {
      e.HasKey(x => x.Id);
      e.Property(x => x.UserName).IsRequired();
      e.HasMany(x => x.Items).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<BasketItem>(e =>
    {
      e.HasKey(x => x.Id);
      e.Property(x => x.CatalogItemId).IsRequired();
      e.Property(x => x.ProductName).IsRequired();
      e.Property(x => x.Price).IsRequired();
      e.Property(x => x.Price).HasColumnType("decimal(18, 2)");
      e.Property(x => x.Quantity).IsRequired();
    });

    // OnModelCreatingPartial(modelBuilder);
  }

  // partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}