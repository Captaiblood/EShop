namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; } = 0;
        public int Price { get; set; } = 0;
        public string Colour { get; set; }= String.Empty;
        public string ProductId { get; set; } = String.Empty;
        public string ProductName { get; set; } = String.Empty;

    }
}
