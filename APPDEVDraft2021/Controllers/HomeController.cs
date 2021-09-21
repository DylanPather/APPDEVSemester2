using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APPDEVDraft2021.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "About.";

            return View();
        }
        public ActionResult ContactUs()
        {  

            return View();
        }
        public ActionResult Services()
        {
            ViewBag.Message = "Services";

            return View();
        }
       
    }
}