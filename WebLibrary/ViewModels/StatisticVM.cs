using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLibrary.ViewModels
{
    public class StatisticVM
    {
        public int Id { get; set; }
        public int CountAuthorChoice { get; set; }
        public int CountTitleChoice { get; set; }
        public int CountGenreChoice { get; set; }
        public int CountIsImageChoice { get; set; }
    }
}