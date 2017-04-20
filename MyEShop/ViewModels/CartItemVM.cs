using System;

namespace MyEShop.Web.ViewModels
{
    public class CartItemVM
    {
        public string ProductName { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public int WebId { get; set; }

        public DateTime Date { get; set; }
    }
}