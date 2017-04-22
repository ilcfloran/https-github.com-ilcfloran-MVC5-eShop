using Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.ModelConfigs
{
    class ProductsFilterItemsConfiguration : EntityTypeConfiguration<ProductsFilterItems>
    {
        public ProductsFilterItemsConfiguration()
        {
            ToTable("Products_FilterItems");
            HasKey(p => new { p.ProductId, p.FilterItemId });
        }
    }
}
