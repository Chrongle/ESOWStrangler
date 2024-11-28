using Ardalis.Specification;

namespace Catalog.Core.Interfaces;
public interface IRepository<T> where T : IAggregateRoot
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> ListAsync(ISpecification<T> spec);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
