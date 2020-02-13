using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using WebLibrary;
using WebLibrary.Repository;

namespace BusinessLayer.BusinessObject
{
    public class BooksBO: BaseBusinessObject
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int? Price { get; set; }
        public int? AuthorsId { get; set; }
        virtual public AuthorBO Authors { get; set; }

        public int? Pages { get; set; }
        
        public int? GenresId { get; set; }
        virtual public GenreBO Genres { get; set; }
        
        public int? ImagesId { get; set; }
        virtual public ImageBO Images { get; set; }
        //-----------------------------------------------------------

        readonly IUnityContainer unityContainer;
        //public BooksBO() { }
        public BooksBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<BooksBO> LoadAll()  //из DataObj в BusinessObj
        {
            var books = unitOfWork.Books.GetAll();
            var res = books.AsEnumerable().Select(a => mapper.Map<BooksBO>(a)).ToList();
            return res;
        }

        public BooksBO Load(int id)
        {
            var book = unitOfWork.Books.GetById(id);
            return mapper.Map(book, this);
        }
        public void Save(BooksBO bookBO)
        {
            var book = mapper.Map<Books>(bookBO);
            if (bookBO.Id == 0) {
                Add(book);
            }
            else {
                Update(book);
            }
            unitOfWork.Books.Save();
        }
        private void Add(Books book)
        {
            unitOfWork.Books.Create(book);
        }
        private void Update(Books book)
        {
            unitOfWork.Books.Update(book);
        }
        public void DeleteSave(BooksBO bookBO)
        {
            var book = mapper.Map<Books>(bookBO);
            unitOfWork.Books.Delete(book.Id);
            unitOfWork.Books.Save();
        }
    }
}
