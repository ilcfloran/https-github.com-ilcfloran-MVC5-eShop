using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Core.Models
{
    public class Auction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public decimal Price { get; set; }

        public bool Win { get; set; }
    }
}