﻿using ListaNegra.DALModels;
using ListaNegra.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ListaNegra.DAL
{
    public interface IUserDAL
    {
        int GetUser();
        int UserName();

        Task<bool> CreatePost(Guid userId, ListItemModel model);

        Task<Posts> GetPost(int postId); 
    }
}