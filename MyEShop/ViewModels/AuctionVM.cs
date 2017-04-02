using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEShop.Web.ViewModels
{
    public class AuctionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Auction should be associated with a product.")]
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "You must enter your offer.")]
        [Range(1, 1000000000, ErrorMessage = "The Quantity must be between 1 and 1,000,000,000.")]
        public decimal Price { get; set; }

        public bool Win { get; set; }
    }
}