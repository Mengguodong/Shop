using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SFO2O.M.Controllers.Common;
using SFO2O.M.ViewModel.Account;

namespace SFO2O.M.Controllers.Filters
{
    /// <summary>
    /// 登录判断
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            var thisurl = filterContext.HttpContext.Request.Url;

            var route = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(
                    new
                    {
                        Controller = "Account",
                        action = "Login",
                        Return_Url = thisurl
                    }));

            LoginUserModel tempLoginModel = null;
            BaseController controller = filterContext.Controller as BaseController;
            var tempIsLogin = LoginHelper.CheckSession(out tempLoginModel);

            if (controller != null)
            {
                if (tempIsLogin == false || tempLoginModel == null)
                {
                    filterContext.Result = !controller.IsAsync ? route :
                    controller.HandleError(MessageType.RequireAuthorize,
                        "您尚未登录或者长时间未操作导致登录超时，要继续操必须先重新登录。", thisurl.ToString());

                }
                else
                {
                    filterContext.Controller.ViewBag.LoginUser = tempLoginModel;

                }
            }
            base.OnActionExecuting(filterContext);

        }
    }
}
