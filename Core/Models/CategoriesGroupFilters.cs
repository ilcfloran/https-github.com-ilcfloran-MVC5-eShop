using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEShop.Core.Models
{
    public class CategoriesGroupFilters
    {
        [Key]
        [Column(Order = 1)]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Key]
        [Column(Order = 2)]
        public int GroupFilterId { get; set; }

        public GroupFilter GroupFilters { get; set; }

    }
}
