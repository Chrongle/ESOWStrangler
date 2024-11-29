using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserManagement.Core.Config;
using UserManagement.Core.Dtos;
using UserManagement.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace UserManagement.Infrastructure.Messaging;
public class RabbitMqConsumerService : IConsumerService,
  IHostedService,
  IDisposable
{
  private readonly RabbitMqSettings settings;
  private readonly ILogger<RabbitMqConsumerService> logger;
  private IConnection connection;
  private IModel channel;

  public RabbitMqConsumerService(IOptions<RabbitMqSettings> options,
    ILogger<RabbitMqConsumerService> logger)
  {
    settings = options.Value;
    this.logger = logger;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    InitializeRabbitMQListener();
    StartConsuming(cancellationToken);
  }

  private void InitializeRabbitMQListener()
  {
    var factory = new ConnectionFactory
    {
      HostName = settings.HostName,
      Port = settings.Port,
      UserName = settings.UserName,
      Password = settings.Password
    };

    connection = factory.CreateConnection();
    channel = connection.CreateModel();
    channel.QueueDeclare(queue: settings.ConsumerQueueName,
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null);
  }

  public void StartConsuming(CancellationToken cancellationToken)
  {
    logger.LogInformation("RabbitMq consumer started");
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += async (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);

      logger.LogInformation($"Received message: {message}");

      await ProcessMessage(message, cancellationToken);
    };

    channel.BasicConsume(queue: settings.ConsumerQueueName,
      autoAck: true,
      consumer: consumer);
  }

  private async Task ProcessMessage(string message,
  CancellationToken cancellationToken)
  {
    logger.LogInformation($"Processing message: {message}");
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    channel?.Close();
    connection?.Close();
    logger.LogInformation("RabbitMq consumer stopped");
    return Task.CompletedTask;
  }

  public void Dispose()
  {
    channel?.Dispose();
    connection?.Dispose();
    logger.LogInformation("RabbitMq consumer disposed");
  }
}
