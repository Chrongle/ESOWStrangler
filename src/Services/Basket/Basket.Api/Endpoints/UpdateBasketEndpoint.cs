using Basket.Core.Entities;
using Basket.Core.Interfaces;
using FastEndpoints;

namespace Basket.Api.Endpoints;

public class UpdateBasketEndpoint(IBasketService getBasketService,
 ILogger<UpdateBasketEndpoint> logger)
: Endpoint<UpdateBasketRequest, UpdateBasketResponse>
{
    public override void Configure()
    {
        Put("/api/basket/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBasketRequest request, CancellationToken cancellationToken)
    {
      logger.LogInformation($"Updating Basket with Id: {request.Id} for user with username: {request.UserName}");
        try
        {
            var model = new CustomerBasket
            {
                Id = request.Id,
                UserName = request.UserName,
                Items = request.Items
            };
            
            await getBasketService.UpdateBasketAsync(model);
            await SendOkAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating basket");
            await SendErrorsAsync();
        }
    }
}

public class UpdateBasketRequest
{
  public required int Id { get; set; }
  public required string UserName { get; set; }
  public required List<BasketItem> Items { get; set; }
}

public class UpdateBasketResponse
{
}
