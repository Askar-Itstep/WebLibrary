 using AutoMapper;
using BusinessLayer.BusinessObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Repository;
using WebLibrary.ViewModels;

namespace WebLibrary.Controllers
{
    public class AuthorController : Controller
    {
        private UnitOfWork unitOfWork;
        IMapper mapper;
        public AuthorController(IMapper mapper)
        {
            unitOfWork = new UnitOfWork();
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            //var authors = unitOfWork.Authors.GetAll().ToList();
            //return View(authors);
            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            var authorBOList = authorBO.LoadAll();
            var viewModel = authorBOList.Select(a => mapper.Map<AuthorViewModel>(a));
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var firstName = Request["FirstName"];
            var lastName = Request["LastName"];
            //if (firstName != null && lastName != null)
            {
                //unitOfWork.Authors.Create(new Authors { FirstName = firstName, LastName = lastName });
                //unitOfWork.Authors.Save();
                //return PartialView("Partial/_AuthorPartialView", unitOfWork.Authors.GetAll().ToList());                
            }
            var authorName = Request.Form["AuthorName"];
            if(authorName != null) {
                var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
                authorBO.AuthorName = authorName + " " + lastName;
                authorBO.Save(authorBO);
                return PartialView("Partial/_AuthorPartialView", authorBO.LoadAll());
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
            ////var author = unitOfWork.Authors.GetById(id);
            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            authorBO.Load(id.Value);
            
            var authorVM = mapper.Map<AuthorViewModel>(authorBO);
            return View(authorVM);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            //Authors author = unitOfWork.Authors.GetById(id);
            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            authorBO.Load(id.Value);
            if (authorBO != null)
            {
                ViewBag.MsgError = "Yes, author is found!";
                return new JsonResult
                {
                    Data = new ArrayList { authorBO, ViewBag.MsgError },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                ViewBag.MsgError = "No, author not found!";
                return PartialView("Partial/AuthorPartialView", authorBO.LoadAll()); //unitOfWork.Authors.GetAll().ToList()
            }

        }
        [HttpPost]
        public ActionResult Edit(AuthorViewModel authorVM)
        {
            if (authorVM == null)
                return HttpNotFound();

            //unitOfWork.Authors.Update(author);
            //unitOfWork.Authors.Save();

            var authorBO = mapper.Map<AuthorBO>(authorVM);
            authorBO.Save(authorBO);
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int? id) //сокращ. метод
        {
            if (id == null)
                return HttpNotFound();
            
            //unitOfWork.Authors.Delete(author.Id); 
            //unitOfWork.Authors.Save(); 

            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            authorBO.Load(id.Value);
            authorBO.DeleteSave(authorBO);  //пока только для не имеющих книг (foreygn key!)
            return RedirectToAction("Index");
        }
    }
}