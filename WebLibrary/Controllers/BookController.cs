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

namespace WebLibrary.Controllers
{

    public class BookController : Controller
    {
        public static List<Users> UserList { get; set; }   
        public ActionResult UsersReadThisBook(int? id)//замена -для возвр. PartView
        {
            //System.Diagnostics.Debug.WriteLine("id: " + id);
            UserList = new List<Users>(); 
            using(Model1 db = new Model1())
            {

                var orders = db.OrderBooks            //выбрать все заказы этой книги
                    .Include(o => o.Users)
                    .Include(o => o.Books).Where(o => o.BooksId == id).ToList();

                UserList = orders.Select(o => o.Users).ToList();    //а из них пользователей

                ViewBag.Book = db.Books.Find(id).Title;
                return PartialView("Partial/_UsersReadThisBook", UserList);   
            }
           
        }
       
        public ActionResult Index()
        {
            using (Model1 db = new Model1())
            {
                var books = db.Books
                    .Include(b => b.Authors).Include(b => b.Genres).Include(b=>b.Images)
                    .ToList();

                return View(books);
            }
        }

        [HttpGet]
        public ActionResult PreCreate()
        {
            using (Model1 db = new Model1())
            {
                //ViewBag.AuthorList = new SelectList(db.Authors.ToList(), "Id", "LastName");
                //ViewBag.GenreList = new SelectList(db.Genres.ToList(), "Id", "Name");
                var authorList = db.Authors.ToList();
                var genreList = db.Genres.ToList();
                var arrayList = new ArrayList(authorList);
                arrayList.AddRange(genreList);

                var json = JsonConvert.SerializeObject(arrayList); 
                //System.Diagnostics.Debug.WriteLine(json);
                return new JsonResult  {
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
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            var myfile = Request.Files;
            var xfiles = Request.InputStream;
            if (upload != null)
            {
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        //public ActionResult Create(Books book, HttpPostedFileBase upload)
        public async Task<ActionResult> Create(Books book, HttpPostedFileBase upload)
        {
            var myfile = Request.Files;
            using (Model1 db = new Model1())
            {
                Image image = new Image();
                Image imageBase = new Image();
                if (ModelState.IsValid)
                {
                    if(upload != null)
                    {
                        string filename = System.IO.Path.GetFileName(upload.FileName);
                        //System.Diagnostics.Debug.WriteLine(filename);
                        image.FileName = filename;
                        byte[] myBytes = new byte[upload.ContentLength];
                        upload.InputStream.Read(myBytes, 0, upload.ContentLength);
                        image.ImageData = myBytes;
                        var temp = await db.Images.Where(i => i.FileName == image.FileName).ToListAsync();
                        if (temp == null || temp.Count() == 0)
                            db.Images.Add(image);
                        db.SaveChanges();
                        //var result = await Task.Factory.StartNew(() => db.SaveChanges());
                        List<Image> imageBases = await db.Images.Where(i => i.FileName == image.FileName).ToListAsync();
                        imageBase = imageBases[0];
                        book.ImageId = imageBase.Id;
                        System.Diagnostics.Debug.WriteLine(book.ImageId);
                    }
                    db.Books.Add(book);
                    db.SaveChanges();
                }
                //else return View(book);
            }
            return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            using(Model1 db = new Model1())
            {
                Books book = db.Books.Find(id);
                ViewBag.AuthorList = new SelectList(db.Authors.ToList(), "Id", "LastName");
                ViewBag.GenreList = new SelectList(db.Genres.ToList(), "Id", "Name");
                return View(book);
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

        [HttpGet]
        public ActionResult Delete(int? id) //развернут. метод
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