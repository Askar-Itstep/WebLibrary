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
    public class UserBO: BaseBusinessObject
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        //-----------------------------------------------------------

        readonly IUnityContainer unityContainer;
        public UserBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<UserBO> LoadAll()  //из DataObj в BusinessObj
        {
            var users = unitOfWork.Users.GetAll();
            var res = users.AsEnumerable().Select(a => mapper.Map<UserBO>(a)).ToList();
            return res;
        }

        public void Load(int id)
        {
            var user = unitOfWork.Users.GetById(id);
            mapper.Map(user, this);
        }
        public void Save(UserBO userBO)
        {
            var user = mapper.Map<Users>(userBO);
            if (userBO.Id == 0) {
                Add(user);
            }
            else {
                Update(user);
            }
            unitOfWork.Users.Save();
        }
        private void Add(Users user)
        {
            unitOfWork.Users.Create(user);
        }
        private void Update(Users user)
        {
            unitOfWork.Users.Update(user);
        }
        public void DeleteSave(UserBO userBO)
        {
            var user = mapper.Map<Users>(userBO);
            unitOfWork.Users.Delete(user.Id);
            unitOfWork.Users.Save();
        }
    }
}
