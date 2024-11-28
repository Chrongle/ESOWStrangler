using Order.Core.Entities;

namespace Order.Core.Interfaces;

public interface IRepository
{
  Task<CustomerOrder> GetOrderByIdAsync(int id);
  Task<IReadOnlyList<CustomerOrder>> GetOrdersByCustomerIdAsync(int customerId);
  Task<CustomerOrder> AddOrderAsync(CustomerOrder order);
  Task<CustomerOrder> UpdateOrderAsync(CustomerOrder order);
}