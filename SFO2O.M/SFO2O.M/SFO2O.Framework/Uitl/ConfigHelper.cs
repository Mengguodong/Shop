using System.Collections.Generic;
using System.Linq;
using System.Configuration;

using SFO2O.Framework.Extensions;

namespace SFO2O.Framework.Uitl
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 宝石的分类
        /// </summary>
        public static string DiamondCategory
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DiamondCategory"]))
                    {
                        return ConfigurationManager.AppSettings["DiamondCategory"];
                    }
                    else
                    {
                        return "";
                    }

                }
                catch
                {
                    return "";
                }
            }
        }

        public static string solrUrl
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["solrUrl"]))
                    {
                        return ConfigurationManager.AppSettings["solrUrl"];
                    }
                    else
                    {
                        return "http://www.milangang.com:8080/solr/";
                    }

                }
                catch
                {
                    return "http://www.milangang.com:8080/solr/";
                }
            }
        }
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

        public static string Logger
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Logger"]))
                    {
                        return ConfigurationManager.AppSettings["Logger"];
                    }
                    else
                    {
                        return "MLG.ShangJia";
                    }

                }
                catch
                {
                    return "MLG.ShangJia";
                }
            }
        }


        public static string MerchantNo
        {
            get
            {
                return ConfigurationManager.AppSettings["merchant_no"];
            }
        }


        public static string SfpayRegUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["sfpayRegUrl"];
            }
        }



        public static string SharePath
        {
            get
            {
                return ConfigurationManager.AppSettings["SharePath"];
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


        public static int RenderServerPort
        {
            get
            {
                return ParseHelper.ToInt(ConfigurationManager.AppSettings["RenderServerPort"]);
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
        /// 只读solr配置串，用分号分隔
        /// </summary>
        public static string SearchServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SearchServer"].ToString();
            }
        }

        /// <summary>
        /// 主索引地址
        /// </summary>
        public static string IndexServer
        {
            get
            {
                return ConfigurationManager.AppSettings["IndexServer"].ToString();
            }
        }

        /// <summary>
        /// 错误服务器重试时间间隔
        /// </summary>
        public static int ReOpenMinute
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ReOpenMinute"].ToString());
            }
        }

        public static bool SearchTestLog
        {
            get
            {
                if (int.Parse(ConfigurationManager.AppSettings["SearchTestLog"].ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        public static string CutImageServer
        {
            get
            {
                return ConfigurationManager.AppSettings["CutImageServer"];
            }
        }


        public static string HouTaiShareUpload
        {
            get
            {
                return ConfigurationManager.AppSettings["HouTaiShareUpload"];
            }
        }

        /// <summary>
        /// 流行趋势图片，是否本地读取
        /// </summary>
        public static bool FashionIsLocalhost
        {
            get
            {
                return ParseHelper.ToBool(ConfigurationManager.AppSettings["fashion_is_localhost"]);
            }
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
        /// 材料分类及配件分类的缓存过期时间
        /// </summary>
        public static int CategoryExpireMinutes
        {
            get
            {
                return ParseHelper.ToInt(ConfigurationManager.AppSettings["CategoryExpireMinutes"]);
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

        /// <summary>
        /// 获取js版本号
        /// </summary>
        public static string JsVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["JsVersion"];
            }
        }

        /// <summary>
        /// 获取css版本号
        /// </summary>
        public static string CssVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["CssVersion"];
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
        /// 允许上传的图片格式
        /// </summary>
        public static string AllowedImageExt
        {
            get { return ConfigurationManager.AppSettings["AllowedImageExt"]; }
        }
        /// <summary>
        /// 商铺 LogoImage1
        /// </summary>
        public static string LogoImage1
        {
            get
            {
                return ConfigurationManager.AppSettings["LogoImage"].Split(',')[0];
            }
        }

        /// <summary>
        /// 商铺 LogoImage2
        /// </summary>
        public static string LogoImage2
        {
            get
            {
                return ConfigurationManager.AppSettings["LogoImage"].Split(',')[1];
            }
        }

        /// <summary>
        /// 商铺 LogoImage3
        /// </summary>
        public static string LogoImage3
        {
            get
            {
                return ConfigurationManager.AppSettings["LogoImage"].Split(',')[2];
            }
        }
        /// <summary>
        /// 商铺 BannerImage1
        /// </summary>
        public static string BannerImage1
        {
            get
            {
                return ConfigurationManager.AppSettings["BannerImage"].Split(',')[0];
            }
        }

        /// <summary>
        /// 商铺 BannerImage2
        /// </summary>
        public static string BannerImage2
        {
            get
            {
                return ConfigurationManager.AppSettings["BannerImage"].Split(',')[1];
            }
        }

        /// <summary>
        /// 商铺 BannerImage3
        /// </summary>
        public static string BannerImage3
        {
            get
            {
                return ConfigurationManager.AppSettings["BannerImage"].Split(',')[2];
            }
        }


        /// <summary>
        /// 上传内置账号SupplierIds
        /// </summary>
        public static string HouTaiSupplierIds
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HouTaiSupplierIds"]))
                    return ConfigurationManager.AppSettings["HouTaiSupplierIds"];
                return "";
            }
        }



        /// <summary>
        /// 设计师图片模板
        /// </summary>
        public static string DesignTemplateImage
        {
            get
            {
                return ConfigurationManager.AppSettings["DesignTemplateImage"];
            }
        }

        #region ==顺银相关==

        /// <summary>
        /// 顺银宝商户代码
        /// </summary>
        public static string merchantId
        {
            get
            {
                return ConfigurationManager.AppSettings["merchantId"];
            }
        }

        /// <summary>
        /// 顺银宝密钥
        /// </summary>
        public static string merchantEncrpty
        {
            get
            {
                return ConfigurationManager.AppSettings["merchantEncrpty"];
            }
        }

        /// <summary>
        /// 顺银宝商户密钥
        /// </summary>
        public static string merchantKey
        {
            get
            {
                return ConfigurationManager.AppSettings["merchantKey"];
            }
        }

        /// <summary>
        /// 顺银宝即时到帐支付接口地址
        /// </summary>
        public static string gw_pay
        {
            get
            {
                return ConfigurationManager.AppSettings["gw_pay"];
            }
        }

        /// <summary>
        /// 顺银宝订单查询接口地址
        /// </summary>
        public static string queryByTrade_url
        {
            get
            {
                return ConfigurationManager.AppSettings["queryByTrade_url"];
            }
        }

        /// <summary>
        /// 顺银宝后台通知
        /// </summary>
        public static string sf_callbackurl
        {
            get
            {
                return ConfigurationManager.AppSettings["sf_callbackurl"];
            }
        }

        /// <summary>
        /// 顺银宝前台返回
        /// </summary>
        public static string sf_frontbackurl
        {
            get
            {
                return ConfigurationManager.AppSettings["sf_frontbackurl"];
            }
        }

        #endregion

        #region 快钱支付
        public static string Bill99SendGateWay
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99SendGateWay"];
            }
        }
        public static string Bill99ReceiveUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99ReceiveUrl"];
            }
        }
        public static string Bill99ShowUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99ShowUrl"];
            }
        }
        public static string Bill99MerchantAcctId
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99MerchantAcctId"];
            }
        }
        public static string Bill99Password
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99Password"];
            }
        }
        public static string Bill99RsaFile
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99RsaFile"];
            }
        }
        public static string Bill99CertRsaFile
        {
            get
            {
                return ConfigurationManager.AppSettings["Bill99CertRsaFile"];
            }
        }
        #endregion
        public static string HouTaiWebURL
        {
            get
            {
                return ConfigurationManager.AppSettings["HouTaiWebURL"];
            }
        }

        /// <summary>
        /// 米兰港前台网址
        /// </summary>
        public static string MiLanGangWebURL
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MiLanGangWebURL"]))
                    return ConfigurationManager.AppSettings["MiLanGangWebURL"];
                return "";
            }
        }

        /// <summary>
        /// 米兰港前台网址
        /// </summary>
        public static string MiLanGangSJURL
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MiLanGangSJURL"]))
                    return ConfigurationManager.AppSettings["MiLanGangSJURL"];
                return "";
            }
        }

        /// <summary>
        /// Bi项目网址
        /// </summary>
        public static string MiLanGangBiURL
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MiLanGangBIURL"]))
                    return ConfigurationManager.AppSettings["MiLanGangBIURL"];
                return "";
            }
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
        /// 业务管理系统url
        /// </summary>
        public static string SJServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SJServer"];
            }

        }

        /// <summary>
        /// 站点域名类型: 配置为test时为演示域名,空串或没有配置时为www线上域名
        /// </summary>
        public static string DomainType
        {
            get
            {
                return ConfigurationManager.AppSettings["DomainType"];
            }
        }
        /// <summary>
        /// 站点域名
        /// </summary>
        public static string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["Domain"];
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
        /// 商家后台域名地址
        /// </summary>
        public static string GetSjWebSite
        {
            get { return ConfigurationManager.AppSettings["WebSite"]; }
        }

        public static string MobileSite
        {
            get { return ConfigurationManager.AppSettings["MobileSite"]; }
        }
        /// <summary>
        /// 商家后台天空盒图片地址
        /// </summary>
        public static string GetSkyboxUrl
        {
            get { return ConfigurationManager.AppSettings["SkyboxUrl"]; }
        }

        /// <summary>
        /// 试用帐户
        /// </summary>
        public static string[] TrialAccount
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["trialAccount"]))
                    return default(string[]);
                return ConfigurationManager.AppSettings["trialAccount"].Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// 不等贷默认的支付方式
        /// </summary>
        public static List<int> DefaultFundProductIds
        {
            get
            {
                var strDefaultFundProductIds = ConfigurationManager.AppSettings["DefaultFundProductIds"];
                if (string.IsNullOrEmpty(strDefaultFundProductIds))
                {
                    return new List<int>();
                }
                else
                {
                    var results = new List<int>();
                    var arrays = strDefaultFundProductIds.Split(',');
                    foreach (var item in arrays)
                    {
                        int id = 0;
                        if (int.TryParse(item, out id))
                        {
                            results.Add(id);
                        }
                    }
                    return results;
                }
            }
        }

        /// <summary>
        /// 默认基金公司id
        /// </summary>
        public static int DefaultFundId
        {
            get
            {
                var defaultFundId = ConfigurationManager.AppSettings["DefaultFundId"];
                int fundId = 0;
                int.TryParse(defaultFundId, out fundId);
                return fundId;
            }
        }

        /// <summary>
        /// webqq客服qq集合
        /// </summary>
        public static List<string> WebQQ
        {
            get
            {
                var qqList = new List<string>();
                var qqData = ConfigurationManager.AppSettings["webqq"];
                if (!string.IsNullOrEmpty(qqData))
                {
                    qqList=qqData.Split(',').ToList();
                }
                return qqList;
            }
        }

        /// <summary>
        /// 提前多少天显示延迟收货按钮
        /// </summary>
        public static int AheadOfTime_ShowDelayReceiveGoodsButton
        {
            get
            {
                int int_AheadOfTime_ShowDelayReceiveGoodsButton = 7;
                var str_AheadOfTime_ShowDelayReceiveGoodsButton = ConfigurationManager.AppSettings["AheadOfTime_ShowDelayReceiveGoodsButton"];
                if (!string.IsNullOrEmpty(str_AheadOfTime_ShowDelayReceiveGoodsButton))
                {
                    int.TryParse(str_AheadOfTime_ShowDelayReceiveGoodsButton,
                        out int_AheadOfTime_ShowDelayReceiveGoodsButton);
                }
                return int_AheadOfTime_ShowDelayReceiveGoodsButton;
            }
        }

        /// <summary>
        /// 发货后多少天自动收货
        /// </summary>
        public static int AutoDelayReceiveGoods
        {
            get
            {
                int intAutoDelayReceiveGoods = 30;
                var strAutoDelayReceiveGoods= ConfigurationManager.AppSettings["AutoDelayReceiveGoods"];
                if (!string.IsNullOrEmpty(strAutoDelayReceiveGoods))
                {
                    int.TryParse(strAutoDelayReceiveGoods, out intAutoDelayReceiveGoods);
                }
                return intAutoDelayReceiveGoods;
            }
        }
        public static string GetConfigByKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string PushPrice
        {
            get { return ConfigurationManager.AppSettings["PushPrice"]; }
        }
    }
}