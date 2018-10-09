using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SFO2O.Framework.Uitl
{
    public static class DomainHelper
    {
        public static string BuyUrl
        {
            get
            {
                string url = GetConfig("BuyServer");

                if (url.Length == 0) url = "http://www.milangang.com";
                
                return url;
            }
        }
        public static string StoreDomainUrl
        {
            get
            {
                string url = GetConfig("StoreDomainUrlTemplate");

                if (url.Length == 0) url = "http://{0}.milangang.com";

                return url;
            }
        }

        public static string WWWUrl
        {
            get
            {
                string url = GetConfig("WWWServer");

                if (url.Length == 0) url = "http://www.milangang.com";

                return url;
            }
        }

        public static string SJUrl
        {
            get
            {
                string url = GetConfig("SJServer");

                if (url.Length == 0) url = "http://sj.milangang.com";

                return url;
            }
        }

        public static string WebIMUrl
        {
            get
            {
                string url = GetConfig("WebIMServer");

                if (url.Length == 0) url = "http://webim.milangang.com";

                return url;
            }
        }
        public static string AgentUrl
        {
            get
            {
                string url = GetConfig("AgentServer");
                if (string.IsNullOrEmpty(url))
                {
                    return "http://agent.milangang.com";
                }
                return url;
            }
        }
        public static string ImageUrl
        {
            get
            {
                string url = GetConfig("ImageServer");

                if (url.Length == 0) url = "http://i1.milangang.com";

                return url;
            }
        }

        /// <summary>
        /// 获取指定站点的演示url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetTestUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string[] arr = url.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length > 0) arr[0] = arr[0] + "test";

                url = string.Join(".", arr);
            }

            return url;
        }

        public static string GetConfig(string configName)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[configName]))
                {
                    result = ConfigurationManager.AppSettings[configName];
                }
                else
                {
                    result = "";
                }

            }
            catch
            {
                result = "";
            }
            return result;
        }
    }
}
