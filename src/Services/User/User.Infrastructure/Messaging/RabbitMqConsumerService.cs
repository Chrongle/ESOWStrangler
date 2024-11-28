using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using User.Core.Config;
using User.Core.Dtos;
using User.Core.Interfaces;
using User.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace User.Infrastructure.Messaging;
public class RabbitMqConsumerService : IRabbitMqConsumerService,
  IHostedService,
  IDisposable
{
  private readonly RabbitMqSettings settings;
  private readonly ILogger<RabbitMqConsumerService> logger;
  private IConnection connection;
  private IModel channel;
  private ICreateUserService createUserService;

  public RabbitMqConsumerService(IOptions<RabbitMqSettings> options,
    ICreateUserService createUserService,
    ILogger<RabbitMqConsumerService> logger)
  {
    settings = options.Value;
    this.createUserService = createUserService;
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
    channel.QueueDeclare(queue: settings.QueueName,
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

    channel.BasicConsume(queue: settings.QueueName,
      autoAck: true,
      consumer: consumer);
  }

  private async Task ProcessMessage(string message,
  CancellationToken cancellationToken)
  {
    logger.LogInformation($"Processing message: {message}");

    var userRequest = JsonSerializer.Deserialize<CreateUserRequest>(message);
    if (userRequest != null)
    {
      await createUserService.CreateUserAsync(userRequest, cancellationToken);
    }
    else
    {
      logger.LogError("Invalid message received");
    }
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
