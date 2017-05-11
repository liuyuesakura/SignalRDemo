using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SRD.Core;
using SRD.Model;
using SRD.Repository;

namespace SRD.MongoDao
{
    public static class DBContext
    {
        private const string DBNAME = ".SRD";

        public static IMongoRepository<User> User
        {
            get
            {
                return RepositoryFactory.CreateMongodb<User>(DBNAME, "User");
            }
        }
        public static IMongoRepository<T> GetRepository<T>()
            where T : IUpdateEntity
        {
            return RepositoryFactory.CreateMongodb<T>(DBNAME, typeof(T).Name);
        }
    }
}
