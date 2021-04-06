using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trendyol_Integration.Dal;
using Trendyol_Integration.Models;
using Trendyol_Integration.ViewModels;

namespace Trendyol_Integration.Controllers
{
   
    public class ProductsController : Controller
    {
        IntegrationContext db = new IntegrationContext();
        static long LastAPICall = 0;
        // GET: Products
        public ActionResult Index()
        {
            long currenTimeStamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            if (currenTimeStamp >= LastAPICall + 3600)
            {
                GetProducts();
            }
            List<Product> products = db.Products.ToList();
            return View(products);
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            CreateProductModel createProductModel = new CreateProductModel();
            List<Provider> providers = GetProviders();
            var categories = db.Categories.ToList().Where(x => x.ParentCategory == null);
            List<SupplierAdress> adresses = GetAddresses();
            createProductModel.ID = 0;
            createProductModel.categories = categories.ToList();
            createProductModel.product = new Product();
            createProductModel.providers = GetProviders();
            createProductModel.supplier = GetAddresses(); 

            return View(createProductModel);
        }
        public ActionResult CreateProceed()
        {
            return View();
        }
        private List<Provider> GetProviders()
        {
            
            var client = new RestClient("https://api.trendyol.com/sapigw");
            client.Authenticator = new HttpBasicAuthenticator("Bnib0D0RMditHE4NEiV8", "rAsrd6PpPEDiahvsZEKy");
            client.AddDefaultHeader("user-agent", "235333-PiaLab");
            var request = new RestRequest("shipment-providers");
            var response = client.Get(request);
            JArray responseJSON = JArray.Parse(response.Content);
            List<Provider> providers = new List<Provider>();
            foreach (var res in responseJSON)
            {
                Provider current = new Provider(int.Parse(res["id"].ToString()),res["name"].ToString());
                providers.Add(current);
            }
            return providers;
        }
        private List<SupplierAdress> GetAddresses()
        {

            var client = new RestClient("https://api.trendyol.com/sapigw");
            client.Authenticator = new HttpBasicAuthenticator("Bnib0D0RMditHE4NEiV8", "rAsrd6PpPEDiahvsZEKy");
            client.AddDefaultHeader("user-agent", "235333-PiaLab");
            var request = new RestRequest("suppliers/235333/addresses");
            var response = client.Get(request);
            JObject responseJSON = JObject.Parse(response.Content);
            JArray responseArray = (JArray)responseJSON["supplierAddresses"];
            List<SupplierAdress> addresses = new List<SupplierAdress>();
            foreach (var res in responseArray)
            {
                SupplierAdress address = new SupplierAdress();
                address.id = (int)res["id"];
                address.city = res["city"].ToString();
                address.district = res["district"].ToString();
                addresses.Add(address);
            }
            return addresses;
        }
        private void GetProducts()
        {   
            
            var client = new RestClient("https://api.trendyol.com/sapigw");
            client.Authenticator = new HttpBasicAuthenticator("Bnib0D0RMditHE4NEiV8", "rAsrd6PpPEDiahvsZEKy");
            client.AddDefaultHeader("user-agent", "235333-PiaLab");
            var request = new RestRequest("suppliers/235333/products?size=500");
            var response = client.Get(request);
            JObject responseJSON = JObject.Parse(response.Content);
            JArray responseArray = (JArray)responseJSON["content"];
            LastAPICall =  ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            foreach (var res in responseArray)
            {
                Product old = db.Products.ToList().Find(x => x.StockCode == res["stockCode"].ToString());
                if (old == null)
                {
                    Product newProduct = new Product();
                    newProduct.Barcode = res["barcode"].ToString();
                    newProduct.Title = res["title"].ToString();
                    newProduct.ProductMainId = res["productMainId"].ToString();
                    newProduct.BrandId = (int)res["brandId"];
                    newProduct.BrandName = res["brand"].ToString();
                    newProduct.Quantity = (int)res["quantity"];
                    newProduct.StockCode = res["stockCode"].ToString();
                    newProduct.DimensionalWeight = (decimal)res["dimensionalWeight"];
                    newProduct.Description = res["description"].ToString();
                    newProduct.ListPrice = (decimal)res["listPrice"];
                    newProduct.SalePrice = (decimal)res["salePrice"];
                    newProduct.CargoCompanyId = -1;
                    JArray pics = (JArray)res["images"];
                    List<Image> imagelist = new List<Image>();
                    foreach (var picture in pics)
                    {
                        Image img = new Image();
                        String url = picture["url"].ToString();
                        img.ImageUrl = url;
                        db.Images.Add(img);
                        imagelist.Add(img);
                    }
                    newProduct.images = imagelist;
                    newProduct.VatRate = (int)res["vatRate"];
                    var Category = db.Categories.ToList().Find(x=>x.CategoryName == res["categoryName"].ToString());
                    newProduct.Category = Category;
                    JArray attrs = (JArray)res["attributes"];
                    List<Models.Attribute> attrlist = new List<Models.Attribute>();
                    foreach (var attr in attrs)
                    {
                        Models.Attribute attribute = new Models.Attribute();
                        attribute.AttributeCode = (int)attr["attributeId"];
                        attribute.Category = Category;
                        attribute.AttributeName = attr["attributeName"].ToString();
                        attribute.AttributeValue = attr["attributeValue"].ToString();
                        attrlist.Add(attribute);
                    }
                    newProduct.Attributes = attrlist;
                    db.Products.Add(newProduct);
                }
            }
            db.SaveChanges();
        }
       
    }
}
