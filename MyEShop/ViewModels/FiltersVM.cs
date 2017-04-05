using MyEShop.Core.Models;
using System.Collections.Generic;

namespace MyEShop.ViewModels
{
    public class FiltersVM
    {
        public FiltersVM()
        {
            List<FilterItem> FilterItemsInGroup = new List<FilterItem>();
        }

        public string GroupName { get; set; }

        public List<FilterItem> FilterItemsInGroup { get; set; }


    }
}