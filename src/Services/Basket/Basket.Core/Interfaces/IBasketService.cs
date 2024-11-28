using Basket.Core.Entities;

namespace Basket.Core.Interfaces;
public interface IBasketService
{
  Task<CustomerBasket> CreateBasketAsync(CustomerBasket basket);
  Task<CustomerBasket> GetBasketByCustomerIdAsync(string customerId);
  Task<CustomerBasket> GetBasketByIdAsync(int basketId);
  Task UpdateBasketAsync(CustomerBasket dto);
  Task DeleteBasketAsync(int basketId);
}
