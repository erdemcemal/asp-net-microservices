using System.Text.Json;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<BasketRepository> _logger;

    public BasketRepository(IDistributedCache distributedCache, ILogger<BasketRepository> logger)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _logger = logger;
    }

    public async Task<ShoppingCart> GetBasket(string username)
    {
        var basket = await _distributedCache.GetStringAsync(username);
        return string.IsNullOrEmpty(basket) 
            ? null 
            : JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        var json = JsonSerializer.Serialize(basket);
        await _distributedCache.SetStringAsync(basket.Username, json);
        return await GetBasket(basket.Username);
    }

    public async Task DeleteBasket(string username)
    {
        await _distributedCache.RemoveAsync(username);
    }
}