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

        [ForeignKey("Authors")]
        public int? AuthorsId { get; set; }
        virtual public Authors Authors { get; set; }

        public int? Pages { get; set; }

        [ForeignKey("Genres")]
        public int? GenresId { get; set; }
        virtual public Genres Genres { get; set; }

        [ForeignKey("Images")]
        public int ImageId { get; set; }
        virtual public Image Images { get; set;}
    }
}
