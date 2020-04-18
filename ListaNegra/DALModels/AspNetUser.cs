using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ListaNegra.DALModels
{
    [Table("AspNetUsers", Schema = "dbo")]
    public class AspNetUser
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("UserName")]
        public int UserName { get; set; }
    }
}