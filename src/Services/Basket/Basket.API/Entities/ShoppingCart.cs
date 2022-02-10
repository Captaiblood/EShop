namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string? UserName { get; set; } 

        public List<ShoppingCartItem> shopingCartItems { get; set; } = new List<ShoppingCartItem>();
        public ShoppingCart() { }       

        
        public ShoppingCart(string username)
        {
            this.UserName = username;
        }



        public decimal TotalPrice
        {
            get
            {

                decimal totalPrice = 0;
                foreach (var item in shopingCartItems)
                {
                    totalPrice = totalPrice + item.Price;
                }
                return totalPrice;

            }

        }


    }
}
