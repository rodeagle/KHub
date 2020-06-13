using KHub.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHub.ViewModels
{
    public class IndexHomeViewModel
    {

        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Project> UserProjects { get; set; }

    }

    public class PostModel{

        public Post Post;

        public bool IsFavorite { get; set; }

        public PostModel(Post post) {
            Post = post;
        }
            
    }
}
