using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Models
{
    public class FilterItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int GroupFilterId { get; set; }

        public GroupFilter GroupFilter { get; set; }
    }
}