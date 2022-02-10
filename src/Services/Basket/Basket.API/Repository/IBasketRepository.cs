using Basket.API.Entities;

namespace Basket.API.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task <bool> DeleteBasket(string userName);
    }
}
