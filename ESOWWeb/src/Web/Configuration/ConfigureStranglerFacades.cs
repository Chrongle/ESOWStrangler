using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureStranglerFacades
{
    public static IServiceCollection AddStranglerFacades(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        // strangle facades
        services.AddScoped<ICatalogViewModelService, CatalogStranglerFacade>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<CatalogStranglerFacade>>();
            var legacy = sp.GetRequiredService<CatalogViewModelService>();
            var microservice = sp.GetRequiredService<CatalogMSViewModelService>();
            var useMicroservice = builder.Configuration.GetValue<bool>("UseCatalogMicroservice");
            return new CatalogStranglerFacade(logger, legacy, microservice, useMicroservice);
        });

        services.AddScoped<IBasketViewModelService, BasketStranglerFacade>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<BasketStranglerFacade>>();
            var legacy = sp.GetRequiredService<BasketViewModelService>();
            var microservice = sp.GetRequiredService<BasketMSViewModelService>();
            var useMicroservice = builder.Configuration.GetValue<bool>("UseBasketMicroservice");
            return new BasketStranglerFacade(logger, legacy, microservice, useMicroservice);
        });

        services.AddScoped<IBasketService, BasketServiceStranglerFacade>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<BasketServiceStranglerFacade>>();
            var legacy = sp.GetRequiredService<BasketService>();
            var microservice = sp.GetRequiredService<BasketMSService>();
            var useMicroservice = builder.Configuration.GetValue<bool>("UseBasketMicroservice");
            return new BasketServiceStranglerFacade(logger, legacy: legacy, microservice: microservice, useMicroservice);
        });

        services.AddScoped<IOrderService, OrderServiceStranglerFacade>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<OrderServiceStranglerFacade>>();
            var legacy = sp.GetRequiredService<OrderService>();
            var microservice = sp.GetRequiredService<OrderMSService>();
            var useMicroservice = builder.Configuration.GetValue<bool>("UseOrderMicroservice");
            return new OrderServiceStranglerFacade(logger, legacy: legacy, microservice: microservice, useMicroservice);
        });

        return services;
    }
}
