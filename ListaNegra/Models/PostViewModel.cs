using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaNegra.Models
{
    public class PostModel
    {
        public string Title { get; set; }

        public string ListName { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }

        public string Description { get; set; }

        public string[] Tags { get; set; }

        public string[] Images { get; set; }

        public string Phone { get; set; }
    }
}
