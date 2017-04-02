using System.Collections.Generic;

namespace MyEShop.Web.ViewModels
{
    public class FilterItemVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int GroupFilterId { get; set; }

        public GroupFilterVM GroupFilter { get; set; }

        public ICollection<ProductVM> Products { get; set; }
    }
}