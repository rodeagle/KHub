using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KHub.BL;
using KHub.Models;
using KHub.DAL;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using KHub.ViewModels;

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
        public async Task<bool> CreateAccount(string alias, string email, string pass) {

            var res = await CreateUser(alias,email, pass);
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

        public async Task<bool> CreateUser(string alias,string email, string pass) {

            var _Users = await _userService.GetUsers();
            // deserialize into a c# object
            var key = _Users.TopID + 1;

            var fullKey = $"{alias}_{key}_{pass}";

            string hash = "";
            SHA512 alg = SHA512.Create();
            byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(fullKey));
            hash = Encoding.UTF8.GetString(result);

            _Users.Users.Add(new User()
            {
                Alias = alias,
                Email = email,
                UserID = key,
                Hash = hash
            });

            _Users.TopID += 1;

            await _userService.UpdateUsers(_Users);

            return true;
        } 


        /// <summary>
        /// cleaned using atomic methods - validate user on BL
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public async Task<User> ValidateUser(string alias, string pass) {

            var users = await _userService.GetUsers();

            string hash = "";
            SHA512 alg = SHA512.Create();

            Func<int, string> GenerateHash = (userid) => {
                var key = $"{alias}_{userid}_{pass}";
                byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(key));
                hash = Encoding.UTF8.GetString(result);
                return hash;
            };

            User res = users.Users.FirstOrDefault(x => x.Hash == GenerateHash(x.UserID));

            return res;

        }

        /// <summary>
        /// Cleaned code - use atomic methods tasks 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public async Task<bool> CreateProject(string name, bool isPublic)
        {
            var userid = Identity.User.UserID;

            // get projects 
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
        public async Task<bool> CreatePost(string title, int projectid, string[] tags, string description, string[] codes) {

            var userid = Identity.User.UserID;

            var postbin = await _userService.GetPosts();

            var newPost = new PostDetail()
            {
                UserID = userid,
                Title = title,
                ProjectID = projectid,
                Description = description,
                Codes = codes.ToList(),
                //Tags = tags.ToList()
            };

            var postDetailBinID = await _userService.AddNewPostDetail(newPost, $"PostDetail_{projectid}_{userid}");

            var _postDetailBinID = JsonConvert.DeserializeObject<dynamic>(postDetailBinID).id;

            postbin.Posts.Add(new Post()
            {
                PostBinID = _postDetailBinID,
                PostID = postbin.TopPostID + 1,
                ProjectID = projectid,
                UserID = userid,
                Title = title
            });
            postbin.TopPostID += 1;

            var res = await _userService.UpdatePosts(postbin);

            return true;

        }

        public async Task<IEnumerable<Project>> GetUserProjects(int userid)
        {
            var projects = await _userService.GetProjects();

            var userproj = projects.Projects.Where(x => x.UserID == userid);

            return userproj;

        }

        public async Task<IEnumerable<Post>> GetMostRecentPosts(){

            var posts = await _userService.GetPosts();
            var projects = await _userService.GetProjects();

            var _project = projects.Projects.Where(x => x.Public);

            var _posts = posts.Posts.Where(x => _project.Any(y => y.ProjectID == x.ProjectID));//.Where(x=> _project.Any(y=> y.ProjectID == x.ProjectID));

            return _posts;

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

        public async Task<PostDetailViewModel> GetPostDetail(int postid)
        {
            var postbin = await _userService.GetPosts();

            var post = postbin.Posts.FirstOrDefault(x => x.PostID == postid);

            var postBinId = post.PostBinID;

            var users = await _userService.GetUsers();

            var user = users.Users.FirstOrDefault(x => x.UserID == post.UserID);

            var postDetail = await _userService.GetPostDetail(postBinId);

            var projects = await _userService.GetProjects();

            var project = projects.Projects.FirstOrDefault(x => x.ProjectID == post.ProjectID);

            var model = new PostDetailViewModel()
            {
                Title = postDetail.Title,
                Description = postDetail.Description,
                Codes = postDetail.Codes,
                Author = user.Alias,
                PostID = post.PostID,
                ProjectID = post.ProjectID,
                ProjectName = project.Name
            };

            return model;
        }

        public async Task<IEnumerable<Project>> GetAllProjects() {

            var projects = await _userService.GetProjects();

            return projects.Projects.AsEnumerable();

        }

        public async Task<IndexHomeViewModel> GetPostsFiltered(int pid, string search, bool favorites, bool myposts)
        {

            var posts = await _userService.GetPosts();
            var projects = await _userService.GetProjects();
            var customerid = Identity.IsSignedIn ? Identity.User.UserID : 0;
            var userProjects = Enumerable.Empty<Project>();
            if (customerid > 0) { 
                userProjects = projects.Projects.Where(x => x.UserID == customerid);
            }
            // show only favorites
            if (favorites)
            {
                var users = Identity.User.Favorites;
                var _posts = posts.Posts.Where(x=> users.Contains(x.PostID));

                return new IndexHomeViewModel()
                {
                    Title = "your Favorite Posts",
                    Posts = _posts,
                    UserProjects = userProjects
                };

            }

            // most recent
            if (pid == 0 && search == string.Empty)
            {

                var _project = projects.Projects.Where(x => x.Public);

                var _posts = posts.Posts.Where(x => _project.Any(y => y.ProjectID == x.ProjectID));

                return new IndexHomeViewModel()
                {
                    Title = "Most Recent Contributions",
                    Posts = _posts,
                    UserProjects = userProjects
                };
            }
            // filter by project
            if (pid > 0)
            {

                var project = projects.Projects.FirstOrDefault(x => x.ProjectID == pid);

                var _posts = posts.Posts.Where(x => project.ProjectID == x.ProjectID);

                return new IndexHomeViewModel()
                {
                    Title = project.Name,
                    Posts = _posts,
                    UserProjects = userProjects
                };

            }
            if (search != string.Empty)
            {
                var _project = projects.Projects.Where(x => x.Public);

                var _posts = posts.Posts.Where(x => _project.Any(y => y.ProjectID == x.ProjectID));

                _posts = _posts.Where(x => x.Title.ToLower().IndexOf(search.ToLower()) > 0);

                return new IndexHomeViewModel()
                {
                    Title = "Search Result: ",
                    Posts = _posts,
                    UserProjects = userProjects
                };

            }
            // show only my posts
            if (customerid > 0 && myposts)
            {
                var _project = projects.Projects.Where(x => x.Public && x.UserID == customerid);

                var _posts = posts.Posts.Where(x => _project.Any(y => y.ProjectID == x.ProjectID));

                return new IndexHomeViewModel()
                {
                    Title = "My Personal Contributions",
                    Posts = _posts,
                    UserProjects = userProjects
                };
            }

            return null;
        }

        public async Task<bool> AddPostToFavorites(int userid, int postid)
        {

            var users = await _userService.GetUsers();

            users.Users.First(x => x.UserID == userid).Favorites.Add(postid);

            await _userService.UpdateUsers(users);

            return true;

        }

        public async Task<bool> AddPostToProject(int postid, int projectid) {

            var projects = await _userService.GetProjects();

            projects.Projects.First(x => x.ProjectID == projectid).AddedPosts.Add(postid);

            await _userService.UpdateProjects(projects);

            return true;
        }
    }//
}//