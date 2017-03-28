using MyEShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MyEShop.ModelConfigs
{
    public class SaleConfiguration : EntityTypeConfiguration<Sale>
    {

        public SaleConfiguration()
        {
            Property(s => s.Price).HasPrecision(18, 2).IsRequired();

            Property(s => s.ProductId).IsRequired().HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_Sale_ProductId") { IsUnique = false }));

            Property(s => s.UserId).IsRequired();
        }

    }
}