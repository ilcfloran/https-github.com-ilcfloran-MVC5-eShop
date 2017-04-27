using MyEShop.Core.Models;
using System;

namespace MyEShop.Web.ViewModels
{
    public class BillVM
    {
        public int Id { get; set; }

        public int Available { get; set; }

        public int Withdrawable { get; set; }

        public int LastRec { get; set; }

        public int TotalRec { get; set; }

        public DateTime TimeOfLastRec { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}