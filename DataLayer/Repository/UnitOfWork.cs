using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLibrary.Entities;

namespace WebLibrary.Repository
{
    //Паттерн Unit of Work позволяет упростить работу с различными репозиториями - 
    //    т.е. все репозитории будут использовать один и тот же контекст данных.
    public class UnitOfWork : IUnitOfWork
    {
        private BaseRepository<Authors> authors;
        private BaseRepository<Books> books;
        private BaseRepository<Genres> genres;
        private BaseRepository<Images> images;
        private BaseRepository<OrderBooks> orderBooks;
        private BaseRepository<Statistic> statistics;
        private BaseRepository<Users> users;
        private Model1 db;
        public UnitOfWork()
        {
            db = new Model1();
        }
        public BaseRepository<Authors> Authors
        {
            get {
                if (authors == null)
                    authors = new BaseRepository<Authors>();
                return authors;
            }
        }

        public BaseRepository<Books> Books
        {
            get
            {
                if (books == null)
                    books = new BaseRepository<Books>();
                return books;
            }
        }

        public BaseRepository<Genres> Genres
        {
            get
            {
                if (genres == null)
                    genres = new BaseRepository<Genres>();
                return genres;
            }
        }

        public BaseRepository<Images> Images
        {
            get
            {
                if (images == null)
                    images = new BaseRepository<Images>();
                return images;
            }
        }

        public BaseRepository<OrderBooks> OrderBooks
        { 
            get
            {
                if (orderBooks == null)
                    orderBooks = new BaseRepository<OrderBooks>();
                return orderBooks;
            }
        }

        public BaseRepository<Statistic> Statistics
        {
            get
            {
                if (statistics == null)
                    statistics = new BaseRepository<Statistic>();
                return statistics;
            }
        }

        public BaseRepository<Users> Users
        {
            get
            {
                if (users == null)
                    users = new BaseRepository<Users>();
                return users;
            }
        }
    }
}