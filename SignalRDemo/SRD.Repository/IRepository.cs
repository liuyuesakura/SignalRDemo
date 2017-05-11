using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using SRD.Core;

namespace SRD.Repository
{
    public interface IRepository<T>
        where T :SRD.Core.IUpdateEntity
    {
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int Insert(T obj);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="items"></param>
        int InsertBatch(IEnumerable<T> items);

        /// <summary>
        /// 不存在插入，存在时更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int InsertOrUpdate(T obj);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="obj"></param>
        /// <param name="removefields"></param>
        /// <returns></returns>
        int Update(Expression<Func<T, bool>> exp, object obj, params string[] removefields);

        /// <summary>
        /// 更新加操作
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int UpdateInc(Expression<Func<T, bool>> exp, Expression<Func<T, int>> field, int value);

        /// <summary>
        /// 更新加操作
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int UpdateInc(Expression<Func<T, bool>> exp, Expression<Func<T, long>> field, long value);

        /// <summary>
        /// 更新乘操作
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int UpdateMul(Expression<Func<T, bool>> exp, Expression<Func<T, int>> field, int value);

        /// <summary>
        /// 更新乘操作
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int UpdateMul(Expression<Func<T, bool>> exp, Expression<Func<T, long>> field, long value);

        /// <summary>
        /// 转换成Linq查询
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// 是否存在符合条件的数据
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 查询符合条件的单个对象
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 查询符合条件的单个对象
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc);

        /// <summary>
        /// 查询符合条件的单个对象到新的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        TResult Get<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector);

        /// <summary>
        /// 查询符合条件的单个对象到新的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        TResult Get<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc);

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        List<T> List(Expression<Func<T, bool>> exp);

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        List<TResult> List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector);


        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="selectSize"></param>
        /// <returns></returns>
        List<TResult> List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, int selectSize);


        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        List<T> List(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc);

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <param name="selectSize"></param>
        /// <returns></returns>
        List<T> List(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc, int selectSize);

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        List<TResult> List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc);

        /// <summary>
        /// 查询符合条件的对象列表
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="selector"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <param name="selectSize"></param>
        /// <returns></returns>
        List<TResult> List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc, int selectSize);

        /// <summary>
        /// 查询符合条件的对象列表.分页
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sort"></param>
        /// <param name="isDesc"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<T> List(Expression<Func<T, bool>> exp, Func<T, object> sort, bool isDesc, int index, int pageSize, out int count);

        /// <summary>
        /// 查询符合条件的对象列表.分页
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sort"></param>
        /// <param name="selector"></param>
        /// <param name="isDesc"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<TResult> List<TResult>(Expression<Func<T, bool>> exp, Func<T, TResult> selector, Func<T, object> sort, bool isDesc, int index, int pageSize, out int count);

        /// <summary>
        /// Aggregate操作 (类似与MSSQL的聚合函数？)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        IAggregateFluent<T> AggregateMatch(Expression<Func<T, bool>> exp);
    }
}
