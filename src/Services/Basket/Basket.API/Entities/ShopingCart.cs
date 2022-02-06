namespace Basket.API.Entities
{
    public class ShopingCart
    {
        public string? UserName { get; set; } 

        public List<ShopingCartItem> shopingCartItems { get; set; } = new List<ShopingCartItem>();
        public ShopingCart() { }       

        
        public ShopingCart(string username)
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
                    totalPrice = totalPrice * item.Price;
                }
                return totalPrice;

            }

        }


    }
}
