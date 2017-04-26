using MyEShop.Core.Models;
using System;

namespace Core.Models
{
    public class Message
    {
        public Message()
        {
            this.Date = DateTime.Now;
        }
        public int Id { get; set; }

        public string UserRecId { get; set; }

        public ApplicationUser UserRec { get; set; }

        public string UserSendId { get; set; }

        public ApplicationUser UserSend { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public DateTime Date { get; set; }
    }
}
