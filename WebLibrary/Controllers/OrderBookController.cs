using AutoMapper;
using BusinessLayer.BusinessObject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLibrary.Repository;
using WebLibrary.ViewModels;

namespace WebLibrary.Controllers
{
    public class OrderBookController : Controller
    {
        private UnitOfWork unitOfWork;
        private IMapper mapper;
        public OrderBookController(IMapper mapper)
        {
            this.mapper = mapper;
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            var ordersBO = DependencyResolver.Current.GetService<OrderBookBO>().LoadAll();
            var ordersVM = mapper.Map<List<OrderBookVM>>(ordersBO);

            var usersBO = DependencyResolver.Current.GetService<UserBO>().LoadAll();
            var usersVM = usersBO.Select(u => mapper.Map<UserVM>(u));
            ViewBag.UserList = new SelectList(usersVM.ToList(), "Id", "Username"); 
                //ordersVM.ToList(), "Id", "Users.UserName");

            var booksBO = DependencyResolver.Current.GetService<BooksBO>().LoadAll();
            var booksVM = mapper.Map <List<BookVM>> (booksBO);
            ViewBag.BookList = new SelectList(booksVM.ToList(), "Id", "Title");
            
            return View(ordersVM.ToList());

        }

        [HttpGet]
        public ActionResult Create()    //fix double click (переопред. ajax для ссылки)
        {
            var userId = Request["UsersId"];
            var bookId = Request["BooksId"];

            var usersBO = DependencyResolver.Current.GetService<UserBO>().LoadAll();
            var usersVM = mapper.Map<List<UserVM>>(usersBO);
            ViewBag.UserList = new SelectList(usersVM.ToList(), "Id", "UserName");
            var booksBO = DependencyResolver.Current.GetService<BooksBO>().LoadAll();
            var booksVM = booksBO.Select(b => mapper.Map<BooksBO>(b));
            ViewBag.BookList = new SelectList(booksVM, "Id", "Title");

            if (userId != null && bookId != null)
            {
                var order = new OrderBookVM();
                order.UsersId = int.Parse(userId);
                order.BooksId = int.Parse(bookId);

                var orderBO = mapper.Map<OrderBookBO>(order);
                orderBO.Save(orderBO);
                var newOrdersBO = DependencyResolver.Current.GetService<OrderBookBO>().LoadAll();
                var newOrdersVM = newOrdersBO.Select(o => mapper.Map<OrderBookVM>(o));
                return PartialView("Partial/_OrderPartialView", newOrdersVM.ToList());
            }
            return PartialView("Partial/_CreatePartialView");

        }
        
        [HttpGet]
        public ActionResult GetTop5(int? userId)    //вызов из index/ajax
        {
            List<BookVM> bookList = GetBooks(userId);
            var data = JsonConvert.SerializeObject(bookList);
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Edit(int? id)   //id-orders
        {
            if (id == null)
            {
                return HttpNotFound();
            }
           var usersBO = DependencyResolver.Current.GetService<UserBO>().LoadAll();
            var usersVM = usersBO.Select(u=>mapper.Map<UserVM>(u));
            ViewBag.UserList = new SelectList(usersVM.ToList(), "Id", "UserName");
            var booksBO = DependencyResolver.Current.GetService<BooksBO>().LoadAll();
            var booksVM = booksBO.Select(b => mapper.Map<BooksBO>(b));
            ViewBag.BookList = new SelectList(booksVM, "Id", "Title");

            OrderBookBO orderBO = DependencyResolver.Current.GetService<OrderBookBO>().Load((int)id);
            var order = mapper.Map<OrderBookVM>(orderBO);
            List<BookVM> bookListBO = GetBooks(order.UsersId);
            var bookList = bookListBO.Select(b => mapper.Map<BookVM>(b)).ToList();
            ViewBag.OrderedBookList = bookList; //исп-ся в _Top5Orders, включенный в Edit-page
            return View(order);
        }


        private List<BookVM> GetBooks(int? userId)//-выбрать 5 книг текущ. юзера--
        {
            var orderBooksBO = DependencyResolver.Current.GetService<OrderBookBO>().LoadAll().Where(o => o.UsersId == userId).Take(5).ToList();
            List<BooksBO> bookListBO = new List<BooksBO>();
            orderBooksBO.ForEach(order =>
            {
                bookListBO.Add(DependencyResolver.Current.GetService<BooksBO>().LoadAll()
                    .Where(b => b.Id == order.BooksId).FirstOrDefault());
            });
            var bookList = bookListBO.Select(b => mapper.Map<BookVM>(b)).ToList();
            return bookList;

        }

        [HttpPost]
        public ActionResult Edit(OrderBookVM orderBook)
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }
            
            var orderBookBO = mapper.Map<OrderBookBO>(orderBook);
            orderBookBO.Save(orderBookBO);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var orderBO = DependencyResolver.Current.GetService<OrderBookBO>().Load((int)id);
            var orderBookVM = mapper.Map<OrderBookVM>(orderBO);
            ViewBag.UserName = orderBookVM.Users.UserName;    //достат. 1-го обращения (virtual!)
            ViewBag.BookTitle = orderBookVM.Books.Title;
            return View(orderBookVM);
        }

        [HttpPost]
        public ActionResult Delete(OrderBookVM orderBook)
        {
            if (orderBook == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (orderBook.BooksId == null || orderBook.UsersId == null)
                {
                    OrderBookBO orderBO = DependencyResolver.Current.GetService<OrderBookBO>().Load(orderBook.Id);
                    orderBO.DeleteSave(orderBO);
                    return RedirectToAction("Index");
                }
                else return HttpNotFound();
            }
            else
                return View(orderBook);

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            OrderBookBO orderBO = DependencyResolver.Current.GetService<OrderBookBO>().Load((int)id);
            var orderVM = mapper.Map<OrderBookVM>(orderBO);
            var username = orderVM.Users.UserName;    //достат. 1-го обращения (virtual!)
            var title = orderVM.Books.Title;
            return View(orderVM);

        }

    }
}