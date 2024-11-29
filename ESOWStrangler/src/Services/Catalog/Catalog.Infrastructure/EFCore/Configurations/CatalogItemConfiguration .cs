using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.EFCore.Configurations;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
  public void Configure(EntityTypeBuilder<CatalogItem> builder)
  {
    builder.HasKey(x => x.Id);
    builder.Property(e => e.Price).HasColumnType("decimal(18, 2)");

    // Seed data
    builder.HasData(
      new CatalogItem { Id = 1, CatalogTypeId =  2, CatalogBrandId =  2, Description = ".NET Bot Black Sweatshirt", Name = ".NET Bot Black Sweatshirt", Price = 19.5m, PictureUri = "http://catalogbaseurltobereplaced/images/products/1.png" },
      new CatalogItem { Id = 2, CatalogTypeId =  1, CatalogBrandId =  2, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price = 8.50m, PictureUri = "http://catalogbaseurltobereplaced/images/products/2.png" },
      new CatalogItem { Id = 3, CatalogTypeId =  2, CatalogBrandId =  5, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/3.png" },
      new CatalogItem { Id = 4, CatalogTypeId =  2, CatalogBrandId =  2, Description = ".NET Foundation Sweatshirt", Name = ".NET Foundation Sweatshirt", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/4.png" },
      new CatalogItem { Id = 5, CatalogTypeId =  3, CatalogBrandId =  5, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5m, PictureUri = "http://catalogbaseurltobereplaced/images/products/5.png" },
      new CatalogItem { Id = 6, CatalogTypeId =  2, CatalogBrandId =  2, Description = ".NET Blue Sweatshirt", Name = ".NET Blue Sweatshirt", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/6.png" },
      new CatalogItem { Id = 7, CatalogTypeId =  2, CatalogBrandId =  5, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/7.png" },
      new CatalogItem { Id = 8, CatalogTypeId =  1, CatalogBrandId =  5, Description = "Kudu Purple Sweatshirt", Name = "Kudu Purple Sweatshirt", Price = 8.5m, PictureUri = "http://catalogbaseurltobereplaced/images/products/8.png" },
      new CatalogItem { Id = 9, CatalogTypeId =  3, CatalogBrandId =  5, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/9.png" },
      new CatalogItem { Id = 10, CatalogTypeId =  3, CatalogBrandId =  2, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/10.png" },
      new CatalogItem { Id = 11, CatalogTypeId =  3, CatalogBrandId =  2, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5m, PictureUri = "http://catalogbaseurltobereplaced/images/products/11.png" },
      new CatalogItem { Id = 12, CatalogTypeId =  2, CatalogBrandId =  5, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, PictureUri = "http://catalogbaseurltobereplaced/images/products/12.png" }
    );
  }

  // private List<CatalogItem> GetCatalogItemsFromJson(string filePath)
  // {
  //   var json = File.ReadAllText(filePath);
  //   return JsonSerializer.Deserialize<List<CatalogItem>>(json);
  // }
}