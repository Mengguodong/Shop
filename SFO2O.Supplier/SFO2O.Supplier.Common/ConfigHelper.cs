using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// Session过期时间
        /// </summary>
        public static int SessionExpireMinutes
        {
            get
            {
                return ConvertHelper.ZParseInt32((ConfigurationManager.AppSettings["SessionExpireMinutes"]), 1);
            }
        }

        public static string SharePath
        {
            get
            {
                return ConfigurationManager.AppSettings["SharePath"];
            }
        }
        public static string MiLanGangWebURL
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MiLanGangWebURL"]))
                    return ConfigurationManager.AppSettings["MiLanGangWebURL"];
                return "";
            }
        }
        public static string RenderServerIP
        {
            get
            {
                return ConfigurationManager.AppSettings["RenderServerIP"];
            }
        }

        public static string RenderInitServerIP
        {
            get
            {
                return ConfigurationManager.AppSettings["RenderInitServerIP"];
            }
        }

        public static string CutImageServer
        {
            get
            {
                return ConfigurationManager.AppSettings["CutImageServer"];
            }
        }


        public static string ImageServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageServer"];
            }
        }

        /// <summary>
        /// 图片验证码开关 true真实验证码, false模拟验证码，固定为1234
        /// </summary>
        public static bool RealValidateCodeImg
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RealValidateCodeImg"]))
                    return ParseHelper.ToBool(ConfigurationManager.AppSettings["RealValidateCodeImg"]);

                return true;
            }
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        public static string Version
        {
            get
            {
                return ConfigurationManager.AppSettings["Version"];
            }
        }

        public static string LoginUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["LoginUrl"];
            }

        }

        /// <summary>
        /// 商家后台域名地址
        /// </summary>
        public static string GetSjWebSite
        {
            get { return ConfigurationManager.AppSettings["WebSite"]; }
        }
    }
}
