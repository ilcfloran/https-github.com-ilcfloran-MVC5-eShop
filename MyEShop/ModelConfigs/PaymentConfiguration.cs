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
    public class PaymentConfiguration : EntityTypeConfiguration<Payment>
    {

        public PaymentConfiguration()
        {
            Property(p => p.TransNo).HasMaxLength(15);

            Property(p => p.BankGetNo).HasMaxLength(15);

            Property(p => p.Price).HasPrecision(18, 2);

            Property(p => p.GroupNo).HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_Payment_GroupNo") { IsUnique = false }));

        }
    }
}