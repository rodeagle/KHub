using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KHub.DALModels;
using KHub.BL;
using KHub.Models;
using KHub.DAL;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Microsoft.AspNetCore.Http;

namespace KHub.BL
{
    public class UserBL : IUserBL
    {
        private IUserDAL _userService;
        private HttpContext _HttpContext;
        public UserBL(IUserDAL userService, IHttpContextAccessor context) {
            _userService = userService;
            _HttpContext = context.HttpContext;


        }
        public async Task<bool> SignIn(string alias, string pass)
        {
            var res = await ValidateUser(alias, pass);
            if (res != null) {
                var guid = Guid.NewGuid();
                _HttpContext.Response.Cookies
                    .Append("Credential", guid.ToString(), new CookieOptions() { Expires = DateTime.Now.AddHours(8) });
                    
                    //(new HttpCookie("Credential", guid.ToString())
                    //{
                    //    Expires = DateTime.Now.AddHours(8)
                    //});

                // if login succesfull then create a cookie for it
                MemoryCache.Default.Add(guid.ToString(), res, DateTimeOffset.Now.AddHours(8));
                return true;
            }
            throw new Exception("Invalid credentials");
        }
        public async Task<bool> CreateAccount(string alias, string pass) {

            var res = await CreateUser(alias, pass);
            if (res)
            {
                var guid = Guid.NewGuid();
                _HttpContext.Response.Cookies
                    .Append("Credential", guid.ToString(), new CookieOptions() { Expires = DateTime.Now.AddHours(8) });
                //.Add(new HttpCookie("Credential", guid.ToString())
                //{
                //    Expires = DateTime.Now.AddHours(8)
                //});
                return true;
            }
            return false;
        }

        public Task<bool> CreateUser(string alias, string pass) {

            return Task.Run(async () => {

                var _Users = await _userService.GetUsers();
                // deserialize into a c# object
                var key = _Users.TopID + 1;

                var fullKey = $"{alias}_{key}_{pass}";

                var hash = fullKey.GetHashCode();

                _Users.Users.Add(new User()
                {
                    Alias = alias,
                    UserID = key,
                    Hash = hash.ToString()
                });

                _Users.TopID += 1;

                await _userService.UpdateUsers(_Users);

                return true;

            });
        } 


        /// <summary>
        /// cleaned using atomic methods - validate user on BL
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public Task<User> ValidateUser(string alias, string pass) {

            return Task.Run(async () => {

                var users = await _userService.GetUsers();

                User res = users.Users.FirstOrDefault(x => x.Hash == $"{alias}_{x.UserID}_{pass}".GetHashCode().ToString());

                return res;

            });

        }

        /// <summary>
        /// Cleaned code - use atomic methods tasks 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public Task<bool> CreateProject(string name, bool isPublic)
        {
            var userid = Identity.User.UserID;

            // get projects 
            return Task.Run(async () => {

                var projects = await _userService.GetProjects();

                var ProjectID = projects.TopID + 1;

                projects.Projects.Add(new Project()
                {
                    Name = name,
                    ProjectID = ProjectID,
                    UserID = userid,
                    Public = isPublic
                });
                projects.TopID = ProjectID;

                //_server.Put(Bins.Projects, projects);
                await _userService.UpdateProjects(projects);

                return true;

            });
        }

        /// <summary>
        /// Creates a new post for a user and project
        /// </summary>
        /// <param name="title"></param>
        /// <param name="projectid"></param>
        /// <param name="tags"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// has been reduced and used atomic methods and removed locks
        public Task<bool> CreatePost(string title, int projectid, string[] tags, string description, string code) {

            var userid = Identity.User.UserID;

            return Task.Run(async () =>
            {

                var postbin = await _userService.GetPosts();

                var newPost = new PostDetail()
                {
                    UserID = userid,
                    Title = title,
                    ProjectID = projectid,
                    Description = description,
                    Code = code,
                    //Tags = tags.ToList()
                };

                var postDetailBinID = await _userService.AddNewPostDetail(newPost, $"PostDetail_{projectid}_{userid}");

                postbin.Posts.Add(new Post()
                {
                    PostBInID = postDetailBinID,
                    PostID = postbin.TopPostID + 1,
                    ProjectID = projectid,
                    UserID = userid,
                    Title = title
                });
                postbin.TopPostID += 1;

                var res =  await _userService.UpdatePosts(postbin);

                return true;
            });
        }

        public Task<IEnumerable<Project>> GetUserProjects(int userid)
        {
            return Task.Run(async () => {

                var projects = await _userService.GetProjects();

                var userproj = projects.Projects.Where(x => x.UserID == userid);

                return userproj;

            });

        }

        public Task<IEnumerable<string>> GetMostCommonTags()
        {
            //return Task.Run(async () => {

            //    //var projectbin = await _userService.GetProjects();

            //    //var userproj = projects.Projects.Where(x => x.UserID == userid);

            //    //return userproj;

               

            //});

            // tags will be implemented in a later state

            throw new NotImplementedException();
        }
    }
}