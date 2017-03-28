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
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            Property(p => p.Name).HasMaxLength(70).IsRequired();
            Property(p => p.Text).HasMaxLength(100).IsOptional();
            Property(p => p.Description).HasMaxLength(100).IsOptional();
            Property(p => p.Price).HasPrecision(18,2);
            Property(p => p.OnSalePrice).HasPrecision(18, 2).IsOptional();
            Property(p => p.weight).IsOptional();

            Property(p => p.CategoryId).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Product_CategoryId") { IsUnique = false }));


        }

    }
}