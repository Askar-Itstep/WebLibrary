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
using WebLibrary.Entities;

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
                .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>());

                mpr.CreateMap<AuthorBO, Authors>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Authors>());

                mpr.CreateMap<IEnumerable<Authors>, List<AuthorBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<AuthorBO>>()).ReverseMap();

                mpr.CreateMap<AuthorBO, AuthorVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorVM>());

                mpr.CreateMap<AuthorVM, AuthorBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>());

                //------------------------------------------------------------------------

                mpr.CreateMap<Books, BooksBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<BooksBO>());

                mpr.CreateMap<BooksBO, Books>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<Books>());

                mpr.CreateMap<IEnumerable<BooksBO>, List<Books>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<Books>>());

                mpr.CreateMap<BooksBO, BookVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<BookVM>());

                mpr.CreateMap<BookVM, BooksBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<BooksBO>());

                //------------------------------------------------------------------------

                mpr.CreateMap<Genres, GenreBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GenreBO>());
                mpr.CreateMap<GenreBO, Genres>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<Genres>());

                mpr.CreateMap<IEnumerable<Genres>, List<GenreBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<GenreBO>>());

                mpr.CreateMap<GenreBO, GenreVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GenreVM>());
                mpr.CreateMap<GenreVM, GenreBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GenreBO>());
                //------------------------------------------------------------------------

                mpr.CreateMap<Images, ImageBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<ImageBO>());
                mpr.CreateMap<ImageBO, Images>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<Images>());
                mpr.CreateMap<IEnumerable<Images>, List<ImageBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<ImageBO>>());

                mpr.CreateMap<ImageBO, ImageVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<ImageVM>());

                mpr.CreateMap<ImageVM, ImageBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<ImageBO>());
                //------------------------------------------------------------------------

                mpr.CreateMap<OrderBooks, OrderBookBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<OrderBookBO>()).ReverseMap();

                mpr.CreateMap<IEnumerable<OrderBooks>, List<OrderBookBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<OrderBookBO>>()).ReverseMap();

                mpr.CreateMap<OrderBookBO, OrderBookVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<OrderBookVM>()).ReverseMap();
                mpr.CreateMap<OrderBookVM, OrderBookBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<OrderBookBO>()).ReverseMap();
                //------------------------------------------------------------------------

                mpr.CreateMap<Statistic, StatisticBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<StatisticBO>()).ReverseMap();

                mpr.CreateMap<IEnumerable<Statistic>, List<StatisticBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<StatisticBO>>()).ReverseMap();

                mpr.CreateMap<StatisticBO, StatisticVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<StatisticVM>());
                mpr.CreateMap<StatisticVM, StatisticBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<StatisticBO>());

                //------------------------------------------------------------------------

                mpr.CreateMap<Users, UserBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<UserBO>()).ReverseMap();

                mpr.CreateMap<IEnumerable<Users>, List<UserBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<UserBO>>()).ReverseMap();

                mpr.CreateMap<UserBO, UserVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<UserVM>());

                mpr.CreateMap<IEnumerable<UserVM>, List<UserBO>>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<List<UserBO>>());
            });
        }
    }
}