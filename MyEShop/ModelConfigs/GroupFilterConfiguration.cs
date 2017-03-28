using MyEShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MyEShop.ModelConfigs
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