using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Pages.Basket;

namespace Microsoft.eShopWeb.Web.Services;

public class BasketMSViewModelService(
    HttpClient httpClient,
    IUriComposer uriComposer,
    ILogger<BasketMSViewModelService> logger) : IBasketViewModelService
{
    public async Task<int> CountTotalBasketItems(string username)
    {
        if (username is null) throw new ArgumentNullException();

        var query = $"gateway/api/basket";   
        logger.LogInformation("Get basket for user: {username}", username);
        var response = await httpClient.GetAsync($"{query}?UserName={username}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return 0;

        var result = await response.Content.ReadFromJsonAsync<CreateBasketResponse>();

        if (result is null) return 0;

        return result.Basket.Items.Count;
    }

    public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
    {
        try
        {
            if (userName is null) throw new ArgumentNullException();

            var query = $"gateway/api/basket";   
            logger.LogInformation("Get basket for user: {userName}", userName);
            var response = await httpClient.GetAsync($"{query}?UserName={userName}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CreateBasketResponse>();

            if (result is not null)
            {
                var viewModel = await StranglerMap(result.Basket);
                return viewModel;
            }
            else
            {
                logger.LogInformation("Creating a new basket");
                return await CreateBasketForUser(userName); 
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting or creating the basket");
            throw;
        }
    }

    public async Task<BasketViewModel> Map(Basket basket)
    {
        return new BasketViewModel()
        {
            BuyerId = basket.BuyerId,
            Id = basket.Id,
            Items = await GetBasketItems(basket.Items)
        };
    }

    public async Task<BasketViewModel> StranglerMap(CustomerBasket basket)
    {
        return new BasketViewModel()
        {
            BuyerId = basket.UserName,
            Id = basket.Id,
            Items = await StranglerGetBasketItems(basket.Items)
        };
    }

    private async Task<BasketViewModel> CreateBasketForUser(string userName)
    {
        var basket = new Basket(userName);
        var response = await httpClient.PostAsJsonAsync("gateway/api/basket", basket);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<Basket>() ??
            throw new ArgumentNullException();
        
        return new BasketViewModel()
        {
            BuyerId = result.BuyerId,
            Id = result.Id,
        };
    }
    
    private async Task<List<BasketItemViewModel>> StranglerGetBasketItems(IReadOnlyCollection<ApplicationCore.GatewayApiDtos.Basket.BasketItem> basketItems)
    {
        var items = new List<BasketItemViewModel>();
        foreach (var basketItem in basketItems)
        {
            var query = $"gateway/api/catalog/item?Id={basketItem.CatalogItemId}";
            var response = await httpClient.GetAsync(query);
            response.EnsureSuccessStatusCode();

            var catalogItem = await response.Content.ReadFromJsonAsync<CatalogItem>() ??
                throw new ArgumentNullException();

            var basketItemViewModel = new BasketItemViewModel
            {
                Id = basketItem.Id,
                UnitPrice = basketItem.Price,
                Quantity = basketItem.Quantity,
                CatalogItemId = basketItem.CatalogItemId,
                PictureUrl = uriComposer.ComposePicUri(catalogItem.PictureUri),
                ProductName = catalogItem.Name
            };
            items.Add(basketItemViewModel);
        }

        return items;
    }

    private async Task<List<BasketItemViewModel>> GetBasketItems(IReadOnlyCollection<ApplicationCore.Entities.BasketAggregate.BasketItem> basketItems)
    {
        var items = new List<BasketItemViewModel>();
        foreach (var basketItem in basketItems)
        {
            var query = $"gateway/api/catalog/item?Id={basketItem.CatalogItemId}";
            var response = await httpClient.GetAsync(query);
            response.EnsureSuccessStatusCode();

            var catalogItem = await response.Content.ReadFromJsonAsync<CatalogItem>() ??
                throw new ArgumentNullException();

            var basketItemViewModel = new BasketItemViewModel
            {
                Id = basketItem.Id,
                UnitPrice = basketItem.UnitPrice,
                Quantity = basketItem.Quantity,
                CatalogItemId = basketItem.CatalogItemId,
                PictureUrl = uriComposer.ComposePicUri(catalogItem.PictureUri),
                ProductName = catalogItem.Name
            };
            items.Add(basketItemViewModel);
        }

        return items;
    }
}
