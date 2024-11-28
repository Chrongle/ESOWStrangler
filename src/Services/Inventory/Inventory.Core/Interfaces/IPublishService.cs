namespace Inventory.Core.Interfaces;
public interface IPublisherService
{
  Task Publish<T>(T message, string queueName);
}