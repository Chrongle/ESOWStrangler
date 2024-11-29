using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Services;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.Logging;

namespace CatalogUnitTest.UnitTests;
public class GetCatalogItemServiceTests
{
  private readonly IRepository<CatalogItem> _repositoryMock;
  private readonly ICacheService _cacheMock;
  private readonly ILogger<GetCatalogItemService> _loggerMock;
  private readonly GetCatalogItemService _service;

  public GetCatalogItemServiceTests()
  {
    _repositoryMock = Substitute.For<IRepository<CatalogItem>>();
    _cacheMock = Substitute.For<ICacheService>();
    _loggerMock = Substitute.For<ILogger<GetCatalogItemService>>();
    _service = new GetCatalogItemService(_loggerMock, _repositoryMock, _cacheMock);
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnAllItems_WhenItemssExistInCache()
  {
    // Arrange
    var items = new List<CatalogItem> { new CatalogItem { Id = 1 }, new CatalogItem { Id = 2 } };
    _cacheMock.GetCachedDataAsync<IEnumerable<CatalogItem>>("CatalogItems")
        .Returns(items);

    // Act
    var result = await _service.GetAllAsync(null, null);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    await _repositoryMock.DidNotReceive().GetAllAsync();
  }

  [Fact]
  public async Task GetAllAsync_ShouldFetchFromRepository_WhenCacheIsEmpty()
  {
    // Arrange
    var items = new List<CatalogItem> { new CatalogItem { Id = 1 }, new CatalogItem { Id = 2 } };
    _cacheMock.GetCachedDataAsync<IEnumerable<CatalogItem>>("CatalogItems")
        .Returns((IEnumerable<CatalogItem>)null);
    _repositoryMock.GetAllAsync().Returns(items);

    // Act
    var result = await _service.GetAllAsync(null, null);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    await _repositoryMock.Received(1).GetAllAsync();
    await _cacheMock.Received(1).SetCachedDataAsync("CatalogItems", items, TimeSpan.FromMinutes(3));
  }

  [Fact]
  public async Task GetAsync_ShouldReturnItem_WhenItemExistsInCache()
  {
    // Arrange
    var itemId = 1;
    var items = new List<CatalogItem> { new CatalogItem { Id = itemId } };
    _cacheMock.GetCachedDataAsync<IEnumerable<CatalogItem>>("CatalogItems")
        .Returns(items);

    // Act
    var result = await _service.GetAsync(itemId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(itemId, result.Id);
  }

  [Fact]
  public async Task GetAsync_ShouldFetchFromRepository_WhenCacheIsEmpty()
  {
    // Arrange
    var itemId = 1;
    var items = new List<CatalogItem> { new CatalogItem { Id = itemId } };
    _cacheMock.GetCachedDataAsync<IEnumerable<CatalogItem>>("CatalogItems")
        .Returns((IEnumerable<CatalogItem>)null);
    _repositoryMock.GetAllAsync().Returns(items);

    // Act
    var result = await _service.GetAsync(itemId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(itemId, result.Id);
    await _repositoryMock.Received(1).GetAllAsync();
    await _cacheMock.Received(1).SetCachedDataAsync("CatalogItems", items, TimeSpan.FromMinutes(3));
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnFilteredItems_WhenItemsExistInCache()
  {
    // Arrange
    var items = new List<CatalogItem>
    {
        new CatalogItem { Id = 1, CatalogBrandId = 1, CatalogTypeId = 1 },
        new CatalogItem { Id = 2, CatalogBrandId = 2, CatalogTypeId = 2 }
    };
    _cacheMock.GetCachedDataAsync<IEnumerable<CatalogItem>>("CatalogItems")
        .Returns(items);

    // Act
    var result = await _service.GetAllAsync(1, 1);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Equal(1, result.First().Id);
  }

  [Fact]
  public async Task GetAllAsync_ShouldreturnFilteredItemsFromRepository_WhenCacheIsEmpty()
  {
    // Arrange
    var items = new List<CatalogItem>
    {
        new CatalogItem { Id = 1, CatalogBrandId = 1, CatalogTypeId = 1 },
        new CatalogItem { Id = 2, CatalogBrandId = 2, CatalogTypeId = 2 }
    };
    _cacheMock.GetCachedDataAsync<IEnumerable<CatalogItem>>("CatalogItems")
        .Returns((IEnumerable<CatalogItem>)null);
    _repositoryMock.GetAllAsync().Returns(items);

    // Act
    var result = await _service.GetAllAsync(1, 1);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Equal(1, result.First().Id);
    await _repositoryMock.Received(1).GetAllAsync();
    await _cacheMock.Received(1).SetCachedDataAsync("CatalogItems", items, TimeSpan.FromMinutes(3));
  }
}