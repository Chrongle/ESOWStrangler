using System.Net;
using System.Net.Http.Json;
using Catalog.Api.Endpoints;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.Api.Endpoints;

public class GetCatalogBrandsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public GetCatalogBrandsEndpointTests(WebApplicationFactory<Program> factory)
  {
    _factory = factory.WithWebHostBuilder(builder =>
    {
      var mockCatalogService = Substitute.For<IGetCatalogService<CatalogBrand>>();
      mockCatalogService.GetAllAsync().Returns(new List<CatalogBrand>
      {
        new CatalogBrand { Id = 1, BrandName = "BrandA" },
        new CatalogBrand { Id = 2, BrandName = "BrandB" }
      });

      builder.ConfigureServices(services =>
      {
        services.AddSingleton(mockCatalogService);
      });
    });
    
  }

  [Fact]
  public async Task GetCatalogBrands_ReturnsBrands_WhenRefererIsGatewayApi()
  {
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/brands");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var brandsResponse = await response.Content.ReadFromJsonAsync<GetCatalogBrandsResponse>();
    Assert.NotNull(brandsResponse);
    Assert.NotEmpty(brandsResponse!.Brands!);
    Assert.Contains(brandsResponse.Brands!, brand => brand.BrandName == "BrandA");
    Assert.Contains(brandsResponse.Brands!, brand => brand.BrandName == "BrandB");
  }

  [Fact]
  public async Task GetCatalogBrands_ReturnsForbidden_WhenRefererIsNotGatewayApi()
  {
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "NotGatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/brands");

    // Assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact]
  public async Task GetCatalogBrands_ReturnsEmptyList_WhenNoBrandsExistAndRefereIsGatewayApi()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogService<CatalogBrand>>();
    mockCatalogService.GetAllAsync().Returns(new List<CatalogBrand>()); // Empty list

    var factory = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.AddSingleton(mockCatalogService);
      });
    });

    var client = factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/brands");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var brandsResponse = await response.Content.ReadFromJsonAsync<GetCatalogBrandsResponse>();
    Assert.NotNull(brandsResponse);
    Assert.Empty(brandsResponse!.Brands!);
  }
}
