using MyEShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MyEShop.ModelConfigs
{
    public class ProductImageConfiguration : EntityTypeConfiguration<ProductImage>
    {

        public ProductImageConfiguration()
        {

            Property(pi => pi.ImageName).HasMaxLength(80).IsRequired();
            Property(pi => pi.ProductId).IsRequired();

        }
    }
}