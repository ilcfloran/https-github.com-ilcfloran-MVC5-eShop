using System;
using System.Collections.Generic;
using TreeUtility;

namespace MyEShop.Web.ViewModels
{
    public class GroupFilterVM : ITreeNode<GroupFilterVM>
    {
        public int Id { get; set; }

        private int? _parentId;

        public int? ParentId
        {
            get { return _parentId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A group filter cannot have itself as its parent.");

                _parentId = value;
            }
        }

        public string Title { get; set; }

        public virtual GroupFilterVM Parent { get; set; }

        public IList<GroupFilterVM> Children { get; set; }

        public ICollection<FilterItemVM> FilterItems { get; set; }

        public ICollection<CategoryVM> Categories { get; set; }

    }
}