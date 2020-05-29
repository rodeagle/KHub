using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KHub.DALModels
{
    [Table("PostTags", Schema = "dbo")]
    public class PostTags
    {
        [Key]
        [Column("PostTagsID")]
        public int PostTagsID { get; set; }

        [ForeignKey("FK__PostTags__PostID")]
        [Column("PostID")]
        public int PostID { get; set; }

        [Column("Tag")]
        public string Tag { get; set; }
    }
}