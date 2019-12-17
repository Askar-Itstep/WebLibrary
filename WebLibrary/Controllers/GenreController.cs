using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    public class GenreController : Controller
    {
        // GET: Genre
        public ActionResult Index()
        {
            using(Model1 db = new Model1())
            {
                List<Genres> genres = db.Genres.ToList();
                return View(genres);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            using(Model1 db = new Model1())
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Genres genre)
        {
            if (genre == null)
                    return HttpNotFound();

            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Genres.Add(genre);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else return View(genre);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == 0)
                return HttpNotFound();

            using(Model1 db = new Model1())
            {
                Genres genre = db.Genres.Find(id);
                return View(genre);
            }
            
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
                    int id = genre.Id;
                    Genres genreOrigin = db.Genres.Find(id);
                    db.Genres.Remove(genreOrigin);
                    db.Genres.Add(genre);
                    //db.Entry(genre).State = EntityState.Modified; //или так
                    db.SaveChanges();
                }
                else return View(genre);
                return RedirectToAction("Index");                
            }
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                Genres genre = db.Genres.Find(id);
                db.Genres.Remove(genre);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }


}