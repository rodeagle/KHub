using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace KHub.Controllers
{
    public class BaseController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileProvider dist;
        public BaseController(IWebHostEnvironment env, IFileProvider fileProvider)
        {
            _env = env;
            dist = fileProvider;
        }
    

        public string GetBundle()
        {

            var files = dist.GetDirectoryContents("wwwroot//App//dist");
            return "App//dist//" + files.First(x => x.Name.Contains("bundle")).Name;

        }
    }
}