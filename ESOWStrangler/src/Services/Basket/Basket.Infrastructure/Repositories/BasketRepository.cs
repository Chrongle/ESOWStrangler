using Basket.Core.Entities;
using Basket.Core.Interfaces;
using Basket.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IRepository<CustomerBasket>
{
  private readonly CustomerBasketDbContext _context;

  public BasketRepository(CustomerBasketDbContext context)
  {
    _context = context;
  }

  public async Task<CustomerBasket> AddAsync(CustomerBasket entity)
  {
    _context.CustomerBaskets.Add(entity);
    await _context.SaveChangesAsync();
    return entity;
  }

  public async Task DeleteAsync(CustomerBasket entity)
  {
    _context.CustomerBaskets.Remove(entity);
    await _context.SaveChangesAsync();
  }

  public async Task<CustomerBasket?> GetByCustomerIdAsync(string customerId)
  {
    return await _context.CustomerBaskets
      .Include(x => x.Items)
      .FirstOrDefaultAsync(x => x.UserName == customerId);
  }

    public async Task<CustomerBasket?> GetByIdAsync(int basketId)
    {
      return await _context.CustomerBaskets
        .Include(x => x.Items)
        .FirstOrDefaultAsync(x => x.Id == basketId);
    }

    public async Task UpdateAsync(CustomerBasket entity)
  {
    var existingBasket = await _context.CustomerBaskets
      .Include(x => x.Items)
      .FirstOrDefaultAsync(x => x.Id == entity.Id) ??
        throw new KeyNotFoundException($"Basket for user {entity.UserName} not found.");

    existingBasket.Items = entity.Items;
    existingBasket.UserName = entity.UserName;

    await _context.SaveChangesAsync();
  }

  // public Task UpdateAsync(CustomerBasket entity)
  // {
  //   //TODO: Fix this weird code for updating basketitems
  //   // _context.BasketItems.AddRange(entities: entity.Items);
  //   _context.Update(entity);
  //   return _context.SaveChangesAsync();
  // }

  // public async Task UpdateAsync(CustomerBasket entity)
  // {
  //     // Get the existing basket from the database
  //     var existingBasket = await _context.CustomerBaskets
  //         .Include(b => b.Items)
  //         .FirstOrDefaultAsync(b => b.Id == entity.Id);
  //
  //     if (existingBasket == null)
  //     {
  //         throw new KeyNotFoundException($"Basket for user {entity.UserName} not found.");
  //     }
  //
  //     _context.Update(entity);
  //
  //     // Update the basket items
  //     foreach (var existingItem in existingBasket.Items.ToList())
  //     {
  //         var updatedItem = entity.Items.FirstOrDefault(i => i.Id == existingItem.Id);
  //         if (updatedItem != null)
  //         {
  //             // Update existing items
  //             _context.Update(updatedItem);
  //         }
  //         else
  //         {
  //             // Remove item that is no longer in the basket
  //             _context.BasketItems.Remove(existingItem);
  //         }
  //     }
  //
  //     // Add new items
  //     foreach (var newItem in entity.Items)
  //     {
  //         if (existingBasket.Items.All(i => i.Id != newItem.Id))
  //         {
  //             existingBasket.Items.Add(newItem);
  //         }
  //     }
  //
  //     await _context.SaveChangesAsync();
  // }
}
