namespace WebLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Books
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Price { get; set; }

        public int AuthorId { get; set; }

        public int Pages { get; set; }
    }
}
