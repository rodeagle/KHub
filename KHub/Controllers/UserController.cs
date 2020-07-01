using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace KHub.Controllers
{
    public class UserController : BaseController
    {

        public UserController(IWebHostEnvironment env, IFileProvider fileProvider)
        : base(env, fileProvider) { }
        public IActionResult Index()
        {
            return View();
        }


        [Route("reset-password")]
        public IActionResult ResetPassword()
        {
            ViewData["Bundle"] = GetBundle();
            return View();
        }

        [HttpPost]
        public IActionResult ResetPasswordPost(string email) {
            return null;
        }

    }
}