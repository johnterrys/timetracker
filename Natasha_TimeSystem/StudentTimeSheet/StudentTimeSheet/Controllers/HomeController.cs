using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTimeSheet.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create() {
            ViewBag.Message = "Your CreateAccount page.";

            return View();
        }

        public ActionResult UserDashboard() {
            ViewBag.Message = "Your UserDashboard page.";

            return View();
        }


        public ActionResult UserProfile() {
            ViewBag.Message = "Your User Profile page.";

            return View();
        }



     
    }
}