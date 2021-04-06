using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Trendyol_Integration.Dal;
using Trendyol_Integration.Models;

namespace Trendyol_Integration.Controllers
{
    public class CategoriesController : Controller
    {

        IntegrationContext db = new IntegrationContext();
        // GET: Categories
        public ActionResult Index()
        {
            var categories = db.Categories.ToList().Where(x=> x.ParentCategory==null);
            return View(categories);
        }
        public ActionResult GetCategories()
        {
            var client = new RestClient("https://api.trendyol.com/sapigw");
            client.Authenticator = new HttpBasicAuthenticator("Bnib0D0RMditHE4NEiV8", "rAsrd6PpPEDiahvsZEKy");
            client.AddDefaultHeader("user-agent", "235333-PiaLab");
            var request = new RestRequest("product-categories");
            var response = client.Get(request);
            JObject responseJSON = JObject.Parse(response.Content);
            JArray responseList = (JArray)responseJSON["categories"];
            foreach (var res in responseList)
            {
                TraverseTree((JObject)res, null);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GetSubCategories(int id)
        {
            Category category = db.Categories.Find(id);
            var res = category.SubCategories.ToList().Select(item => new { CategoryId = item.CategoryId, CategoryName = item.CategoryName , leaf = (item.SubCategories.Count==0)});
            return Json(new { data = res },JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAttributes(int id)
        {
            var client = new RestClient("https://api.trendyol.com/sapigw");
            client.Authenticator = new HttpBasicAuthenticator("Bnib0D0RMditHE4NEiV8", "rAsrd6PpPEDiahvsZEKy");
            client.AddDefaultHeader("user-agent", "235333-PiaLab");
            var request = new RestRequest("product-categories/"+id+"/attributes");
            var response = client.Get(request);
            JObject responseJSON = JObject.Parse(response.Content);
            return Json(responseJSON.ToString(), JsonRequestBehavior.AllowGet);
        }
        private Category TraverseTree(JObject node, Category parent)
        {
            
            Category category = new Category();
            category.CategoryId = int.Parse(node["id"].ToString());
            Category old = db.Categories.ToList().Find(x => x.CategoryId == category.CategoryId);
            if(old == null)
            {
                category.CategoryName = node["name"].ToString();
                category.ParentCategory = parent;
                List<Category> SubCategories = new List<Category>();
                JArray childs = (JArray)node["subCategories"];
                foreach (var child in childs)
                {
                    SubCategories.Add(TraverseTree((JObject)child, category));
                }
                category.SubCategories = SubCategories;
                db.Categories.Add(category);
                return category;
            }
            return null;
        }
    }
}