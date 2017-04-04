using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class FilterItemConfiguration : EntityTypeConfiguration<FilterItem>
    {
        public FilterItemConfiguration()
        {
            HasRequired(f => f.GroupFilter)
                .WithMany(g => g.FilterItems)
                .HasForeignKey(f => f.GroupFilterId);



            Property(f => f.Title)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_FilterItem_Title") { IsUnique = false }));

            HasOptional(f => f.Parent)
                .WithMany(f => f.Children)
                .HasForeignKey(f => f.ParentFilterItemId)
                .WillCascadeOnDelete(false);

            //HasMany<GroupFilter>(c => c.GroupFilters)
            //    .WithMany(gf => gf.Categories)
            //    .Map(cg =>
            //    {
            //        cg.MapLeftKey("CategoryId");
            //        cg.MapRightKey("GroupFilterId");
            //        cg.ToTable("Categories_GroupFilters");
            //    });

        }
    }
}