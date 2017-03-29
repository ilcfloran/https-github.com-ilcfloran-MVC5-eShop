using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEShop.Models
{
    public class Product
    {

        public Product()
        {
            Count = 1;
            ProductImages = new List<ProductImage>();
            Comments = new List<Comment>();
                

        }
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Visit {get; set; }

        public DateTime date { get; set; }

        public DateTime? EndDate { get; set; }

        public int? weight { get; set; }

        public bool Onsale { get; set; }

        public decimal? OnSalePrice { get; set; }

        public int CategoryId { get; set; }

        public int Count { get; set; }

        public virtual IEnumerable<ProductImage> ProductImages { get; set; }

        public virtual IEnumerable<Comment> Comments { get; set; }

        public ICollection<FilterItem> FilterItems { get; set; }

    }
}