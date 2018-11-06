using SFO2O.Utility.Uitl;
namespace Common
{
    /// <summary>
    ///     静态文件访问类
    ///     创建人：zhuzh
    ///     创建时间：2014-10-17
    /// </summary>
    public class PubConstant
    {
        /// <summary>
        /// 域
        /// </summary>
        public static string Domain
        {
            get
            {
                return Auxiliary.ConfigKey("domain");
            }
        }
        /// <summary>
        /// 静态JS/CSS地址
        /// </summary>
        public static string StaticUrl
        {
            get
            {
                return Auxiliary.ConfigKey("static.baseurl");
            }
        }

        /// <summary>
        /// 富文本编辑器地址
        /// </summary>
        public static string UMEditorUrl
        {
            get
            {
                return Auxiliary.ConfigKey("umeditor.baseurl");
            }
        }
        /// <summary>
        /// 官网地址
        /// </summary>
        public static string WebBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("web.baseurl");
            }
        }
        /// <summary>
        /// 供货商地址
        /// </summary>
        public static string SptBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("ghs.baseurl");
            }
        }
        /// <summary>
        /// 超市地址
        /// </summary>
        public static string GosBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("cs.baseurl");
            }
        }
        /// <summary>
        /// 超市地址
        /// </summary>
        public static string OldGosBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("csold.baseurl");
            }
        }

        /// <summary>
        /// 超市营销站点地址
        /// </summary>
        public static string SaleGosBaseUrl
        {
            get {
                return Auxiliary.ConfigKey("sales.baseurl");
            }
        }
        /// <summary>
        /// 服务站地址
        /// </summary>
        public static string SvcBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("fwz.baseurl");
            }
        }
        /// <summary>
        /// 总部地址
        /// </summary>
        public static string HmcBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("zb.baseurl");
            }
        }
        /// <summary>
        /// 厂商地址
        /// </summary>
        public static string FactoryBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("factory.baseurl");
            }
        }

        /// <summary>
        /// 云工厂地址
        /// </summary>
        public static string YgcBaseUrl
        {
            get
            {
                return Auxiliary.ConfigKey("ygc.baseurl");
            }
        }

        /// <summary>
        /// 登录CookieName
        /// </summary>
        public static string COOKIE_NAME
        {
            get
            {
                return Auxiliary.ConfigKey("cookie_name");
            }
        }
        /// <summary>
        /// 引导页COOKIE
        /// </summary>
        public static string GUIDE_COOKIE
        {
            get
            {
                return Auxiliary.ConfigKey("guide_cookie");
            }
        }

        public static string SHOPS_COOKIE
        {
            get { return Auxiliary.ConfigKey("shops_cookie"); }
        }

        /// <summary>
        /// 当前版本号
        /// </summary>
        public static string CurrentVersion
        {
            get { return Auxiliary.ConfigKey("current.version"); }
        }
        /// <summary>
        /// 酒游戏前台地址
        /// </summary>
        public static string ShowBaseUrl
        {
            get { return Auxiliary.ConfigKey("show.baseurl"); }
        }

        /// <summary>
        /// 酒游戏后台地址
        /// </summary>
        public static string ManagementBaseUrl
        {
            get { return Auxiliary.ConfigKey("management.baseurl"); }
        }

     
        /// <summary>
        /// 是否开启sql查询监控
        /// </summary>
        public static bool IsOpenQuerySqlMonitorLog
        {
            get { return Auxiliary.ConfigKey("monitor.querysqlmonitorlog") == "true"; }
        }

        public static bool IsOpenNonQuerySqlMonitorLog
        {
            get { return Auxiliary.ConfigKey("monitor.nonquerysqlmonitorlog") == "true"; }
        }

        /// <summary>
        /// 是否开启api 监控
        /// </summary>
        public static bool IsOpenApiMonitor
        {
            get { return Auxiliary.ConfigKey("monitor.apimonitorlog") == "true"; }
        }

        public static string GoodsImgUrl
        {
            get { return Auxiliary.ConfigKey("imgUrl"); }
        }



        #region 对外API域名地址
        /// <summary>
        /// ERP地址
        /// </summary>
        public static string WebApi_Erp
        {
            get
            {
                return Auxiliary.ConfigKey("erp.webapi");
            }
        }
        #endregion

        /// <summary>
        /// 酒游戏WebApi地址
        /// </summary>
        public static string WineGameWebApi
        {
            get { return Auxiliary.ConfigKey("webapi.WineGame"); }
        }
        /// <summary>
        /// 酒游戏WebApi地址
        /// </summary>
        public static string GreenWebApi
        {
            get { return Auxiliary.ConfigKey("webapi.GreenWebApi"); }
        }
        /// <summary>
        /// 酒游戏前台引用地址
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="ActionName"></param>
        /// <returns></returns>
        public static string WineGameShowUrl(string ControllerName,string ActionName)
        {
            return ShowBaseUrl + ControllerName + "/" + ActionName;
 
        }
        /// <summary>
        /// 酒游戏管理后台引用地址
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="ActionName"></param>
        /// <returns></returns>
        public static string WineGameManagementUrl(string ControllerName,string ActionName)
        {
            return ManagementBaseUrl + ControllerName + "/" + ActionName;
        }
        ///// <summary>
        ///// 首次充值直升作坊条件
        ///// </summary>
        ///// <returns></returns>
        //public static int WorkHouse()
        //{
        //    return ConvertHelper.ToInt32(Auxiliary.ConfigKey("zuofang"));
        //}
        ///// <summary>
        ///// 首次充值直升车间条件
        ///// </summary>
        ///// <returns></returns>
        //public static int WorkShop()
        //{
        //    return ConvertHelper.ToInt32(Auxiliary.ConfigKey("chejian"));
        //}

    }
}