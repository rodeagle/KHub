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
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserBL _userBL;
        public HomeController(ILogger<HomeController> logger, IUserBL userbl, IWebHostEnvironment env, IFileProvider fileProvider)
            : base(env, fileProvider)
        {
            _logger = logger;
            _userBL = userbl;
        }

        public async Task<IActionResult> Index(int projectid = 0, bool favorite = false, bool myposts = false, string search = "")
        {
            ViewData["Bundle"] = GetBundle();
            var posts = await _userBL.GetPostsFiltered(projectid, search, favorite, myposts);

            return View(posts);
        }

        [HttpGet]
        [Route("~/post")]
        public async Task<IActionResult> Post([FromQuery]int postid) {

            ViewData["Bundle"] = GetBundle();
            var post = await _userBL.GetPostDetail(postid);
            return View(post);

        }

        [HttpGet]
        [Route("~/edit")]
        public async Task<IActionResult> PostEdit([FromQuery]int postid)
        {

            ViewData["Bundle"] = GetBundle();
            var post = await _userBL.GetPostDetail(postid);
            return View(post);

        }

        [HttpGet]
        [Route("~/projects")]
        public async Task<IActionResult> Projects()
        {

            ViewData["Bundle"] = GetBundle();
            var projects = await _userBL.GetAllProjects();
            return View(projects);

        }

        [HttpGet]
        [Route("~/project")]
        public async Task<IActionResult> ProjectDetail([FromQuery]int projectid)
        {

            ViewData["Bundle"] = GetBundle();
            var model = await _userBL.GetUserProject(projectid);
            return View(model);

        }


        public class SignInModel {
            public string alias { get; set; }
            public string email { get; set; }
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
                var res = await _userBL.CreateAccount(model.alias, model.email, model.password);
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
            public string[] codes { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody]CreatePostModel model)
        {

            try
            {
                var result = await _userBL.CreatePost(model.title, model.projectid, model.tags, model.description, model.codes);
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

        public class AddPostToFavoritesModal {
            public int postid { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> AddPostToFavorites([FromBody] AddPostToFavoritesModal model)
        {
            try
            {

                var result = await _userBL.AddPostToFavorites(Identity.User.UserID, model.postid);
                return Json(new { success = true, result });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class AddPostToProjectModel
        {
            public int postid { get; set; }
            public int projectid { get; set; }

        }


        [HttpPost]
        public async Task<ActionResult> AddPostToProject([FromBody] AddPostToProjectModel model)
        {
            try
            {

                var result = await _userBL.AddPostToProject(model.postid, model.projectid);
                return Json(new { success = true, result });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class UpdateProjectViewModel{
            public int ProjectID { get; set; }
            public bool UsersEdited { get; set; }
            public int[] MemberUserIDs { get; set; }
            public string Color { get; set; }
            public string Icon { get; set; }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProject(UpdateProjectViewModel model)
        {
            try
            {
                var result = await _userBL.UpdateProject(model);
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
