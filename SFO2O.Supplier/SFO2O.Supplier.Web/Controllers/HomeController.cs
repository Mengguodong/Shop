using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFO2O.Supplier.ViewModels;
using SFO2O.Supplier.Businesses.Account;
using SFO2O.Supplier.Common;
using System.Web.Security;
using SFO2O.Supplier.Common.Security;
using SFO2O.Supplier.Businesses;

namespace SFO2O.Supplier.Web.Controllers
{
    public class HomeController : BaseController
    {
        private HomeBLL homeBLL = new HomeBLL();

        public ActionResult Index()
        {
            HomePageViewModel model;
            try
            {
                model = homeBLL.GetHomePageStatistics(CurrentUser.SupplierID);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                model = new HomePageViewModel();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword))
            {
                return Json(new { Error = 1, OldPassword = "请输入旧密码" });
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return Json(new { Error = 1, NewPassword = "请输入新密码" });
            }
            try
            {
                var oldPass = MD5Hash.GetMd5String(oldPassword);
                var oriPass = SupplierUserBll.GetPassHashByUserID(CurrentUser.ID);
                if (!string.Equals(oriPass, oldPass, StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { Error = 1, OldPassword = "旧密码错误" });
                }
                var flag = SupplierUserBll.UpdatePassWordByUserID(CurrentUser.ID, newPassword) > 0;
                if (flag)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1, Message = "密码修改失败" });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1, Message = ex.Message });
            }
        }

        public ActionResult LoginOut()
        {
            LoginHelper.RemoveCurrentLoginInfo();

            string redirectUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                redirectUrl = HttpUtility.UrlEncode(Request.UrlReferrer.PathAndQuery);
            }
            string loginUrl = FormsAuthentication.LoginUrl;
            if (Request.HttpMethod == System.Net.WebRequestMethods.Http.Get && !string.IsNullOrEmpty(redirectUrl))
            {
                loginUrl += "?ReturnUrl=" + redirectUrl;
            }
            return Redirect(loginUrl);
        }
    }
}