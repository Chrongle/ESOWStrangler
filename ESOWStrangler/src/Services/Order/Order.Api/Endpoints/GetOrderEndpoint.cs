using FastEndpoints;
using Order.Core.Entities;
using Order.Core.Interfaces;
namespace Order.Api.Endpoints;
public class GetOrdersEndpoint(ICustomerOrderService customerOrderService,
  ILogger<GetOrdersEndpoint> logger) 
  : Endpoint<GetOrdersRequest, GetOrdersResponse>
{
  public override void Configure()
  {
    Get("/api/orders");
  }

  public override async Task HandleAsync(GetOrdersRequest request, CancellationToken cancellationToken)
  {
    try
    {
      var response = await customerOrderService.GetOrdersByCustomerIdAsync(request.CustomerId);
      if (response == null)
      {
        await SendNotFoundAsync();
        return;
      }
      var orders = new GetOrdersResponse { Orders = response };
      await SendAsync(orders);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error getting orders");
      await SendErrorsAsync();
    }
  }
}

public class GetOrdersRequest
{
  public int CustomerId { get; set; }
}

public class GetOrdersResponse
{
  public required IEnumerable<CustomerOrder> Orders { get; set; }
}
