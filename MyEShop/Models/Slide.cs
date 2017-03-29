using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEShop.Models
{
    public class Slide
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Link { get; set; }

        public Byte Sort { get; set; }

        public bool Show { get; set; }
    }
}