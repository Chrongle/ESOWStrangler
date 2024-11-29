using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.EFCore.Configurations;

public class CatalogBrandConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
  public void Configure(EntityTypeBuilder<CatalogBrand> builder)
  {
    builder.HasKey(x => x.Id);

    // Seed data
    builder.HasData(
      new CatalogBrand { Id = 1, BrandName = "Azure" },
      new CatalogBrand { Id = 2, BrandName = ".NET" },
      new CatalogBrand { Id = 3, BrandName = "Visual Studio" },
      new CatalogBrand { Id = 4, BrandName = "SQL Server" },
      new CatalogBrand { Id = 5, BrandName = "Other" }
    );
  }
}