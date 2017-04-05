using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public interface ICategory
    {
        string CategoryName { get; set; }
        IList<Category> Children { get; set; }
        //ICollection<GroupFilter> GroupFilters { get; set; }
        int Id { get; set; }
        Category Parent { get; set; }
        int? ParentCategoryId { get; set; }
    }
}