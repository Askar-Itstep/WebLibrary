using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    public class OrderBookController : Controller
    {
        public ActionResult Index()
        {
            using(Model1 db = new Model1())
            {
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");
                List<OrderBooks> orders = db.OrderBooks.Include(nameof(Users))
                    .Include(nameof(Books)).ToList();
                return View(orders);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            using(Model1 db = new Model1())
            {
                ViewBag.UserList =new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult Create(OrderBooks orderBook)
        {
            if(orderBook == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.OrderBooks.Add(orderBook);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return View(orderBook);
            }
        }
        public ActionResult GetTop5(int? userId)
        {
            using (Model1 db = new Model1())
            {
                List<Books> bookList = GetBooks(userId);
               
                return new JsonResult
                {
                    Data = bookList,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)   //id-orders
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using(Model1 db = new Model1())
            {
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");

                OrderBooks order = db.OrderBooks.Find(id);
                List<Books> bookList = GetBooks(order.UsersId);
                ViewBag.OrderedBookList = bookList; //исп-ся в _Top5Orders, включ. в Edit-page
                return View(order);
            }
        }
        //---------------выбрать 5 книг текущ. юзера---------------
        private static List<Books> GetBooks(int? userId)
        {
            using (Model1 db = new Model1())
            {
                var orderBooks = db.OrderBooks.Where(o => o.UsersId == userId).Take(5).ToList();
                List<Books> bookList = new List<Books>();
                orderBooks.ForEach(order =>
                {
                    bookList.Add(db.Books.Include(nameof(Authors)).Include(nameof(Genres))  //Lazy-load !
                                            .Where(b => b.Id == order.BooksId).FirstOrDefault());
                });
                return bookList;
            }
        }

        [HttpPost]
        public ActionResult Edit(OrderBooks orderBook)
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                db.Entry(orderBook).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using(Model1 db = new Model1())
            {
                OrderBooks orderBook = db.OrderBooks.Find(id);
                //int userId = (int)orderBook.UsersId;

                //ViewBag.UserName = db.Users.Find(userId).UserName;    //navi field d't need
                ViewBag.UserName = orderBook.Users.UserName;    //без virtual -  не работ.
                ViewBag.BookTitle = orderBook.Books.Title;

                return View(orderBook);
            }
        }
        [HttpPost]
        public ActionResult Delete(OrderBooks orderBook)
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {           
                    if (orderBook.BooksId == null || orderBook.UsersId == null)
                    {
                        OrderBooks use = db.OrderBooks.Find(orderBook.Id);

                        db.OrderBooks.Remove(use);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else return HttpNotFound();                    
                }
                else
                    return View(orderBook);                
            }
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                OrderBooks orderBook = db.OrderBooks.Find(id);
                ViewBag.UserName = orderBook.Users.UserName;
                ViewBag.BookTitle = orderBook.Books.Title;
                return View(orderBook);
            }
        }

    }
}