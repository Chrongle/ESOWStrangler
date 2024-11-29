using Order.Core.Entities;

namespace Order.Core.Interfaces;
public interface ICustomerOrderService
{
  Task<CustomerOrder> GetOrderByIdAsync(int id);
  Task<IEnumerable<CustomerOrder>> GetOrdersByCustomerIdAsync(int customerId);
  Task<CustomerOrder> AddOrderAsync(CustomerOrder order);
  Task<CustomerOrder> UpdateOrderAsync(CustomerOrder order);
}