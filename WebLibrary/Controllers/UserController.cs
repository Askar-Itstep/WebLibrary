using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Repository;

namespace WebLibrary.Controllers
{
    public class UserController : Controller
    {
        private UnitOfWork unityOfWork;
        public UserController()
        {
            unityOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            List<Users> users = unityOfWork.Users.GetAll().ToList();
            return View(users);
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

            if (ModelState.IsValid)
            {
                unityOfWork.Users.Create(user);
                unityOfWork.Users.Save();
                return RedirectToAction("Index");
            }
            else
                return View(user);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Users user = unityOfWork.Users.GetById(id);
            return View(user);

        }
        [HttpPost]
        public ActionResult Edit(Users user)
        {
            if (user == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                unityOfWork.Users.Update(user);
                unityOfWork.Users.Save();
                return RedirectToAction("Index");
            }
            else
                return View(user);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Users user = unityOfWork.Users.GetById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(Users user)
        {
            if (user == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                unityOfWork.Users.Delete(user.Id);
                unityOfWork.Users.Save();
                return RedirectToAction("Index");
            }
            else
                return View(user);
        }
    }
}