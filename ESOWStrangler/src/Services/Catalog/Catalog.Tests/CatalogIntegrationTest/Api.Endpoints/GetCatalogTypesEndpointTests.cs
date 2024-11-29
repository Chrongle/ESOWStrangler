using System.Net;
using System.Net.Http.Json;
using Catalog.Api.Endpoints;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.Api.Endpoints;

public class GetCatalogTypesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public GetCatalogTypesEndpointTests(WebApplicationFactory<Program> factory)
  {
    _factory = factory.WithWebHostBuilder(builder =>
    {
      var mockCatalogService = Substitute.For<IGetCatalogService<CatalogType>>();
      mockCatalogService.GetAllAsync().Returns(new List<CatalogType>
      {
        new CatalogType { Id = 1, TypeName = "TypeA" },
        new CatalogType { Id = 2, TypeName = "TypeB" }
      });

      builder.ConfigureServices(services =>
      {
        services.AddSingleton(mockCatalogService);
      });
    });
    
  }

  [Fact]
  public async Task GetCatalogTypes_ReturnsTypes_WhenRefererIsGatewayApi()
  {
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "GatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/types");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var typesResponse = await response.Content.ReadFromJsonAsync<GetCatalogTypesResponse>();
    Assert.NotNull(typesResponse);
    Assert.NotEmpty(typesResponse!.Types!);
    Assert.Contains(typesResponse.Types!, type => type.TypeName == "TypeA");
    Assert.Contains(typesResponse.Types!, type => type.TypeName == "TypeB");
  }

  [Fact]
  public async Task GetCatalogTypes_ReturnsForbidden_WhenRefererIsNotGatewayApi()
  {
    // Arrange
    var client = _factory.CreateClient();
    client.DefaultRequestHeaders.Add("Referer", "NotGatewayApi");

    // Act
    var response = await client.GetAsync("/api/catalog/types");

    // Assert
    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact]
  public async Task GetCatalogTypes_ReturnsEmptyList_WhenNoTypesExistAndRefereIsGatewayApi()
  {
    // Arrange
    var mockCatalogService = Substitute.For<IGetCatalogService<CatalogType>>();
    mockCatalogService.GetAllAsync().Returns(new List<CatalogType>()); // Empty list

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
    var response = await client.GetAsync("/api/catalog/types");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var typesResponse = await response.Content.ReadFromJsonAsync<GetCatalogTypesResponse>();
    Assert.NotNull(typesResponse);
    Assert.Empty(typesResponse!.Types!);
  }
}
