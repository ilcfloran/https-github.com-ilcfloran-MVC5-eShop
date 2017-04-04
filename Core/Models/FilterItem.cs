using System;
using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public class FilterItem : IFilterItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int GroupFilterId { get; set; }

        public GroupFilter GroupFilter { get; set; }

        public ICollection<Product> Products { get; set; }



        private int? _parentFilterItemId;

        public int? ParentFilterItemId
        {
            get { return _parentFilterItemId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentFilterItemId = value;
            }
        }


        public virtual FilterItem Parent { get; set; }

        public IList<FilterItem> Children { get; set; }

        public ICollection<GroupFilter> GroupFilters { get; set; }
    }
}