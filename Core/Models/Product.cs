using System;
using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public class Product : IProduct
    {

        public Product()
        {
            Count = 1;
            ProductImages = new List<ProductImage>();
            Comments = new List<Comment>();


        }
        public int Id { get; set; }

        public String UserId { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Visit { get; set; }

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

        public virtual ApplicationUser User { get; set; }


    }
}