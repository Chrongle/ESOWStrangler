using MediatR;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;


namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(BasketViewModelService).Assembly));
        services.AddScoped<CatalogViewModelService>();
        services.AddScoped<BasketViewModelService>();
        services.Configure<CatalogSettings>(configuration);

        //strangler services
        services.AddScoped<IBasketViewModelService, BasketMSViewModelService>();
        services.AddScoped<ICatalogViewModelService, CatalogMSViewModelService>();
        services.AddScoped<ICatalogItemViewModelService, CatalogMSItemViewModelService>();
        services.AddScoped<IUserManagementService, UserManagementService>();

        return services;
    }
}
