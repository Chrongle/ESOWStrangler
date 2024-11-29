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

public class GetCatalogItemEndpointTests(WebApplicationFactory<Program> _factory)
: IClassFixture<WebApplicationFactory<Program>>
{

  [Fact]
  public async Task GetCatalogItem_ReturnsIdSpecificItem_WhenRefererIsGatewayApi()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogItemService>();
    mockCatalogService.GetAsync(id: 1).Returns(new CatalogItem
      {
        Id = 1,
        CatalogTypeId = 1,
        CatalogBrandId = 2,
        Name = "ItemA"
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
    var response = await client.GetAsync("/api/catalog/item?Id=1");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var itemResponse = await response.Content.ReadFromJsonAsync<GetCatalogItemResponse>();
    Assert.NotNull(itemResponse);
    Assert.Equal(1, itemResponse.Id);
    Assert.Equal("ItemA", itemResponse.Name);
  }

  [Fact]
  public async Task GetCatalogItem_ReturnsForbidden_WhenRefererIsNotGatewayApi()
  {
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "NotGatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/item?Id=1");

    // Assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }
}