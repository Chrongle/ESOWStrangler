using System.Net;
using System.Net.Http.Json;
using Catalog.Api.Endpoints;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Catalog.IntegrationTests.Api.Endpoints;

public class GetCatalogItemsEndpointTests(WebApplicationFactory<Program> _factory)
: IClassFixture<WebApplicationFactory<Program>>
{

  [Fact]
  public async Task GetCatalogItems_ReturnsAllItems_WhenRefererIsGatewayApi_AndBrandIdAndTypeIdAre0()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogItemService>();
    mockCatalogService.GetAllAsync(brandId: 0, typeId: 0).Returns(new List<CatalogItem>
      {
        new CatalogItem { Id = 1, CatalogTypeId = 1, CatalogBrandId = 2, Name = "ItemA" },
        new CatalogItem { Id = 2, CatalogTypeId = 2, CatalogBrandId = 1, Name = "ItemB" }
      });

    var factory = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.RemoveAll<IGetCatalogItemService>();
        services.AddSingleton(mockCatalogService);
      });
    });

    var client = factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    // var response = await client.GetAsync("/api/catalog/items");
    var response = await client.GetAsync("/api/catalog/items?brandId=0&typeId=0");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var itemsResponse = await response.Content.ReadFromJsonAsync<GetCatalogItemsResponse>();
    Assert.NotNull(itemsResponse);
    Assert.NotEmpty(itemsResponse.Items!);
    Assert.Contains(itemsResponse.Items!, item => item.Name == "ItemA");
    Assert.Contains(itemsResponse.Items!, item => item.Name == "ItemB");
  }

  [Fact]
  public async Task GetCatalogItems_ReturnsBrandSpecificItems_WhenRefererIsGatewayApi_AndBrandIdIsNot0()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogItemService>();
    mockCatalogService.GetAllAsync(brandId: 2, typeId: 0).Returns(new List<CatalogItem>
      {
        new CatalogItem { Id = 1, CatalogTypeId = 1, CatalogBrandId = 2, Name = "ItemA" },
      });

    var factory = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.RemoveAll<IGetCatalogItemService>();
        services.AddSingleton(mockCatalogService);
      });
    });

    var client = factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/items?brandId=2&typeId=0");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var itemsResponse = await response.Content.ReadFromJsonAsync<GetCatalogItemsResponse>();
    Assert.NotNull(itemsResponse);
    Assert.NotEmpty(itemsResponse.Items!);
    Assert.Contains(itemsResponse.Items!, item => item.Name == "ItemA");
  }
  [Fact]
  public async Task GetCatalogItems_ReturnTypeSpecificItems_WhenRefererIsGatewayApi_AndTypeIdIsNot0()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogItemService>();
    mockCatalogService.GetAllAsync(brandId: 0, typeId: 2).Returns(new List<CatalogItem>
      {
        new CatalogItem { Id = 2, CatalogTypeId = 2, CatalogBrandId = 1, Name = "ItemB" }
      });

    var factory = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.RemoveAll<IGetCatalogItemService>();
        services.AddSingleton(mockCatalogService);
      });
    });

    var client = factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/items?brandId=0&typeId=2");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var itemsResponse = await response.Content.ReadFromJsonAsync<GetCatalogItemsResponse>();
    Assert.NotNull(itemsResponse);
    Assert.NotEmpty(itemsResponse.Items!);
    Assert.Contains(itemsResponse.Items!, item => item.Name == "ItemB");
  }

  [Fact]
  public async Task GetCatalogItems_ReturnsForbidden_WhenRefererIsNotGatewayApi()
  {
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "NotGatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/items");

    // Assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact]
  public async Task GetCatalogItems_ReturnsEmptyList_WhenNoItemsExistAndRefereIsGatewayApi()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogItemService>();
    mockCatalogService.GetAllAsync(brandId: 0, typeId: 0).Returns(new List<CatalogItem>()); // Empty list

    var factory = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.RemoveAll<IGetCatalogItemService>();
        services.AddSingleton(mockCatalogService);
      });
    });

    var client = factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/items");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var itemsResponse = await response.Content.ReadFromJsonAsync<GetCatalogItemsResponse>();
    Assert.NotNull(itemsResponse);
    Assert.Empty(itemsResponse!.Items!);
  }
}
