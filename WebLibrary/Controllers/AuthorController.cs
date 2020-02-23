 using AutoMapper;
using BusinessLayer.BusinessObject;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
        IMapper mapper;
        public AuthorController(IMapper mapper) { 
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            var authorBOList = authorBO.LoadAll();
            var viewModel = authorBOList.Select(a => mapper.Map<AuthorVM>(a));

            //UploadFile();
            return View(viewModel);
        }
        public void UploadFile()
        {
            string path = @"C:\17.jpg";
            string blobStorage = ConfigurationManager.ConnectionStrings["blob"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobStorage);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference("blobstorage");
            blobContainer.CreateIfNotExists();

            blobContainer.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
                );
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference("test.jpg");
            using (var file = System.IO.File.OpenRead(path))
            {
                blob.UploadFromStream(file);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            var firstName = Request["FirstName"];
            var lastName = Request["LastName"];
            
            var authorName = Request.Form["AuthorName"];
            if(authorName != null) {
                var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
                authorBO.Save(authorBO);
                return PartialView("Partial/_AuthorPartialView", authorBO.LoadAll());
            }
            return PartialView("Partial/_CreatePartialView"); ;
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(403);
            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            authorBO.Load(id.Value);
            
            var authorVM = mapper.Map<AuthorVM>(authorBO);
            return View(authorVM);
        }
        //---------Edit ------------------------------------
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            
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
                return PartialView("Partial/AuthorPartialView", authorBO.LoadAll()); 
            }

        }
        [HttpPost]
        public ActionResult Edit(AuthorVM authorVM)
        {
            if (authorVM == null)
                return HttpNotFound();

            var authorBO = mapper.Map<AuthorBO>(authorVM);
            authorBO.Save(authorBO);
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int? id) //сокращ. метод
        {
            if (id == null)
                return HttpNotFound();
            var authorBO = DependencyResolver.Current.GetService<AuthorBO>();
            authorBO.Load(id.Value);
            var booksOfAuthorBO = DependencyResolver.Current.GetService<BooksBO>().LoadAll().Where(b => b.AuthorsId == id).ToList();

            var orderListBO = DependencyResolver.Current.GetService<OrderBookBO>().LoadAll().ToList();

            foreach (var order in orderListBO) {
                foreach (var book in booksOfAuthorBO) {
                    if (order.Books.Id == book.Id) {
                        order.DeleteSave(order);
                    }
                }
            }

            booksOfAuthorBO.ForEach(b => b.DeleteSave(b));// для не имеющих книги (foreygn key!)
            authorBO.DeleteSave(authorBO);  
            return RedirectToAction("Index");
        }
    }
}