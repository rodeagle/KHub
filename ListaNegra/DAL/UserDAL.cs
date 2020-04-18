using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ListaNegra.DALModels;
using ListaNegra.Repository;

namespace ListaNegra.DAL
{
    public class UserDAL : IUserDAL
    {
        private AppDbContext _userRepo;

        public UserDAL(IRepositoryContext userRepo) => _userRepo = (AppDbContext)userRepo;

        public int GetUser() {
            return 1;
        }

        public int UserName()
        {
            return _userRepo.AspNetUser.Count();
        }
    }
}