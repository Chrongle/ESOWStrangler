using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class OrderServiceStranglerFacade : IOrderService
{
    private readonly ILogger<OrderServiceStranglerFacade> logger;
    private readonly OrderMSService microservice;
    private readonly OrderService legacy;
    private bool useMicroservice = false;

    public OrderServiceStranglerFacade(
        ILogger<OrderServiceStranglerFacade> logger,
        OrderService legacy,
        OrderMSService microservice,
        bool useMicroservice)
    {
        this.logger = logger;
        this.microservice = microservice;
        this.legacy = legacy;
        this.useMicroservice = useMicroservice;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        logger.LogInformation("CreateOrderAsync called.");

        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            await microservice.CreateOrderAsync(basketId, shippingAddress);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            await legacy.CreateOrderAsync(basketId, shippingAddress);
        }
    }
}
