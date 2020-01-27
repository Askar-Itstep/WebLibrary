using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using WebLibrary.ViewModels;
using BusinessLayer;
using BusinessLayer.BusinessObject;

namespace WebLibrary.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterWithUnity(UnityContainer container)
        {
            IMapper mapper = CreateMapperConfiguration().CreateMapper();
            container.RegisterInstance<IMapper>(mapper);
        }

        public static MapperConfiguration CreateMapperConfiguration()
        {
            return new MapperConfiguration(mpr =>
            {
                mpr.CreateMap<Authors, AuthorBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>()).ForMember("AuthorName", opt=>opt.MapFrom(
                    c=>c.FirstName + " " + c.LastName));

                mpr.CreateMap<AuthorBO, Authors>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Authors>()).ForMember("FirstName", opt=>opt.MapFrom(
                    c=>c.AuthorName.Split(' ')[0])).ForMember("LastName", opt=>opt.MapFrom(c=>c.AuthorName.Split(' ')[1]));
                        
                
                mpr.CreateMap<IEnumerable<Authors>, List<AuthorBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<AuthorBO>>());
                //------------------------------------------------------------------------

                mpr.CreateMap<AuthorBO, AuthorViewModel>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<AuthorViewModel>());
                .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorViewModel>()).ForMember("FirstName", opt => opt.MapFrom(
                    c => c.AuthorName.Split(' ')[0])).ForMember("LastName", opt => opt.MapFrom(c => c.AuthorName.Split(' ')[1]));
                          

                mpr.CreateMap<AuthorViewModel, AuthorBO>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>());
                .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>()).ForMember("AuthorName", opt => opt.MapFrom(
                    c => c.FirstName + " " + c.LastName));

            });
        }
    }
}