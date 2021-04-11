using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trendyol_Integration.Dal;
using Trendyol_Integration.Models;
using Trendyol_Integration.Util;
using Trendyol_Integration.ViewModels;

namespace Trendyol_Integration.Controllers
{
    [HandleError]
    public class ProductsController : Controller
    {
        IntegrationContext db = new IntegrationContext();
        ApiHelper apiHelper = new ApiHelper();
        // GET: Products
        public ActionResult Index()
        {
            try
            {
               //Fetch Products Data
                GetProducts();
            }
            catch (Exception e)
            {
                ViewBag.Error = "<div class='alert alert-danger' role='alert'>Trendyol'a Erişilemiyor</div>";
            }
            List<Product> products = db.Products.ToList();
            return View(products);
        }
        // GET: Products/Details/id
        public ActionResult Details(int id)
        {

            Product product = db.Products.Include("images").Include("Attributes").ToList().Find(x => x.ProductId == id);
            return View(product);
        }


        // GET: Products/Create
        public ActionResult Create()
        {
            //Get categories and providers
            List<Category> categories = db.Categories.ToList().Where(x => x.ParentCategory == null).ToList();
            CreateProductModel createProductModel = new CreateProductModel(GetProviders(),categories);
            return View(createProductModel);
        }
        // GET: Products/CreateProceed
        public ActionResult CreateProceed(CreateProductModel model)
        {
            
            if (!ModelState.IsValid)
            {
                List<Category> categories = db.Categories.ToList().Where(x => x.ParentCategory == null).ToList();
                CreateProductModel createProductModel = new CreateProductModel(GetProviders(), categories);
                
                return View("Create",createProductModel);
            }
            //Fetch Attributes with model category id
            CreateProceedProductModel proceedProductModel = new CreateProceedProductModel();
            proceedProductModel.Attributes = GetAttributes(model.CategoryId);
            proceedProductModel.CargoCompanyId = model.CargoCompanyId;
            proceedProductModel.CategoryId = model.CategoryId;            
            return View(proceedProductModel);
        }
        // POST: Products/CreateProceed
        [HttpPost]
        public ActionResult CreateProceed(CreateProceedProductModel model)
        {
            
            if (!ModelState.IsValid)
            {
                model.Attributes = GetAttributes(model.CategoryId);
                return View(model);
            }
            if(model.Product.SalePrice> model.Product.ListPrice)
            {
                ModelState.AddModelError("Product.SalePrice", "Satış fiyatı liste fiyatından büyük olamaz");
                model.Attributes = GetAttributes(model.CategoryId);
                return View(model);
            }
            if(model.Product.VatRate!=0&& model.Product.VatRate != 1 && model.Product.VatRate != 8 && model.Product.VatRate != 18)
            {
                ModelState.AddModelError("Product.VatRate", "Ürün KDV oranı 0,1,8,18 gibi olmalı");
                model.Attributes = GetAttributes(model.CategoryId);
                return View(model);
            }
            Product product = model.Product;
            product.Category = db.Categories.Find(model.CategoryId);
            product.images = new List<Image>();
            //Create images
            foreach (var picture in product.images)
            {
                if (string.IsNullOrEmpty(picture.ImageUrl))
                {
                    picture.Product = product;
                    product.images.Add(picture);
                }   
            }
            //Create Attributes
            product.Attributes = new List<Models.Attribute>();
            List<CategoryAttribute> categoryAttributes = GetAttributes(product.Category.CategoryId);
            foreach (var item in model.Attributes)
            {
                Models.Attribute attribute = new Models.Attribute();
                attribute.AttributeCode = item.Id;
                attribute.AttributeName = item.Name;
                if (item.AllowCustom&&!string.IsNullOrEmpty(item.SelectedValue))
                {
                    
                    attribute.AttributeValue = item.SelectedValue;
                    attribute.AttributeValueId = -1;
                }
                else if(!item.AllowCustom&& item.SelectedId != 0)
                {
                    string value = categoryAttributes.Find(x => x.Id == item.Id).Values.Find(x => x.Id == item.SelectedId).Value;
                    attribute.AttributeValue = value;
                    attribute.AttributeValueId = item.SelectedId;
                    attribute.Category = product.Category;
                }
                if(attribute.AttributeValueId!=0) product.Attributes.Add(attribute);
            }
            try
            {
                //Find if existing barcode code exist
                Product oldprouct = db.Products.ToList().Find(x => x.Barcode == product.Barcode);
                if (oldprouct == null)
                {   
                     //Create Product
                     IRestResponse res = apiHelper.CreateProduct(product);
                     if (res.StatusCode == System.Net.HttpStatusCode.OK)
                     {
                         //Check if request is properly handled
                         product.batchRequestId = JObject.Parse(res.Content)["batchRequestId"].ToString();
                         apiHelper.setStatus(product);
                        if (product.Status != "FAILED")
                        {
                            db.Products.Add(product);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Error = "Ürün oluşturulamıyor";
                            model.Attributes = GetAttributes(model.CategoryId);
                            return View(model);
                        }
                        
                     }
                     else
                     {
                        throw new Exception("API Error");
                     }
                }
                else
                {
                    ModelState.AddModelError("Product.Barcode", "Sistemde aynı barkodlu ürün bulunmaktadır");
                    model.Attributes = GetAttributes(model.CategoryId);
                    return View(model);
                }
               
            }
            catch (Exception e)
            {
                CreateProceedProductModel proceedProductModel = model;
                proceedProductModel.Attributes = GetAttributes(model.CategoryId);
                ViewBag.Error = "Ürün oluşturulamıyor. Lütfen daha sonra yeniden deneyiniz";
                return View(proceedProductModel);
            }
            return RedirectToAction("Index");
        }

        //POST : Products/UpdateStocksAndPrice
        [HttpPost]
        public ActionResult UpdateStocksAndPrice(Product product)
        {
            //Get original product
            Product oldproduct = db.Products.Include("Attributes").Include("images").ToList().Find(x => x.ProductId == product.ProductId);
            if (oldproduct == null) return new HttpNotFoundResult("Product Not Found");
            if (product.SalePrice > product.ListPrice)
            {
                ModelState.AddModelError("Product.SalePrice", "Satış fiyatı liste fiyatından büyük olamaz");
                return View("Details",oldproduct);
            }
            try
            {   
                //Post the data
                IRestResponse response = apiHelper.UpdateStocksAndPrice(product);
                if (response.StatusCode == HttpStatusCode.OK)
                {   
                    //Change data
                    oldproduct.Quantity = product.Quantity;
                    oldproduct.SalePrice = product.SalePrice;
                    oldproduct.ListPrice = product.ListPrice;
                    oldproduct.batchRequestId = JObject.Parse(response.Content)["batchRequestId"].ToString();
                    db.SaveChanges();
                    return View("Details",oldproduct );
                }
                else throw new Exception("Server Error");

            }
            catch (Exception e)
            {
            }
                ViewBag.Error = "Ürün Güncellenemiyor. Daha sonra tekrar deneyiniz";

            return View("Details",oldproduct);
        }
        // GET: Products/Brands/?name
        public ActionResult Brands(string name)
        {
            //Fetch Brands by name
            try
            {
                List<Brand> brands = apiHelper.GetBrands(name);
                int index = brands.Count;
                if (index == 0) return null;
                if (index > 5) index = 5;
                return PartialView(brands.GetRange(0, index));
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<CategoryAttribute> GetAttributes(int categoryId)
        {
            //Get attributes by categoryID
            try
            {
                return apiHelper.GetAttributes(categoryId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<Provider> GetProviders()
        {
            try
            {
                return apiHelper.GetProviders();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private List<SupplierAdress> GetAddresses()
        {
            try
            {
                return apiHelper.GetAddresses();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //Create Products from API
        private void GetProducts()
        {
            List<Product> newProducts = apiHelper.GetProducts();
            foreach (var product in newProducts)
            {
                Product oldProduct = db.Products.ToList().Find(x=> x.Barcode == product.Barcode);
                if(oldProduct == null)
                {
                    product.Category = db.Categories.ToList().Find(x=> x.CategoryName == product.Category.CategoryName);
                    foreach (var image in product.images)
                    {
                        db.Images.Add(image);
                    }
                    foreach (var attribute in product.Attributes)
                    {
                        attribute.Category = product.Category;
                    }
                    db.Products.Add(product);
                }
                else
                {
                    //If statust is not found check batch status
                    if (string.IsNullOrEmpty(product.Status))
                    {
                      
                        apiHelper.setStatus(oldProduct);
                        oldProduct.Quantity = product.Quantity;
                    }
                    else
                    {
                        oldProduct.Status = product.Status;
                        oldProduct.StatusDescription = product.StatusDescription;
                        oldProduct.Quantity = product.Quantity;
                    }
                }
            }
            db.SaveChanges();
        }
       
    }
}
