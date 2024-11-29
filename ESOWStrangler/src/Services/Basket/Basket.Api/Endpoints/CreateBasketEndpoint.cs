using Basket.Core.Entities;
using Basket.Core.Interfaces;
using FastEndpoints;

namespace Basket.Api.Endpoints;

public class CreateBasketEndpoint(IBasketService getBasketService,
 ILogger<CreateBasketEndpoint> logger)
: Endpoint<CreateBasketRequest, CreateBasketResponse>
{
    public override void Configure()
    {
        Post("/api/basket/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBasketRequest request, CancellationToken cancellationToken)
    {
        try
        {   
            var model = new CustomerBasket
            {
                UserName = request.UserName,
                Items = request.Items
            };
            
            await getBasketService.CreateBasketAsync(model);

            var response = new CreateBasketResponse { Basket = model };

            await SendOkAsync(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating basket");
            await SendErrorsAsync();
        }
    }
}

public class CreateBasketRequest
{
  public required string UserName { get; set; }
  public required List<BasketItem> Items { get; set; }
}

public class CreateBasketResponse
{
    public required CustomerBasket Basket { get; set; }
}
