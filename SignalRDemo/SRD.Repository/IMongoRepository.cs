using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using SRD.Core;

namespace SRD.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMongoRepository<T>:IRepository<T>
        where T :IUpdateEntity
    {
        /// <summary>
        /// 获取当前集合
        /// </summary>
        /// <returns></returns>
        IMongoCollection<T> Collection { get; }

        /// <summary>
        /// 获取当前数据库
        /// </summary>
        /// <returns></returns>
        IMongoDatabase Database { get; }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        IMongoDatabase GetDatabase(string dbName);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection(string collectionName);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection(string dbName, string collectionName);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        int Update(FilterDefinition<T> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="filter"></param>
        int Delete(FilterDefinition<T> filter);

        /// <summary>
        /// Aggregate操作
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IAggregateFluent<T> AggregateMatch(FilterDefinition<T> filter);
    }
}
