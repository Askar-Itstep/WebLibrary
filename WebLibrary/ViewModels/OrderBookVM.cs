using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLibrary.ViewModels
{
    public class OrderBookVM
    {
        public int Id { get; set; }
        public int? UsersId { get; set; }
        virtual public UserVM Users { get; set; }
        
        public int? BooksId { get; set; }
        virtual public BookVM Books { get; set; }
    }
}