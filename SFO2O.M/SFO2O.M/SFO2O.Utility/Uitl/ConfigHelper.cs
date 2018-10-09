using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SFO2O.Utility.Extensions;

namespace SFO2O.Utility.Uitl
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string key, T defaultValue = default(T))
        {

            return ConfigurationManager.AppSettings[key].As<T>(defaultValue);
        }

        /// 获取当前某个模块的某个配置字符串
        /// 模块的配置项在web.config里，以 模块名:配置名为key
        /// 如果key不存在，返回null
        public static string GetAppConfigString(string moduleName, string configKey)
        {
            return ConfigurationManager.AppSettings.Get(moduleName + ":" + configKey);
        }
        public static string GetAppConfigString(string configKey)
        {
            return ConfigurationManager.AppSettings.Get(configKey);
        }
        public static string ImageServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageServer"];
            }
        }

        public static string NationalFlagImageServer
        {
            get
            {
                return ConfigurationManager.AppSettings["NationalFlagImageServer"];
            }
        }


        public static string SharePath
        {
            get
            {
                return ConfigurationManager.AppSettings["SharePath"];
            }
        }
          

        /// <summary>
        /// Session过期时间
        /// </summary>
        public static int SessionExpireMinutes
        {
            get
            {
                return ParseHelper.ToInt(ConfigurationManager.AppSettings["SessionExpireMinutes"]);
            }
        }
          

        /// <summary>
        /// 获取images版本号
        /// </summary>
        public static string ImgVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["ImgVersion"];
            }
        }
        public static string JsVersion
        {
            get { return ConfigurationManager.AppSettings["JSVersion"]; }
        }
        public static string CssVersion
        {
            get { return ConfigurationManager.AppSettings["CSSVersion"]; }
        }
         
        /// <summary>
        /// 允许上传的图片格式
        /// </summary>
        public static string AllowedImageExt
        {
            get { return ConfigurationManager.AppSettings["AllowedImageExt"]; }
        } 

        public static string SfHome
        {
            get
            {
                return ConfigurationManager.AppSettings["SFHome"];
            }
        }

        /// <summary>
        /// Error异常拦截开关 
        ///     true拦截异常,记录ErrorLog后进入404 
        ///     false不拦截,直接黄页
        /// </summary>
        public static bool TraceError
        {
            get
            {
                return ParseHelper.ToBool(ConfigurationManager.AppSettings["TraceError"]);
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
        /// 验证码开关 true真实验证码, false模拟验证码，固定为123456
        /// </summary>
        public static bool RealVerifyCode
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RealVerifyCode"]))
                    return ParseHelper.ToBool(ConfigurationManager.AppSettings["RealVerifyCode"]);

                return false;
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
        /// 当前使用虚拟路径名
        /// </summary>
        public static string CurrentVirtualDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrentVirtualDirectory"];
            }
        }

        /// <summary>
        /// （销售区域）：1：大陆 2：中华人民共和国大陆地区 3：大陆&中华人民共和国大陆地区
        /// </summary>
        public static int SalesTerritory
        {
            get
            {
                return StringUtils.ToInt(ConfigurationManager.AppSettings["SalesTerritory"],1);
            }
        }

        /// <summary>
        /// LanguageVersion（语言版本）：1：SimplifiedChinese 2：TraditionalChinese 3：English
        /// </summary>
        public static int LanguageVersion
        {
            get
            {
                return StringUtils.ToInt(ConfigurationManager.AppSettings["LanguageVersion"],1);
            }
        }

        #region ePayLinks(支付)
        /// <summary>
        /// 商户编号ID，由易票联公司分配
        /// </summary>
        public static string EPartner
        {
            get
            {
                return ConfigurationManager.AppSettings["EPartner"];
            }
        }
        /// <summary>
        /// 商户编号ID，由支付宝公司分配
        /// </summary>
        public static string ZFBPartner
        {
            get
            {
                return ConfigurationManager.AppSettings["ZFBPartner"];
            }
        }
        
        /// <summary>
        /// 商户密钥，由数字和字母组成的32位字符串，用于订单参数的签名
        /// </summary>
        public static string EKey
        {
            get
            {
                return ConfigurationManager.AppSettings["EKey"];
            }
        }

        /// <summary>
        /// 支付宝密钥，由数字和字母组成的32位字符串，用于订单参数的签名
        /// </summary>
        public static string ZFBKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ZFBKey"];
            }
        }

        /// <summary>
        /// 支付网关入口地址
        /// </summary>
        public static string EGateWayUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["EGateWayUrl"];
            }
        }

        /// <summary>
        /// 支付宝网关入口地址
        /// </summary>
        public static string ZFBGateWayUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ZFBGateWayUrl"];
            }
        }

        /// <summary>
        /// 字符编码格式 目前支持 gbk 或 utf-8
        /// </summary>
        public static string EInputCharset
        {
            get
            {
                return ConfigurationManager.AppSettings["EInputCharset"];
            }
        }
                /// <summary>
        /// 字符编码格式 目前支持 gbk 或 utf-8
        /// </summary>
        public static string ZFBInputCharset
        {
            get
            {
                return ConfigurationManager.AppSettings["ZFBInputCharset"];
            }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string ESignType
        {
            get
            {
                return ConfigurationManager.AppSettings["ESignType"];
            }
        }
        /// <summary>
        /// 获取支付宝的签名方式
        /// </summary>
        public static string ZFBSignType
        {
            get
            {
                return ConfigurationManager.AppSettings["ZFBSignType"];
            }
        }

        /// <summary>
        /// 交易完成后跳转的URL，用来接收易票联网关的页面转跳即时通知结果
        /// </summary>
        public static string EReturnUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["EReturnUrl"];
            }
        }
        /// <summary>
        /// 交易完成后跳转的URL，用来接收支付宝的页面转跳即时通知结果
        /// </summary>
        public static string ZFBReturnUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ZFBReturnUrl"];
            }
        }

        /// <summary>
        /// 接收易票联支付网关异步通知的URL
        /// </summary>
        public static string ENotifyUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ENotifyUrl"];
            }
        }

          /// <summary>
        /// 接收支付宝支付网关异步通知的URL
        /// </summary>
        public static string ZFBNotifyUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ENotifyUrl"];
            }
        }
        #endregion

        /// <summary>
        /// 验证码
        /// </summary>
        public static string RandomCode
        {
            get { return "RandomCode"; }
        }
        /// <summary>
        /// 验证码创建时间
        /// </summary>
        public static string RandomCodeCreateTime
        {
            get { return "RandomCodeCreateTime"; }
        }

        /// <summary>
        /// LanguageVersion（语言版本）：1：SimplifiedChinese 2：TraditionalChinese 3：English
        /// </summary>
        public static decimal CustomsDutiesRate
        {
            get
            {
                return StringUtils.ToDecimal(ConfigurationManager.AppSettings["CustomsDutiesRate"], 0.7M);
            }
        }

        /// <summary>
        /// LanguageVersion（语言版本）：1：SimplifiedChinese 2：TraditionalChinese 3：English
        /// </summary>OrderLimitValue
        public static decimal ConsolidatedPrice
        {
            get
            {
                return StringUtils.ToDecimal(ConfigurationManager.AppSettings["consolidatedPrice"], 2000);
            }
        }

        /// <summary>
        /// LanguageVersion（语言版本）：1：SimplifiedChinese 2：TraditionalChinese 3：English
        /// </summary>
        public static decimal OrderLimitValue
        {
            get
            {
                return StringUtils.ToDecimal(ConfigurationManager.AppSettings["OrderLimitValue"], 50000);
            }
        }

        /// <summary>
        /// 产品搜索服务器地址
        /// </summary>
        public static string SolrSearchUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SolrSearchUrl"];
            }
        }

        /// <summary>
        /// (2016.8.03月饼)时令产品的二级目录
        /// </summary>
        public static int HolidayCategoryId
        {
            get 
            {
                return StringUtils.ToInt(ConfigurationManager.AppSettings["holidayFoodsKey"]);
            }
        }
    }
}