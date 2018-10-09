using SFO2O.Admin.Common.Security;
using SFO2O.Admin.DAO.Admin;
using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Admin;
using SFO2O.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Businesses
{
    public class AdminUserBLL
    {
        private readonly AdminUserDAL adminUserDAL = new AdminUserDAL();

        #region 获取登录用户信息
        /// <summary>
        /// 获取用户列表（所有角色为客服且无分组的）
        /// </summary>
        /// <returns></returns>
        public LoginUserModel GetLoginUserByUserID(int userID)
        {
            LoginUserModel loginUserModel = null;

            AdminUserInfo userInfo = adminUserDAL.GetByUserID(userID);

            if (userInfo != null)
            {
                loginUserModel = GetUserPermission(userInfo);
            }
            return loginUserModel;
        }

        private LoginUserModel GetUserPermission(AdminUserInfo userInfo)
        {
            var loginUserModel = new LoginUserModel();
            loginUserModel.AdminUserInfo = userInfo;
            if (userInfo.Status == 1)
            {
                var lstMenuRoot = new List<AdminMenuModel>();
                IList<AdminMenuInfo> lstMenu;
                //获取菜单
                if (userInfo.IsAdmin)
                {
                    lstMenu = adminUserDAL.GetAllMenuPermission();
                }
                else
                {
                    lstMenu = adminUserDAL.GetMenuListByUserID(userInfo.id);
                }
                if (lstMenu != null && lstMenu.Count > 0)
                {
                    //添加主菜单
                    lstMenuRoot.AddRange(lstMenu.Where(root => root.ParentPermission == EnumPermission.None).Select(p => new AdminMenuModel
                    {
                        root = p,
                        //添加子菜单
                        children = lstMenu.Where(child => child.IsShow && child.ParentPermission == p.Permission).ToList()
                    }));
                }
                var lstPermission = lstMenu.Select(p => p.Permission).ToList();
                loginUserModel.MenuList = lstMenuRoot;
                loginUserModel.PermissionSet = new HashSet<EnumPermission>(lstPermission);
            }
            return loginUserModel;
        }

        /// <summary>
        /// 使用账号和密码登录
        /// </summary>
        /// <returns></returns>
        public LoginUserModel GetLoginUserByLogin(string userName, string password)
        {
            LoginUserModel loginUserModel = null;

            AdminUserInfo userInfo = adminUserDAL.GetByPassword(userName, MD5Hash.GetMd5String(password));

            if (userInfo != null)
            {
                if(userInfo.Status==1)
                {
                    var loginTime = DateTime.Now;
                    userInfo.LastLoginTime = loginTime;
                    adminUserDAL.UpdateUserLastLoginTime(userInfo.id, loginTime);
                }
                loginUserModel = GetUserPermission(userInfo);
            }

            return loginUserModel;
        }

        #endregion

        public string GetPassHashByUserID(int userID)
        {
            return adminUserDAL.GetPassHashByUserID(userID);
        }

        public bool UpdatePassWordByUserID(int userID, string password)
        {
            return adminUserDAL.UpdatePassWordByUserID(userID, MD5Hash.GetMd5String(password)) > 0;
        }

        public PageOf<AdminUserInfo> GetAdminUserList(PagingModel page)
        {
            return adminUserDAL.GetAdminUserList(page);
        }

        public IList<AdminUserInfo> GetAdminUserListByRoleID(int roleID)
        {
            return adminUserDAL.GetAdminUserListByRoleID(roleID);
        }

        public AdminUserInfo GetAdminUserInfo(int userID)
        {
            return adminUserDAL.GetAdminUserInfo(userID);
        }

        public bool ExistsAdminUserName(string userName)
        {
            return string.IsNullOrEmpty(userName) || adminUserDAL.ExistsAdminUserName(userName);
        }

        public bool SaveAdminUserInfo(AdminUserInfo user)
        {
            user.password = MD5Hash.GetMd5String(user.password);
            return adminUserDAL.SaveAdminUserInfo(user);
        }

        public bool ChangeAdminUserStatus(int userID, int status)
        {
            return adminUserDAL.ChangeAdminUserStatus(userID, status);
        }

        public PageOf<AdminRoleInfo> GetAdminRoleList(PagingModel page)
        {
            return adminUserDAL.GetAdminRoleList(page);
        }

        public AdminRoleInfo GetAdminRoleInfo(int roleID)
        {
            return adminUserDAL.GetAdminRoleInfo(roleID);
        }

        public bool ExistsAdminRoleName(int roleID, string roleName)
        {
            return string.IsNullOrEmpty(roleName) || adminUserDAL.ExistsAdminRoleName(roleID, roleName);
        }

        public bool SaveAdminRoleInfo(AdminRoleInfo role)
        {
            return adminUserDAL.SaveAdminRoleInfo(role);
        }

        public bool DeleteAdminRoleInfo(int roleID)
        {
            return adminUserDAL.DeleteAdminRoleInfo(roleID);
        }

        /// <summary>
        /// 获取全部菜单信息
        /// </summary>
        /// <returns></returns>
        public IList<AdminMenuInfo> GetAllMenuPermission()
        {
            return adminUserDAL.GetAllMenuPermission();
        }
    }
}
