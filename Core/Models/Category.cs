using System;
using System.Collections.Generic;
using TreeUtility;

namespace MyEShop.Core.Models
{
    public class Category : ITreeNode<Category>, ICategory
    {

        public Category()
        {
            IList<Category> Children = new List<Category>();
        }

        public int Id { get; set; }

        private int? _parentCategoryId;

        public int? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentCategoryId = value;
            }
        }

        public string CategoryName { get; set; }

        public virtual Category Parent { get; set; }

        public IList<Category> Children { get; set; }

        public List<CategoriesGroupFilters> CategoriesGroupFilters { get; set; }


        // public ICollection<GroupFilter> GroupFilters { get; set; }

    }
}