using ListaNegra.DAL;
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

        Task<bool> CreateAccount(string alias, string pass);

        Task<bool> SignIn(string alias, string pass);

        Task<bool> CreateProject(string name, bool isPublic);

        Task<bool> CreatePost(string title, int projectid, string[] tags, string description, string data);

        Task<User> ValidateUser(string alias, string pass);

        Task<IEnumerable<Project>> GetUserProjects(int userid);

    }
}