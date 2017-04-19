using System;

namespace MyEShop.Core.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }

        public int GroupNo { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public string TransNo { get; set; }

        public string BankGetNo { get; set; }

    }
}