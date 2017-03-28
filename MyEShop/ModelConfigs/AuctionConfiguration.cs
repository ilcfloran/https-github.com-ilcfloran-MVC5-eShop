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
    public class AuctionConfiguration : EntityTypeConfiguration<Auction>
    {

        public AuctionConfiguration()
        {

            Property(a => a.Price).HasPrecision(18, 2).IsRequired();

            Property(a => a.ProductId).IsRequired();

            Property(a => a.UserId).IsRequired();

            Property(a => a.ProductId).HasColumnAnnotation(
                "Index", new IndexAnnotation(new IndexAttribute("IX_Auction", 2) { IsUnique = true }));

            Property(a => a.UserId).HasColumnAnnotation(
                "Index", new IndexAnnotation(new IndexAttribute("IX_Auction", 1) { IsUnique = true }));

        }

    }
}