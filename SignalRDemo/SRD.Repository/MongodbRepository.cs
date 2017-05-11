using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using SRD.Core;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Reflection;

using LinqKit;

//using MySoft;


namespace SRD.Repository
{
    public class MongodbRepository<T>:IMongoRepository<T>
        where T :IUpdateEntity
    {
         private readonly MongoFactory _dbfactory;
        private readonly IMongoDatabase _database;
        private string _tableName;

        /// <summary>
        /// 获取一个新的集合
        /// </summary>
        private IMongoCollection<T> GetNewCollection()
        {
            return _database.GetCollection<T>(_tableName);
        }

        /// <summary>
        /// 初始化MongodbRepository
        /// </summary>
        /// <param name="key"></param>
        public MongodbRepository(string key)
        {
            var tableName = GetTableName(typeof(T));
            tableName = !string.IsNullOrWhiteSpace(tableName) ? GetTableName(typeof(T)) : typeof(T).Name;

            _dbfactory = new MongoFactory(key);
            _tableName = tableName;
            _database = _dbfactory.GetMongoDatabase();
        }

        /// <summary>
        /// 初始化MongodbRepository
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tableName"></param>
        public MongodbRepository(string key, string tableName)
        {
            _dbfactory = new MongoFactory(key);
            _tableName = tableName;
            _database = _dbfactory.GetMongoDatabase();
        }

        private string GetTableName(MemberInfo type)
        {
            var attribute = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
            return attribute != null ? ((TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute))).Name : string.Empty;
        }

        int IRepository<T>.Insert(T obj)
        {
            obj.LastUpdateTime = DateTime.UtcNow;
            obj.NeedUpdate = true;

            GetNewCollection().InsertOne(obj);
            return 1;
        }

        int IRepository<T>.InsertBatch(IEnumerable<T> items)
        {
            items.ForEach(x =>
            {
                x.LastUpdateTime = DateTime.UtcNow;
                x.NeedUpdate = true;
            });

            GetNewCollection().InsertMany(items);

            return items.Count();
        }

        int IRepository<T>.InsertOrUpdate(T obj)
        {
            FilterDefinition<T> filter = null;

            //获取BsonId并排除
            var property = GetBsonIdProperty(obj.GetType());
            if (property != null)
            {
                filter = Builders<T>.Filter.Eq("_id", property.FastGetValue(obj));
            }

            if (filter == null) return 0;

            var options = new UpdateOptions { IsUpsert = true };

            obj.LastUpdateTime = DateTime.UtcNow;
            obj.NeedUpdate = true;

            return Convert.ToInt32(GetNewCollection().ReplaceOne(filter, obj, options).ModifiedCount);
        }

        int IRepository<T>.Delete(Expression<Func<T, bool>> exp)
        {
            var filter = new ExpressionFilterDefinition<T>(exp.Expand());
            return (this as IMongoRepository<T>).Delete(filter);
        }

        int IRepository<T>.Update(Expression<Func<T, bool>> exp, object obj, params string[] removefields)
        {
            if (obj == null || !obj.GetType().IsClass)
                throw new ArgumentNullException(string.Format("更新对象必须是class类型"));

            //构造update结构
            var list = new List<string> { "LastUpdateTime", "NeedUpdate" };

            //获取BsonId并排除
            var property = GetBsonIdProperty(obj.GetType());
            if (property != null) list.Add(property.Name);

            if (removefields != null && removefields.Length > 0) list.AddRange(removefields);

            var filter = new ExpressionFilterDefinition<T>(exp.Expand());
            var update = Builders<T>.Update.Set(x => x.LastUpdateTime, DateTime.UtcNow).Set(x => x.NeedUpdate, true);

            //移除不需要的项
            var bsonDoc = BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            foreach (var p in bsonDoc.Elements)
            {
                if (list.Contains(p.Name)) continue;
                update = update.Set(p.Name, GetBsonValue(p.Value));
            }

            return UpdateData(filter, update, null);
        }

        int IRepository<T>.UpdateInc(Expression<Func<T, bool>> exp, Expression<Func<T, int>> field, int value)
        {
            var filter = new ExpressionFilterDefinition<T>(exp.Expand());
            var update = Builders<T>.Update.Inc(field, value);

            return (this as IMongoRepository<T>).Update(filter, update);
        }

        int IRepository<T>.UpdateInc(Expression<Func<T, bool>> exp, Expression<Func<T, long>> field, long value)
        {
            var filter = new ExpressionFilterDefinition<T>(exp.Expand());
            var update = Builders<T>.Update.Inc(field, value);

            return (this as IMongoRepository<T>).Update(filter, update);
        }

        int IRepository<T>.UpdateMul(Expression<Func<T, bool>> exp, Expression<Func<T, int>> field, int value)
        {
            var filter = new ExpressionFilterDefinition<T>(exp.Expand());
            var update = Builders<T>.Update.Mul(field, value);

            return (this as IMongoRepository<T>).Update(filter, update);
        }

        int IRepository<T>.UpdateMul(Expression<Func<T, bool>> exp, Expression<Func<T, long>> field, long value)
        {
            var filter = new ExpressionFilterDefinition<T>(exp.Expand());
            var update = Builders<T>.Update.Mul(field, value);

            return (this as IMongoRepository<T>).Update(filter, update);
        }

        private BsonValue GetBsonValue(BsonValue value)
        {
            //处理时间类型
            if (value != null && value.BsonType == BsonType.String)
            {
                try { return BsonDateTime.Create(value.AsString); } catch { }
            }

            return value;
        }

        /// <summary>
        /// 转换成Linq查询
        /// </summary>
        /// <returns></returns>
        IQueryable<T> IRepository<T>.AsQueryable()
        {
            return GetNewCollection().AsQueryable();
        }

        bool IRepository<T>.Exists(Expression<Func<T, bool>> exp)
        {
            return GetNewCollection().AsQueryable().Any(exp.Expand());
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> exp)
        {
            return GetNewCollection().AsQueryable().FirstOrDefault(exp.Expand());
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.FirstOrDefault();
        }

        TResult IRepository<T>.Get<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector)
        {
            return GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel().Select(selector).FirstOrDefault();
        }

        TResult IRepository<T>.Get<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.Select(selector).FirstOrDefault();
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        int IRepository<T>.Count(Expression<Func<T, bool>> exp)
        {
            return Convert.ToInt32(GetNewCollection().AsQueryable().Count(exp.Expand()));
        }

        List<T> IRepository<T>.List(Expression<Func<T, bool>> exp)
        {
            return GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel().ToList();
        }

        List<T> IRepository<T>.List(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.ToList();
        }

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isDesc"></param>
        /// <param name="selectSize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        List<T> IRepository<T>.List(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc, int selectSize)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.Take(selectSize).ToList();
        }

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        List<TResult> IRepository<T>.List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.Select(selector).ToList();
        }

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        List<TResult> IRepository<T>.List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector)
        {
            return GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel().Select(selector).ToList();
        }

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="selectSize"></param>
        /// <returns></returns>
        List<TResult> IRepository<T>.List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, int selectSize)
        {
            return GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel().Select(selector).Take(selectSize).ToList();
        }

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="isDesc"></param>
        /// <param name="selectSize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        List<TResult> IRepository<T>.List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc, int selectSize)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.Select(selector).Take(selectSize).ToList();
        }

        List<T> IRepository<T>.List(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc, int pageIndex, int pageSize,
            out int count)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            count = Convert.ToInt32(queryable.Count());

            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        List<TResult> IRepository<T>.List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc, int pageIndex, int pageSize,
            out int count)
        {
            var queryable = GetNewCollection().AsQueryable().Where(exp.Expand()).AsParallel();
            count = Convert.ToInt32(queryable.Count());

            var sortable = isDesc
                ? queryable.OrderByDescending(sort)
                : queryable.OrderBy(sort);

            return sortable.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(selector).ToList();
        }

        int IMongoRepository<T>.Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            //合并多个update项
            var _update = Builders<T>.Update.Set(x => x.LastUpdateTime, DateTime.UtcNow).Set(x => x.NeedUpdate, true);
            var newupdate = Builders<T>.Update.Combine(update, _update);

            return UpdateData(filter, update, null);
        }

        private int UpdateData(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options)
        {
            return Convert.ToInt32(GetNewCollection().UpdateMany(filter, update, options).ModifiedCount);
        }

        int IMongoRepository<T>.Delete(FilterDefinition<T> filter)
        {
            return Convert.ToInt32(GetNewCollection().DeleteMany(filter).DeletedCount);
        }

        /// <summary>
        /// 当前集合
        /// </summary>
        IMongoCollection<T> IMongoRepository<T>.Collection
        {
            get { return GetNewCollection(); }
        }

        /// <summary>
        /// 当前数据库
        /// </summary>
        IMongoDatabase IMongoRepository<T>.Database
        {
            get { return _database; }
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        IMongoDatabase IMongoRepository<T>.GetDatabase(string dbName)
        {
            return _dbfactory.GetMongoDatabase(dbName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        IMongoCollection<T> IMongoRepository<T>.GetCollection(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        IMongoCollection<T> IMongoRepository<T>.GetCollection(string dbName, string collectionName)
        {
            return _dbfactory.GetMongoDatabase(dbName).GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Aggregate操作
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IAggregateFluent<T> IMongoRepository<T>.AggregateMatch(FilterDefinition<T> filter)
        {
            return GetNewCollection().Aggregate().Match(filter);
        }

        /// <summary>
        /// Aggregate操作
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        IAggregateFluent<T> IRepository<T>.AggregateMatch(Expression<Func<T, bool>> exp)
        {
            return GetNewCollection().Aggregate().Match(exp.Expand());
        }

        private PropertyInfo GetBsonIdProperty(Type type)
        {
            //缓存提高性能
            return DynamicReflectionCache<Type, PropertyInfo>.Get(type, _type =>
            {
                //获取BsonId并排除
                foreach (var p in _type.GetProperties())
                {
                    var attribute = CoreHelper.GetMemberAttribute<BsonIdAttribute>(p);
                    if (attribute != null)
                    {
                        return p;
                    }
                }

                return null;
            });
        }
    }
}
