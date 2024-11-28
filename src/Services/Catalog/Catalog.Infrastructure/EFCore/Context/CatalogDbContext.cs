using Catalog.Core.Entities;
using Catalog.Infrastructure.EFCore.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.EFCore.Context;

public partial class CatalogDbContext : DbContext
{
    public CatalogDbContext()
    {
    }

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatalogBrand> CatalogBrands { get; set; }
    public virtual DbSet<CatalogItem> CatalogItems { get; set; }
    public virtual DbSet<CatalogType> CatalogTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CatalogBrandConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogTypeConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
