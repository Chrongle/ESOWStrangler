using Microsoft.EntityFrameworkCore;
using Order.Core.Entities;
using Order.Core.Interfaces;
using Order.Infrastructure.EFCore.Context;

namespace Order.Infrastructure.Repository;

public class Repository : IRepository
{
  private readonly OrderDbContext _dbContext;

  public Repository(OrderDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<CustomerOrder> GetOrderByIdAsync(int id)
  {
    return await _dbContext.CustomerOrders
      .Include(o => o.OrderItems)
      .FirstOrDefaultAsync(o => o.Id == id) ??
      throw new ArgumentNullException("Order not found");
  }

  public async Task<IReadOnlyList<CustomerOrder>> GetOrdersByCustomerIdAsync(int customerId)
  {
    return await _dbContext.CustomerOrders
      .Include(o => o.OrderItems)
      .Include(o => o.ShippingAddress)
      .Where(o => o.CustomerId == customerId)
      .ToListAsync();
  }

  public async Task<CustomerOrder> AddOrderAsync(CustomerOrder order)
  {
    _dbContext.CustomerOrders.Add(order);
    await _dbContext.SaveChangesAsync();
    return order;
  }

  public async Task<CustomerOrder> UpdateOrderAsync(CustomerOrder order)
  {
    _dbContext.CustomerOrders.Update(order);
    await _dbContext.SaveChangesAsync();
    return order;
  }
}