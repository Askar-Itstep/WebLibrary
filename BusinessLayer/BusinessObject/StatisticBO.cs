using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using WebLibrary.Entities;
using WebLibrary.Repository;

namespace BusinessLayer.BusinessObject
{
    public class StatisticBO: BaseBusinessObject
    {
        public int Id { get; set; }
        public int CountAuthorChoice { get; set; }
        public int CountTitleChoice { get; set; }
        public int CountGenreChoice { get; set; }
        public int CountIsStatisticChoice { get; set; }
        //-----------------------------------------------------------

        readonly IUnityContainer unityContainer;
        public StatisticBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<StatisticBO> LoadAll() 
        {
            var Statistics = unitOfWork.Statistics.GetAll();
            var res = Statistics.AsEnumerable().Select(a => mapper.Map<StatisticBO>(a)).ToList();
            return res;
        }

        public void Load(int id)
        {
            var Statistic = unitOfWork.Statistics.GetById(id);
            mapper.Map(Statistic, this);
        }
        public void Save(StatisticBO statisticBO)
        {
            var statistic = mapper.Map<Statistic>(statisticBO);
            if (statisticBO.Id == 0) {
                Add(statistic);
            }
            else {
                Update(statistic);
            }
            unitOfWork.Statistics.Save();
        }
        private void Add(Statistic statistic)
        {
            unitOfWork.Statistics.Create(statistic);
        }
        private void Update(Statistic statistic)
        {
            unitOfWork.Statistics.Update(statistic);
        }
        public void DeleteSave(StatisticBO statisticBO)
        {
            var statistic = mapper.Map<Statistic>(statisticBO);
            unitOfWork.Statistics.Delete(statistic.Id);
            unitOfWork.Statistics.Save();
        }
    }
}
