using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHub.ViewModels
{
    public class PostDetailViewModel
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Codes { get; set; }
        public string Author { get; set; }
        public int PostID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
    }
}
