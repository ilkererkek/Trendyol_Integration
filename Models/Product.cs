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
        [Display(Name = "Barkod")]
        public string Barcode { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Ürün Adı")]
        public string Title { get; set; }
        [Required]
        [StringLength(40)]
        [Display(Name = "Ana Ürün Kodu")]
        public string ProductMainId { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        [Display(Name = "Marka Adı")]
        public string BrandName { get; set; }
        [Required]
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Stok Kodu")]
        public string StockCode { get; set; }
        [Required]
        [Display(Name = "Desi Miktarı")]
        public decimal DimensionalWeight { get; set; }
        [Required]
        [StringLength(20000)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Liste Satış Fiyatı")]
        public decimal ListPrice { get; set; }
        [Required]
        [Display(Name = "Satış Fiyatı")]
        public decimal SalePrice { get; set; }
        [Display(Name = "Kargo Şirketi")]
        public int CargoCompanyId { get; set; }
        public ICollection<Image> images { get; set; }
        [Display(Name = "KDV Oranı")]   
        [Required]
        public int VatRate { get; set; }

        public string batchRequestId { get; set; }
        public ICollection<Attribute> Attributes { get; set; }
        public virtual Category Category { get; set; }

        public Product(string barcode, string title, string productMainId, int brandId, string brandName, int quantity, string stockCode, 
            decimal dimensionalWeight, string description, decimal listPrice, decimal salePrice, int cargoCompanyId, int vatRate)
        {
            Barcode = barcode;
            Title = title;
            ProductMainId = productMainId;
            BrandId = brandId;
            BrandName = brandName;
            Quantity = quantity;
            StockCode = stockCode;
            DimensionalWeight = dimensionalWeight;
            Description = description;
            ListPrice = listPrice;
            SalePrice = salePrice;
            CargoCompanyId = cargoCompanyId;
            VatRate = vatRate;
            this.images = new List<Image>();
            this.Attributes = new List<Models.Attribute>();
        }

        public Product()
        {

        }
    }
}