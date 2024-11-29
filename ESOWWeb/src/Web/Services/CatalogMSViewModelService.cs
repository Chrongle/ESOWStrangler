using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiResponseDtos.Catalog;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Services;

public class CatalogMSViewModelService(
    HttpClient httpClient,
    IUriComposer uriComposer,
    ILogger<CatalogMSViewModelService> logger) : ICatalogViewModelService
{
    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
        // var filterSpecification = new CatalogFilterSpecification(brandId, typeId);
        // var filterPaginatedSpecification =
        //     new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId);

        // var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
        // var totalItems = await _itemRepository.CountAsync(filterSpecification);

        var query = "gateway/api/catalog/items";
        if (brandId is not null) query = ($"{query}?brandId={brandId}");
        if (brandId is not null && typeId is not null) query = ($"{query}&typeId={typeId}");
        if (brandId is null && typeId is not null) query = ($"{query}?typeId={typeId}");

        var response = await httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CatalogItemsResponseDto>();
        var items = result?.Items;
        if (items is null) throw new ArgumentNullException();

        var itemsOnPage = items.Skip(itemsPage * pageIndex).Take(itemsPage);
        var totalItems = items.Count();

        var vm = new CatalogIndexViewModel()
        {
            CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel()
            {
                Id = i.Id,
                Name = i.Name,
                PictureUri = uriComposer.ComposePicUri(i.PictureUri ?? ""),
                Price = i.Price ?? 0M
            }).ToList(),
            Brands = (await GetBrands()).ToList(),
            Types = (await GetTypes()).ToList(),
            BrandFilterApplied = brandId ?? 0,
            TypesFilterApplied = typeId ?? 0,
            PaginationInfo = new PaginationInfoViewModel()
            {
                ActualPage = pageIndex,
                ItemsPerPage = itemsOnPage.Count(),
                TotalItems = totalItems,
                TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
            }
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

        return vm;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var response = await httpClient.GetAsync("gateway/api/catalog/brands");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CatalogBrandsResponseDto>() ?? throw new ArgumentNullException();
        if (result.Brands is null) throw new ArgumentNullException();

        var items = result.Brands
            .Select(brand => new SelectListItem() { Value = brand.Id.ToString(), Text = brand.BrandName })
            .OrderBy(b => b.Text)
            .ToList();

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
        items.Insert(0, allItem);

        return items;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var response = await httpClient.GetAsync("gateway/api/catalog/types");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CatalogTypesResponseDto>() ?? throw new ArgumentNullException();
        if (result.Types is null) throw new ArgumentNullException();

        var items = result.Types
            .Select(type => new SelectListItem() { Value = type.Id.ToString(), Text = type.TypeName })
            .OrderBy(b => b.Text)
            .ToList();

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
        items.Insert(0, allItem);

        return items;
    }
}
