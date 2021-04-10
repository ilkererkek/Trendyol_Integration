using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Trendyol_Integration.Models;

namespace Trendyol_Integration.ViewModels
{
   

    public class CreateProceedProductModel
    {
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public int CargoCompanyId { get; set; }
        public List<CategoryAttribute> Attributes { get; set; }
        public List<Image> images { get; set; }
        public CreateProceedProductModel()
        {
        }
       


    }
}