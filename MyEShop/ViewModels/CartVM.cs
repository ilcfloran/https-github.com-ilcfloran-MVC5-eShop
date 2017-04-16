using System.Collections.Generic;

namespace MyEShop.ViewModels
{
    public class CartVM
    {
        public decimal TotalPrice { get; set; }

        public List<CartItemVM> CartItems { get; set; }

        public int Count { get; set; }
    }
}