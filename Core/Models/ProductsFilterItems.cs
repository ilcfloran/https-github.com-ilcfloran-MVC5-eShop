using MyEShop.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ProductsFilterItems
    {

        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Key]
        [Column(Order = 2)]
        public int FilterItemId { get; set; }

        public FilterItem FilterItem { get; set; }
    }
}
