namespace User.Core.Interfaces;

public interface IRabbitMqConsumerService
{
  void StartConsuming(CancellationToken cancellationToken);
}