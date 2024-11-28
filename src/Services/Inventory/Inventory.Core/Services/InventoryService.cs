using Inventory.Core.Entities;
using Inventory.Core.Interfaces;

namespace Inventory.Core.Services;

public class InventoryService(
  IRepository<InventoryItem> repository,
  IPublisherService publisherService) : IInventoryService
{
  public async Task<int> GetInventoryStatusAsync(IList<int> Ids)
  {
    foreach (var id in Ids)
    {
      var item = await repository.GetByIdAsync(id);
      if (item == null)
      {
        return -1;
      }
      if (item.AvailableStock <= 0)
      {
        return 0;
      }
    }
    return 1;
  }

  public async Task<int> ReserveInventoryItemsAsync(IList<int> Ids)
  {
    foreach (var id in Ids)
    {
      var item = await repository.GetByIdAsync(id);
      if (item == null)
      {
        return -1;
      }
      if (item.AvailableStock <= 0)
      {
        return 0;
      }
      item.ReservedCount++;
      await repository.UpdateAsync(item);
    }
    return 1;
  }

  private async Task PublishInventoryStatusAsync(InventoryItem item)
  {
    await publisherService.Publish(item, "order_status_queue");
  }
}