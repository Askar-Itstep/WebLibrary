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
    public class AuthorBO:BaseBusinessObject
    {
        readonly IUnityContainer unityContainer;

        public int Id { get; set; }
        public string AuthorName { get; set; }

        public AuthorBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<AuthorBO> LoadAll()  //из DataObj в BusinessObj
        {
            var authors = unitOfWork.Authors.GetAll();
            ////mapper.Map<IEnumerable<Author>, List<AuthorBO>>(authors);
            //authors.Select(a => mapper.Map<AuthorBO>(a)).ToList();
            var res = authors.AsEnumerable().Select(a => mapper.Map<AuthorBO>(a)).ToList();
            return res;
        }

        public void Load(int id)
        {
            var author = unitOfWork.Authors.GetById(id);
            mapper.Map(author, this);
        }
        public void Save(AuthorBO authorBO)
        {
            var author = mapper.Map<Authors>(authorBO);
            if (authorBO.Id == null) {
                Add(author);
            }
            else {
                Update(author);
            }
            unitOfWork.Authors.Save();
        }
        private void Add(Authors author)
        {
            unitOfWork.Authors.Create(author);
        }
        private void Update(Authors author)
        {
            unitOfWork.Authors.Update(author);
        }
        public void DeleteSave(AuthorBO authorBO)
        {
            var author = mapper.Map<Authors>(authorBO);
            unitOfWork.Authors.Delete(author.Id);
            unitOfWork.Authors.Save();
        }
    }
}
