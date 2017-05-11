using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace SRD.Repository
{
    /// <summary>
    /// MongoDB工厂类
    /// </summary>
    public class MongoFactory
    {
        private const string Base = "Mongodb{0}.{1}";

        string _connection;
        string _database;

        /// <summary>
        /// 
        /// </summary>
        public MongoFactory()
        {
            InitSetting(string.Empty);
        }

        public MongoFactory(string key)
        {
            InitSetting(key);
        }

        void InitSetting(string key)
        {
            _connection = string.Format(Base, key, "Connection");//ConfigHelper.GetString(string.Format(Base, key, "Connection"));
            _database = string.Format(Base, key, "Database");//ConfigHelper.GetString(string.Format(Base, key, "Database"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient(_connection);
            return client.GetDatabase(_database);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IMongoDatabase GetMongoDatabase(string dbName)
        {
            var client = new MongoClient(_connection);
            return client.GetDatabase(dbName);
        }
    }
}
