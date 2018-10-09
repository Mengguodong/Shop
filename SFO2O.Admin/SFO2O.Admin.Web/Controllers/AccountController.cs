using SFO2O.Admin.Businesses;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {

            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var userBll = new AdminUserBLL();
                var loginUserModel = userBll.GetLoginUserByLogin(model.UserName, model.Password);
                if (loginUserModel != null)
                {
                    if (loginUserModel.AdminUserInfo.Status != 1)
                    {
                        model.ErrorInfoForUserName = "該賬號已被禁用";
                    }
                    else if (loginUserModel.MenuList != null && loginUserModel.MenuList.Count > 0)
                    {
                        LoginHelper.Cache(loginUserModel);
                        if (string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(model.ReturnUrl);
                        }
                    }
                    else
                    {
                        model.ErrorInfoForUserName = "該用戶的角色已被禁用";
                    }
                }
                else
                {
                    model.ErrorInfoForUserName = "賬號或密碼錯誤";
                }
            }
            else
            {
                model.ErrorInfoForUserName = "請輸入賬號";
                model.ErrorInfoForPassword = "請輸入密碼";
            }
            return View(model);
        }
    }
}