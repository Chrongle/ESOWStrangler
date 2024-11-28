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
public class GetCatalogTypeServiceTests
{
    private readonly IRepository<CatalogType> _repositoryMock;
    private readonly ICacheService _cacheMock;
    private readonly ILogger<GetCatalogTypeService> _loggerMock;
    private readonly GetCatalogTypeService _service;

    public GetCatalogTypeServiceTests()
    {
        _repositoryMock = Substitute.For<IRepository<CatalogType>>();
        _cacheMock = Substitute.For<ICacheService>();
        _loggerMock = Substitute.For<ILogger<GetCatalogTypeService>>();
        _service = new GetCatalogTypeService(_loggerMock, _repositoryMock, _cacheMock);
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTypes_WhenTypesExistInCache()
    {
        // Arrange
        var types = new List<CatalogType> { new CatalogType { Id = 1 }, new CatalogType { Id = 2 } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogType>>("CatalogTypes")
            .Returns(types);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        await _repositoryMock.DidNotReceive().GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldFetchFromRepository_WhenCacheIsEmpty()
    {
        // Arrange
        var types = new List<CatalogType> { new CatalogType { Id = 1 }, new CatalogType { Id = 2 } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogType>>("CatalogTypes")
            .Returns((IEnumerable<CatalogType>)null);
        _repositoryMock.GetAllAsync().Returns(types);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        await _repositoryMock.Received(1).GetAllAsync();
        await _cacheMock.Received(1).SetCachedDataAsync("CatalogTypes", types, TimeSpan.FromMinutes(3));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnType_WhenTypeExistsInCache()
    {
        // Arrange
        var typeId = 1;
        var types = new List<CatalogType> { new CatalogType { Id = typeId } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogType>>("CatalogTypes")
            .Returns(types);

        // Act
        var result = await _service.GetAsync(typeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeId, result.Id);
    }

    [Fact]
    public async Task GetAsync_ShouldFetchFromRepository_WhenCacheIsEmpty()
    {
        // Arrange
        var typeId = 1;
        var types = new List<CatalogType> { new CatalogType { Id = typeId } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogType>>("CatalogTypes")
            .Returns((IEnumerable<CatalogType>)null);
        _repositoryMock.GetAllAsync().Returns(types);

        // Act
        var result = await _service.GetAsync(typeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeId, result.Id);
        await _repositoryMock.Received(1).GetAllAsync();
        await _cacheMock.Received(1).SetCachedDataAsync("CatalogTypes", types, TimeSpan.FromMinutes(3));
    }
}