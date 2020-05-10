using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ListaNegra.DALModels;
using ListaNegra.Repository;
using ListaNegra.BL;
using ListaNegra.Models;
using ListaNegra.DAL;
using System.Threading.Tasks;

namespace ListaNegra.BL
{
    public class UserBL : IUserBL
    {
        private IUserDAL _userService;

        public UserBL(IUserDAL userService) => _userService = userService;

        public async Task<bool> CreatePost(Guid userId, ListItemModel model)
        {
            return await _userService.CreatePost(userId, model);
        }

        public async Task<PostModel> GetPost(int postId)
        {
            try
            {
                var post = await _userService.GetPost(postId);

                var images = await _userService.GetPostImages(postId);
                
                var  tags = await _userService.GetPostTags(postId);

                return new PostModel() {
                    PostID = post.PostID,
                    Description = post.PostInfo,
                    Latitude = post.Latitude,
                    Longitude = post.Longitude,
                    Phone = post.Phone,
                    Title = post.Title,
                    Images = images.Select(x => x.ImageUrl).ToArray(),
                    Tags = tags.Select(x => x.Tag).ToArray()
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}