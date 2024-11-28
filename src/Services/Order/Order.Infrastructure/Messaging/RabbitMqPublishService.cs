using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Order.Core.Config;
using Order.Core.Interfaces;

namespace Order.Infrastructure.Messaging;

public class RabbitMqPublishService : IPublisherService, IDisposable
{
  private readonly IConnection connection;
  private readonly IChannel channel;

  public RabbitMqPublishService(IOptions<RabbitMqSettings> rabbitMqSettings)
  {
    var factory = new ConnectionFactory
    {
      HostName = rabbitMqSettings.Value.HostName,
      Port = rabbitMqSettings.Value.Port,
      UserName = rabbitMqSettings.Value.UserName,
      Password = rabbitMqSettings.Value.Password
    };

    connection = factory.CreateConnectionAsync().Result;
    channel = connection.CreateChannelAsync().Result;
  }

  public async Task Publish<T>(T message, string queueName)
  {
    await channel.QueueDeclareAsync(queue: queueName,
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null);
    
    var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

    await channel.BasicPublishAsync(exchange: string.Empty,
      routingKey: queueName,
      body: body);
  }

  public void Dispose()
  {
    channel?.CloseAsync();
    connection?.CloseAsync();
    channel?.Dispose();
    connection?.Dispose();
  }
}