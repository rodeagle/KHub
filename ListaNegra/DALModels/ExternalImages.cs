using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ListaNegra.DALModels
{
    [Table("ExternalImages", Schema = "dbo")]
    public class ExternalImages
    {
        [Key]
        [Column("ExternalImageID")]
        public int ExternalImageID { get; set; }

        [ForeignKey("FK__ExternalI__PostI")]
        [Column("PostID")]
        public int PostID { get; set; }

        [Column("Tag")]
        public string ImageUrl { get; set; }
    }
}