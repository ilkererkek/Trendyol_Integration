using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models.JSONModels
{

    //NOTE: JSON Models are created to be a schema for creating Product JSON files.
    public class ProductJSONModel
    {
        public string barcode { get; set; }
        public string title { get; set; }
        public string productMainId { get; set; }
        public int brandId { get; set; }
        public int categoryId { get; set; }
        public int quantity { get; set; }
        public string stockCode { get; set; }
        public decimal dimensionalWeight { get; set; }
        public string description { get; set; }
        public string currencyType { get; set; }
        public decimal listPrice { get; set; }
        public decimal salePrice { get; set; }
        public int cargoCompanyId { get; set; }
        public List<ImageJSONModel> images { get; set; }
        public int vatRate { get; set; }
        public List<AttributeJSONModel> attributes { get; set; }

        public ProductJSONModel(Product product)
        {
            this.barcode = product.Barcode;
            this.title = product.Title;
            this.productMainId = product.ProductMainId;
            this.brandId = product.BrandId;
            this.categoryId = product.Category.CategoryId;
            this.quantity = product.Quantity;
            this.stockCode = product.StockCode;
            this.dimensionalWeight = product.DimensionalWeight;
            this.description = product.Description;
            this.currencyType = "TRY";
            this.listPrice = product.ListPrice;
            this.salePrice = product.SalePrice;
            this.cargoCompanyId = product.CargoCompanyId;
            this.vatRate = product.VatRate;
            this.images = new List<ImageJSONModel>();
            this.attributes = new List<AttributeJSONModel>();
            foreach(var image in product.images)
            {
                ImageJSONModel imageJSON = new ImageJSONModel();
                imageJSON.url = image.ImageUrl;
                this.images.Add(imageJSON);
            }
            foreach (var attribute in product.Attributes)
            {
                AttributeJSONModel attributeJSON = new AttributeJSONModel();
                attributeJSON.attributeId = attribute.AttributeCode;
                if (attribute.AttributeValueId == -1)
                {
                    attributeJSON.customAttributeValue = attribute.AttributeValue;
                    attributeJSON.attributeValueId = null;
                }
                else
                {
                    attributeJSON.attributeValueId = attribute.AttributeValueId;
                }
                this.attributes.Add(attributeJSON);
            }
        }
    }
}