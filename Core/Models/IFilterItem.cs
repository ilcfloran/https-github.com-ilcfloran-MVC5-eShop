using System.Collections.Generic;
using TreeUtility;

namespace MyEShop.Core.Models
{
    public interface IFilterItem : ITreeNode<FilterItem>
    {
        GroupFilter GroupFilter { get; set; }
        int GroupFilterId { get; set; }
        int Id { get; set; }
        ICollection<Product> Products { get; set; }
        string Title { get; set; }
    }
}