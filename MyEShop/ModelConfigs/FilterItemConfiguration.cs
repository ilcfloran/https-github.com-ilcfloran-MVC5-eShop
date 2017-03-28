using MyEShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MyEShop.ModelConfigs
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