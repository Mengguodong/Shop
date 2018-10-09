using SFO2O.Admin.Businesses;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Admin;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SFO2O.Admin.Web.Controllers
{
    public class BaseController : Controller
    {
        #region 分页信息
        protected int PageSize
        {
            get
            {
                if (Request["PageSize"] != null)
                    return int.Parse(Request["PageSize"]);
                else return WebConfig.PageSize;
            }
        }
        protected int PageNo
        {
            get
            {
                if (Request["PageNo"] != null)
                    return int.Parse(Request["PageNo"]);
                else return 1;
            }
        }
        #endregion

        #region 登录用户信息
        private LoginUserModel _currentLoginModel;
        public LoginUserModel CurrentLoginModel
        {
            get
            {
                return _currentLoginModel;
            }
        }
        private int _UserID = -1;
        /// <summary>
        /// 用户ID
        /// </summary>
        protected int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }
        private string _UserName = "system";
        /// <summary>
        /// 用户名
        /// </summary>
        protected string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        private bool _IsAdmin;
        /// <summary>
        /// 是否管理员
        /// </summary>
        protected bool IsAdmin
        {
            get
            {
                return _IsAdmin;
            }
            set
            {
                _IsAdmin = value;
            }
        }

        public HashSet<EnumPermission> PermissionSet
        {
            get
            {
                return _currentLoginModel != null ? _currentLoginModel.PermissionSet : null;
            }
        }

        protected List<string> ShieldUsers
        {
            get
            {
                var shieldUsers = System.Configuration.ConfigurationManager.AppSettings["ShieldUsers"];
                if (string.IsNullOrEmpty(shieldUsers))
                    return new List<string>();
                else
                    return shieldUsers.Split(';').ToList();
            }
        }

        #endregion

        #region 页面初始化信息
        /// <summary>
        /// 开始时间
        /// </summary>
        protected string CreateTimeStart { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        protected string CreateTimeEnd { get; set; }
        /// <summary>
        /// 当前页面
        /// </summary>
        protected string CurrPageName { get; set; }
        /// <summary>
        /// 是否可以查询
        /// </summary>
        protected bool CanSearch { get; set; }
        #endregion

        public BaseController()
        {
            ViewBag.JsAndCssVersion = System.Configuration.ConfigurationManager.AppSettings["JsAndCssVersion"].ToString();
        }

        #region 当前请求信息
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //*防止限制漏洞导致拒绝服务攻击
            //if (filterContext.RequestContext.HttpContext.Request.HttpMethod == "POST"
            //    && filterContext.RequestContext.HttpContext.Request.ContentLength > 1024 * 1024)
            //    return;

            #region 身份验证
            LoginUserModel loginUserModel = UserLogin(filterContext);
            if (loginUserModel != null && loginUserModel.AdminUserInfo != null)
            {
                _currentLoginModel = loginUserModel;
                UserID = loginUserModel.AdminUserInfo.id;
                UserName = loginUserModel.AdminUserInfo.UserName;
                IsAdmin = loginUserModel.AdminUserInfo.IsAdmin;
                ViewData["PermissionSet"] = loginUserModel.PermissionSet;
                ViewBag.UserMenu = loginUserModel.MenuList;
                ViewBag.AdminUserInfo = loginUserModel.AdminUserInfo;
                ViewBag.UserName = UserName;
                ViewBag.User = UserName;
            }
            #endregion
            if (Request["PageName"] != null)
            {
                CurrPageName = Request["PageName"];
            }
            else
            {
                string url = filterContext.HttpContext.Request.Url.AbsolutePath;
                CurrPageName = url.Substring(url.LastIndexOf('/') + 1);
            }
            //当前页面
            ViewBag.PageName = CurrPageName;
            //初始化页面时间控件值
            CreateTimeStart = Request["CreateTimeStart"];
            CreateTimeEnd = Request["CreateTimeEnd"];
            //第一次加载页面时，初始时间范围为一个月
            if (string.IsNullOrEmpty(CreateTimeStart)
                && string.IsNullOrEmpty(CreateTimeEnd)
                && Request.HttpMethod == "GET"
                && Request["PageNo"] == null)
            {
                CreateTimeStart = FormatDateTime.ToDate(DateTime.Now.AddMonths(-1));
                CreateTimeEnd = FormatDateTime.ToDate(DateTime.Now);
            }
            //是否可以查询
            //if (Request.HttpMethod == "POST" || Request["PageNo"] != null)
            {
                CanSearch = true;
            }

        }
        #endregion

        #region 用户身份验证
        private readonly AdminUserBLL adminUserBLL = new AdminUserBLL();
        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        private LoginUserModel UserLogin(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            //获取登录信息
            LoginUserModel loginUserModel = LoginHelper.GetCurrentUserInfo();
            var userid = "" + Session[LoginHelper.SessionKey];
            int id = 0;
            if (!string.IsNullOrEmpty(userid))
            {
                int.TryParse(userid.ToString(), out id);
            }
            if (loginUserModel == null)
            {
                var loginKey = LoginHelper.GetCurrentCacheKey();
                if (id > 0)
                {
                    //从本地缓存中获取登录对象
                    loginUserModel = (LoginUserModel)HttpRuntime.Cache.Get(loginKey);
                }
                if (loginUserModel == null)
                {
                    if (id == 0 || ShieldUsers.Contains(userid))
                    {
                        filterContext.Result = Redirect(GetLoginUrl(filterContext.HttpContext.Request));
                    }
                    else
                    {
                        loginUserModel = adminUserBLL.GetLoginUserByUserID(id);
                        RedisCacheHelper.Add<LoginUserModel>(loginKey, loginUserModel, DateTime.Now.AddMinutes(Session.Timeout));
                    }
                }
            }
            else
            {
                /* 测试阶段暂时注销，待菜单稳定后打开
                 
                if (filterContext.HttpContext.Request.Url.AbsolutePath.Equals("/"))
                    return loginUserModel;
                int urlHashCode = filterContext.HttpContext.Request.Url.AbsolutePath.GetHashCode();
                bool usable = false;
                //安全验证,判断请求的URL是否属于用户权限菜单
                foreach (var item in loginUserModel.MenuList)
                {
                    if (item.children != null)
                    {
                        foreach (var childMenu in item.children)
                        {
                            if (urlHashCode == childMenu.ModuleURL.GetHashCode())
                            {
                                usable = true;
                                break;
                            }
                        }

                        if (usable)
                            break;
                    }
                }

                if (!usable)
                {
                    filterContext.RequestContext.HttpContext.Response.Redirect("http://Admin.SFO2O.com/", true);
                }
                */
            }

            return loginUserModel;
        }
        #endregion

        private string GetLoginUrl(HttpRequestBase request)
        {
            string targetUrl = "";
            string returnUrl = "";
            if (request.HttpMethod == System.Net.WebRequestMethods.Http.Get && !string.Equals(request.Url.AbsolutePath, "/Home/LoginOut", StringComparison.OrdinalIgnoreCase))
            {
                returnUrl = HttpUtility.UrlEncode(request.Url.PathAndQuery);
            }
            targetUrl = FormsAuthentication.LoginUrl;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                targetUrl += "?ReturnUrl=" + returnUrl;
            }
            return targetUrl;
        }

        /// <summary>
        /// 导出Xlsx文件
        /// </summary>
        /// <param name="fileContents">要发送到响应的二进制内容。</param>
        /// <param name="fileDownloadName">器中显示的文件下载对话框内要使用的文件名。</param>
        /// <returns></returns>
        protected FileContentResult XlsxFile(byte[] fileContents, string fileDownloadName)
        {
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName);
        }
    }

    /// <summary>
    /// Transfers execution to the supplied url.
    /// </summary>
    public class TransferResult : ActionResult
    {
        public string Url { get; private set; }

        public TransferResult(string url)
        {
            this.Url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpContext = HttpContext.Current;

            // MVC 3 running on IIS 7+
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(this.Url, true);
            }
            else
            {
                // Pre MVC 3
                httpContext.RewritePath(this.Url, false);

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest(httpContext);
            }
        }
    }

    #region 登录权限
    /// <summary>
    /// 需要相应权限才能访问
    /// </summary>
    public class RequirePermissionAttribute : ActionFilterAttribute
    {
        private EnumPermission[] Permissions;
        public RequirePermissionAttribute(EnumPermission permission, params EnumPermission[] permissions)
        {
            var arr = new EnumPermission[permissions.Length + 1];
            arr[0] = permission;
            Array.Copy(permissions, 0, arr, 1, permissions.Length);
            Permissions = arr;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (LoginHelper.IsAuthenticated())
            {
                var absolutePath = httpContext.Request.Url.AbsolutePath;
                string redirectUrl = HttpUtility.UrlEncode(httpContext.Request.Url.PathAndQuery);
                string loginUrl = FormsAuthentication.LoginUrl;
                if (httpContext.Request.HttpMethod == System.Net.WebRequestMethods.Http.Get)
                {
                    loginUrl = FormsAuthentication.LoginUrl + "?ReturnUrl=" + redirectUrl;
                }
                LoginUserModel loginModel;
                var baseContr = filterContext.Controller as BaseController;
                if (baseContr != null)
                {
                    loginModel = baseContr.CurrentLoginModel;
                }
                else
                {
                    int userid = 0;
                    if (!LoginHelper.TryGetCurrentUserID(out userid) || userid == 0)
                    {
                        filterContext.Result = new RedirectResult(loginUrl);
                        return;
                    }
                    loginModel = LoginHelper.GetCurrentUserInfo();
                }

                if (loginModel != null)
                {
                    var userInfo = loginModel.AdminUserInfo;
                    if (userInfo.CacheTime.AddMinutes(httpContext.Session.Timeout - 5) < DateTime.Now)
                    {
                        LoginHelper.RefreshCache(loginModel);
                    }
                    if (userInfo.IsAdmin)
                    {
                        return;
                    }
                    //获取登录人的权限
                    HashSet<EnumPermission> permissionSet = loginModel.PermissionSet;
                    if (!Array.Exists(this.Permissions, p => permissionSet.Contains(p)))
                    {
                        // TODO 跳转到无权限提示页面
                        filterContext.Result = new ContentResult() { Content = "你沒有訪問此項目的權限，請聯繫管理員。" };
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult(loginUrl);
                }
            }
        }
    }
    #endregion

    static class LoginHelper
    {
        public const String SessionKey = "SFO2O.Admin.UserID";
        private const String CacheKeyFormat = "SFO2O_AdminLoginUser_{0}";
        private static String GetCacheKey(String userID)
        {
            return String.Format(CacheKeyFormat, userID);
        }

        private static String GetCacheKey(int userID)
        {
            return GetCacheKey(userID.ToString());
        }
        /// <summary>
        /// 获取当前登录用户的LoginUserModel
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static LoginUserModel GetCurrentUserInfo()
        {
            var Session = HttpContext.Current.Session;
            int userid = 0;
            if (TryGetCurrentUserID(out userid) && userid > 0)
            {
                var model = CacheHelper.AutoCache<LoginUserModel>(
                            GetCacheKey(userid), "", () =>
                            {
                                return new AdminUserBLL().GetLoginUserByUserID(userid);
                            });
                if (model != null && model.AdminUserInfo.Status != 1)
                {
                    //用户状态无效，使用户的会话失效
                    model = null;
                    RemoveCurrentLoginInfo();
                }
                return model;
            }
            return null;
        }
        public static String GetCurrentCacheKey()
        {
            var Session = HttpContext.Current.Session;
            return GetCacheKey("" + Session[SessionKey]);

        }

        public static void RefreshCache(LoginUserModel longinuser)
        {
            var Session = HttpContext.Current.Session;
            string cacheKey = GetCacheKey(longinuser.AdminUserInfo.id);
            RedisCacheHelper.Remove(cacheKey);
            Session[SessionKey] = null;
            Session[SessionKey] = longinuser.AdminUserInfo.id;
            longinuser.AdminUserInfo.CacheTime = DateTime.Now;
            RedisCacheHelper.Add<LoginUserModel>(cacheKey, longinuser, DateTime.Now.AddMinutes(Session.Timeout));
        }

        public static void RemoveCurrentLoginInfo()
        {
            var userId = 0;
            if (TryGetCurrentUserID(out userId))
            {
                var Session = HttpContext.Current.Session;
                Session[SessionKey] = null;
                var key = GetCacheKey(userId);
                RedisCacheHelper.Remove(key);
            }
        }

        public static void RemoveLoginInfo(int userID)
        {
            var key = GetCacheKey(userID);
            RedisCacheHelper.Remove(key);
        }

        public static void Cache(LoginUserModel longinuser)
        {
            var Session = HttpContext.Current.Session;
            string cacheKey = GetCacheKey(longinuser.AdminUserInfo.id);
            Session[SessionKey] = longinuser.AdminUserInfo.id;
            longinuser.AdminUserInfo.CacheTime = DateTime.Now;
            RedisCacheHelper.Add<LoginUserModel>(cacheKey, longinuser, DateTime.Now.AddMinutes(Session.Timeout));
        }

        public static bool IsAuthenticated()
        {
            return HttpContext.Current.Session[SessionKey] != null;
        }

        public static bool TryGetCurrentUserID(out int userid)
        {
            var value = HttpContext.Current.Session[SessionKey];
            userid = 0;
            return value != null && int.TryParse(value.ToString(), out userid);
        }
    }
}