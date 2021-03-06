﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Web.ViewModels
{
    public class PaymentVM
    {
        public int Id { get; set; }

        public int GroupNo { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public string TransNo { get; set; }

        public string BankGetNo { get; set; }
    }
}