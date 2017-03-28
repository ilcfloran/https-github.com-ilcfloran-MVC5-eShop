using MyEShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TreeUtility;

namespace MyEShop.ViewModels
{
    public class CommentVM : ITreeNode<CommentVM>
    {

        public CommentVM()
        {
            Children = new List<CommentVM>();
        }
        public int Id { get; set; }


        public string UserId { get; set; }

        public string Email { get; set; }


        [Required(ErrorMessage = "You must enter your comment.")]
        [StringLength(500, ErrorMessage = "Your comment must be 500 characters or shorter.")]
        [Display(Name = "Comment")]
        public string Text { get; set; }

        public DateTime date { get; set; }

        public bool Confirmed { get; set; }

        public int ProductId { get; set; }

        private int? _parentId { get; set; }

        public int? ParentId
        {
            get { return _parentId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A comment cannot have itself as its parent.");

                _parentId = value;
            }
        }

        public virtual ApplicationUser User { get; set; }


        public virtual CommentVM Parent { get; set; }

        public IList<CommentVM> Children { get; set; }
       
    }
}