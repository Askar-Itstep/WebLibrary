using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebLibrary.Entities
{
    public class Statistic
    {
        [Key]
        public int Id { get; set; }
        public int CountAuthorChoice { get; set; }
        public int CountTitleChoice { get; set; }
        public int CountGenreChoice { get; set; }
        public int CountIsImageChoice { get; set; }
    }
}