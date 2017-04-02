using System;
using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public class Comment : IComment
    {
        public Comment()
        {
            Children = new List<Comment>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public DateTime date { get; set; }

        public bool Confirmed { get; set; }

        public int ProductId { get; set; }

        private int? _parentId;

        public int? ParentId
        {
            get { return _parentId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentId = value;
            }
        }

        public virtual ApplicationUser User { get; set; }

        public virtual Comment Parent { get; set; }

        public IList<Comment> Children { get; set; }
    }
}