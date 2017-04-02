using MyEShop.Core.Models;
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

        }
    }
}