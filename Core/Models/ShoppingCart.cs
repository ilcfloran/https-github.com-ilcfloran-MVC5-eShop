using System;

namespace MyEShop.Core.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        public int Count { get; set; }

    }
}