using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Trendyol_Integration.Models;

namespace Trendyol_Integration.ViewModels
{
    public class CreateProductModel
    {
        [Key]
        public int ID { get; set; }
        public Product product { get; set; }
        public List<Provider> providers { get; set; }
        public List<SupplierAdress> supplier { get; set; }
        public List<Category> categories { get; set; }
    }
}