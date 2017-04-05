namespace MyEShop.Core.Models
{
    public class CategoriesGroupFilters
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int GroupFilterId { get; set; }

        public GroupFilter GroupFilters { get; set; }

    }
}
