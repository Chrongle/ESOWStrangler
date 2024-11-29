using FastEndpoints;
using Order.Core.Entities;
using Order.Core.Interfaces;
namespace Order.Api.Endpoints;

public class CreateOrderEndpoint : Endpoint<CreateOrderRequest, CreateOrderResponse>
{
  private readonly ICustomerOrderService customerOrderService;
  private readonly ILogger<CreateOrderEndpoint> logger;

  public CreateOrderEndpoint(ICustomerOrderService customerOrderService,
    ILogger<CreateOrderEndpoint> logger)
  {
    this.customerOrderService = customerOrderService;
    this.logger = logger;
  }

  public override void Configure()
  {
    Post("/api/orders");
  }

  public override async Task HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var response = await customerOrderService.AddOrderAsync(request.Order);
      if (response == null)
      {
        await SendErrorsAsync();
        return;
      }
      var order = new CreateOrderResponse { Order = response };
      await SendCreatedAtAsync<CreateOrderEndpoint>($"/api/orders/{response.Id}", order);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error creating order");
      await SendErrorsAsync();
    }
  }
}

public class CreateOrderRequest
{
  public required CustomerOrder Order { get; set; }
}

public class CreateOrderResponse
{
  public required CustomerOrder Order { get; set; }
}