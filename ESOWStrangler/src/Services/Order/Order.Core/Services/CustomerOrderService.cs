using Microsoft.Extensions.Logging;
using Order.Core.Entities;
using Order.Core.Interfaces;

namespace Order.Core.Services;

public class CustomerOrderService(
  IRepository repository,
  IPublisherService publisher,
  ILogger<CustomerOrder> logger)  : ICustomerOrderService
{
  public async Task<CustomerOrder> AddOrderAsync(CustomerOrder order)
  {
    try 
    {
      logger.LogInformation("Adding order");
      var result = await repository.AddOrderAsync(order);
      logger.LogInformation("Order added");

      // Publish order to RabbitMQ
      await publisher.Publish(result, "order_created_queue");
      
      return result;
    } 
    catch (Exception ex) 
    {
      logger.LogError(ex, "Error adding order");
      throw;
    }
  }

  public async Task<CustomerOrder> GetOrderByIdAsync(int id)
  {
    try 
    {
      logger.LogInformation("Getting order");
      var result = await repository.GetOrderByIdAsync(id);
      logger.LogInformation("Order retrieved");
      return result;
    } 
    catch (Exception ex) 
    {
      logger.LogError(ex, "Error getting order");
      throw;
    }
  }

  public async Task<IEnumerable<CustomerOrder>> GetOrdersByCustomerIdAsync(int customerId)
  {
    try 
    {
      logger.LogInformation("Getting orders");
      var result = await repository.GetOrdersByCustomerIdAsync(customerId);
      logger.LogInformation("Orders retrieved");
      return result;
    } 
    catch (Exception ex) 
    {
      logger.LogError(ex, "Error getting orders");
      throw;
    }
  }

  public async Task<CustomerOrder> UpdateOrderAsync(CustomerOrder order)
  {
    try 
    {
      logger.LogInformation("Updating order");
      var result = await repository.UpdateOrderAsync(order);
      logger.LogInformation("Order updated");
      return result;
    } 
    catch (Exception ex) 
    {
      logger.LogError(ex, "Error updating order");
      throw;
    }
  }
}
