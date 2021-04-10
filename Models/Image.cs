using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        [Display(Name = "Fotoğraf URL'si")]
        public string ImageUrl { get; set; }
    }
}   