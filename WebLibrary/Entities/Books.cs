namespace WebLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Books
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int? Price { get; set; }

        public int? AuthorId { get; set; }

        public int? Pages { get; set; }

        public int? GenreId { get; set; }
    }
}
