using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ListaNegra.Models
{
    public class ListItemModel
    {
        [Required]
        public string Title { get; set; }

        // name of the cheat 
        [Required]
        public string ListName { get; set; }
        [Required]
        public int Latitude { get; set; }
        [Required]
        public int Longitude { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string[] Tags { get; set; }
        [Required]
        public string[] Images { get; set; }

        public string Phone { get; set; }
    }
}
