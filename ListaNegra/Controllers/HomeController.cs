using ListaNegra.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ListaNegra.Controllers
{
    public class HomeController : Controller
    {

        private IUserDAL _userService;

        public HomeController(IUserDAL userService) {
            _userService = userService;
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}