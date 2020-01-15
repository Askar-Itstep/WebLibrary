using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary.Entities;

namespace WebLibrary.Repository
{
    interface IUnitOfWork
    {
        BaseRepository<Authors> Authors { get; }
        BaseRepository<Books> Books { get; }
        BaseRepository<Genres> Genres { get; }
        BaseRepository<Images> Images { get; }
        BaseRepository<OrderBooks> OrderBooks { get; }
        BaseRepository<Statistic> Statistics { get; }
        BaseRepository<Users> Users { get; }
    }
}
