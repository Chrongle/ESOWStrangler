using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
{
  private readonly CatalogDbContext _dbContext;
  public Repository(CatalogDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<T> AddAsync(T entity)
  {
    await _dbContext.Set<T>().AddAsync(entity);
    await _dbContext.SaveChangesAsync();
    return entity;
  }

  public async Task DeleteAsync(T entity)
  {
    _dbContext.Set<T>().Remove(entity);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _dbContext.Set<T>().ToListAsync();
  }

  public async Task<T> GetByIdAsync(int id)
  {
    return await _dbContext.Set<T>().FindAsync(id);
  }

  public Task<IEnumerable<T>> ListAllAsync()
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
  {
    throw new NotImplementedException();
  }

  public Task UpdateAsync(T entity)
  {
    throw new NotImplementedException();
  }
}