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
            try
            {
                return await _userService.CreatePost(userId, model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}