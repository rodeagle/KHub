using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ListaNegra.DALModels
{
    [Table("Posts", Schema = "dbo")]
    public class Posts
    {
        [Key]
        [Column("PostID")]
        public int PostID { get; set; }

        [ForeignKey("FK__Posts__UserID")]
        [Column("UserID")]
        public Guid UserID { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("SearchTag")]
        public string SearchTag { get; set; }

        [Column("PostInfo")]
        public string PostInfo { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }

        [Column("Latitude")]
        public decimal Latitude { get; set; }

        [Column("Longitude")]
        public decimal Longitude { get; set; }
    }
}