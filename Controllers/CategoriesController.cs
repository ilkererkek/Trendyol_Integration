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
    [HandleError]
    public class CategoriesController : Controller
    {

        IntegrationContext db = new IntegrationContext();
        ApiHelper apiHelper = new ApiHelper();
        // GET: Categories/GetCategories
        public ActionResult GetCategories()
        {
            //Fetch data
            string response =   apiHelper.GetCategories();
            if (!string.IsNullOrEmpty(response))
            {
                JObject responseJSON = JObject.Parse(response);
                JArray categoryList = (JArray)responseJSON["categories"];
                //Traverse tree from roots
                foreach (var res in categoryList)
                {
                    TraverseTree((JObject)res, null);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return RedirectToAction ("Index","Products");
        }
        // GET: Categories/GetSubCategoriers/id?index
        public ActionResult GetSubCategories(int id,int? index)
        {
            ViewBag.index = index;
            Category category = db.Categories.Find(id);
            List<Category> categories= category.SubCategories.ToList();
            if (categories.Count == 0) return null;
            return PartialView(categories);
        }
        //Post Order Traversal of Category Tree
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