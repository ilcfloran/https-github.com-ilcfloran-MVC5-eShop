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
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            Property(c => c.Id).HasColumnName("CategoryId");

            Property(c => c.CategoryName)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_Category_CategoryName") { IsUnique = true }));

            HasOptional(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentCategoryId)
                .WillCascadeOnDelete(false);

            HasMany<GroupFilter>(c => c.GroupFilters)
                .WithMany(gf => gf.Categories)
                .Map(cg =>
                {
                    cg.MapLeftKey("CategoryId");
                    cg.MapRightKey("GroupFilterId");
                    cg.ToTable("Categories_GroupFilters");
                });

        }
    }
}