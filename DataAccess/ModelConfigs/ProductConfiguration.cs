using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            Property(p => p.Name).HasMaxLength(70).IsRequired();
            Property(p => p.Text).HasMaxLength(100).IsOptional();
            Property(p => p.Description).HasMaxLength(100).IsOptional();
            Property(p => p.Price).HasPrecision(18, 2);
            Property(p => p.OnSalePrice).HasPrecision(18, 2).IsOptional();
            Property(p => p.weight).IsOptional();

            Property(p => p.CategoryId).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Product_CategoryId") { IsUnique = false }));

            HasMany<FilterItem>(p => p.FilterItems)
                .WithMany(f => f.Products)
                .Map(pf =>
                {
                    pf.MapLeftKey("Product_Id");
                    pf.MapRightKey("FilterItem_Id");
                    pf.ToTable("FilterItemProducts");
                });

        }

    }
}