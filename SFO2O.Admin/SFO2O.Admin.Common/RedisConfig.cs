using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Common
{
    public static class RedisConfig
    {
        /// <summary>
        /// Redis服务器的地址和端口，先按照单机配置，将来改成集群方式再说，这个数据结构得改
        /// </summary>
        public static Dictionary<string, string> RedisHostList
        {
            get
            {
                if (_RedisHostList == null)
                {
                    _RedisHostList = new Dictionary<string, string>();
                    foreach (string key in ConfigurationManager.AppSettings.Keys)
                    {
                        if (key.StartsWith("redis_server_"))
                        {
                            _RedisHostList.Add(key.Replace("redis_server_", "").ToUpper(), ConfigurationManager.AppSettings[key]);
                        }
                    }
                }
                return _RedisHostList;
            }
        }
        private static Dictionary<string, string> _RedisHostList;


        /// <summary>
        /// 看client代码，pool里最大就允许max个连接，所以得增加以下，这个数字先这样定，回头看数据说话，关键是不能阻塞
        /// </summary>
        public static int RedisMaxReadPool = int.Parse(ConfigurationManager.AppSettings["redis_max_read_pool"]);
        public static int RedisMaxWritePool = int.Parse(ConfigurationManager.AppSettings["redis_max_write_pool"]);
    }
}
