using Basket.Core.Entities;
using Basket.Core.Interfaces;
using FastEndpoints;

namespace Basket.Api.Endpoints;

public class DeleteBasketEndpoint(IBasketService basketService,
 ILogger<DeleteBasketEndpoint> logger)
: Endpoint<DeleteBasketRequest, DeleteBasketResponse>
{
    public override void Configure()
    {
        Delete("/api/basket/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteBasketRequest request, CancellationToken cancellationToken)
    {
      logger.LogInformation($"Deleting basket for user: {request.UserName}");

        try
        {   
            var basketToDelete = await basketService.GetBasketByCustomerIdAsync(request.UserName);
            logger.LogInformation($"Found basket to delete with Id: {basketToDelete.Id}");
            await basketService.DeleteBasketAsync(basketToDelete.Id);
            logger.LogInformation($"Delete basket with Id: {basketToDelete.Id}");
            await SendOkAsync();
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting basket");
            await SendNotFoundAsync();
        }
    }
}

public class DeleteBasketRequest
{
  public required string UserName { get; set; }
}

public class DeleteBasketResponse
{
}
