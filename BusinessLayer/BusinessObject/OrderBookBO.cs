using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using WebLibrary;
using WebLibrary.Repository;

namespace BusinessLayer.BusinessObject
{
    public class OrderBookBO: BaseBusinessObject
    {
        public int Id { get; set; }
        public int? UsersId { get; set; }
        virtual public UserBO Users { get; set; }
        
        public int? BooksId { get; set; }
        virtual public BooksBO Books { get; set; }
        //-----------------------------------------------------------

        readonly IUnityContainer unityContainer;
        public OrderBookBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<OrderBookBO> LoadAll()  //из DataObj в BusinessObj
        {
            var orders = unitOfWork.OrderBooks.GetAll();
            var res = orders.AsEnumerable().Select(a => mapper.Map<OrderBookBO>(a)).ToList();
            return res;
        }

        public OrderBookBO Load(int id)
        {
            var order = unitOfWork.OrderBooks.GetById(id);
            return mapper.Map(order, this);
        }
        public void Save(OrderBookBO orderBO)
        {
            var order = mapper.Map<OrderBooks>(orderBO);
            if (orderBO.Id == 0) {
                Add(order);
            }
            else {
                Update(order);
            }
            unitOfWork.OrderBooks.Save();
        }
        private void Add(OrderBooks order)
        {
            unitOfWork.OrderBooks.Create(order);
        }
        private void Update(OrderBooks order)
        {
            unitOfWork.OrderBooks.Update(order);
        }
        public void DeleteSave(OrderBookBO orderBO)
        {
            var order = mapper.Map<OrderBooks>(orderBO);
            unitOfWork.OrderBooks.Delete(order.Id);
            unitOfWork.OrderBooks.Save();
        }
    }
}
