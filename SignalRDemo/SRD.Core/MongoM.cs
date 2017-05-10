using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
namespace SRD.Core
{
    /// <summary>
    /// MongoDB 连接基类
    /// </summary>
    public partial class MongoM:IDisposable
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;

        public MongoM()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("SRD");
        }


        public void Dispose()
        {
            if (_client != null)
                _client = null;
        }
    }
}
