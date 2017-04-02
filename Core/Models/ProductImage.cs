using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Core.Models
{
    public class ProductImage : IProductImage
    {
        public int Id { get; set; }

        public string ImageName { get; set; }

        public int ProductId { get; set; }

    }
}