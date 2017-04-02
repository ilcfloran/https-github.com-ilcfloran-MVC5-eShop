using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Core.Models
{
    public class TempSale
    {
        public int Id { get; set; }

        public int ShoppingCartId { get; set; }

        public DateTime Date { get; set; }

        public string BankGetNo { get; set; }

        public bool Status { get; set; }
    }
}