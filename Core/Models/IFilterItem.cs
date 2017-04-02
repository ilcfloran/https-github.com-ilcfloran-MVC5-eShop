using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public interface IFilterItem
    {
        IGroupFilter GroupFilter { get; set; }
        int GroupFilterId { get; set; }
        int Id { get; set; }
        ICollection<Product> Products { get; set; }
        string Title { get; set; }
    }
}