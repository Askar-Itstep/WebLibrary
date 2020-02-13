using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebLibrary.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int? Price { get; set; }
        
        public int? AuthorsId { get; set; }
        virtual public Authors Authors { get; set; }

        public int? Pages { get; set; }

        
        public int? GenresId { get; set; }
        virtual public Genres Genres { get; set; }

        
        public int? ImagesId { get; set; }
        virtual public Images Images { get; set; }
    }
}