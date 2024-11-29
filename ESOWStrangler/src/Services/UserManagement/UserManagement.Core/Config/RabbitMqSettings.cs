namespace UserManagement.Core.Config;
public class RabbitMqSettings
{
  public required string HostName { get; set; }
  public required int Port { get; set; }
  public required string UserName { get; set; }
  public required string Password { get; set; }
  public required string ConsumerQueueName { get; set; }
}