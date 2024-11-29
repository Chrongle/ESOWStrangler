using Basket.Core.Entities;
using Basket.Core.Interfaces;
using FastEndpoints;

namespace Basket.Api.Endpoints;

public class GetBasketEndpoint(IBasketService getBasketService,
 ILogger<GetBasketEndpoint> logger)
: Endpoint<GetBasketRequest, GetBasketResponse>
{
    public override void Configure()
    {
        Get("/api/basket/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBasketRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await getBasketService.GetBasketByCustomerIdAsync(request.UserName);

            if (result is null) 
            {
              await SendNotFoundAsync(cancellationToken);
              return;
            }

            var response = new GetBasketResponse { Basket = result };
            await SendAsync(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting basket");

            if (!HttpContext.ResponseStarted())
            {
              await SendNotFoundAsync();
            }
        }
    }
}

public class GetBasketRequest
{
    public string UserName { get; set; } = string.Empty;
}

public class GetBasketResponse
{
    public CustomerBasket? Basket { get; set; }
}
