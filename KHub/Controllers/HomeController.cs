using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KHub.Models;
using KHub.BL;
using KHub.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace KHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserBL _userBL;
        private readonly IWebHostEnvironment _env;
        private readonly IFileProvider dist;
        public HomeController(ILogger<HomeController> logger, IUserBL userbl, IWebHostEnvironment env, IFileProvider fileProvider)
        {
            _logger = logger;
            _userBL = userbl;
            _env = env;
            dist = fileProvider;
        }

        public string GetBundle()
        {
            var files = dist.GetDirectoryContents("wwwroot//App//dist");
            return "App//dist//" + files.First(x => x.Name.Contains("bundle")).Name;

        }

        public async Task<IActionResult> Index()
        {

            ViewData["Bundle"] = GetBundle();

            var posts = await _userBL.GetMostRecentPosts();

            return View(posts);

        }

        public class SignInModel {
            public string alias { get; set; }
            public string password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]SignInModel model)
        {
            try
            {
                var res = await _userBL.SignIn(model.alias, model.password);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody]SignInModel model)
        {
            try
            {
                var res = await _userBL.CreateAccount(model.alias, model.password);
                return Json(new { success = res });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }

        public class CreateProjectModel
        {
            public string name { get; set; }
            public bool isPublic { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody]CreateProjectModel model)
        {
            try
            {
                var res = await _userBL.CreateProject(model.name, model.isPublic);
                return Json(new { success = res });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }

        public class CreatePostModel {
            public string title { get; set; }
            public int projectid { get; set; }
            public string[] tags { get; set; }
            public string description { get; set; }
            public string code { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody]CreatePostModel model)
        {

            try
            {
                var result = await _userBL.CreatePost(model.title, model.projectid, model.tags, model.description, model.code);
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetUserProjects()
        {

            try
            {
                var userid = Identity.User.UserID;
                var result = await _userBL.GetUserProjects(userid);
                return Json(new { success = true, result });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetMostCommonTags()
        {
            try
            {

                var result = await _userBL.GetMostCommonTags();
                return Json(new { success = true, result });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
