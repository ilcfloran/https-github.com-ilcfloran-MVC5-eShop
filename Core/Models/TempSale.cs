using System;

namespace MyEShop.Core.Models
{
    public class TempSale
    {
        public int Id { get; set; }

        public int ShoppingCartId { get; set; }

        public DateTime Date { get; set; }

        public string BankGetNo { get; set; }

        public bool Status { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

    }
}