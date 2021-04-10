using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trendyol_Integration.Dal;
using Trendyol_Integration.Models;
using Trendyol_Integration.Models.JSONModels;
using Trendyol_Integration.Util;
using Trendyol_Integration.ViewModels;

namespace Trendyol_Integration.Controllers
{
   
    public class ProductsController : Controller
    {
        IntegrationContext db = new IntegrationContext();
        ApiHelper apiHelper = new ApiHelper();
        // GET: Products
        public ActionResult Index()
        {
            try
            {
                GetProducts();
            }
            catch (Exception)
            {
                ViewBag.Error = "<div class='alert alert-danger' role='alert'>Trendyol'a Erişilemiyor</div>";
            }
            List<Product> products = db.Products.ToList();
            return View(products);
        }
        public ActionResult Details(int id)
        {

            Product product = db.Products.Find(id);
            return View(product);
        }


        // GET: Products/Create
        public ActionResult Create()
        {

            List<Category> categories = db.Categories.ToList().Where(x => x.ParentCategory == null).ToList();
            CreateProductModel createProductModel = new CreateProductModel(GetProviders(),categories);
            return View(createProductModel);
        }
        public ActionResult CreateProceed(CreateProductModel model)
        {
            
            if (!ModelState.IsValid)
            {
                List<Category> categories = db.Categories.ToList().Where(x => x.ParentCategory == null).ToList();
                CreateProductModel createProductModel = new CreateProductModel(GetProviders(), categories);
                
                return View("Create",createProductModel);
            }
            CreateProceedProductModel proceedProductModel = new CreateProceedProductModel();
            proceedProductModel.Attributes = GetAttributes(model.CategoryId);
            proceedProductModel.CargoCompanyId = model.CargoCompanyId;
            proceedProductModel.CategoryId = model.CategoryId;            
            return View(proceedProductModel);
        }
        [HttpPost]
        public ActionResult CreateProceed(CreateProceedProductModel model)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                model.Attributes = GetAttributes(model.CategoryId);
                return View(model);
            }
            Product product = model.Product;
            product.Category = db.Categories.Find(model.CategoryId);
            product.images = model.images;
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
                Product oldprouct = db.Products.ToList().Find(x => x.StockCode == product.StockCode);
                if (oldprouct == null)
                {
                     IRestResponse res = apiHelper.CreateProduct(product);
                     if (res.StatusCode == System.Net.HttpStatusCode.OK)
                     {
                         product.batchRequestId = JObject.Parse(res.Content)["batchRequestId"].ToString();
                          db.Products.Add(product);
                          db.SaveChanges();
                     }
                     else
                     {
                        throw new Exception("API Error");
                     }
                }
                else
                {
                    ModelState.AddModelError("Product.StockCode", "Sistemde aynı stok kodlu ürün bulunmaktadır");
                    model.Attributes = GetAttributes(model.CategoryId);
                    return View(model);
                }
               
            }
            catch (Exception)
            {
                CreateProceedProductModel proceedProductModel = new CreateProceedProductModel();
                proceedProductModel.Attributes = GetAttributes(model.CategoryId);
                ViewBag.Error = "Ürün oluşturulamıyor. Lütfen daha sonra yeniden deneyiniz";
                return View(proceedProductModel);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Brands(string name)
        {

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
        private void GetProducts()
        {
            List<Product> newProducts = apiHelper.GetProducts();
            foreach (var product in newProducts)
            {
                Product oldProduct = db.Products.ToList().Find(x=> x.StockCode == product.StockCode);
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
            }
            db.SaveChanges();
        }
       
    }
}
