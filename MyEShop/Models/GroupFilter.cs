using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreeUtility;

namespace MyEShop.Models
{
    public class GroupFilter : ITreeNode<GroupFilter>
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

        public virtual GroupFilter Parent { get; set; }

        public IList<GroupFilter> Children { get; set; }
    }
}