using SFO2O.Admin.Businesses;
using SFO2O.Admin.Common;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SFO2O.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private HomeBLL homeBLL = new HomeBLL();

        public ActionResult Index()
        {
            HomePageViewModel model;
            try
            {
                model = homeBLL.GetHomePageStatistics();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                model = new HomePageViewModel();
            }
            return View(model);
        }

        public ActionResult TopSellSupplier()
        {
            List<SupplierSellRank> model;
            try
            {
                model = homeBLL.GetTopSupplierSellRank();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                model = new List<SupplierSellRank>();
            }
            return View(model);
        }
        public ActionResult Top50SellProduct()
        {
            List<ProductSellRank> model;
            try
            {
                model = homeBLL.GetTop50ProductSellRank();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                model = new List<ProductSellRank>();
            }
            return View(model);
        }

        public ActionResult Top50ReturnProduct()
        {
            List<ProductReturnRank> model;
            try
            {
                model = homeBLL.GetTop50ProductReturnRank();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                model = new List<ProductReturnRank>();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldPassword,string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword))
            {
                return Json(new { Error = 1, Message = "請輸入舊密碼" });
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return Json(new { Error = 1, Message = "請輸入新密碼" });
            }
            try
            {
                var oldPass = MD5Hash.GetMd5String(oldPassword);
                AdminUserBLL adminUserBll = new AdminUserBLL();
                var oriPass = adminUserBll.GetPassHashByUserID(UserID);
                if (!string.Equals(oriPass, oldPass, StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { Error = 1, Message = "舊密碼錯誤" });
                }
                var flag = adminUserBll.UpdatePassWordByUserID(UserID, newPassword);
                if (flag)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1, Message = "密碼修改失敗" });
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