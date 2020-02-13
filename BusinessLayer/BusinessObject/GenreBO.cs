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
    public class GenreBO: BaseBusinessObject
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        //-----------------------------------------------------------

        readonly IUnityContainer unityContainer;
        public GenreBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<GenreBO> LoadAll()  //из DataObj в BusinessObj
        {
            var genres = unitOfWork.Genres.GetAll();
            var res = genres.AsEnumerable().Select(a => mapper.Map<GenreBO>(a)).ToList();
            return res;
        }

        public GenreBO Load(int id)
        {
            var genre = unitOfWork.Genres.GetById(id);
            return mapper.Map(genre, this);
        }
        public void Save(GenreBO genreBO)
        {
            var genre = mapper.Map<Genres>(genreBO);
            if (genreBO.Id == 0) {
                Add(genre);
            }
            else {
                Update(genre);
            }
            unitOfWork.Genres.Save();
        }
        private void Add(Genres genre)
        {
            unitOfWork.Genres.Create(genre);
        }
        private void Update(Genres genre)
        {
            unitOfWork.Genres.Update(genre);
        }
        public void DeleteSave(GenreBO genreBO)
        {
            var genre = mapper.Map<Genres>(genreBO);
            unitOfWork.Genres.Delete(genre.Id);
            unitOfWork.Genres.Save();
        }
    }
}
