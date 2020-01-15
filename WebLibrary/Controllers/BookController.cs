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

namespace WebLibrary.Controllers
{

    public class BookController : Controller
    {
        public static List<Users> UserList { get; set; }
        public static List<Statistic> Statistics { get; set; }

        public UnitOfWork unitOfWork;
        public BookController()
        {
            unitOfWork = new UnitOfWork();
        }
        //========================= ActionMethods ======================================

        public ActionResult UsersReadThisBook(int? id)//замена -для возвр. PartView
        {
            UserList = new List<Users>();
            using (Model1 db = new Model1())
            {

                //var orders = db.OrderBooks            //выбрать все заказы этой книги
                //    //.Include(o => o.Users).Include(o => o.Books)
                //    .Where(o => o.BooksId == id).ToList();
                var orders = unitOfWork.OrderBooks.GetAll().Where(o => o.BooksId == id);
                UserList = orders.Select(o => o.Users).ToList();    //а из них пользователей

                ViewBag.Book = db.Books.Find(id).Title;
                return PartialView("Partial/_UsersReadThisBook", UserList);
            }
        }

        public ActionResult Index()
        {
            using (Model1 db = new Model1())
            {
                //var books = db.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Images).ToList();
                var books = unitOfWork.Books.Include("Authors", "Genres", "Images").ToList();
                return View(books);
            }
        }
        //==================================== Create ================================
        [HttpGet]   //No ViewBag - No SelectList!
        public ActionResult PreCreate() //Index-view ->this-> Index-view-> Create-view..
        {
            using (Model1 db = new Model1())
            {
                var authorList = unitOfWork.Authors.GetAll().ToList();//db.Authors.ToList();
                var genreList = unitOfWork.Genres.GetAll().ToList();//db.Genres.ToList();
                var arrayList = new ArrayList(authorList);
                arrayList.AddRange(genreList);

                var json = JsonConvert.SerializeObject(arrayList);
                return new JsonResult
                {
                    Data = json,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase upload)
        //{
        //    var myfile = Request.Files;
        //    var xfiles = Request.InputStream;
        //    if (upload != null)
        //    {
        //        string fileName = System.IO.Path.GetFileName(upload.FileName);
        //        // сохраняем файл в папку Files в проекте
        //        upload.SaveAs(Server.MapPath("~/Files/" + fileName));
        //    }
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<ActionResult> Create(Books book, HttpPostedFileBase upload)
        {
            var myfile = Request.Files;
            //using (Model1 db = new Model1())
            //{
            Images image = new Images();
            Images imageBase = new Images();
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string filename = System.IO.Path.GetFileName(upload.FileName);
                    //System.Diagnostics.Debug.WriteLine(filename);
                    image.FileName = filename;
                    byte[] myBytes = new byte[upload.ContentLength];
                    upload.InputStream.Read(myBytes, 0, upload.ContentLength);
                    image.ImageData = myBytes;
                    //var temp = await db.Images.Where(i => i.FileName == image.FileName).ToListAsync();
                    var temp = await unitOfWork.Images.GetAll().Where(i => i.FileName == image.FileName).ToListAsync();
                    if (temp == null || temp.Count() == 0)  //если такого нет - сохранить
                    {
                        unitOfWork.Images.Create(image);//db.Images.Add(image);
                        unitOfWork.Images.Save();   //db.SaveChanges();
                    }
                    //List<Images> imageBases = await db.Images.Where(i => i.FileName == image.FileName).ToListAsync();
                    List<Images> imageBases = await unitOfWork.Images.GetAll().Where(i => i.FileName == image.FileName).ToListAsync();
                    imageBase = imageBases[0];
                    book.ImagesId = imageBase.Id;
                    System.Diagnostics.Debug.WriteLine(book.ImagesId);
                }
                unitOfWork.Books.Create(book);// db.Books.Add(book);
                unitOfWork.Books.Save(); //db.SaveChanges();
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else return View(book);
            //}
        }
        //================================== SurveyPage =======================================
        [HttpGet]
        public ActionResult SurveyPage()
        {
            using (Model1 db = new Model1())
            {
                var authorList = unitOfWork.Authors.GetAll().ToList();//db.Authors.ToList();
                authorList.Add(new Authors { LastName = ""});  //в select верх д/н быть пустой
                ViewBag.AuthorList = new SelectList(authorList.OrderBy(a=>a.LastName), "Id", "LastName");
                ViewBag.GenreList = unitOfWork.Genres.GetAll().ToList();//db.Genres.ToList();
                return View();
            }
        }

        [HttpPost]
        public ActionResult SurveyPage(Books book)
        {
            if (Request.Form.Count != 0)
            {
                Statistic statistic = new Statistic();
                int authorId = 0;
                string title = "";
                int genreId = 0;
                string isImageString = "";
                bool isImage = false;
                HandlerForm(statistic, ref authorId, ref title, ref genreId, ref isImageString, ref isImage);

                //----------------Seach------------------------                  
                using (Model1 db = new Model1())
                {
                    Statistics = db.Statistics.ToList();
                    Statistics.Add(statistic);
                    db.SaveChanges();
                    ArrayList bigList = GetRequestBooks(authorId, title, genreId, isImage, db);
                    var data = JsonConvert.SerializeObject(bigList);   //
                    return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return View();
        }
        private void HandlerForm(Statistic statistic, ref int authorId, ref string title, ref int genreId, ref string isImageString, ref bool isImage)
        {
            foreach (string item in Request.Form.Keys)
            {
                var value = Request.Form[item];
                if (value != null && value != "")
                {
                    if (item == "AuthorsId")
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

        private static ArrayList GetRequestBooks(int authorId, string title, int genreId, bool isImage, Model1 db)   
        {
            List<Books> books = db.Books.Include(nameof(Authors)).Include(nameof(Genres)).Include(nameof(Images))
                                    .ToList();
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
                Tuple<Books, string> tuple = new Tuple<Books, string>(b, imgData);
                bigList.Add(tuple);
            });
            return bigList;
        }


        public ActionResult StatisticReport()
        {
            using(Model1 db = new Model1())
            {
                var fullStatistic = db.Statistics.ToList();
                var fullCountAuthorChoice = db.Statistics.Sum(s => s.CountAuthorChoice);
                var fullCountTitleChoice = db.Statistics.Sum(s => s.CountTitleChoice);
                var fullCountGenreChoice = db.Statistics.Sum(s => s.CountGenreChoice);
                var fullCountImageChoice = db.Statistics.Sum(s => s.CountIsImageChoice);

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
                ViewBag.MaxPop = orderedTuples[0].Item2;
                ViewBag.MinPop = orderedTuples[3].Item2;
                return View(fullStatistic);
            }            
        }
        //============================== Edit ========================================
        [HttpGet]
        public ActionResult Edit(int? id)   
        {
            if (id == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                Books book = db.Books.Find(id);
                ViewBag.AuthorList = new SelectList(db.Authors.ToList(), "Id", "LastName");
                ViewBag.GenreList = new SelectList(db.Genres.ToList(), "Id", "Name");
                ViewBag.ImageBook = db.Images.Where(i => i.Id == book.ImagesId).FirstOrDefault();
                ViewBag.Images = new SelectList(db.Images.ToList(), "Id", "FileName");
                return View(book);
            }
        }

        [HttpGet]
        public ActionResult PreEdit(int? id)    //обраб. запроса на изм. изобр. из Edit-view
        {
            if (id == null)
                return HttpNotFound();
            
            using(Model1 db = new Model1())
            {
                var res = db.Images.Find(id);
                var data = Convert.ToBase64String(res.ImageData);
                return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }           
        }

        [HttpPost]
        public ActionResult Edit(Books book)
        {
            using (Model1 db = new Model1())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else return View(book);
            }
            return RedirectToAction("Index");
        }
        //=======================Delete=================================
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                Books book = db.Books.Find(id);
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Delete(Books book)
        {
            if (book == null)
                return HttpNotFound();

            using (Model1 db = new Model1())
            {
                //db.Books.Remove(book);    //при аннот. - ошибка?
                db.Entry(book).State = EntityState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}