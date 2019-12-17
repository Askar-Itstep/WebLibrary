using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLibrary.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            using(Model1 db = new Model1())
            {
                List<Users> users = db.Users.ToList();
                return View(users);
            }
            
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Users user)
        {
            if (user == null)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(user);
                }
                
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                Users user = db.Users.Find(id);
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult Edit(Users user)
        {
            if (user == null)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return View(user);
            }
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using(Model1 db = new Model1())
            {
                Users user = db.Users.Find(id);
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult Delete(Users user)
        {
            if (user == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(user).State = EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return View(user);
            }
        }
    }
}