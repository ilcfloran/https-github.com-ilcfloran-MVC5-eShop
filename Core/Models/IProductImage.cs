namespace MyEShop.Core.Models
{
    public interface IProductImage
    {
        int Id { get; set; }
        string ImageName { get; set; }
        int ProductId { get; set; }
    }
}