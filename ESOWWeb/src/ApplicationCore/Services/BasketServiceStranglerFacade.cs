using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class BasketServiceStranglerFacade : IBasketService
{
    private readonly ILogger<BasketServiceStranglerFacade> logger;
    private readonly BasketMSService microservice;
    private readonly BasketService legacy;
    private bool useMicroservice = false;

    public BasketServiceStranglerFacade(
        ILogger<BasketServiceStranglerFacade> logger,
        BasketService legacy,
        BasketMSService microservice,
        bool useMicroservice)
    {
        this.logger = logger;
        this.microservice = microservice;
        this.legacy = legacy;
        this.useMicroservice = useMicroservice;
    }

    public async Task<Basket> AddItemToBasket(string username, int catalogItemId, decimal price, int quantity = 1)
    {
        logger.LogInformation("AddItemToBasket called.");

        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.AddItemToBasket(username, catalogItemId, price, quantity);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.AddItemToBasket(username, catalogItemId, price, quantity);
        }
    }

    public async Task DeleteBasketAsync(int basketId)
    {
        logger.LogInformation("DeleteBasketAsync called.");

        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            await microservice.DeleteBasketAsync(basketId);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            await legacy.DeleteBasketAsync(basketId);
        }
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
        logger.LogInformation("SetQuantities called.");
     
        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.SetQuantities(basketId, quantities);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.SetQuantities(basketId, quantities);
        }
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
        logger.LogInformation("TransforBasketAsync called");

        if (useMicroservice)
        {
            logger.LogInformation("using microservice");
            await microservice.TransferBasketAsync(anonymousId, userName);
        }
        else
        {
            logger.LogInformation("using legacy service");
            await legacy.TransferBasketAsync(anonymousId, userName);
        }
    }
}
