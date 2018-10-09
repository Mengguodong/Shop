using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using ServiceStack.Common.Extensions;
using ServiceStack.Redis;
using ServiceStack.Logging;

namespace SFO2O.Supplier.Common
{
    public class RedisCacheHelper
    {
        private static readonly PooledRedisClientManager pool = null;
        private static readonly string[] readWriteHosts = null;
        public static int RedisMaxReadPool = int.Parse(ConfigurationManager.AppSettings["redis_max_read_pool"]);
        public static int RedisMaxWritePool = int.Parse(ConfigurationManager.AppSettings["redis_max_write_pool"]);
        static RedisCacheHelper()
        {
            var redisHost = ConfigurationManager.AppSettings["redis_server_session"];

            if (!string.IsNullOrEmpty(redisHost))
            {
                readWriteHosts = redisHost.Split(',');

                if (readWriteHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(readWriteHosts, readWriteHosts,
                        new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = RedisMaxWritePool,
                            MaxReadPoolSize = RedisMaxReadPool,
                            AutoStart = true
                        });
                }
            }
        }
        public static void Add<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, expiry - DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
                LogHelper.Error(ex);
            }

            try
            {
                HttpContext.Current.Cache.Add(key, value, null, expiry, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }
        }

        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, slidingExpiration);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }

            try
            {
                HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.BelowNormal, null);
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }

        }

        [Obsolete("请使用新方法　Add<T>")]
        public static bool Add(string key, object value, int minutes)
        {
            if (value == null)
            {
                return false;
            }
            if (minutes <= 0)
            {
                Remove(key);

                return false;
            }
            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, new TimeSpan(0, minutes, 0));
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }

            return true;
        }
        [Obsolete("请使用新方法　Add<T>")]
        public static bool Add(string key, object value)
        {
            if (value == null)
            {
                return false;
            }
            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, new TimeSpan(0, ConfigHelper.SessionExpireMinutes, 0));
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }

            return true;
        }
        [Obsolete("请使用新方法　Get<T>")]
        public static object Get(string key)
        {
            if (key.Length == 0)
            {
                return null;
            }

            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.Get<string>(key);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = default(T);

            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }
            return obj;
        }

        public static void Remove(string key)
        {
            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;
                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }

            try
            {
                System.Web.HttpContext.Current.Cache.Remove(key);
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }

        }

        public static bool Exists(string key)
        {
            try
            {
                if (pool != null)
                {
                    int index = Math.Abs(key.GetHashCode()) % readWriteHosts.Length;

                    using (var r = pool.GetClient(index))
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.ContainsKey(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
                LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
            }

            return false;
        }

        public static IDictionary<string, T> GetAll<T>(IEnumerable<string> keys) where T : class
        {
            if (keys == null)
            {
                return null;
            }

            keys = keys.Where(k => !string.IsNullOrWhiteSpace(k));

            if (keys.Count() == 1)
            {
                T obj = Get<T>(keys.Single());

                if (obj != null)
                {
                    return new Dictionary<string, T>() { { keys.Single(), obj } };
                }

                return null;
            }

            if (!keys.Any())
            {
                return null;
            }

            IDictionary<string, T> dict = null;

            if (pool != null)
            {
                keys.Select(s => new
                {
                    Index = Math.Abs(s.GetHashCode()) % readWriteHosts.Length,
                    KeyName = s
                })
                .GroupBy(p => p.Index)
                .Select(g =>
                {
                    try
                    {
                        using (var r = pool.GetClient(g.Key))
                        {
                            if (r != null)
                            {
                                r.SendTimeout = 1000;
                                return r.GetAll<T>(g.Select(p => p.KeyName));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", keys.Aggregate((a, b) => a + "," + b));
                        LogManager.GetLogger(typeof(RedisCacheHelper)).Error(msg, ex);
                    }
                    return null;
                })
                .Where(x => x != null)
                .ForEach(d =>
                {
                    d.ForEach(x =>
                    {
                        if (dict == null || !dict.Keys.Contains(x.Key))
                        {
                            if (dict == null)
                            {
                                dict = new Dictionary<string, T>();
                            }
                            dict.Add(x);
                        }
                    });
                });
            }

            IEnumerable<Tuple<string, T>> result = null;

            if (dict != null)
            {
                result = dict.Select(d => new Tuple<string, T>(d.Key, d.Value));
            }
            else
            {
                result = keys.Select(key => new Tuple<string, T>(key, Get<T>(key)));
            }

            return result
                .Select(d => new Tuple<string[], T>(d.Item1.Split('_'), d.Item2))
                .Where(d => d.Item1.Length >= 2)
                .ToDictionary(x => x.Item1[1], x => x.Item2);
        }

    }
}
