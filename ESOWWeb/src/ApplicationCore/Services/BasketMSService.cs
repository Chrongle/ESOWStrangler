using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.GatewayApiDtos.Basket;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class BasketMSService(
    HttpClient httpClient,
    ILogger<BasketMSService> logger) : IBasketService
{
    public async Task<Basket> AddItemToBasket(string username, int catalogItemId, decimal price, int quantity = 1)
    {
        try
        {
            logger.LogInformation("Adding item to basket...");
            if (username is null) throw new ArgumentNullException();

            var basket = new Basket(username);

            var query = $"gateway/api/basket";   
            logger.LogInformation("Get basket for user: {username}", username);
            var getResponse = await httpClient.GetAsync($"{query}?UserName={username}");

            if (getResponse.StatusCode == HttpStatusCode.NotFound)
            {
                getResponse.Dispose();

                logger.LogInformation("Existing Basket for user not found. Creating Basket...");
                basket = new Basket(username);

                var createBasketRequest = new CreateBasketRequest
                {
                    UserName = username,
                    Items = new List<GatewayApiDtos.Basket.BasketItem>()
                };

                var createResponse = await httpClient.PostAsJsonAsync("gateway/api/basket", createBasketRequest);
                logger.LogInformation("Created Basket. Adding items...");
            }
            else
            {
                var responseDto = await getResponse.Content.ReadFromJsonAsync<CreateBasketResponse>() ?? 
                    throw new ArgumentNullException("Response dto null");
                getResponse.Dispose();

                logger.LogInformation("Existing Basket for user found. Adding items... ");

                basket.Id = responseDto.Basket.Id;

                foreach (var item in responseDto.Basket.Items)
                {
                    basket.AddItem(item.CatalogItemId, item.Price, item.Quantity);
                }
            }

            basket.AddItem(catalogItemId, price, quantity);

            var updateBasketRequest = new UpdateBasketRequest
            {
                Id = basket.Id,
                UserName = basket.BuyerId,
                Items = new List<GatewayApiDtos.Basket.BasketItem>()
            };

            foreach (var item in basket.Items)
            {
                var requestItem = new GatewayApiDtos.Basket.BasketItem
                {
                    Id = item.Id,
                    CatalogItemId = item.CatalogItemId,
                    ProductName = "",
                    Price = item.UnitPrice,
                    Quantity = item.Quantity
                };

                updateBasketRequest.Items.Add(requestItem);
            }

            logger.LogInformation("Updating Basket for user with added items...");
            var updateResponse = await httpClient.PutAsJsonAsync(query, updateBasketRequest);
            updateResponse.EnsureSuccessStatusCode();
            updateResponse.Dispose();

            logger.LogInformation("Items added to Basket");

            return basket;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding item to basket");
            throw;   
        }
    }

    public async Task DeleteBasketAsync(int basketId)
    {
        var query = $"gateway/api/basket";
        var response = await httpClient.DeleteAsync($"{query}?Id={basketId}");
        response.EnsureSuccessStatusCode();
        response.Dispose();
    }

    public async Task<Result<Basket>> SetQuantities(int basketId, Dictionary<string, int> quantities)
    {
        var query = $"gateway/api/basket/id";
        var getResponse = await httpClient.GetAsync($"{query}/id?Id={basketId}");
        getResponse.EnsureSuccessStatusCode();

        var responseDto = await getResponse.Content.ReadFromJsonAsync<CreateBasketResponse>();
        getResponse.Dispose();
        if (responseDto is null) return Result<Basket>.NotFound();

        var basket = new Basket(responseDto.Basket.UserName);
        basket.Id = responseDto.Basket.Id;

        foreach (var item in responseDto.Basket.Items)
        {
            basket.AddItem(item.CatalogItemId, item.Price, item.Quantity);
        }

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.Id.ToString(), out var quantity))
            {
                if (logger != null) logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                item.SetQuantity(quantity);
            }
        }
        basket.RemoveEmptyItems();

        var updateResponse = await httpClient.PutAsJsonAsync(query, basket);
        updateResponse.EnsureSuccessStatusCode();
        updateResponse.Dispose();

        return basket;
    }

    public async Task TransferBasketAsync(string anonymousId, string userName)
    {
        var query = $"gateway/api/basket";
        var annonGetResponse = await httpClient.GetAsync($"{query}?UserName={anonymousId}");
        
        var responseDto = await annonGetResponse.Content.ReadFromJsonAsync<CreateBasketResponse>();
        if (responseDto is null) return;   

        var anonymousBasket = responseDto.Basket;

        annonGetResponse.Dispose();

        logger.LogInformation($"Fetched annon basket for user with username: {anonymousBasket.UserName}");

        var userGetResponse = await httpClient.GetAsync($"{query}?UserName={userName}");

        if (userGetResponse.IsSuccessStatusCode)
        {
            responseDto = await userGetResponse.Content.ReadFromJsonAsync<CreateBasketResponse>() ??
                throw new ArgumentNullException();
            userGetResponse.Dispose();
        }
        else
        {
            var createRequest = new CreateBasketRequest
            {
                UserName = userName,
                Items = new List<GatewayApiDtos.Basket.BasketItem>()
            };
            var createResponse = await httpClient.PostAsJsonAsync(query, createRequest);
            responseDto = await createResponse.Content.ReadFromJsonAsync<CreateBasketResponse>() ??
                throw new ArgumentNullException();
            createResponse.Dispose();
        }

        logger.LogInformation("Transfering basket to user...");

        var basket = new Basket(responseDto.Basket.UserName);
        basket.Id = responseDto.Basket.Id;

        foreach (var item in responseDto.Basket.Items)
        {
            basket.AddItem(item.CatalogItemId, item.Price, item.Quantity);
        }

        foreach (var item in anonymousBasket.Items)
        {
            basket.AddItem(item.CatalogItemId, item.Price, item.Quantity);
        }

        var updateRequest = new UpdateBasketRequest
        {
            Id = basket.Id,
            UserName = basket.BuyerId,
            Items = new List<GatewayApiDtos.Basket.BasketItem>()
        };

        foreach (var item in basket.Items)
        {
            var basketItem = new GatewayApiDtos.Basket.BasketItem
            {
                Id = item.Id,
                CatalogItemId = item.CatalogItemId,
                ProductName = "",
                Price = item.UnitPrice,
                Quantity = item.Quantity
            };

            updateRequest.Items.Add(basketItem);
            
        }

        var updateResponse = await httpClient.PutAsJsonAsync(query, updateRequest);
        updateResponse.EnsureSuccessStatusCode();
        updateResponse.Dispose();

        var deleteResponse = await httpClient.DeleteAsync($"{query}?UserName={anonymousId}");
        deleteResponse.EnsureSuccessStatusCode();
        deleteResponse.Dispose();
    }
}
