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

namespace SFO2O.Supplier.Web.Controllers
{
    public class UserManageController : BaseController
    {
        [RequirePermission(EnumPermission.Account_UserManage)]
        public ActionResult UserList(int PageIndex = 1)
        {
            var supplierID = CurrentUser.SupplierID;
            var page = new PageDTO() { PageIndex = PageIndex, PageSize = 50 };
            try
            {
                var dataList = SupplierUserBll.GetSupplierUserBySupplierID(supplierID, page);
                return View(dataList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(new PageOf<SupplierUserInfo>());
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_UserManage_Add, EnumPermission.Account_UserManage_Edit)]
        public ActionResult EditUser(int id = 0)
        {
            var supplierID = CurrentUser.SupplierID;
            try
            {
                SupplierUserInfo userInfo;
                if (id > 0)
                {
                    userInfo = SupplierUserBll.GetSupplierUserBySupplierIDAndUserID(supplierID, id);
                }
                else
                {
                    userInfo = new SupplierUserInfo();
                }
                var page = new PageDTO() { PageIndex = 1, PageSize = int.MaxValue };
                var RoleList = SupplierUserBll.GetSupplierRoleBySupplierID(supplierID, page);
                ViewBag.RoleList = RoleList.Items;
                return View(userInfo);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Content("");
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_UserManage_Add, EnumPermission.Account_UserManage_Edit)]
        public JsonResult SaveUser(SupplierUserInfo userinfo)
        {
            if (userinfo.ID == 0)
            {
                userinfo.UserName = userinfo.UserName.SafeTrim();
                if (string.IsNullOrEmpty(userinfo.UserName) )
                {
                    return Json(new { Error = 1, Message = "請輸入賬號" });
                }
                if (userinfo.UserName.Length > 50)
                {
                    return Json(new { Error = 1, Message = "賬號長度不能超過50個字符" });
                }
                if (string.IsNullOrEmpty(userinfo.Password))
                {
                    return Json(new { Error = 1, Message = "請輸入密碼" });
                }
                if (userinfo.Password.Length < 6 || userinfo.Password.Length > 32)
                {
                    return Json(new { Error = 1, Message = "密碼長度只允許為6到32个字符" });
                }
                if (!string.IsNullOrEmpty(userinfo.NickName) && (userinfo.NickName.Length < 4 || userinfo.NickName.Length > 20))
                {
                    return Json(new { Error = 1, Message = "使用者姓名長度只允許為6到32个字符" });
                }
            }
            var supplierID = CurrentUser.SupplierID;
            try
            {
                if (userinfo.ID == 0 && SupplierUserBll.ExistsSupplierUserName(userinfo.UserName))
                {
                    return Json(new { Error = 1, Message = "此賬號已被佔用" });
                }
                var falg = SupplierUserBll.SaveSupplierUserInfo(supplierID, userinfo);
                if (falg)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1 });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1, Message = ex.Message });
            }
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_UserManage_ChangeStatus)]
        public ActionResult ChangeUserStatus(int userID,int status)
        {
            var supplierID = CurrentUser.SupplierID;
            try
            {
                var falg = SupplierUserBll.ChangeSupplierUserStatus(supplierID, userID, status);
                if (falg)
                {
                    if (status == 2)
                    {
                        //账号被禁用的时候使用户的会话失效
                        LoginHelper.RemoveLoginInfo(userID);
                    }
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1 });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1, Message = ex.Message });
            }
        }

        [RequirePermission(EnumPermission.Account_RoleManage)]
        public ActionResult GroupList(int PageIndex = 1)
        {
            var supplierID = CurrentUser.SupplierID;
            var page = new PageDTO() { PageIndex = PageIndex, PageSize = 50 };

            try
            {
                var dataList = SupplierUserBll.GetSupplierRoleBySupplierID(supplierID, page);
                return View(dataList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(new PageOf<SupplierRoleInfo>());
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_RoleManage_ViewUser)]
        public ActionResult ViewGroupUsers(int id = 0)
        {
            var supplierID = CurrentUser.SupplierID;
            try
            {
                var dataList = SupplierUserBll.GetSupplierUserBySupplierIDAndRoleID(supplierID, id);
                return View(dataList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(new List<SupplierUserInfo>());
        }

        [RequirePermission(EnumPermission.Account_RoleManage_Add, EnumPermission.Account_RoleManage_Edit)]
        public ActionResult EditGroup(int id = 0)
        {
            SupplierRolePermissionModel model = new SupplierRolePermissionModel();
            var supplierID = CurrentUser.SupplierID;
            try
            {
                if (id == 0)
                {
                    model.Role = new SupplierRoleInfo();
                }
                else
                {
                    model.Role = SupplierUserBll.GetSupplierRoleInfo(supplierID, id);
                    if (model.Role == null)
                    {
                        throw new Exception("未找到用戶分組(id:" + id.ToString() + ")");
                    }
                }
                var menuIds = model.Role.MenuIdList ?? new List<int>();
                var lstMenu = new SupplierUserMenuBLL().GetAllMenuPermission();
                var dic = lstMenu.ToDictionary(p => p.Permission,
                    p => new Tree<SupplierMenuInfo>()
                    {
                        Node = p,
                        IsActive = menuIds.Contains(p.MenuId) ? (bool?)true : null,
                        SubNodeList = new List<Tree<SupplierMenuInfo>>()
                    });
                foreach (var menu in lstMenu.Where(p => p.ParentPermission != EnumPermission.None))
                {
                    Tree<SupplierMenuInfo> node;
                    if (dic.TryGetValue(menu.ParentPermission, out node))
                    {
                        node.SubNodeList.Add(dic[menu.Permission]);
                    }
                }
                var PermissionTree = new List<Tree<SupplierMenuInfo>>();
                PermissionTree.AddRange(lstMenu.Where(p => p.ParentPermission == EnumPermission.None).Select(p => dic[p.Permission]));
                foreach (var node in PermissionTree.Where(p => p.IsActive == true))
                {
                    if (node.Exists(p => !p.IsActive.HasValue))
                    {
                        node.IsActive = false;
                    }
                }
                model.PermissionTree = PermissionTree;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new TransferResult("/Error/PageNotFound");
            }
            return View(model);
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_RoleManage_Add, EnumPermission.Account_RoleManage_Edit)]
        public ActionResult SaveGroup(SupplierRoleInfo roleinfo)
        {
            var supplierID = CurrentUser.SupplierID;
            try
            {
                if (SupplierUserBll.ExistsSupplierRoleName(supplierID, roleinfo.RoleID, roleinfo.RoleName))
                {
                    return Json(new { Error = 1, Message = "此分組名已存在" });
                }
                var falg = SupplierUserBll.SaveSupplierRoleInfo(supplierID, roleinfo);
                if (falg)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1 });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1, Message = ex.Message });
            }
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_RoleManage_Delete)]
        public ActionResult DeleteGroup(int id)
        {
            var supplierID = CurrentUser.SupplierID;
            try
            {
                var falg = SupplierUserBll.DeleteSupplierRoleInfo(supplierID, id);
                if (falg)
                {
                    return Json(new { Error = 0 });
                }
                else
                {
                    return Json(new { Error = 1 });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new { Error = 1, Message = ex.Message });
            }
        }
    }
}