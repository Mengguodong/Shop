using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SFO2O.M.ViewModel.Account;
using SFO2O.M.Controllers.Common;
using SFO2O.BLL.Account;

namespace SFO2O.M.Controllers.Filters
{
    /// <summary>
    /// 判断是否首次下单
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FirstOrderAuthorizeAttribute:ActionFilterAttribute
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
                        action = "Anuthorization",
                        return_url = thisurl
                    }));

            LoginUserModel tempLoginModel = null;
            BaseController controller = filterContext.Controller as BaseController;
            var tempIsLogin = LoginHelper.CheckSession(out tempLoginModel);

            if (controller != null)
            {
                if (tempLoginModel.FirstOrderAuthorize !=1)
                {
                    AccountBll accountBll = new AccountBll();
                    if (!accountBll.IsFirstOrderAuthorize(tempLoginModel.UserName))
                    {
                        filterContext.Result = !controller.IsAsync ? route :
                        controller.HandleError(MessageType.RequireAuthorize,
                            "首次购买授权提示。", thisurl.ToString());
                    }

                }
            }
            base.OnActionExecuting(filterContext);

        }
    }
}
