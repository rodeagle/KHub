using KHub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KHub.DAL
{
    public interface IUserDAL
    {
        //int GetUser();
        //int UserName();

        //Task<bool> CreatePost(Guid userId, ListItemModel model);

        //Task<Posts> GetPost(int postId); 
        //Task<List<PostTags>> GetPostTags(int postId); 
        //Task<List<ExternalImages>> GetPostImages(int postId); 

        // validated methods

        Task<bool> UpdateUsers(UserBin users);
        Task<UserBin> GetUsers();
        Task<bool> UpdateProjects(ProjectBin projects);
        Task<string> AddNewPostDetail(PostDetail detail, string name);
        Task<bool> UpdatePosts(PostsBin posts);
        Task<ProjectBin> GetProjects();
        Task<PostsBin> GetPosts();
        Task<PostDetail> GetPostDetail(string postBinId);
    }
}