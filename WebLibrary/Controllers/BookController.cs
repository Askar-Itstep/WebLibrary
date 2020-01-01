using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{

    public class BookController : Controller
    {
        public static List<Users> UserList { get; set; }    //static-для вар.1
        public ActionResult _UsersReadThisBook(int? id)//замена -для возвр. PartView
        {
            System.Diagnostics.Debug.WriteLine("id: " + id);
            UserList = new List<Users>(); 
            using(Model1 db = new Model1())
            {

                var orders = db.OrderBooks            //выбрать все заказы этой книги
                    .Include(o => o.Users)
                    .Include(o => o.Books).Where(o => o.BooksId == id).ToList();

                UserList = orders.Select(o => o.Users).ToList();    //а из них пользователей

                //return RedirectToAction("Index"); //вар.1
                ViewBag.Book = db.Books.Find(id).Title;
                return PartialView(UserList);   //error-при нахожд. представл. в папке  Partial?!
            }
           
        }
       
        public ActionResult Index()
        {
            using (Model1 db = new Model1())
            {
                var books = db.Books
                    .Include(b => b.Authors).Include(b => b.Genres)
                    .ToList();

                //ViewBag.UsersReadThisBook = UserList; //вар.1
                return View(books);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            using (Model1 db = new Model1())
            {
                ViewBag.AuthorList = new SelectList(db.Authors.ToList(), "Id", "LastName");
                ViewBag.GenreList = new SelectList(db.Genres.ToList(), "Id", "Name");
            }
            return View();

        }

        [HttpPost]
        public ActionResult Create(Books book)
        {
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                }
                else return View(book);
            }
            return Redirect("Index");

        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            using(Model1 db = new Model1())
            {
                Books book = db.Books.Find(id);
                ViewBag.AuthorList = new SelectList(db.Authors.ToList(), "Id", "LastName");
                ViewBag.GenreList = new SelectList(db.Genres.ToList(), "Id", "Name");
                return View(book);
            }
            
            
        }
        [HttpPost]
        public ActionResult Edit(Books book)    
        {
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else return View(book);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id) //развернут. метод
        {
            if (id == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                Books book = db.Books.Find(id);
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Delete(Books book)
        {
            if (book == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                //db.Books.Remove(book);    //при аннот. - ошибка?
                db.Entry(book).State = EntityState.Deleted;
                db.SaveChanges();                
            }
            return RedirectToAction("Index");
        }
    }
}