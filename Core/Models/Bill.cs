using System;

namespace MyEShop.Core.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public int LastRec { get; set; }

        public int TotalRec { get; set; }

        public DateTime? TimeOfLastRec { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
