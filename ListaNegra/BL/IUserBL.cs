using ListaNegra.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ListaNegra.BL
{
    public interface IUserBL
    {
        Task<bool> CreatePost(Guid userId, ListItemModel model);
        Task<PostModel> GetPost(int postId);

    }
}