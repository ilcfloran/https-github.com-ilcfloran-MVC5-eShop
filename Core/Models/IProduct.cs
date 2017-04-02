using System;
using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public interface IProduct
    {
        int CategoryId { get; set; }
        IEnumerable<Comment> Comments { get; set; }
        int Count { get; set; }
        DateTime date { get; set; }
        string Description { get; set; }
        DateTime? EndDate { get; set; }
        ICollection<FilterItem> FilterItems { get; set; }
        int Id { get; set; }
        string Image { get; set; }
        string Name { get; set; }
        bool Onsale { get; set; }
        decimal? OnSalePrice { get; set; }
        decimal Price { get; set; }
        IEnumerable<ProductImage> ProductImages { get; set; }
        string Text { get; set; }
        string UserId { get; set; }
        int Visit { get; set; }
        int? weight { get; set; }
    }
}