using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class ShoppingCartConfiguration : EntityTypeConfiguration<ShoppingCart>
    {

        public ShoppingCartConfiguration()
        {
            Property(s => s.UserId).IsRequired().HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_ShoppingCart_UserId") { IsUnique = false }));


        }
    }
}