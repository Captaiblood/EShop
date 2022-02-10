using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public async Task <bool> DeleteBasket(string userName)
        {
            var basket = await GetBasket(userName);
           
            if (!String.IsNullOrEmpty(basket.UserName))
            {
                await _redisCache.RemoveAsync(userName);
                return true;
            }
            else { return false; }
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            var _basetModel = new ShoppingCart();
           
            var basket = await _redisCache.GetStringAsync(userName);
           
            if (String.IsNullOrEmpty(basket))
                return _basetModel;
            
             var _cart = JsonConvert.DeserializeObject<ShoppingCart>(basket);

            if (_cart is not null) { return _cart; }

            return _basetModel;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            var _basketModel = new ShoppingCart();
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
           var _userName = basket.UserName;
            if (String.IsNullOrEmpty(_userName)) { return _basketModel; }
            return await GetBasket(_userName);
        }
    }
}
