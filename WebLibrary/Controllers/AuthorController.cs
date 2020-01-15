using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Repository;

namespace WebLibrary.Controllers
{
    public class AuthorController : Controller
    {
        private UnitOfWork unitOfWork;
        public AuthorController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            var authors = unitOfWork.Authors.GetAll().ToList();

            return View(authors);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var firstName = Request["FirstName"];
            var lastName = Request["LastName"];

            if (firstName != null && lastName != null)
            {
                unitOfWork.Authors.Create(new Authors { FirstName = firstName, LastName = lastName });
                unitOfWork.Authors.Save();
                return PartialView("Partial/_AuthorPartialView", unitOfWork.Authors.GetAll().ToList());
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

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(403);
            var author = unitOfWork.Authors.GetById(id);
            return View(author);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Authors author = unitOfWork.Authors.GetById(id);
            if (author != null)
            {
                ViewBag.MsgError = "Yes, author is found!";
                return new JsonResult
                {
                    Data = new ArrayList { author, ViewBag.MsgError },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                ViewBag.MsgError = "No, author not found!";
                return PartialView("Partial/AuthorPartialView", unitOfWork.Authors.GetAll().ToList());
            }

        }
        [HttpPost]
        public ActionResult Edit(Authors author)
        {
            if (author == null)
                return HttpNotFound();

            unitOfWork.Authors.Update(author);
            unitOfWork.Authors.Save();
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int? id) //сокращ. метод
        {
            if (id == null)
                return HttpNotFound();

            Model1 db = new Model1();
            Authors author = unitOfWork.Authors.GetById(id);
            List<Books> books = unitOfWork.Books.GetAll().Where(b => b.AuthorsId == author.Id).ToList();
            for(int i =0; i < books.Count(); i++)
            {
                unitOfWork.Books.Delete(books[i].Id);
            }
            
            unitOfWork.Authors.Delete(author.Id); 
            unitOfWork.Authors.Save(); 

            return RedirectToAction("Index");
        }
    }
}