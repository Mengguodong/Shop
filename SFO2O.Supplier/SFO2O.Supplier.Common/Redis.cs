using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Common
{
    public static class Redis
    {

        /// <summary>
        /// 静态化的Redis连接池，将来这里可能会很复杂，根据KEY的不同来找不同的服务器集群
        /// </summary>
        public static Hashtable REDIS_CLIENT_POOL = InitClientPool();

        private static string space = "SESSION";

        /// <summary>
        /// 初始化Redis的连接池
        /// </summary>
        /// <returns></returns>
        private static Hashtable InitClientPool()
        {
            Hashtable ht = new Hashtable();

            foreach (KeyValuePair<string, string> host in RedisConfig.RedisHostList)
            {
                string this_key_space = host.Key;
                string this_host_ip = host.Value;

                var PoolConfig = new RedisClientManagerConfig();
                PoolConfig.MaxReadPoolSize = RedisConfig.RedisMaxReadPool;
                PoolConfig.MaxWritePoolSize = RedisConfig.RedisMaxWritePool;

                var this_client = new PooledRedisClientManager(new string[] { this_host_ip }, new string[] { this_host_ip }, PoolConfig);

                ht.Add(this_key_space, this_client);
            }

            return ht;
        }


        /// <summary>
        /// 从当前的池子里为一个keyspace找一个客户端
        /// </summary>
        /// <param name="key_space"></param>
        /// <returns></returns>
        private static RedisNativeClient GetNativeClientForKeySpace(string key_space)
        {
            var this_client = (PooledRedisClientManager)REDIS_CLIENT_POOL[key_space];

            return (RedisNativeClient)(this_client.GetClient());

        }
        private static bool IsCon = false;
        /// <summary>
        /// 判断Redis 是否可以连接
        /// </summary>
        /// <returns></returns>
        public static bool IsConRedis()
        {

            try
            {
                Thread th = new Thread(ConRedis);
                th.Start();
                for (int i = 0; i < 3; i++)
                {

                    if (!IsCon)
                        Thread.Sleep(2);
                    else
                        return true;

                }
                return false;

            }
            catch
            {
                return false;
            }
        }

        private static void ConRedis()
        {
            try
            {
                using (RedisNativeClient RNC = GetNativeClientForKeySpace(space))
                {

                    RNC.Ping();
                }
                IsCon = true;
            }
            catch
            {
                IsCon = false;
            }



        }

        /// <summary>
        /// 将一个字符串中的空格和冒号转义，并trim，然后返回，null会返回空字符串
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public static string NormalizeRedisStr(string instr)
        {
            if (instr == null)
                return "";

            return instr.Trim().Replace(" ", "_").Replace(":", "_");

        }
        public static void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key) || key == "-1")
                return;

            string redis_key = "SESS_" + key;
            using (RedisNativeClient RNC = GetNativeClientForKeySpace(space))
            {
                RNC.Del(redis_key);
            }

        }

        public static void RemoveLoginUserSession(int userId)
        {
            if (userId <= 0)
            {
                return;
            }

            string redis_key = "SESS_*_LoginUser_UserID:" + userId.ToString();

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(space))
            {
                var temp = RNC.Keys(redis_key);

                if (temp != null && temp.Length > 0)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        var tempKey = Encoding.UTF8.GetString(temp[i], 0, temp[i].Length);

                        if (tempKey != null)
                        {
                            RNC.Del(tempKey.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置Session，不过期。如果要修改一个Session为永不过期，需要单独调用SetSessioNeverExpire
        /// </summary>
        /// <param name="session_key"></param>
        /// <param name="session_obj"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static void SetSession(string key, object session_message)
        {
            if (key == "-1")
                return;
            string redis_key = "SESS_" + key;

            byte[] bytes = Encoding.UTF8.GetBytes((string)JsonHelper.ToJson(session_message));

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(redis_key))
            {
                RNC.Set(redis_key, bytes);
            }
        }
        /// <summary>
        /// 设置key 的值和保存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="session_message"></param>
        /// <param name="expire_minutes"></param>
        public static void SetSession(string key, object session_message, int expire_minutes)
        {
            if (key == "-1")
                return;
            string redis_key = "SESS_" + key;
            byte[] bytes = Encoding.UTF8.GetBytes((string)JsonHelper.ToJson(session_message));
            using (RedisNativeClient RNC = GetNativeClientForKeySpace(space))
            {
                RNC.Set(redis_key, bytes);
                RNC.Expire(redis_key, Convert.ToInt32(new TimeSpan(0, expire_minutes, 0).TotalSeconds));
            }

        }

        /// <summary>
        /// 根据Key得到一个对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObject<T>(string key)
        {

            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            string redis_key = "SESS_" + key;

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(space))
            {
                byte[] saved_content = RNC.Get(redis_key);


                if (saved_content == null)
                    return default(T);
                else
                {
                    var content = Encoding.UTF8.GetString(saved_content, 0, saved_content.Length);

                    return JsonHelper.ToObject<T>(content);
                }


                // return Serializer.DeSerialize(saved_content);
            }
        }

        /// <summary>
        /// 获取sessionid 对应的用户
        /// </summary>
        /// <returns></returns>
        public static object QueryUserBySessionid(string SessionID)
        {
            if (SessionID == "-1") ///SessionID都不存在，返回false
                return "";
            string redis_key = "SESS_" + SessionID;
            return Redis.LTGetObj("SESSION", redis_key);

        }

        public static object LTGetObj(string key_space, string obj_id)
        {
            if (obj_id == null || key_space == null)
                return false;

            string redis_key = key_space + ":" + obj_id;

            using (RedisNativeClient RNC = GetNativeClientForKeySpace(key_space))
            {

                byte[] saved_content = RNC.Get(redis_key);

                if (saved_content == null)
                    return null;

                return Serializer.DeSerialize(saved_content);
            }
        }
    }
}
