using System;
using System.Configuration;

namespace SFO2O.Utility.Uitl
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

        public static string MUrl
        {
            get
            {
                string url = GetConfig("MServer");

                if (url.Length == 0) url = "http://www.discountmassworld.com";

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
        public static string CustomerServicesUrl
        {
            get
            {
                string url = GetConfig("CustomerServices");

                if (url.Length == 0) url = "http://sf-ocs.sf-express.com:8080/live800/chatClient/chatbox.jsp?companyID=8935&configID=48&skillId=43&enterurl=hko2o&syslanguage=1";

                return url;
            }
        }
        public static string GetImageUrl(string imgUrl)
        {
            string strImagUrl = "";
            try
            {
                if (string.IsNullOrEmpty(imgUrl))
                {
                    strImagUrl = "";
                }
                else
                {
                    strImagUrl = DomainHelper.ImageUrl.TrimEnd('/') + "/" + imgUrl.TrimStart('/');
                }
            }
            catch { strImagUrl = ""; }
            return strImagUrl.Replace("\\", "/");
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
