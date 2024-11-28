using Basket.Core.Entities;
using Basket.Core.Interfaces;
using FastEndpoints;

namespace Basket.Api.Endpoints;

public class GetBasketByIdEndpoint(IBasketService getBasketService,
 ILogger<GetBasketByIdEndpoint> logger)
: Endpoint<GetBasketByIdRequest, GetBasketResponse>
{
    public override void Configure()
    {
        Get("/api/basket/id/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBasketByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await getBasketService.GetBasketByIdAsync(request.Id);

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
              await SendErrorsAsync();
            }
        }
    }
}

public class GetBasketByIdRequest
{
  public int Id { set; get; }
}

