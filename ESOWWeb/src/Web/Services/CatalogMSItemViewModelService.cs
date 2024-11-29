using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Services;

public class CatalogMSItemViewModelService(HttpClient httpClient) : ICatalogItemViewModelService

{

    public async Task UpdateCatalogItem(CatalogItemViewModel viewModel)
    {
        var getResponse = await httpClient.GetAsync($"gateway/catalog/item?{viewModel.Id}");
        getResponse.EnsureSuccessStatusCode();
        var existingCatalogItem = await getResponse.Content.ReadFromJsonAsync<CatalogItem>();

        Guard.Against.Null(existingCatalogItem, nameof(existingCatalogItem));

        CatalogItem.CatalogItemDetails details = new(viewModel.Name, existingCatalogItem.Description, viewModel.Price);
        existingCatalogItem.UpdateDetails(details);

        var content = new StringContent(
            JsonSerializer.Serialize(existingCatalogItem),
            Encoding.UTF8,
            "application/json");

        var updateResponse = await httpClient.PutAsJsonAsync("gateway/catalog/item/update", content);
        updateResponse.EnsureSuccessStatusCode();
    }
}
