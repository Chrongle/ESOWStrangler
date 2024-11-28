using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserManagement.Core.Config;
using UserManagement.Core.Interfaces;

namespace UserManagement.Infrastructure.Messaging;

public class RabbitMqPublishService : IPublisherService, IDisposable
{
  private readonly IConnection connection;
  private readonly IModel channel;

  public RabbitMqPublishService(IOptions<RabbitMqSettings> rabbitMqSettings)
  {
    var factory = new ConnectionFactory
    {
      HostName = rabbitMqSettings.Value.HostName,
      Port = rabbitMqSettings.Value.Port,
      UserName = rabbitMqSettings.Value.UserName,
      Password = rabbitMqSettings.Value.Password
    };

    connection = factory.CreateConnection();
    channel = connection.CreateModel();
  }

  public Task Publish<T>(T message, string queueName)
  {
    channel.QueueDeclare(queue: queueName,
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null);
    
    var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

    channel.BasicPublish(exchange: "",
      routingKey: queueName,
      basicProperties: null,
      body: body);
    
    return Task.CompletedTask;
  }
  public void Dispose()
  {
    channel?.Close();
    connection?.Close();
    channel?.Dispose();
    connection?.Dispose();
  }
}