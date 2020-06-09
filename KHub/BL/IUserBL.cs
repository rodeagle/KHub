using KHub.DAL;
using KHub.Models;
using KHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KHub.BL
{
    public interface IUserBL
    { 

        Task<bool> CreateAccount(string alias, string pass);

        Task<bool> SignIn(string alias, string pass);

        Task<bool> CreateProject(string name, bool isPublic);

        Task<bool> CreatePost(string title, int projectid, string[] tags, string description, string data);

        Task<User> ValidateUser(string alias, string pass);

        Task<IEnumerable<Project>> GetUserProjects(int userid);

        Task<IEnumerable<string>> GetMostCommonTags();

        Task<IEnumerable<Post>> GetMostRecentPosts();

        Task<PostDetailViewModel> GetPostDetail(int postBinId);

        Task<IEnumerable<Project>> GetAllProjects();

        Task<IndexHomeViewModel> GetPostsFiltered(int pid, string search, bool favorites, bool myposts);

        Task<bool> AddPostToFavorites(int userid, int postid);
    }
}