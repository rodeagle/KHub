using ListaNegra.BL;
using ListaNegra.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ListaNegra.Controllers
{
    public class HomeController : Controller
    {

        private IUserBL _userBL;

        public HomeController(IUserBL userBL) {
            _userBL = userBL;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> SignIn(string alias, string password) 
        {
            try
            {
                var res = await _userBL.SignIn(alias, password);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }   
        }

        public async Task<ActionResult> CreateAccount(string alias, string password)
        {
            try
            {
                var res = await _userBL.CreateAccount(alias, password);
                return Json(new { success = res });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<ActionResult> CreateProject(string name, bool isPublic)
        {
            try
            {
                var res = await _userBL.CreateProject(name, isPublic);
                return Json(new { success = res });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<ActionResult> CreatePost(string title, int projectid,  string[] tags, string description, string code) {

            try {
                var result = await _userBL.CreatePost(title, projectid, tags, description, code);
                return Json(new { success = true });

            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<ActionResult> GetUserProjects()
        {

            try
            {
                var userid = ApplicationUserManager.User.UserID;
                var result = await _userBL.GetUserProjects(userid);
                return Json(new { success = true,  result });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}