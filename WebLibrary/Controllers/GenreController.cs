using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Repository;

namespace WebLibrary.Controllers
{
    public class GenreController : Controller
    {
        private UnitOfWork unitOfWork;
        public GenreController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
                List<Genres> genres = unitOfWork.Genres.GetAll().ToList();
                return View(genres);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Genres genre)
        {
            if (genre == null)
                    return HttpNotFound();

           
                if (ModelState.IsValid)
                {
                unitOfWork.Genres.Create(genre);
                unitOfWork.Genres.Save();
                    return RedirectToAction("Index");
                }
                else return View(genre);
            
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
           
                Genres genre = unitOfWork.Genres.GetById(id);
                return View(genre);                       
        }
        [HttpPost]
        public ActionResult Edit(Genres genre)
        {
            if (genre == null)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(genre).State = EntityState.Modified; //или так
                    //db.SaveChanges();
                    unitOfWork.Genres.Update(genre);
                    unitOfWork.Genres.Save();
                }
                else return View(genre);
                return RedirectToAction("Index");                
            }
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
                return HttpNotFound();
           
                Genres genre = unitOfWork.Genres.GetById(id);//db.Genres.Find(id);
            unitOfWork.Genres.Delete(genre.Id);
            unitOfWork.Genres.Save();
            
            return RedirectToAction("Index");
        }
    }


}