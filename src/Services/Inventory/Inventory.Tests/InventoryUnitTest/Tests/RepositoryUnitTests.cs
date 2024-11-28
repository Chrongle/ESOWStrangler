using Inventory.Core.Entities;
using Inventory.Infrastructure.EFCore.Context;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class RepositoryUnitTests
{
  private readonly InventoryDbContext _dbContext;
  private readonly Repository<InventoryItem> _repository;

  public RepositoryUnitTests()
  {
    var options = new DbContextOptionsBuilder<InventoryDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;

    _dbContext = new InventoryDbContext(options);
    _repository = new Repository<InventoryItem>(_dbContext);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
  {
    // Arrange
    var testEntity = new InventoryItem { Id = 154 };
    _dbContext.Set<InventoryItem>().Add(testEntity);
    await _dbContext.SaveChangesAsync();

    // Act
    var result = await _repository.GetByIdAsync(154);

    // Assert
    Assert.Equal(testEntity, result);
  }

  [Fact]
  public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
  {
    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(1584390));
  }

  [Fact]
  public async Task ListAsync_ShouldReturnAllEntities()
  {
    // Arrange
    var testEntities = new List<InventoryItem>
        {
            new InventoryItem { Id = 1 },
            new InventoryItem { Id = 2 }
        };

    _dbContext.Set<InventoryItem>().AddRange(testEntities);
    await _dbContext.SaveChangesAsync();

    // Act
    var result = await _repository.ListAsync();

    // Assert
    Assert.Equal(testEntities.Count, result.Count());
    Assert.Equal(testEntities.OrderBy(e => e.Id), result.OrderBy(e => e.Id));
  }
}