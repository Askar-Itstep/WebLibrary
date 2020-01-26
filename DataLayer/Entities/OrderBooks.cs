namespace WebLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderBooks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Users))]
        public int? UsersId { get; set; }
        virtual public Users Users { get; set; }

        [ForeignKey("Books")]
        public int? BooksId { get; set; }
        virtual   public Books Books { get; set; }
    }
}
