using ListaNegra.BL;
using ListaNegra.DAL;
using ListaNegra.Models;
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
        public IUserBL _userBL;
        public MainController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Add()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        [ActionName("Add_Post")]
        public async Task<ActionResult> Add(ListItemModel model)
        {
            try
            {

                await _userBL.CreatePost(ApplicationUserManager.UserID, model);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Post(int postId) {

            var post = _userBL.GetPost(postId);

            return View(post);
        }
    }
}