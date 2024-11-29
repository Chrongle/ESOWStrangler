using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Pages.Basket;

namespace Microsoft.eShopWeb.Web.Services;

public class BasketStranglerFacade : IBasketViewModelService
{
    private readonly ILogger<BasketStranglerFacade> logger;
    private readonly BasketViewModelService legacy;
    private readonly BasketMSViewModelService microservice;
    private bool useMicroservice = false;

    public BasketStranglerFacade(
        ILogger<BasketStranglerFacade> logger,
        BasketViewModelService legacy,
        BasketMSViewModelService microservice,
        bool useMicroservice)
    {
        this.logger = logger;
        this.legacy = legacy;
        this.microservice = microservice;
        this.useMicroservice = useMicroservice;
    }

    public async Task<int> CountTotalBasketItems(string username)
    {
        logger.LogInformation("CountTotalBasketItems called.");

        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.CountTotalBasketItems(username);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.CountTotalBasketItems(username);
        }
    }

    public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
    {
        logger.LogInformation("GetOrCreateBasketForUser called.");

        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.GetOrCreateBasketForUser(userName);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.GetOrCreateBasketForUser(userName);
        }
    }

    public async Task<BasketViewModel> Map(Basket basket)
    {
        logger.LogInformation("Map called");
        
        if (useMicroservice)
        {
            logger.LogInformation("Using microservice");
            return await microservice.Map(basket);
        }
        else
        {
            logger.LogInformation("Using legacy");
            return await legacy.Map(basket);
        }
    }
}
