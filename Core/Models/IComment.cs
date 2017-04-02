using System;
using System.Collections.Generic;

namespace MyEShop.Core.Models
{
    public interface IComment
    {
        IList<Comment> Children { get; set; }
        bool Confirmed { get; set; }
        DateTime date { get; set; }
        string Email { get; set; }
        int Id { get; set; }
        Comment Parent { get; set; }
        int? ParentId { get; set; }
        int ProductId { get; set; }
        string Text { get; set; }
        ApplicationUser User { get; set; }
        string UserId { get; set; }
    }
}