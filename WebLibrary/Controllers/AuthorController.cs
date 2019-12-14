using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Authors
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
           return View();
        }

        [HttpPost]
        public ActionResult Create(Authors author)
            {
                using (Model1 db = new Model1())
                {
                    if (ModelState.IsValid)
                    {
                        db.Authors.Add(author);
                        db.SaveChanges();
                    }
                    else return View(author);
                }
                return Redirect("Index");

            }

        public ActionResult Details()
        {
           return new HttpStatusCodeResult(403);            
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                Authors author = db.Authors.Find(id);
                return View(author);
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
                List<Books> books = db.Books.Where(b => b.AuthorId == id).ToList();
                db.Books.RemoveRange(books);
                db.Authors.Remove(author);
                db.SaveChanges();
            }
                return RedirectToAction("Index");
        }
    }
}