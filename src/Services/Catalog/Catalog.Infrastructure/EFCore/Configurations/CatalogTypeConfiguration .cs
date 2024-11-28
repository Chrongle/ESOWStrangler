using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.EFCore.Configurations;

public class CatalogTypeConfiguration : IEntityTypeConfiguration<CatalogType>
{
  public void Configure(EntityTypeBuilder<CatalogType> builder)
  {
    builder.HasKey(x => x.Id);

    // Seed data
    builder.HasData(
      new CatalogType { Id = 1, TypeName = "Mug" },
      new CatalogType { Id = 2, TypeName = "T-Shirt" },
      new CatalogType { Id = 3, TypeName = "Sheet" },
      new CatalogType { Id = 4, TypeName = "USB Memory Stick" }
    );
  }
}