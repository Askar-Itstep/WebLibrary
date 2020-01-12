using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    public class AuthorController : Controller
    {
        public ActionResult Index()
        {
            using (Model1 db = new Model1())
            {
                var authors = db.Authors.ToList();

                return View(authors);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var firstName = Request["FirstName"];
            var lastName = Request["LastName"];
            using (Model1 db = new Model1())
            {
                if(firstName != null && lastName != null)
                {
                    db.Authors.Add(new Authors { FirstName = firstName, LastName = lastName });
                    db.SaveChanges();
                    return PartialView("Partial/_AuthorPartialView", db.Authors.ToList());
                }
            }
                return PartialView("Partial/_CreatePartialView"); ;
        }

        //[HttpPost]        //уже не исп-ся (переопред. Ajax.BeginForm из-за двойной отправки)
        //public ActionResult Create(Authors author)
        //{
        //    using (Model1 db = new Model1())
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Authors.Add(author);
        //            db.SaveChanges();
        //        }
        //        else return View(author);
        //    }
        //    return Redirect("Index");

        //}

        public ActionResult Details()
        {
            return new HttpStatusCodeResult(403);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (Model1 db = new Model1())
            {
                Authors author = db.Authors.Find(id);
                if (author != null)
                {
                    ViewBag.MsgError = "Yes, author is found!";
                    return new JsonResult {
                        Data = new ArrayList { author, ViewBag.MsgError },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    ViewBag.MsgError = "No, author not found!";
                    return PartialView("Partial/AuthorPartialView", db.Authors.ToList());
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(Authors author)
        {
            if (author == null)
                return HttpNotFound();
            using (Model1 db = new Model1())
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(int? id) //сокращ. метод
        {
            if (id == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                Authors author = db.Authors.Find(id);
                List<Books> books = db.Books.Where(b => b.AuthorsId == id).ToList();
                db.Books.RemoveRange(books);
                db.Authors.Remove(author);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}