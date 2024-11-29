namespace Basket.Core.Interfaces;
public interface IRepository<T> where T : class
{
  Task<T?> GetByCustomerIdAsync(string customerId);
  Task<T?> GetByIdAsync(int basketId);
  Task<T> AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(T entity);
}
