namespace Inventory.Core.Interfaces;
public interface IRepository<T> where T : class
{
  Task<T> GetByIdAsync(int id);
  Task<IEnumerable<T>> ListAsync();
  Task<T> AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(T entity);
}