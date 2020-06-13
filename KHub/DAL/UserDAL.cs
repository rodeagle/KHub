using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using KHub.DAL;
using KHub.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Runtime.Caching;

namespace KHub.DAL
{
   class JsonBin {
        private  const string secretKey = "$2b$10$kcvZsWjIktemeWDWQlxhWeX7tSAHcfN8QvgAeGBOI1S98yWN9v4gO";
        private  const string root = "https://api.jsonbin.io/";
        private readonly IRestClient _restClient;

        public JsonBin()
        {
            _restClient = new RestClient(root);
            _restClient.AddDefaultHeader("secret-key", secretKey);
            _restClient.AddDefaultHeader("Content-Type", "application/json");
        }

        public string Get(string bin) {

            var request = new RestRequest(bin + "/latest", Method.GET, DataFormat.Json);

            var response = _restClient.Get(request);

            return response.Content;

        }

        public bool Put(string bin, dynamic json)
        {

            var request = new RestRequest(bin, Method.PUT, DataFormat.Json);
            request.AddJsonBody(json);

            var response = _restClient.Put(request);

            return true;

        }

        public string Create(dynamic json, string collectionBinID = "", string name = "")
        {

            var request = new RestRequest("/b/", Method.POST, DataFormat.Json);

            if (collectionBinID != string.Empty) {
                request.AddHeader("collection-id", collectionBinID);
            }
            if (name != string.Empty) {
                request.AddHeader("name", name);
            }

            request.AddHeader("private", "false");

            request.AddJsonBody(json);

            var response = _restClient.Post(request);

            return response.Content;

        }
    }


    public class ProjectBin
    {

        public ProjectBin (){
            Projects = new List<Project>();
        }
        public List<Project> Projects { get; set; }

        public int TopID { get; set; }
    }

    public class Project
    {
        public Project() {
            Members = new List<int>();
            AddedPosts = new List<int>();
        }
        public int ProjectID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public bool Public { get; set; }
        public List<int> Members { get; set; }
        public List<int> AddedPosts { get; set; }

    }

    public class Post
    {
        public int ProjectID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public int PostID { get; set; }
        public string PostBinID { get; set; }
    }

    public class PostsBin
    {
        public PostsBin()
        {
            Posts = new List<Post>();
        }
        public List<Post> Posts { get; set; }
        public int TopPostID { get; set; }
    }

    public class PostDetail
    {
        public PostDetail() {
            Tags = new List<string>();
            Codes = new List<string>();
        }
        public int ProjectID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public List<string>  Codes { get; set; }
        public bool Private { get; set; }
    }



    public class UserBin
    {
        public UserBin() {
            Users = new List<User>();
        }
        public List<User> Users { get; set; }

        public int TopID { get; set; }
    }

    public class User
    {
        public User()
        {
            Favorites = new List<int>();
        }
        public int UserID { get; set; }
        public string Alias { get; set; }
        public string Hash { get; set; }
        public string Email { get; set; }

        public List<int> Favorites { get; set; }
    }


    public class UserDAL : IUserDAL
    {
        //private AppDbContext _userRepo;
        private JsonBin _server;
        public UserDAL()
        {
            _server = new JsonBin();
        }

        //public UserDAL(IRepositoryContext userRepo) => _userRepo = (AppDbContext)userRepo;
        private const string screteKey = "$2b$10$kcvZsWjIktemeWDWQlxhWeX7tSAHcfN8QvgAeGBOI1S98yWN9v4gO";
        private const string Users = "b/5ebdf896a47fdd6af16378cc";

        public struct Bins
        { 
            public const string Users = "b/5ebdf896a47fdd6af16378cc";
            public const string Projects = "b/5ec3253f18c8475bf16bdab3";
            public const string Posts = "b/5ec32e2bf7c0f67062ceefba";
            public const string PostsCollection = "5ec33060f7c0f67062cef0ec";
        }

        private static object users_lock = new object();
        private static object projects_lock = new object();
        private static object posts_lock = new object();

        // locks are only required for PUT and GET methods CREATE not at the moment


        public async Task<bool> UpdateUsers(UserBin users) {

            lock (users_lock)
            {
                dynamic json = JsonConvert.SerializeObject(users);
                _server.Put(Bins.Users, json);
                MemoryCache.Default.Set("Users", users, DateTimeOffset.Now.AddHours(6));
                return true;
            }

        }


        /// <summary>
        /// atomic method - get the userbin
        /// </summary>
        /// <returns>userbin</returns>
        public Task<UserBin> GetUsers()
        {
            return Task.Run(() => {
                lock (users_lock) {
                    var users = (UserBin)MemoryCache.Default.Get("Users");
                    if (users == null)
                    {
                        var response = _server.Get(Bins.Users);
                        users = JsonConvert.DeserializeObject<UserBin>(response);
                        MemoryCache.Default.Set("Users", users, DateTimeOffset.Now.AddHours(6));
                    }
                    return users;
                }
            });
        }

        /// <summary>
        /// atomic method - update the project bins
        /// </summary>
        /// <param name="projects"></param>
        /// <returns>bool</returns>
        public Task<bool> UpdateProjects(ProjectBin projects) 
        {

            return Task.Run(() => {
                
                lock (projects_lock) { 
                    dynamic json = JsonConvert.SerializeObject(projects);
                    _server.Put(Bins.Projects, json);
                    MemoryCache.Default.Set("Projects", projects, DateTimeOffset.Now.AddHours(6));
                    return true;
                }
    
            });

        }

        /// <summary>
        /// Get the Project bin this is a an atomic method
        /// </summary>
        /// <returns></returns>
        public Task<ProjectBin> GetProjects() {

            return Task.Run(() =>
            {
                lock (projects_lock)
                {


                    var projects = (ProjectBin)MemoryCache.Default.Get("Projects");

                    if (projects != null)
                    {

                        return projects;

                    }
                    else
                    {

                        var response = _server.Get(Bins.Projects);
                        // deserialize into a c# object
                        var _projects = JsonConvert.DeserializeObject<ProjectBin>(response);

                        MemoryCache.Default.Set("Projects", _projects, DateTimeOffset.Now.AddHours(6));

                        return _projects;

                    }
                }
            });
        }

        /// <summary>
        /// Atomic methdo to add new post detail to storage service
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="name"></param>
        public Task<string> AddNewPostDetail(PostDetail detail, string name)
        {
            return Task.Run(() => {
                return _server.Create(detail, Bins.PostsCollection, name);
            });
        }

        /// <summary>
        /// Reduced to its atomic nature
        /// </summary>
        /// <param name="json"></param>
        /// <returns>bool</returns>
        public Task<bool> UpdatePosts(PostsBin posts) {
            return Task.Run(()=>{
                lock (posts_lock) {
                    dynamic json = JsonConvert.SerializeObject(posts);
                    _server.Put(Bins.Posts, json);
                    MemoryCache.Default.Set("Posts", posts, DateTimeOffset.Now.AddHours(6));
                    return true;
                }
            });
        }

        /// <summary>
        ///  this is an atomic method locked to posts
        /// </summary>
        /// <returns>PostsBin</returns>
        public Task<PostsBin> GetPosts() {
            return Task.Run(() => {

                lock (posts_lock) {

                    var posts = (PostsBin)MemoryCache.Default.Get("Posts");

                    if (posts != null)
                        return posts;

                    var response = _server.Get(Bins.Posts);

                    var postbin = JsonConvert.DeserializeObject<PostsBin>(response);
                    MemoryCache.Default.Set("Posts", postbin, DateTimeOffset.Now.AddHours(6));
                    return postbin;
                }

            });
        }

        public Task<PostDetail> GetPostDetail(string postDetailId) {

            return Task.Run(() => {

                var key = $"$PostDetail_{postDetailId}";
                var postDetail = (PostDetail)MemoryCache.Default.Get(key);

                if (postDetail != null)
                    return postDetail;

                var response = _server.Get($"b/{postDetailId}");

                postDetail = JsonConvert.DeserializeObject<PostDetail>(response);
                MemoryCache.Default.Set(key, postDetail, DateTimeOffset.Now.AddMinutes(5));
                return postDetail;

            });

        }
    }
}