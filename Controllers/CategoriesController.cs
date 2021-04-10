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
using Trendyol_Integration.Util;

namespace Trendyol_Integration.Controllers
{
    public class CategoriesController : Controller
    {

        IntegrationContext db = new IntegrationContext();
        ApiHelper apiHelper = new ApiHelper();
        public ActionResult GetCategories()
        {
            string response =   apiHelper.GetCategories();
            if (!string.IsNullOrEmpty(response))
            {
                JObject responseJSON = JObject.Parse(response);
                JArray categoryList = (JArray)responseJSON["categories"];
                foreach (var res in categoryList)
                {
                    TraverseTree((JObject)res, null);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Create","Products");
        }
        public ActionResult GetSubCategories(int id,int? index)
        {
            ViewBag.index = index;
            Category category = db.Categories.Find(id);
            List<Category> categories= category.SubCategories.ToList();
            if (categories.Count == 0) return null;
            return PartialView(categories);
        }
        private Category TraverseTree(JObject node, Category parent)
        {
            
            Category category = new Category();
            category.CategoryId = (int)node["id"];
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