using Microsoft.EntityFrameworkCore;
using Order.Core.Entities;

namespace Order.Infrastructure.EFCore.Context;
public partial class OrderDbContext : DbContext
{
  public OrderDbContext()
  {
  }
  
  public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
  {
  }

  public DbSet<CustomerOrder> CustomerOrders { get; set; }
  public DbSet<OrderItem> OrderItems { get; set; }
  public DbSet<ShippingAddress> ShippingAddresses { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<CustomerOrder>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.OrderNumber).IsRequired();
      entity.Property(e => e.CustomerId).IsRequired();
      entity.Property(e => e.OrderDate).IsRequired();
      entity.Property(e => e.OrderStatus).IsRequired();
    });

    modelBuilder.Entity<OrderItem>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.CatalogId).IsRequired();
      entity.Property(e => e.CatalogName).IsRequired();
      entity.Property(e => e.UnitPrice).IsRequired();
      entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
      entity.Property(e => e.Units).IsRequired();
      entity.Property(e => e.PictureUrl).IsRequired();
    });

    modelBuilder.Entity<ShippingAddress>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.FirstName).IsRequired();
      entity.Property(e => e.LastName).IsRequired();
      entity.Property(e => e.Address).IsRequired();
      entity.Property(e => e.ZipCode).IsRequired();
      entity.Property(e => e.City).IsRequired();
      entity.Property(e => e.Country).IsRequired();
      entity.Property(e => e.Mobile).IsRequired();
      entity.Property(e => e.Email).IsRequired();
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
