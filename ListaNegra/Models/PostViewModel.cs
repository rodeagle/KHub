using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaNegra.Models
{
    public class PostModel
    {
        public int PostID { get; set; }
        public string Title { get; set; }

        public string ListName { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Description { get; set; }

        public string[] Tags { get; set; }

        public string[] Images { get; set; }

        public string Phone { get; set; }
    }
}
