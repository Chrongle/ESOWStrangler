using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Order.Core.Config;
using Order.Core.Dtos;
using Order.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Order.Infrastructure.Messaging;
public class RabbitMqConsumerService : IConsumerService,
  IHostedService,
  IDisposable
{
  private readonly RabbitMqSettings settings;
  private readonly ILogger<RabbitMqConsumerService> logger;
  private IConnection? connection;
  private IChannel? channel;

  public RabbitMqConsumerService(IOptions<RabbitMqSettings> options,
    ILogger<RabbitMqConsumerService> logger)
  {
    settings = options.Value;
    this.logger = logger;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    await InitializeRabbitMQListener();
    StartConsuming(cancellationToken);
  }

  private async Task InitializeRabbitMQListener()
  {
    var factory = new ConnectionFactory
    {
      HostName = settings.HostName,
      Port = settings.Port,
      UserName = settings.UserName,
      Password = settings.Password
    };

    connection = await factory.CreateConnectionAsync();
    channel = await connection.CreateChannelAsync();
    await channel.QueueDeclareAsync(queue: settings.ConsumerQueueName,
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null);
  }

  public async void StartConsuming(CancellationToken cancellationToken)
  {
    logger.LogInformation("RabbitMq consumer started");
    if (channel == null)
    {
      logger.LogError("RabbitMq channel is not initialized");
      return;
    }
    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.ReceivedAsync += async (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);

      logger.LogInformation($"Received message: {message}");

      await ProcessMessage(message, cancellationToken);
    };

    await channel.BasicConsumeAsync(queue: settings.ConsumerQueueName,
      autoAck: true,
      consumer: consumer);
  }

  private async Task ProcessMessage(string message,
  CancellationToken cancellationToken)
  {
    logger.LogInformation($"Processing message: {message}");
  }

  public async Task StopAsync(CancellationToken cancellationToken)
  {
    logger.LogInformation("RabbitMq consumer stopping");
    if (channel == null || connection == null)
    {
      logger.LogError("RabbitMq channel or connection is not initialized");
      return;
    }
    await channel.CloseAsync();
    await connection.CloseAsync();
    logger.LogInformation("RabbitMq consumer stopped");
  }

  public void Dispose()
  {
    channel?.Dispose();
    connection?.Dispose();
    logger.LogInformation("RabbitMq consumer disposed");
  }
}
