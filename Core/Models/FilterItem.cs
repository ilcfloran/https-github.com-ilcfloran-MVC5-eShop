using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public class FilterItem : IFilterItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int GroupFilterId { get; set; }

        public IGroupFilter GroupFilter { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}