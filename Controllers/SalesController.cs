using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Trendyol_Integration.Dal;
using Trendyol_Integration.Models;
using Trendyol_Integration.Util;

namespace Trendyol_Integration.Controllers
{
    [HandleError]
    public class SalesController : Controller
    {
        IntegrationContext db = new IntegrationContext();
        ApiHelper apiHelper = new ApiHelper();
        // GET: Sales
        public ActionResult Index()
        {

            List<Sale> sales = getSales();
            if (sales == null) return RedirectToAction("Index", "Products");
            return View(sales);
        }
        //Get sales list from API
        private List<Sale> getSales()
        {
            try
            {
                string res = apiHelper.GetSales();
                JObject response = JObject.Parse(res);
                JArray responseList = (JArray)response["content"];
                List<Sale> sales = new List<Sale>();
                foreach (var sale in responseList)
                {
                    Sale newsale = new Sale(sale["shipmentAddress"]["fullAddress"].ToString(), sale["invoiceAddress"]["fullAddress"].ToString(),
                         (decimal)sale["totalDiscount"], (decimal)sale["totalPrice"], sale["customerFirstName"].ToString(), sale["customerLastName"].ToString(), sale["customerEmail"].ToString(),
                         sale["cargoTrackingNumber"].ToString(), sale["cargoTrackingLink"].ToString(), sale["cargoSenderNumber"].ToString(), sale["cargoProviderName"].ToString(),
                         ConvertFromUnixTimestamp((int)sale["orderDate"]), sale["tcIdentityNumber"].ToString(), sale["shipmentPackageStatus"].ToString(),
                         ConvertFromUnixTimestamp((int)sale["estimatedDeliveryEndDate"]), ConvertFromUnixTimestamp((int)sale["estimatedDeliveryStartDate"])
                        );
                    JArray linesJSON = (JArray)response["lines"];
                    foreach (var line in linesJSON)
                    {
                        Line newline = new Line((int)line["quantity"], line["productName"].ToString(), (decimal)line["amount"], (decimal)line["discount"],
                            (decimal)line["price"], (int)line["vatBaseAmount"]);
                        Product product = db.Products.ToList().Find(x => x.Barcode == line["barcode"].ToString());
                        if (product != null)
                        {
                            newline.Product = product;
                        }
                        newsale.Lines.Add(newline);
                    }
                    sales.Add(newsale);
                }
                return sales;
            }
            catch (Exception)
            {

                return null;
            }
           
        }
        private DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
   
}