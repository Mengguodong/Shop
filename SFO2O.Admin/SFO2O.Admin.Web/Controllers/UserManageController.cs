using SFO2O.Admin.Businesses;
using SFO2O.Admin.Common;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Admin;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFO2O.Admin.Web.Controllers
{
    public class UserManageController : BaseController
    {
        private AdminUserBLL adminUserBLL = new AdminUserBLL();

        [RequirePermission(EnumPermission.Account_UserManage)]
        public ActionResult UserList(int PageIndex = 1)
        {
            var userID = UserID;
            var page = new PagingModel() { PageIndex = PageIndex, PageSize = 50 };
            try
            {
                var dataList = adminUserBLL.GetAdminUserList(page);
                return View(dataList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(new PageOf<AdminUserInfo>());
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_UserManage_Add, EnumPermission.Account_UserManage_Edit)]
        public ActionResult EditUser(int id = 0)
        {
            var userID = UserID;
            try
            {
                AdminUserInfo userInfo;
                if (id > 0)
                {
                    userInfo = adminUserBLL.GetAdminUserInfo(id);
                }
                else
                {
                    userInfo = new AdminUserInfo();
                }
                var page = new PagingModel() { PageIndex = 1, PageSize = int.MaxValue };
                var RoleList = adminUserBLL.GetAdminRoleList(page);
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
        public JsonResult SaveUser(AdminUserInfo userinfo)
        {
            var userID = UserID;
            try
            {
                if (userinfo.id == 0 && adminUserBLL.ExistsAdminUserName(userinfo.UserName))
                {
                    return Json(new { Error = 1, Message = "此賬號已被佔用" });
                }
                var falg = adminUserBLL.SaveAdminUserInfo(userinfo);
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
        public ActionResult ChangeUserStatus(int userID, int status)
        {
            try
            {
                var falg = adminUserBLL.ChangeAdminUserStatus(userID, status);
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
            var userID = UserID;
            var page = new PagingModel() { PageIndex = PageIndex, PageSize = 50 };

            try
            {
                var dataList = adminUserBLL.GetAdminRoleList(page);
                return View(dataList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(new PageOf<AdminRoleInfo>());
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_RoleManage_ViewUser)]
        public ActionResult ViewGroupUsers(int id = 0)
        {
            var userID = UserID;
            try
            {
                var dataList = adminUserBLL.GetAdminUserListByRoleID(id);
                return View(dataList);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return View(new List<AdminUserInfo>());
        }

        [RequirePermission(EnumPermission.Account_RoleManage_Add, EnumPermission.Account_RoleManage_Edit)]
        public ActionResult EditGroup(int id = 0)
        {
            AdminRolePermissionTreeModel model = new AdminRolePermissionTreeModel();
            var userID = UserID;
            try
            {
                if (id == 0)
                {
                    model.Role = new AdminRoleInfo();
                }
                else
                {
                    model.Role = adminUserBLL.GetAdminRoleInfo(id);
                    if (model.Role == null)
                    {
                        throw new Exception();
                    }
                }
                var ModuleIDs = model.Role.ModuleIDList ?? new List<int>();
                var lstMenu = adminUserBLL.GetAllMenuPermission();
                var dic = lstMenu.ToDictionary(p => p.Permission,
                    p => new Tree<AdminMenuInfo>()
                    {
                        Node = p,
                        IsActive = ModuleIDs.Contains(p.id) ? (bool?)true : null,
                        SubNodeList = new List<Tree<AdminMenuInfo>>()
                    });
                foreach (var menu in lstMenu.Where(p => p.ParentPermission != EnumPermission.None))
                {
                    Tree<AdminMenuInfo> node;
                    if (dic.TryGetValue(menu.ParentPermission, out node))
                    {
                        node.SubNodeList.Add(dic[menu.Permission]);
                    }
                }
                var PermissionTree = new List<Tree<AdminMenuInfo>>();
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
            }
            return View(model);
        }

        [HttpPost]
        [RequirePermission(EnumPermission.Account_RoleManage_Add, EnumPermission.Account_RoleManage_Edit)]
        public ActionResult SaveGroup(AdminRoleInfo roleinfo)
        {
            try
            {
                if (adminUserBLL.ExistsAdminRoleName(roleinfo.id, roleinfo.RoleName))
                {
                    return Json(new { Error = 1, Message = "此分組名已存在" });
                }
                var falg = adminUserBLL.SaveAdminRoleInfo(roleinfo);
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
            try
            {
                var falg = adminUserBLL.DeleteAdminRoleInfo(id);
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