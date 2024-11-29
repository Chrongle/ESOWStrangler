using Basket.Core.Entities;
using Basket.Core.Interfaces;

namespace Basket.Core.Services;

public class BasketService(IRepository<CustomerBasket> _repository) : IBasketService
{
  public async Task<CustomerBasket> CreateBasketAsync(CustomerBasket basket)
  {
    return await _repository.AddAsync(basket);
  }
  public async Task DeleteBasketByCustomerIdAsync(string customerId)
  {
    var basket = await _repository.GetByCustomerIdAsync(customerId) ??
      throw new ArgumentNullException();

    await _repository.DeleteAsync(basket);
  }

    public async Task DeleteBasketAsync(int basketId)
    {
      var entity = await _repository.GetByIdAsync(basketId) ??
        throw new ArgumentNullException();
      await _repository.DeleteAsync(entity);
    }

    public async Task<CustomerBasket> GetBasketByCustomerIdAsync(string customerId)
  {
    return await _repository.GetByCustomerIdAsync(customerId) ??
      throw new ArgumentNullException();
  }

    public async Task<CustomerBasket> GetBasketByIdAsync(int basketId)
    {
      return await _repository.GetByIdAsync(basketId) ??
        throw new ArgumentNullException();
    }

    public async Task UpdateBasketAsync(CustomerBasket basket)
  {
    await _repository.UpdateAsync(basket);
  }
}
