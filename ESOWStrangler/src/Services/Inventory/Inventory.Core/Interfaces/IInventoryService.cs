namespace Inventory.Core.Interfaces;

public interface IInventoryService
{
  Task<int>GetInventoryStatusAsync(IList<int> Ids);
  Task<int>ReserveInventoryItemsAsync(IList<int> Ids);
}