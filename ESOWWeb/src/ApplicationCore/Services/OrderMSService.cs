using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Order;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiResponseDtos.Catalog;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiResponseDtos.User;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class OrderMSService(
    HttpClient httpClient,
    IUriComposer uriComposer,
    ILogger<OrderMSService> logger)  : IOrderService
{
    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        var basket = await GetBasketByIdAsync(basketId);    
        Guard.Against.Null(basket, nameof(basket));
        Guard.Against.EmptyBasketOnCheckout(basket.Items);

        var catalogItemList = new List<CatalogItemDto>();

        foreach (var item in basket.Items)
        {
            var catalogItem = await GetCatalogItem(item.CatalogItemId);
            catalogItemList.Add(catalogItem);
        }

        var items = basket.Items.Select(basketItem =>
        {
            var catalogItem = catalogItemList.First(c => c.Id == basketItem.CatalogItemId);
            var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, uriComposer.ComposePicUri(catalogItem.PictureUri));
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
            return orderItem;
        }).ToList();

        var order = new Order(basket.BuyerId, shippingAddress, items);
        await CreateOrderAsync(order);

    }

    private async Task<Basket> GetBasketByIdAsync(int basketId)
    {
        logger.LogInformation($"Getting basket with id: {basketId}");
        var query = $"gateway/api/basket/id?id={basketId}";
        var response = await httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();
        
        var responseDto = await response.Content.ReadFromJsonAsync<CreateBasketResponse>() ??
            throw new ArgumentNullException();

        var basket = new Basket(responseDto.Basket.UserName);
        basket.Id = responseDto.Basket.Id;

        foreach (var item in responseDto.Basket.Items)
        {
            basket.AddItem(item.CatalogItemId, item.Price, item.Quantity);
        }

        return basket;
    }

    private async Task<CatalogItemDto> GetCatalogItem(int catalogItemId)
    {
        var query = $"gateway/api/catalog/item?id={catalogItemId}";
        var response = await httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var catalogItem = await response.Content.ReadFromJsonAsync<CatalogItemDto>() ??
            throw new ArgumentNullException();

        return catalogItem;
    }

    private async Task CreateOrderAsync(Order order)
    {
        var request = new CreateOrderRequest
        {
            BuyerId = order.BuyerId,
            OrderDate = order.OrderDate,
            ShipToAddress = order.ShipToAddress
        };

        var query = $"gateway/api/order";
        var response = await httpClient.PostAsJsonAsync(query, request);
    }
}
