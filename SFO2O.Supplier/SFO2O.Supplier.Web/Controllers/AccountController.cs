using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFO2O.Supplier.ViewModels;
using SFO2O.Supplier.Businesses.Account;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models.Account;

namespace SFO2O.Supplier.Web.Controllers
{
    public class AccountController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                do
                {
                    var date = DateTime.Now.Date;
                    var userID = SupplierUserBll.GetSupplierUserID(model.UserName.SafeTrim());
                    if (userID <= 0)
                    {
                        model.ErrorInfoForUserName = "账号或密码错误";
                        break;
                    }
                    var counter = SupplierUserBll.GetSupplierCounter(userID, EnumCountType.LoginFail, date);
                    if (counter != null && counter.Value >= 10)
                    {
                        model.ErrorInfoForUserName = "该账号今天已超过验证次数限制，请明天再试";
                        break;
                    }
                    var userInfo = SupplierUserBll.GetUserInfoByLogin(model.UserName.SafeTrim(), model.Password);
                    if (userInfo != null)
                    {
                        if (userInfo.Status != 1)
                        {
                            model.ErrorInfoForUserName = "该账号已被禁用";
                        }
                        else if (userInfo.SupplierStatus != 1)
                        {
                            model.ErrorInfoForUserName = "该商户已被冻结";
                        }
                        else
                        {
                            var userMenuBll = new SupplierUserMenuBLL();
                            SupplierPermissionModel userPermission;
                            if (userInfo.IsAdmin == 1)
                            {
                                userPermission = userMenuBll.GetAllPermissionInfo();
                            }
                            else
                            {
                                userPermission = userMenuBll.GetMenuBySupplierUserId(userInfo.ID);
                            }
                            if (userPermission != null && userPermission.MenuList != null && userPermission.MenuList.Count > 0)
                            {
                                var loginUserModel = new LoginUserModel()
                                {
                                    SupplierUserInfo = userInfo,
                                    MenuList = userPermission.MenuList,
                                    PermissionSet = userPermission.PermissionSet
                                };
                                LoginHelper.Cache(Session, loginUserModel);
                                if (string.IsNullOrEmpty(model.ReturnUrl) || string.Equals(model.ReturnUrl, "/") || model.ReturnUrl.StartsWith("/?"))
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
                                model.ErrorInfoForUserName = "该账号的角色已被禁用";
                            }
                        }
                    }
                    else
                    {
                        model.ErrorInfoForUserName = "账号或密码错误";
                    }
                }
                while (false);
            }
            else
            {
                model.ErrorInfoForUserName = "请输入账号";
                model.ErrorInfoForPassword = "请输入密码";
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ForgetPassword(string supplierName, string userName)
        {
            supplierName = supplierName.SafeTrim();
            userName = userName.SafeTrim();
            var flag = true;
            var errors = new System.Collections.Hashtable();
            errors.Add("Error", "1");
            if (string.IsNullOrEmpty(supplierName))
            {
                errors.Add("SupplierNameWrong", "请输入公司名称");
                flag = false;
            }
            if (string.IsNullOrEmpty(userName))
            {
                errors.Add("UserNameWrong", "请输入账号");
                flag = false;
            }
            if (!flag)
            {
                return Json(errors);
            }
            else
            {
                var date = DateTime.Now.Date;
                var supplierID = SupplierUserBll.GetSupplierID(supplierName, userName);
                if (supplierID <= 0)
                {
                    errors.Add("ClearInput", "1");
                    errors.Add("UserNameWrong", "公司名称&登录账号错误或不匹配，请核实后重新输入");
                    return Json(errors);
                }
                var counter = SupplierUserBll.GetSupplierCounter(supplierID, EnumCountType.ForgetPassword, date);
                if (counter != null)
                {
                    if (DateTime.Now < counter.UpdateTime.AddMinutes(10))
                    {
                        return Json(new { Error = 1, Message = "密码重置邮件已经发送，如果您未收到重置邮件，请在10分钟后重新验证索取" });
                    }
                    if (counter.Value >= 5)
                    {
                        return Json(new { Error = 1, Message = "该账号今天已超过验证次数限制，请明天再试" });
                    }
                }
                var token = SupplierUserBll.GetFindPasswordToken(supplierName, userName);
                if (token == null)
                {
                    errors.Add("ClearInput", "1");
                    errors.Add("UserNameWrong", "公司名称&登录账号错误或不匹配，请核实后重新输入");
                    return Json(errors);
                }
                else
                {
                    return Json(new { Error = 0 });
                }
            }
        }

        [AllowAnonymous]
        public ActionResult FindPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new TransferResult("/Error/TokenExpired");
            }
            var fpToken = SupplierUserBll.VerifyFindPasswordToken(token);
            if (fpToken == null)
            {
                return new TransferResult("/Error/TokenExpired");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult FindPassword(string token, string password1, string password2)
        {
            var flag = true;
            if (string.IsNullOrEmpty(password1))
            {
                ViewBag.password1Wrong = "请输入新密码";
                flag = false;
            }
            if (string.IsNullOrEmpty(password2))
            {
                ViewBag.password2Wrong = "请再次输入新密码";
                flag = false;
            }
            if (!string.IsNullOrEmpty(password1) && !string.IsNullOrEmpty(password2) && password1 != password2)
            {
                ViewBag.password2Wrong = "密码不一致，请重新输入";
                flag = false;
            }
            if (!flag)
            {
                return View();
            }
            flag = SupplierUserBll.UpdatePassWordByToken(token, password1);
            if (flag)
            {
                return View("FindPasswordSuccess");
            }
            else
            {
                return new TransferResult("/Error/TokenExpired");
            }
        }
    }
}