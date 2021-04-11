using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trendyol_Integration.Models;
using Trendyol_Integration.Models.JSONModels;
using Trendyol_Integration.ViewModels;

namespace Trendyol_Integration.Util
{

    //Object that manages operations with trendyol api
    public class ApiHelper
    {
        private RestHelper restHelper;
        static long LastListCall = 0;
        static long LastAPICall = 0;
        public ApiHelper()
        {
            restHelper = new RestHelper();
        }

        public IRestResponse CreateProduct(Product product)
        {
            string url = "suppliers/" + 235333 + "/v2/products";
            List<ProductJSONModel> items = new List<ProductJSONModel>();
            items.Add(new ProductJSONModel(product));
            return restHelper.PostRequest(url, JsonConvert.SerializeObject(new { items}));
        }
        public List<Brand> GetBrands(string name)
        {
            string url = "suppliers/" + 235333 + "/v2/products";
            string response = restHelper.GetRequest("brands/by-name?name=" + name + "&size=3");
            JArray responseJSON = JArray.Parse(response);
            List<Brand> brands = new List<Brand>();
            foreach (var brandJSON in responseJSON)
            {
                Brand brand = new Brand();
                brand.Id = (int)brandJSON["id"];
                brand.Name = brandJSON["name"].ToString();
                brands.Add(brand);
            }
            return brands;
        }
        public List<CategoryAttribute> GetAttributes(int id)
        {
            string response = restHelper.GetRequest("product-categories/" + id + "/attributes");
            JObject responseJSON = JObject.Parse(response);
            JArray responseArray = (JArray)responseJSON["categoryAttributes"];
            List<CategoryAttribute> attributes = new List<CategoryAttribute>();
            foreach (var item in responseArray)
            {
                CategoryAttribute categoryAttribute = new CategoryAttribute((int)item["attribute"]["id"], item["attribute"]["name"].ToString(), (bool)item["required"], (bool)item["allowCustom"]);
                JArray valuesJSON = (JArray)item["attributeValues"];
                foreach (var valueJSON in valuesJSON)
                {
                    AttributeValue value = new AttributeValue((int)valueJSON["id"], valueJSON["name"].ToString());
                    categoryAttribute.Values.Add(value);
                }
                attributes.Add(categoryAttribute);
            }
            return attributes;
        }
        public List<Provider> GetProviders()
        {
            var response = restHelper.GetRequest("shipment-providers");
            JArray responseJSON = JArray.Parse(response);
            List<Provider> providers = new List<Provider>();
            foreach (var res in responseJSON)
            {
                Provider current = new Provider(int.Parse(res["id"].ToString()), res["name"].ToString());
                providers.Add(current);
            }
            return providers;
        }
        public List<SupplierAdress> GetAddresses()
        {
            string response = restHelper.GetRequest("suppliers/235333/addresses");
            JObject responseJSON = JObject.Parse(response);
            JArray responseArray = (JArray)responseJSON["supplierAddresses"];
            List<SupplierAdress> addresses = new List<SupplierAdress>();
            foreach (var res in responseArray)
            {
                SupplierAdress address = new SupplierAdress((int)res["id"], res["city"].ToString(), res["district"].ToString());
                addresses.Add(address);
            }
            return addresses;
        }
        public List<Product> GetProducts()
        {
            long currenTimeStamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            List<Product> products = new List<Product>();
            if (true)
            {
                //Create product from JSON
                string response = restHelper.GetRequest("suppliers/235333/products?size=500");
                JObject responseJSON = JObject.Parse(response);
                JArray responseArray = (JArray)responseJSON["content"];
                LastAPICall = currenTimeStamp;
                foreach (var res in responseArray)
                {
                    Product newProduct = new Product(res["barcode"].ToString(), res["title"].ToString(),
                        res["productMainId"].ToString(),(int)res["brandId"], res["brand"].ToString(), (int)res["quantity"], res["stockCode"].ToString(),
                        (decimal)res["dimensionalWeight"], res["description"].ToString(), (decimal)res["listPrice"], (decimal)res["salePrice"], -1,
                        (int)res["vatRate"]);
                    JArray pics = (JArray)res["images"];
                    foreach (var picture in pics)
                    {
                        Image img = new Image();
                        String url = picture["url"].ToString();
                        img.ImageUrl = url;
                        newProduct.images.Add(img);
                    }
                    newProduct.Category = new Category();
                    newProduct.Category.CategoryName = res["categoryName"].ToString();
                    JArray attrs = (JArray)res["attributes"];
                    foreach (var attr in attrs)
                    {
                        Models.Attribute attribute = new Models.Attribute();
                        attribute.AttributeCode = (int)attr["attributeId"];
                        attribute.Category = newProduct.Category;
                        attribute.AttributeName = attr["attributeName"].ToString();
                        attribute.AttributeValue = attr["attributeValue"].ToString();
                        newProduct.Attributes.Add(attribute);
                    }
                    if ((bool)res["rejected"])
                    {
                        newProduct.Status = "Reddedildi";
                        JArray status = (JArray)res["rejectReasonDetails"];
                        newProduct.StatusDescription = status.ToString();
                    }
                    else if ((bool)res["blacklisted"])
                    {
                        newProduct.Status = "Karaliste";
                        newProduct.StatusDescription = res["blacklistReason"].ToString();
                    }
                    else if((bool)res["approved"])
                    {
                        newProduct.Status = "Onaylandı";
                        newProduct.StatusDescription = "";
                    }
                    products.Add(newProduct);
                }
            }
            LastAPICall = currenTimeStamp;
            return products;
        }
        public string GetCategories()
        {
            long currenTimeStamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            if (currenTimeStamp >= LastListCall+ 86400)
            {
               LastListCall = currenTimeStamp;
                return restHelper.GetRequest("product-categories");
            }
            else return null;
            
        }
        public string GetSales()
        {
           return restHelper.GetRequest("suppliers/235333/orders?orderByField=PackageLastModifiedDate&orderByDirection=DESC&size=100");
            
        }
        public void setStatus(Product product)
        {
            string response = restHelper.GetRequest("suppliers/235333/products/batch-requests/"+product.batchRequestId);
            JObject responseJSON = JObject.Parse(response);
            if (responseJSON["status"]!=null && responseJSON["status"].ToString() == "COMPLETED")
            {
                JArray responseArray = (JArray)responseJSON["items"];
                foreach (var item in responseArray)
                {
                    if(item["requestItem"]["barcode"].ToString() == product.Barcode)
                    {
                        product.Status = item["status"].ToString();
                        if (product.Status == "SUCCESS")
                        {
                            product.Status = "Başarılı";
                            product.StatusDescription = "";
                        }
                    }
                }
            }
            else
            {
                //If batch status is not found
                product.Status = "Bulunamadı";
            }

        }
    }
}