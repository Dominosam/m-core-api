using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mover.Data.Interfaces
{
    public interface IMongoDbContext
    {
        public IMongoDatabase GetDatabase();
    }
}
