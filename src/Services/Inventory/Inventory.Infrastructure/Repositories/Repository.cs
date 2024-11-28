using Inventory.Core.Interfaces;
using Inventory.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
  private readonly InventoryDbContext _dbContext;

  public Repository(InventoryDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<T> GetByIdAsync(int id)
  {
    return await _dbContext.Set<T>().FindAsync(id) ?? 
      throw new KeyNotFoundException();
  }

  public async Task<IEnumerable<T>> ListAsync()
  {
    return await _dbContext.Set<T>().ToListAsync();
  }

  public async Task<T> AddAsync(T entity)
  {
    _dbContext.Set<T>().Add(entity);
    await _dbContext.SaveChangesAsync();
    return entity;
  }

  public async Task UpdateAsync(T entity)
  {
    _dbContext.Entry(entity).State = EntityState.Modified;
    await _dbContext.SaveChangesAsync();
  }

  public async Task DeleteAsync(T entity)
  {
    _dbContext.Set<T>().Remove(entity);
    await _dbContext.SaveChangesAsync();
  }
}