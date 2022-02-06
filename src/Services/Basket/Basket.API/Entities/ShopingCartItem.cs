namespace Basket.API.Entities
{
    public class ShopingCartItem
    {
        public int Quantity { get; set; }
        public int Price { get; set; } = 0;
        public string? Colour { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

    }
}
