using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Repository;

namespace WebLibrary.Controllers
{
    public class OrderBookController : Controller
    {
        private UnitOfWork unitOfWork;
        public OrderBookController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            ViewBag.UserList = new SelectList(unitOfWork.Users.GetAll().ToList(), "Id", "UserName");
            ViewBag.BookList = new SelectList(unitOfWork.Books.GetAll().ToList(), "Id", "Title");
            //List<OrderBooks> orders = db.OrderBooks.Include(nameof(Users)).Include(nameof(Books)).ToList();
            var orders = unitOfWork.OrderBooks.GetAll();
            return View(orders.ToList());

        }

        [HttpGet]
        public ActionResult Create()    //fix double click (переопред. ajax для ссылки)
        {
            var userId = Request["UsersId"];
            var bookId = Request["BooksId"];
            ViewBag.UserList = new SelectList(unitOfWork.Users.GetAll().ToList(), "Id", "UserName");
            ViewBag.BookList = new SelectList(unitOfWork.Books.GetAll(), "Id", "Title");
            if (userId != null && bookId != null)
            {
                var order = new OrderBooks();
                order.UsersId = int.Parse(userId);
                order.BooksId = int.Parse(bookId);
                unitOfWork.OrderBooks.Create(order);
                unitOfWork.OrderBooks.Save();
                return PartialView("Partial/_OrderPartialView", unitOfWork.OrderBooks.GetAll().ToList());
            }
            return PartialView("Partial/_CreatePartialView");

        }

        //[HttpPost]          //уже не исп-ся (переопред. Ajax.BeginForm из-за двойной отправки)
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(OrderBooks orderBook)  //
        //{
        //    if (orderBook == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    using (Model1 db = new Model1())
        //    {
        //        ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UsersName");
        //        ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");

        //        if (ModelState.IsValid)
        //        {
        //            db.OrderBooks.Add(orderBook);
        //            db.SaveChanges();           //Ajax.BeginForm - двойной клик или дребезг мыши??????
        //            return PartialView("Partial/_OrderPartialView", db.OrderBooks.ToList());
        //        }
        //        return PartialView("Partial/_CreatePartialView");
        //    }
        //}

        [HttpGet]
        public ActionResult GetTop5(int? userId)    //вызов из index/ajax
        {

            List<Books> bookList = GetBooks(userId);
            var data = JsonConvert.SerializeObject(bookList);
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [HttpGet]
        public ActionResult Edit(int? id)   //id-orders
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserList = new SelectList(unitOfWork.Users.GetAll().ToList(), "Id", "UserName");
            ViewBag.BookList = new SelectList(unitOfWork.Books.GetAll().ToList(), "Id", "Title");

            OrderBooks order = unitOfWork.OrderBooks.GetById(id);
            List<Books> bookList = GetBooks(order.UsersId);
            ViewBag.OrderedBookList = bookList; //исп-ся в _Top5Orders, включенный в Edit-page
            return View(order);
        }


        private List<Books> GetBooks(int? userId)//-выбрать 5 книг текущ. юзера--
        {
            var orderBooks = unitOfWork.OrderBooks.GetAll().Where(o => o.UsersId == userId).Take(5).ToList();
            List<Books> bookList = new List<Books>();
            orderBooks.ForEach(order =>
            {
                bookList.Add(unitOfWork.Books.GetAll().Where(b => b.Id == order.BooksId).FirstOrDefault());
            });
            return bookList;

        }

        [HttpPost]
        public ActionResult Edit(OrderBooks orderBook)
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }

            unitOfWork.OrderBooks.Update(orderBook);
            unitOfWork.OrderBooks.Save();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            OrderBooks orderBook = unitOfWork.OrderBooks.GetById(id);
            ViewBag.UserName = orderBook.Users.UserName;    //достат. 1-го обращения (virtual!)
            ViewBag.BookTitle = orderBook.Books.Title;
            return View(orderBook);
        }

        [HttpPost]
        public ActionResult Delete(OrderBooks orderBook)
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (orderBook.BooksId == null || orderBook.UsersId == null)
                {
                    OrderBooks use = unitOfWork.OrderBooks.GetById(orderBook.Id);

                    unitOfWork.OrderBooks.Delete(use.Id);
                    unitOfWork.OrderBooks.Save();
                    return RedirectToAction("Index");
                }
                else return HttpNotFound();
            }
            else
                return View(orderBook);

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            OrderBooks orderBook = unitOfWork.OrderBooks.GetById(id);
            var username = orderBook.Users.UserName;    //достат. 1-го обращения (virtual!)
            var title = orderBook.Books.Title;
            return View(orderBook);

        }

    }
}