using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class PaymentConfiguration : EntityTypeConfiguration<PaymentModel>
    {

        public PaymentConfiguration()
        {
            ToTable("Payments");
            Property(p => p.TransNo).HasMaxLength(15);

            Property(p => p.BankGetNo).HasMaxLength(15);

            Property(p => p.Price).HasPrecision(18, 2);

            Property(p => p.GroupNo).HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_Payment_GroupNo") { IsUnique = false }));

        }
    }
}