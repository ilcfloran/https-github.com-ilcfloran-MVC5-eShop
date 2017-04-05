using System;
using System.Collections.Generic;
using TreeUtility;

namespace MyEShop.Core.Models
{
    public class GroupFilter : ITreeNode<GroupFilter>, IGroupFilter
    {
        public GroupFilter()
        {
            IList<GroupFilter> Children = new List<GroupFilter>();
        }

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

        public GroupFilter Parent { get; set; }

        public IList<GroupFilter> Children { get; set; }

        public virtual ICollection<FilterItem> FilterItems { get; set; }

        public List<CategoriesGroupFilters> CategoriesGroupFilters { get; set; }


        //public ICollection<Category> Categories { get; set; }
    }
}