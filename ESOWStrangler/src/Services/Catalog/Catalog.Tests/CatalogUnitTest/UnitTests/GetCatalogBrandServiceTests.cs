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
public class GetCatalogBrandServiceTests
{
    private readonly IRepository<CatalogBrand> _repositoryMock;
    private readonly ICacheService _cacheMock;
    private readonly ILogger<GetCatalogBrandService> _loggerMock;
    private readonly GetCatalogBrandService _service;

    public GetCatalogBrandServiceTests()
    {
        _repositoryMock = Substitute.For<IRepository<CatalogBrand>>();
        _cacheMock = Substitute.For<ICacheService>();
        _loggerMock = Substitute.For<ILogger<GetCatalogBrandService>>();
        _service = new GetCatalogBrandService(_loggerMock, _repositoryMock, _cacheMock);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnBrand_WhenBrandExistsInCache()
    {
        // Arrange
        var brandId = 1;
        var brands = new List<CatalogBrand> { new CatalogBrand { Id = brandId } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogBrand>>("CatalogBrands")
            .Returns(brands);

        // Act
        var result = await _service.GetAsync(brandId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brandId, result.Id);
    }

    [Fact]
    public async Task GetAsync_ShouldFetchFromRepository_WhenCacheIsEmpty()
    {
        // Arrange
        var brandId = 1;
        var brands = new List<CatalogBrand> { new CatalogBrand { Id = brandId } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogBrand>>("CatalogBrands")
            .Returns((IEnumerable<CatalogBrand>)null);
        _repositoryMock.GetAllAsync().Returns(brands);

        // Act
        var result = await _service.GetAsync(brandId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brandId, result.Id);
        await _repositoryMock.Received(1).GetAllAsync();
        await _cacheMock.Received(1).SetCachedDataAsync("CatalogBrands", brands, TimeSpan.FromMinutes(3));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBrands_WhenCacheIsEmpty()
    {
        // Arrange
        var brands = new List<CatalogBrand> { new CatalogBrand { Id = 1 }, new CatalogBrand { Id = 2 } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogBrand>>("CatalogBrands")
            .Returns((IEnumerable<CatalogBrand>)null);
        _repositoryMock.GetAllAsync().Returns(brands);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        await _repositoryMock.Received(1).GetAllAsync();
        await _cacheMock.Received(1).SetCachedDataAsync("CatalogBrands", brands, TimeSpan.FromMinutes(3));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBrands_WhenBrandsExistInCache()
    {
        // Arrange
        var brands = new List<CatalogBrand> { new CatalogBrand { Id = 1 }, new CatalogBrand { Id = 2 } };
        _cacheMock.GetCachedDataAsync<IEnumerable<CatalogBrand>>("CatalogBrands")
            .Returns(brands);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        await _repositoryMock.DidNotReceive().GetAllAsync();
    }
}