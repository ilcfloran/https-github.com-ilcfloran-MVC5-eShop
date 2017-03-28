using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImageName { get; set; }

        public int ProductId { get; set; }

    }
}