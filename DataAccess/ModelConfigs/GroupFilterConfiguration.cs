using MyEShop.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    public class GroupFilterConfiguration : EntityTypeConfiguration<GroupFilter>
    {
        public GroupFilterConfiguration()
        {
            Property(g => g.Title).HasMaxLength(100).IsRequired();

            HasOptional(g => g.Parent)
                 .WithMany(g => g.Children)
                 .HasForeignKey(g => g.ParentId)
                 .WillCascadeOnDelete(false);


        }
    }
}