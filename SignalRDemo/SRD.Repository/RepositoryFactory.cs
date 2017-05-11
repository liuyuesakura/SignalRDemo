using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SRD.Core;
using MySoft;
using MySoft.Cache;

namespace SRD.Repository
{
    /// <summary>
    /// 仓库工厂
    /// </summary>
    public class RepositoryFactory
    {
                /// <summary>
        /// 创建Mongodb数据工厂
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IMongoRepository<T> CreateMongodb<T>(string key) where T : IUpdateEntity
        {
            var keyString = string.Format("{typeof(T).FullName}_{key}");

            return CacheHelper<IMongoRepository<T>>.Get(keyString, TimeSpan.FromHours(1), () =>
            {
                return new MongodbRepository<T>(key);
            });
        }

        /// <summary>
        /// 创建Mongodb数据工厂
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static IMongoRepository<T> CreateMongodb<T>(string key, string collectionName) where T : IUpdateEntity
        {
            //var keyString = string.Format("{typeof(T).FullName}_{key}");

            //return CacheHelper<IMongoRepository<T>>.Get(keyString, TimeSpan.FromHours(1), () =>
            //{
            //    return new MongodbRepository<T>(key, collectionName);
            //});
            return new MongodbRepository<T>(key, collectionName);
        }
    }
}
