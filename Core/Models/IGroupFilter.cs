using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public interface IGroupFilter
    {
        ICollection<Category> Categories { get; set; }
        IList<GroupFilter> Children { get; set; }
        ICollection<FilterItem> FilterItems { get; set; }
        int Id { get; set; }
        GroupFilter Parent { get; set; }
        int? ParentId { get; set; }
        string Title { get; set; }
    }
}