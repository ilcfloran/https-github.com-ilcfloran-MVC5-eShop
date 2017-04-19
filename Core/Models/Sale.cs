using System;

namespace MyEShop.Core.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public int GroupNo { get; set; }

        public SalesStatus StatusId { get; set; }

        public bool Payed { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int TransId { get; set; }

        public int Count { get; set; }

        public int TrackingCode { get; set; }

    }


    public enum SalesStatus
    {
        OnHold = 1,
        Preparing = 2,
        Sent = 3,
        Delivered = 4
    }

}