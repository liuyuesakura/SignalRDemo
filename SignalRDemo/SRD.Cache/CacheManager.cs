using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRD.Cache
{
    /// <summary>
    /// 缓存的主管理入口
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// 获取访问次数，限制频率
        /// </summary>
        /// <param name="cacheName">ip+接口名称，如61.164.43.236ykjdetail</param>
        /// <param name="cacheTime">单位分钟</param>
        /// <returns></returns>
        public static int GetVisits(string cacheName, double cacheTime)
        {
            try
            {
                int visitNum = 0;
                using (var redisClient = RedisManager.GetClient())
                {
                    redisClient.Password = RedisManager.password;
                    if (redisClient.ContainsKey(cacheName))
                    {
                        visitNum = redisClient.Get<int>(cacheName);
                        visitNum += 1;
                    }
                    redisClient.Set<int>(cacheName, visitNum);
                    redisClient.ExpireEntryAt(cacheName, DateTime.Now.AddMinutes(cacheTime));
                }
                return visitNum;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取一个elementkey对应的字符串值
        /// </summary>
        /// <param name="elementKey"></param>
        /// <returns></returns>
        public static string GetRedisContent(string elementKey)
        {

            using (var rc = RedisManager.GetClient())
            {
                string res = rc.Get<string>(elementKey);
                if (string.IsNullOrEmpty(res))
                    res = "0";
                return res;
            }
        }
        public static T GetRedisContent<T>(string elementKey)
        {
            using (var rc = RedisManager.GetClient())
            {
                try
                {
                    var res = rc.Get<T>(elementKey);
                    if (res == null)
                        return default(T);
                    return res;
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }
        /// <summary>
        /// 将一个字符串写入redis
        /// </summary>
        /// <param name="elementKey"></param>
        /// <param name="elementContent"></param>
        /// <returns></returns>
        public static bool SetRedisContent(string elementKey, string elementContent)
        {
            using (var rc = RedisManager.GetClient())
            {
                return rc.Set(elementKey, elementContent) && rc.Set(elementKey + "TimeStemp", DateTime.Now);
            }
        }
        public static bool SetRedisContent<T>(string key, T content)
        {
            using (var rc = RedisManager.GetClient())
            {
                try
                {
                    return rc.Set<T>(key, content);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 从redis中读取list数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elementKey"></param>
        /// <returns></returns>
        public static List<T> GetRedisList<T>(string elementKey)
        {
            using (var vc = RedisManager.GetClient())
            {
                List<T> res = vc.Get<List<T>>(elementKey);
                if (res == null)
                    res = new List<T>();
                return res;
            }
        }
        /// <summary>
        /// 向redis中写入list数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elementKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool SetRedisList<T>(string elementKey, T content)
        {
            using (var rc = RedisManager.GetClient())
            {
                List<T> res = rc.Get<List<T>>(elementKey);
                if (res == null)
                    res = new List<T>();
                res.Add(content);
                return rc.Set<List<T>>(elementKey, res);
            }
        }
        public static bool SetRedisList<T>(string elementKey, List<T> content)
        {
            using (var rc = RedisManager.GetClient())
            {
                List<T> res = rc.Get<List<T>>(elementKey);
                if (res == null)
                    res = new List<T>();
                content.ForEach(x => { res.Add(x); });
                res = res.Distinct().ToList();
                return rc.Set<List<T>>(elementKey, res);
            }
        }
        /// <summary>
        /// 如果距离上次更新时间超过一个小时，则重写缓存
        /// </summary>
        /// <param name="elementKey"></param>
        /// <returns></returns>
        public static bool IsNeedRefresh(string elementKey)
        {
            using (var rc = RedisManager.GetClient())
            {
                DateTime t = rc.Get<DateTime>(elementKey + "TimeStemp");
                if (t == null)
                    return true;
                if ((DateTime.Now - t).TotalHours > 1)
                    return true;
                else
                    return false;
            }
        }
    }
}
