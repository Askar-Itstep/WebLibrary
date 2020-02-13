using AutoMapper;
using BusinessLayer.BusinessObject;
using System.Collections.Generic;
using System.Web.Mvc;
using WebLibrary.Repository;
using WebLibrary.ViewModels;

namespace WebLibrary.Controllers
{
    public class GenreController : Controller
    {
        //private UnitOfWork unitOfWork;
        private IMapper mapper;
        public GenreController(IMapper mapper)
        {
            //unitOfWork = new UnitOfWork();
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            var genresBO = DependencyResolver.Current.GetService<GenreBO>().LoadAll();
            var genres = mapper.Map<List<GenreVM>>(genresBO);
            return View(genres);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(GenreVM genre)
        {
            if (genre == null) {
                return HttpNotFound();
            }

            if (ModelState.IsValid) {
                var genreBO = mapper.Map<GenreBO>(genre);
                genreBO.Save(genreBO);
                return RedirectToAction("Index");
            }
            else {
                return View(genre);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return HttpNotFound();
            }

            GenreBO genre = DependencyResolver.Current.GetService<GenreBO>().Load((int)id);
            var genreVM = mapper.Map<GenreVM>(genre);
            return View(genreVM);
        }
        [HttpPost]
        public ActionResult Edit(GenreVM genre)
        {
            if (genre == null) {
                return HttpNotFound();
            }

            using (Model1 db = new Model1()) {
                if (ModelState.IsValid) {
                    var genreBO = mapper.Map<GenreBO>(genre);
                    genreBO.Save(genreBO);
                return RedirectToAction("Index");
                }
                else 
                    return View(genre);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return HttpNotFound();
            }
            
            GenreBO genre = DependencyResolver.Current.GetService<GenreBO>().Load((int)id);
            var genreBO = mapper.Map<GenreBO>(genre);
            genreBO.DeleteSave(genreBO);
            return RedirectToAction("Index");
        }
    }


}