using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    public class UseBookController : Controller
    {
        // GET: UseBook
        public ActionResult Index()
        {
            using(Model1 db = new Model1())
            {
                List<UseBooks> useBooks = db.UseBooks.ToList();
                return View(useBooks);
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
        public ActionResult Create(UseBooks useBooks)
        {
            if(useBooks == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.UseBooks.Add(useBooks);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return View(useBooks);
            }

        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using(Model1 db = new Model1())
            {
                UseBooks useBook = db.UseBooks.Find(id);
                ViewBag.UserList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.BookList = new SelectList(db.Books.ToList(), "Id", "Title");
                return View(useBook);
            }
        }
        [HttpPost]
        public ActionResult Edit(UseBooks useBook)
        {
            if (useBook == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                db.Entry(useBook).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return View();
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
                UseBooks useBook = db.UseBooks.Find(id);
                int userId = (int)useBook.UserId;
                ViewBag.UserName = db.Users.Find(userId).UserName;

                int bookId = (int)useBook.BookId;
                ViewBag.BookTitle = db.Books.Find(bookId).Title;
                return View(useBook);
            }
        }
        [HttpPost]
        public ActionResult Delete(UseBooks useBook)
        {
            if (useBook == null)
            {
                return HttpNotFound();
            }
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {               //@Html.DisplayFor(model => model.UserId)-закоммент.=>BookId == null???????
                    if (useBook.BookId == null || useBook.UserId == null)
                    {
                        UseBooks use = db.UseBooks.Find(useBook.Id);

                        db.UseBooks.Remove(use);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else return HttpNotFound();                    
                }
                else
                    return View(useBook);                
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
                UseBooks useBook = db.UseBooks.Find(id);
                int userId = (int)useBook.UserId;
                ViewBag.UserName = db.Users.Find(userId).UserName;

                int bookId = (int)useBook.BookId;
                ViewBag.BookTitle = db.Books.Find(bookId).Title;
                return View(useBook);
            }
        }

    }
}