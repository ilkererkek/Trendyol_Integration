using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models
{
    public class Product
    {
        
        public int ProductId { get; set; }
        [Required]
        [StringLength(40)]
        public string Barcode { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(40)]
        public string ProductMainId { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [StringLength(100)]
        public string StockCode { get; set; }
        [Required]
        public decimal DimensionalWeight { get; set; }
        [Required]
        [StringLength(20000)]
        public string Description { get; set; }
        [Required]
        public decimal ListPrice { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        public int CargoCompanyId { get; set; }
        [Required]
        public ICollection<Image> images { get; set; }
        [Required]
        public int VatRate { get; set; }
        [Required]
        public ICollection<Attribute> Attributes { get; set; }
        [Required]
        public virtual Category Category { get; set; }
    }
}