using SFO2O.Supplier.Businesses.Account;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Account;
using SFO2O.Supplier.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SFO2O.Supplier.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly int DefaultPageSize = 10;

        private LoginUserModel _currentLoginModel;
        public LoginUserModel CurrentLoginModel
        {
            get
            {
                return _currentLoginModel;
            }
        }
        public SupplierUserInfo CurrentUser
        {
            get
            {
                return _currentLoginModel != null ? _currentLoginModel.SupplierUserInfo : null;
            }
        }

        public HashSet<EnumPermission> PermissionSet
        {
            get
            {
                return CurrentLoginModel != null ? CurrentLoginModel.PermissionSet : null;
            }
        }
        public string Msg { get; set; }

        public BaseController()
        {
            ViewBag.Version = ConfigHelper.Version;
            ViewBag.ImageService = ConfigHelper.ImageServer;
            ViewBag.CutImageServer = ConfigHelper.CutImageServer;
        }

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

        /// <summary>
        /// 路由过滤器
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            string targetUrl = "";
            if (!CheckLogin()) // 判断是否登录
            {
                var url = httpContext.Request.Url;
                string returnUrl = "";
                if (httpContext.Request.HttpMethod == System.Net.WebRequestMethods.Http.Get)
                {
                    returnUrl = HttpUtility.UrlEncode(url.PathAndQuery);
                }
                if (string.IsNullOrEmpty(returnUrl))
                {
                    targetUrl = FormsAuthentication.LoginUrl;
                }
                else
                {
                    targetUrl = FormsAuthentication.LoginUrl + "?ReturnUrl=" + returnUrl;
                }
                filterContext.Result = new RedirectResult(targetUrl);
            }
        }

        #region 是否已经登录

        public bool CheckLogin()
        {
            if (CurrentUser == null)
            {
                int userid = 0;
                if (LoginHelper.TryGetCurrentUserID(out userid) && userid > 0)
                {
                    _currentLoginModel = LoginHelper.GetUserInfo(Session, userid);

                    if (_currentLoginModel != null)
                    {
                        RefreshSession();
                        return true;
                    }
                    else
                    {
                        LoginHelper.RemoveCurrentLoginInfo();
                        return false;
                    }
                }
            }
            return false;
        }

        #endregion

        #region 属性值


        private void RefreshSession()
        {
            var _currentUser = CurrentUser;

            ViewBag.SupplierUser = _currentLoginModel.SupplierUserInfo;
            ViewBag.UserMenu = _currentLoginModel.MenuList;
            ViewData["PermissionSet"] = _currentLoginModel.PermissionSet;

            if (_currentUser.CacheTime.AddMinutes(Session.Timeout - 5) < DateTime.Now)
            {
                LoginHelper.RefreshCache(Session, _currentLoginModel);
            }
        }

        #endregion
        #region json时间格式处理
        /// <summary>
        /// 返回JsonResult.24         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="behavior">行为</param>
        /// <param name="format">json中dateTime类型的格式</param>
        /// <returns>Json</returns>
        protected JsonResult CustomJson(object data, JsonRequestBehavior behavior, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                FormateStr = format
            };
        }

        /// <summary>
        /// 返回JsonResult42         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">数据格式</param>
        /// <returns>Json</returns>
        protected JsonResult CustomJson(object data, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                FormateStr = format
            };
        }
        #endregion

        #region Login标签

        public class LoginAjaxAttribute : AuthorizeAttribute
        {
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                var httpContext = filterContext.HttpContext;
                if (httpContext.Request.IsAjaxRequest() && !LoginHelper.IsAuthenticated(httpContext.Session))
                {
                    filterContext.Result = new AjaxUnauthorizedResult(httpContext.Request.UrlReferrer.AbsoluteUri);
                    return;
                }
            }
        }
        public class AjaxUnauthorizedResult : JavaScriptResult
        {
            public AjaxUnauthorizedResult(string url)
            {
                var loginurl = string.Format(ConfigHelper.LoginUrl, url);
                this.Script = "location.href='" + loginurl + "'";
            }
        }

        #endregion

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
            if (LoginHelper.IsAuthenticated(httpContext.Session))
            {
                var absolutePath = httpContext.Request.Url.AbsolutePath;
                string redirectUrl = HttpUtility.UrlEncode(httpContext.Request.Url.PathAndQuery);
                string loginUrl = FormsAuthentication.LoginUrl;
                if (httpContext.Request.HttpMethod == System.Net.WebRequestMethods.Http.Get && !string.Equals(absolutePath, "/Home/LoginOut", StringComparison.OrdinalIgnoreCase))
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
                    loginModel = LoginHelper.GetUserInfo(httpContext.Session, userid);
                }
                if (loginModel != null)
                {
                    var userInfo = loginModel.SupplierUserInfo;
                    if (loginModel.SupplierUserInfo.CacheTime.AddMinutes(httpContext.Session.Timeout - 5) < DateTime.Now)
                    {
                        LoginHelper.RefreshCache(httpContext.Session, loginModel);
                    }
                    if (userInfo.IsAdmin == 1)
                    {
                        return;
                    }
                    //获取登录人的权限
                    HashSet<EnumPermission> permissionSet = loginModel.PermissionSet;
                    if (!Array.Exists(this.Permissions, p => permissionSet.Contains(p)))
                    {
                        filterContext.Result = new TransferResult("/Error/NoPermission");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult(loginUrl);
                }
            }
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
    #endregion

    static class LoginHelper
    {
        public const String SessionKey = "SFO2O.SJ.USERID";
        private const String CacheKeyFormat = "SFO2O_SJLoginUser_{0}";
        private static String GetCacheKey(String userId)
        {
            return String.Format(CacheKeyFormat, userId);
        }
        /// <summary>
        /// 获取当前登录用户的LoginUserModel
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static LoginUserModel GetUserInfo(HttpSessionStateBase Session, int userid)
        {
            var model = CacheHelper.AutoCache<LoginUserModel>(
                        GetCacheKey(userid.ToString()), "", () =>
                        {
                            return SupplierUserBll.GetLoginUserModelByUserID(userid);
                        });
            if (model != null && (model.SupplierUserInfo.Status != 1 || model.SupplierUserInfo.SupplierStatus != 1))
            {
                //用户状态无效，使用户的会话失效
                model = null;
                RemoveCurrentLoginInfo();
            }
            return model;
        }

        public static void RefreshCache(HttpSessionStateBase Session, LoginUserModel longinuser)
        {
            string cacheKey = GetCacheKey(longinuser.SupplierUserInfo.ID.ToString());
            RedisCacheHelper.Remove(cacheKey);
            Session[SessionKey] = null;
            Session[SessionKey] = longinuser.SupplierUserInfo.ID;
            longinuser.SupplierUserInfo.UserID = longinuser.SupplierUserInfo.ID;
            longinuser.SupplierUserInfo.CacheTime = DateTime.Now;
            RedisCacheHelper.Add<LoginUserModel>(cacheKey, longinuser, DateTime.Now.AddMinutes(Session.Timeout));
        }

        public static void RemoveCurrentLoginInfo()
        {
            int userId = 0;
            if (TryGetCurrentUserID(out userId))
            {
                var Session = HttpContext.Current.Session;
                Session[SessionKey] = null;
                RedisCacheHelper.Remove(GetCacheKey(userId.ToString()));
            }
        }

        public static void RemoveLoginInfo(int userId)
        {
            var key = GetCacheKey(userId.ToString());
            RedisCacheHelper.Remove(key);
        }

        public static void Cache(HttpSessionStateBase Session, LoginUserModel longinuser)
        {
            string cacheKey = GetCacheKey(longinuser.SupplierUserInfo.ID.ToString());
            Session[SessionKey] = longinuser.SupplierUserInfo.ID;
            longinuser.SupplierUserInfo.UserID = longinuser.SupplierUserInfo.ID;
            longinuser.SupplierUserInfo.CacheTime = DateTime.Now;
            RedisCacheHelper.Add<LoginUserModel>(cacheKey, longinuser, DateTime.Now.AddMinutes(Session.Timeout));
        }

        public static bool IsAuthenticated(HttpSessionStateBase Session)
        {
            return Session[SessionKey] != null;
        }

        public static bool TryGetCurrentUserID(out int userid)
        {
            var value = HttpContext.Current.Session[SessionKey];
            userid = 0;
            return value != null && int.TryParse(value.ToString(), out userid);
        }
    }
}