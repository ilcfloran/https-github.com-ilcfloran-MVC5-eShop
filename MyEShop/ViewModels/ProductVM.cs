using MyEShop.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEShop.Web.ViewModels
{
    public class ProductVM
    {
        public ProductVM()
        {
            Count = 1;
            ProductImages = new List<ProductImageVM>();
            Comments = new List<CommentVM>();
        }


        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "You must enter a product name.")]
        [StringLength(50, ErrorMessage = "product name must be 50 characters or shorter.")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter text.")]
        [StringLength(100, ErrorMessage = "product name must be 50 characters or shorter.")]
        [Display(Name = "Product Text")]
        public string Text { get; set; }


        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Visit { get; set; }

        public DateTime date { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(typeof(int), "0", "2000000")]
        public int? weight { get; set; }

        public bool Onsale { get; set; }

        public decimal? OnSalePrice { get; set; }

        public int CategoryId { get; set; }

        public int Count { get; set; }

        public virtual IEnumerable<ProductImageVM> ProductImages { get; set; }

        public virtual IEnumerable<CommentVM> Comments { get; set; }

        public ICollection<FilterItemVM> FilterItems { get; set; }

    }
}