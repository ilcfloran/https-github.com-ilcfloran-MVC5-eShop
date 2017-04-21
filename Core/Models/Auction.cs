using System;

namespace MyEShop.Core.Models
{
    public class Auction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public decimal Price { get; set; }

        public bool Win { get; set; }

        public DateTime Date { get; set; }
    }
}