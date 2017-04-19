using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class TempSaleConfiguration : EntityTypeConfiguration<TempSale>
    {

        public TempSaleConfiguration()
        {

            Property(t => t.ShoppingCartId).IsRequired().HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_TempSale_ShoppingCartId") { IsUnique = false }));


        }
    }
}