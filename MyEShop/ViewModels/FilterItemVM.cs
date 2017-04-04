using System;
using System.Collections.Generic;
using TreeUtility;

namespace MyEShop.Web.ViewModels
{
    public class FilterItemVM : ITreeNode<FilterItemVM>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int GroupFilterId { get; set; }

        public GroupFilterVM GroupFilter { get; set; }

        public ICollection<ProductVM> Products { get; set; }



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


        public virtual FilterItemVM Parent { get; set; }

        public IList<FilterItemVM> Children { get; set; }

        public ICollection<GroupFilterVM> GroupFilters { get; set; }


    }
}