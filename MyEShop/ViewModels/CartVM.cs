using System.Collections.Generic;

namespace MyEShop.Web.ViewModels
{
    public class CartVM
    {
        public CartVM()
        {
            List<CartItemVM> CartItems = new List<CartItemVM>();
        }

        public decimal TotalPrice { get; set; }

        public List<CartItemVM> CartItems { get; set; }

        public int Count { get; set; }
    }
}