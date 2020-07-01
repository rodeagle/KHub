using KHub.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHub.ViewModels
{
    public class ProjectDetailViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<UserViewModel> Members { get; set; }
        public IEnumerable<UserViewModel> AllUsers { get; set; }
    }

    public class UserViewModel {
        public string Alias { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
    }
}
