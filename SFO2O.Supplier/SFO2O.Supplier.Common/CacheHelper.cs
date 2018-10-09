using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Common
{
    public static class CacheHelper
    {
        /// <summary>
        /// 手动删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="cachePrefix">缓存项前缀</param>
        public static void ClearCache(string cachePrefix, string key)
        {
            RedisCacheHelper.Remove(cachePrefix + key);
        }

        /// <summary>
        /// 缓存Helper
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <param name="method">获取数据的委托</param>
        /// <param name="cachePrefix">缓存项前缀</param>
        /// <param name="cacheMinutes">缓存时间，默认为配置文件的值</param>
        /// <returns>从缓存或方法委托中获取的对象</returns>
        public static T AutoCache<T>(string cachePrefix, string key, Func<T> method, int cacheMinutes = 0)
        {
            if (cacheMinutes == 0)
            {
                cacheMinutes = ConfigHelper.SessionExpireMinutes;
            }

            var temp = RedisCacheHelper.Get<T>(cachePrefix + key);

            if (temp == null || EqualityComparer<T>.Default.Equals(temp, default(T)))
            {
                T temp1 = method.Invoke();

                if (temp1 != null)
                {
                    RedisCacheHelper.Add(cachePrefix + key, temp1);

                    return (T)temp1;
                }
            }
            else
            {
                return (T)temp;
            }

            return default(T);
        }

        /// <summary>
        /// 前台所有hash缓存的储存中心，当前使用的hash缓存的Index
        /// 0:分类数据
        /// </summary>
        static Hashtable[] hash = new Hashtable[] { 
            new Hashtable(),
            new Hashtable()
        };

        /// <summary>
        /// 全站Hash缓存的数组
        /// 索引0为全部分类，索引1为根分类
        /// </summary>
        public static Hashtable[] HashCacheManager
        {
            get
            {
                return hash;
            }
        }

        /// <summary>
        /// 缓存Helper，定时刷新hash
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="method">获取数据的委托</param>
        /// <param name="cachePrefix">缓存项前缀</param>
        /// <param name="cacheMinutes">缓存时间，默认为配置文件的值</param>
        /// <returns>从缓存或方法委托中获取的对象</returns>
        internal static void AutoHashtable(string cachePrefix, string key, Func<DataTable> method, int hashIndex, string primaryKey, int cacheMinutes = 0)
        {
            if (cacheMinutes == 0)
            {
                cacheMinutes = ConfigHelper.SessionExpireMinutes;
            }

            string tempkey = cachePrefix + key + "Index_" + hashIndex.ToString();

            var temp = RedisCacheHelper.Get<string>(tempkey);

            if (string.IsNullOrEmpty(temp) || hash[hashIndex].Count == 0)
            {
                RedisCacheHelper.Add(tempkey, "1");

                DataTable temp1 = method.Invoke();

                hash[hashIndex].Clear();

                if (temp1 != null)
                {
                    foreach (DataRow item in temp1.Rows)
                    {
                        hash[hashIndex].Add(item[primaryKey].ToString(), item);
                    }
                }
            }
        }
    }
}
