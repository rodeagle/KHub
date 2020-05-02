using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ListaNegra.Controllers
{
    public class MainController : Controller
    {

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Add()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}