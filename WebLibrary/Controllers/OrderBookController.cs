using Newtonsoft.Json;
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
            using (Model1 db = new Model1())
            {
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");
                List<OrderBooks> orders = db.OrderBooks.Include(nameof(Users))
                    .Include(nameof(Books)).ToList();
                return View(orders);
            }
        }

        [HttpGet]
        public ActionResult Create()    //fix double click (переопред. ajax для ссылки)
        {
            var userId = Request["UsersId"];
            var bookId = Request["BooksId"];
            using (Model1 db = new Model1())
            {
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");
                if (userId != null && bookId != null)
                {
                    var order = new OrderBooks();
                    order.UsersId = int.Parse(userId);
                    order.BooksId = int.Parse(bookId);
                    db.OrderBooks.Add(order);
                    db.SaveChanges();

                    return PartialView("Partial/_OrderPartialView", db.OrderBooks.ToList());
                }
            }
            return PartialView("Partial/_CreatePartialView");

        }

        [HttpPost]//уже не исп-ся (переопред. Ajax.BeginForm из-за двойной отправки)
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderBooks orderBook)  //
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UsersName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");

                if (ModelState.IsValid)
                {
                    db.OrderBooks.Add(orderBook);
                    db.SaveChanges();           //Ajax.BeginForm - двойной клик или дребезг мыши??????
                    return PartialView("Partial/_OrderPartialView", db.OrderBooks.ToList());
                }
                return PartialView("Partial/_CreatePartialView");
            }
        }
        [HttpGet]
        public ActionResult GetTop5(int? userId)    //вызов из index/ajax
        {
            using (Model1 db = new Model1())
            {
                List<Books> bookList = GetBooks(userId);
                //bookList.ForEach(b => b.Images == null);
                var data = JsonConvert.SerializeObject(bookList);
                return new JsonResult
                {
                    Data = data,
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
            using (Model1 db = new Model1())
            {
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");

                OrderBooks order = db.OrderBooks.Find(id);
                List<Books> bookList = GetBooks(order.UsersId);
                ViewBag.OrderedBookList = bookList; //исп-ся в _Top5Orders, включ. в Edit-page
                return View(order);
            }
        }


        private static List<Books> GetBooks(int? userId)//-выбрать 5 книг текущ. юзера--
        {
            using (Model1 db = new Model1())
            {
                var orderBooks = db.OrderBooks.Where(o => o.UsersId == userId).Take(5).ToList();
                List<Books> bookList = new List<Books>();
                orderBooks.ForEach(order =>
                {
                    bookList.Add(db.Books.Include(nameof(Authors)).Include(nameof(Genres))  //Lazy-load !
                                         .Include(nameof(Images)).Where(b => b.Id == order.BooksId).FirstOrDefault());
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
            using (Model1 db = new Model1())
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
                //ViewBag.UserName
                var username = orderBook.Users.UserName;    //достат. первого обращения (virtual!)
                //ViewBag.BookTitle
                var title = orderBook.Books.Title;
                return View(orderBook);
            }
        }

    }
}