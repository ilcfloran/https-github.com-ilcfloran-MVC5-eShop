using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TreeUtility;

namespace MyEShop.Models
{
    public class Category 
    {
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
    }
}