using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trendyol_Integration.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            return View();
        }
    }
}