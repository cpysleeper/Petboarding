using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppTemplate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "OwnerDashboard");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "The application description page.";

            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contact page.";

            return View();
        }

        public ActionResult GitBasics()
        {
            return Content("Git Basics");
        }

        public ActionResult EndpointA()
        {
            return Content("This is endpoint A");
        }
    }
}