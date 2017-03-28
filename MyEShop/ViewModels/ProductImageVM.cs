using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEShop.ViewModels
{
    public class ProductImageVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Image name is required.")]
        [StringLength(50, ErrorMessage = "Image name must be 50 characters or shorter.")]
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }


        public int ProductId { get; set; }
    }
}