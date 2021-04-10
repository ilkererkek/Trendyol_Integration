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
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen Kategori Seçiniz")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Kargo Şirketi")]
        public int CargoCompanyId { get; set; }
        public List<Provider> Providers { get; set; }
        public List<Category> Categories;

        public CreateProductModel(List<Provider> providers, List<Category> categories)
        {
           
            Providers = providers;
            Categories = categories;
        }
        public CreateProductModel()
        {

        }
    }
}