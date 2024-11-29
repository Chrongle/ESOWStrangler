using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Services;

public class CatalogStranglerFacade : ICatalogViewModelService
{
    private readonly ILogger<CatalogStranglerFacade> logger;
    private readonly CatalogViewModelService legacy;
    private readonly CatalogMSViewModelService microservice;
    private bool useMicroservice = false;

    public CatalogStranglerFacade(
        ILogger<CatalogStranglerFacade> logger,
        CatalogViewModelService legacy,
        CatalogMSViewModelService microservice,
        bool useMicroservice)
    {
        this.logger = logger;
        this.legacy = legacy;
        this.microservice = microservice;
        this.useMicroservice = useMicroservice;
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
        logger.LogInformation("GetCatalogItems called.");

        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.GetCatalogItems(pageIndex, itemsPage, brandId, typeId);
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.GetCatalogItems(pageIndex, itemsPage, brandId, typeId);
        }
    }
    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        logger.LogInformation("GetBrands called.");
        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.GetBrands();
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.GetBrands();
        }
    }
    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        logger.LogInformation("GetTypes called.");
        if (useMicroservice)
        {
            logger.LogInformation("Using microservice.");
            return await microservice.GetTypes();
        }
        else
        {
            logger.LogInformation("Using legacy service.");
            return await legacy.GetTypes();
        }
    }
}
