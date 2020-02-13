using AutoMapper;
using BusinessLayer.BusinessObject;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Entities;
using WebLibrary.Repository;
using WebLibrary.ViewModels;

namespace WebLibrary.Controllers
{

    public class BookController : Controller
    {
        public static List<StatisticVM> Statistics { get; set; }
        
        IMapper mapper;
        public BookController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ActionResult UsersReadThisBook(int? id)
        {
            using (Model1 db = new Model1())
            {
                var ordersBO = DependencyResolver.Current.GetService<OrderBookBO>().LoadAll().Where(o=>o.BooksId==id);
                var ordersVM = mapper.Map<List<OrderBookVM>>(ordersBO);
                var userList = ordersVM.Select(o => o.Users).ToList();
                ViewBag.Book = db.Books.Find(id).Title;
                return PartialView("Partial/_UsersReadThisBook", userList);
            }
        }

        public ActionResult Index()
        {
            using (Model1 db = new Model1())
            {
                var books = DependencyResolver.Current.GetService<BooksBO>().LoadAll().ToList();    //virtual!
                var booksVM = books.Select(b => mapper.Map<BookVM>(b)).ToList();
                return View(booksVM);
            }
        }
        //==================================== Create ================================
        [HttpGet]   //заполнен. select'ов в Create без ViewBag.SelectList
        public ActionResult PreCreate() //переход по ссылке IndexPage ->PreCreate-> IndexPage -> CreatePage ..
        {
            using (Model1 db = new Model1())
            {
                var authorList = DependencyResolver.Current.GetService<AuthorBO>().LoadAll().ToList();
                var authorListVM = mapper.Map<List<AuthorVM>>(authorList);

                var genreList = DependencyResolver.Current.GetService<GenreBO>().LoadAll().ToList();
                var genreListVM = mapper.Map<List<GenreVM>>(genreList);

                var dataList = new ArrayList(authorListVM);
                dataList.AddRange(genreList);

                var json = JsonConvert.SerializeObject(dataList);
                return new JsonResult
                {
                    Data = json,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpGet]//+Edit!
        public ActionResult Create(int? id)
        {
            if(id != null) {
                var bookBO = DependencyResolver.Current.GetService<BooksBO>().Load((int)id);
                var bookVM = mapper.Map<BookVM>(bookBO);
                return View(bookVM);
            }
            return View();
        }
        [HttpPost]  //+Edit!
        public async Task<ActionResult> Create(BookVM bookVM, HttpPostedFileBase upload)
        {
            ImageVM image = DependencyResolver.Current.GetService<ImageVM>();
            ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();
            var bookBO = mapper.Map<BooksBO>(bookVM);
            if (ModelState.IsValid)
            {                
                if (bookVM.Id == 0) {   //Create
                    if (upload != null) { //with img
                        imageBase = await SetImage(bookVM, upload, image, imageBase);
                        bookBO.ImagesId = imageBase.Id;
                    }
                    else {
                        bookVM.Images = new Images { FileName = "", ImageData = new byte[1] { 0 } };
                    }
                    
                }
                else {  //Update
                    if (upload != null) {   //with img
                        imageBase = await SetImage(bookVM, upload, image, imageBase);
                        bookBO.ImagesId = imageBase.Id;
                    }
                    else { 
                        var tempBookBO = DependencyResolver.Current.GetService<BooksBO>().Load(bookVM.Id);
                        int imagesIdTempBook = (int)tempBookBO.ImagesId;
                        bookBO.ImagesId = DependencyResolver.Current.GetService<ImageBO>().Load(imagesIdTempBook).Id;
                    }
                }               
                bookBO.Save(bookBO); 
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else return View(bookVM);
            //}
        }

        private async Task<ImageBO> SetImage(BookVM book, HttpPostedFileBase upload, ImageVM image, ImageBO imageBase)
        {
            string filename = System.IO.Path.GetFileName(upload.FileName);
            image.FileName = filename;
            byte[] myBytes = new byte[upload.ContentLength];
            upload.InputStream.Read(myBytes, 0, upload.ContentLength);
            image.ImageData = myBytes;
            var imgListBO = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.FileName == image.FileName).ToList();
            if (imgListBO == null || imgListBO.Count() == 0)  //если такого нет - сохранить
            {
                var imageBO = mapper.Map<ImageBO>(image);
                imageBase.Save(imageBO);
            }
            List<ImageBO> imageBases = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.FileName == image.FileName).ToList();
            imageBase = imageBases[0];
            return imageBase;
        }

        //================================== SurveyPage =======================================

        [HttpGet]
        public ActionResult SurveyPage()
        {
            var authorListBO = DependencyResolver.Current.GetService<AuthorBO>().LoadAll().ToList();
            var authorList = mapper.Map<List<AuthorVM>>(authorListBO);
            authorList.Add(new AuthorVM { LastName = "" });         //в select верх д/н быть пустой
            ViewBag.AuthorList = new SelectList(authorList.OrderBy(a => a.LastName), "Id", "LastName");
            var genresBO = DependencyResolver.Current.GetService<GenreBO>().LoadAll();
            var genresVM = mapper.Map<List<GenreVM>>(genresBO);
            ViewBag.GenreList = genresVM.ToList();
            return View();

        }

        [HttpPost]
        public ActionResult SurveyPage(BookVM book)
        {
            if (Request.Form.Count != 0)
            {
                StatisticVM statistic = new StatisticVM();
                int authorId = 0;
                string title = "";
                int genreId = 0;
                string isImageString = "";
                bool isImage = false;
                HandlerForm(statistic, ref authorId, ref title, ref genreId, ref isImageString, ref isImage);
                var statisticBO = mapper.Map<StatisticBO>(statistic);
                statisticBO.Save(statisticBO);
               
                 //----------------Seach------------------------                  
                ArrayList bigList = GetRequestBooks(authorId, title, genreId, isImage);
                var data = JsonConvert.SerializeObject(bigList);   //
                return new JsonResult
                {
                    Data = data,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = 2147483644
                };

            }
            return View();
        }
        private void HandlerForm(StatisticVM statistic, ref int authorId, ref string title, ref int genreId, ref string isImageString, ref bool isImage)
        {
            foreach (string item in Request.Form.Keys)
            {
                var value = Request.Form[item];
                if (value != null && value != "")
                {
                    if (item == "AuthorsId" && value != "0")
                    {
                        statistic.CountAuthorChoice++;
                        authorId = int.Parse(value);
                    }
                    else if (item == "Genres")
                    {
                        statistic.CountGenreChoice++;
                        genreId = int.Parse(value);
                    }
                    else if (item == "Title")
                    {
                        statistic.CountTitleChoice++;
                        title = value;
                    }
                    else if (item == "IsImage")
                    {
                        isImageString = value;
                        if (isImageString.Contains("true"))
                        {
                            statistic.CountIsImageChoice++;
                            isImage = true;
                        }
                        else isImage = false;
                    }
                }
            }
        }

        private ArrayList GetRequestBooks(int authorId, string title, int genreId, bool isImage)
        {
            //List<Books> books = unitOfWork.Books.Include(nameof(Authors)).Include(nameof(Genres)).Include(nameof(Images)).ToList();
            var booksBO = DependencyResolver.Current.GetService<BooksBO>().LoadAll().ToList();
            var books = mapper.Map<List<BookVM>>(booksBO);

            if (authorId != 0)//1)all books for author
                books = books.Where(b => b.AuthorsId == authorId).ToList();

            if (title != "") //фильтрация по назв.
                books = books.Where(b => b.Title == title).ToList();

            if (genreId != 0)   //..по жанру
                books = books.Where(b => b.GenresId == genreId).ToList();

            if (isImage == false)
                books = books.Where(b => b.ImagesId == 0).ToList();
            //books.ForEach(b => System.Diagnostics.Debug.WriteLine(b.Title));
            ArrayList bigList = new ArrayList();
            books.ForEach(b =>
            {
                var imgData = Convert.ToBase64String(b.Images.ImageData);
                b.Images.ImageData = null;  //облегч. поклажи
                Tuple<BookVM, string> tuple = new Tuple<BookVM, string>(b, imgData);
                bigList.Add(tuple);
            });
            return bigList;
        }


        public ActionResult StatisticReport()
        {
            var fullStatistic = DependencyResolver.Current.GetService<StatisticBO>().LoadAll().ToList();
            var fullStatisticVM = mapper.Map<List<StatisticVM>>(fullStatistic);

            var fullCountAuthorChoice = fullStatisticVM.Sum(s => s.CountAuthorChoice);
            var fullCountTitleChoice = fullStatisticVM.Sum(s => s.CountTitleChoice);
            var fullCountGenreChoice = fullStatisticVM.Sum(s => s.CountGenreChoice);
            var fullCountImageChoice = fullStatisticVM.Sum(s => s.CountIsImageChoice);

            ViewBag.FullCountAuthorChoice = fullCountAuthorChoice;
            ViewBag.FullCountTitleChoice = fullCountTitleChoice;
            ViewBag.FullCountGenreChoice = fullCountGenreChoice;
            ViewBag.FullCountImageChoice = fullCountImageChoice;
            List<Tuple<int, string>> tuples = new List<Tuple<int, string>> {
                    new Tuple<int, string>(fullCountAuthorChoice, "AuthorChoice"),
                    new Tuple<int, string>(fullCountTitleChoice, "AuthorChoice"),
                    new Tuple<int, string>(fullCountGenreChoice, "GenreChoice"),
                    new Tuple<int, string>(fullCountImageChoice, "ImageChoice")
                };
            var orderedTuples = tuples.OrderByDescending(t => t.Item1).ToList();
            ViewBag.MaxPop = orderedTuples[0].Item2;    //после сортировки 1 и последн.-это
            ViewBag.MinPop = orderedTuples[3].Item2;
            return View(fullStatisticVM.ToList());

        }
        //=======================Delete=================================
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var bookBO = DependencyResolver.Current.GetService<BooksBO>().Load((int)id);
            var bookVM = mapper.Map<BookVM>(bookBO);
            return View(bookVM);           
        }

        [HttpPost]
        public ActionResult Delete(BookVM book)
        {
            if (book == null)
                return HttpNotFound();
            var bookBO = mapper.Map<BooksBO>(book);
            var orderListBO = DependencyResolver.Current.GetService<OrderBookBO>().LoadAll().Where(o => o.BooksId == bookBO.Id).ToList();
            orderListBO.ForEach(o => o.DeleteSave(o));
            bookBO.DeleteSave(bookBO);  
            return RedirectToAction("Index");
        }
    }
}