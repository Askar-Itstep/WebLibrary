using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary.Repository;
using DataLayer.UnitOfWork;
using AutoMapper;

namespace BusinessLayer.BusinessObject
{
    public class BaseBusinessObject
    {
        protected IMapper mapper;
        UnitOfWorkFactory unitOfWorkFactory;
        protected UnitOfWork unitOfWork;
        //public UnitOfWork uow { get; set; }

        public BaseBusinessObject(IMapper mapper, UnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
    }
}
