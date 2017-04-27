using MyEShop.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyEShop.DataAccess.ModelConfigs
{
    class CategoriesGroupFiltersConfiguration : EntityTypeConfiguration<CategoriesGroupFilters>
    {
        public CategoriesGroupFiltersConfiguration()
        {
            ToTable("Categories_GroupFilters");

            HasKey(c => new { c.CategoryId, c.GroupFilterId });
        }
    }
}
