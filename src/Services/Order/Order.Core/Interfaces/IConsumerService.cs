namespace Order.Core.Interfaces;
public interface IConsumerService
{
  void StartConsuming(CancellationToken cancellationToken);
}