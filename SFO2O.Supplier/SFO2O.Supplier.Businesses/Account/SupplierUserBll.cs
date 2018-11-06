using SFO2O.Supplier.Common;
using SFO2O.Supplier.Common.Security;
using SFO2O.Supplier.DAO.Account;
using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Account;
using SFO2O.Supplier.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Businesses.Account
{
    public class SupplierUserBll
    {
        private static readonly SupplierUserDAL supplierUserDAL = new SupplierUserDAL();

        public static SupplierUserInfo GetUserInfoById(int id)
        {
            return supplierUserDAL.GetUserByUserId(id);
        }

        public static int GetSupplierUserID(string userName)
        {
            return supplierUserDAL.GetSupplierUserID(userName);
        }

        public static SupplierUserInfo GetUserInfoByLogin(string userName, string password)
        {
            var loginTime = DateTime.Now;
            var passHash = MD5Hash.GetMd5String(password);
            var userInfo = supplierUserDAL.GetUserInfoByLogin(userName);
            if (userInfo != null)
            {
                if (!string.Equals(userInfo.Password, passHash, StringComparison.OrdinalIgnoreCase))
                {
                    supplierUserDAL.IncrementSupplierCounter(userInfo.ID, 0, loginTime.Date);
                    userInfo = null;
                }
                else if (userInfo.SupplierStatus == 1 && userInfo.Status == 1)
                {
                    userInfo.LastLoginTime = loginTime;
                    supplierUserDAL.UpdateUserLastLoginTime(userInfo.ID, loginTime);
                    GetSupplierLogo(userInfo);
                }
            }
            return userInfo;
        }

        private static void GetSupplierLogo(SupplierUserInfo userInfo)
        {
            if (userInfo.IsAdmin != 1)
            {
                userInfo.ImageUrl = supplierUserDAL.GetSupplierLogo(userInfo.SupplierID);
            }
            if (!string.IsNullOrEmpty(userInfo.ImageUrl))
            {
                userInfo.ImageUrl = ConfigHelper.ImageServer + userInfo.ImageUrl.Replace('\\', '/');
            }
        }

        public static LoginUserModel GetLoginUserModelByUserID(int id)
        {
            var userInfo = supplierUserDAL.GetUserByUserId(id);
            if (userInfo == null)
            {
                return null;
            }
            LoginUserModel model = new LoginUserModel();
            model.SupplierUserInfo = userInfo;
            if (userInfo.SupplierStatus == 1 && userInfo.Status == 1)
            {
                GetSupplierLogo(userInfo);
                SupplierPermissionModel userPermission;
                if (userInfo.IsAdmin == 1)
                {
                    userPermission = new SupplierUserMenuBLL().GetAllPermissionInfo();
                }
                else
                {
                    userPermission = new SupplierUserMenuBLL().GetMenuBySupplierUserId(userInfo.ID);
                }
                model.MenuList = userPermission.MenuList;
                model.PermissionSet = userPermission.PermissionSet;
            }
            return model;
        }

        public static string GetPassHashByUserID(int userID)
        {
            return supplierUserDAL.GetPassHashByUserID(userID);
        }

        public static int UpdatePassWordByUserID(int userID, string password)
        {
            return supplierUserDAL.UpdatePassWordByUserID(userID, MD5Hash.GetMd5String(password));
        }

        public static int GetSupplierID(string companyName, string userName)
        {
            return supplierUserDAL.GetSupplierID(companyName, userName);
        }

        public static SupplierCounter GetSupplierCounter(int objectID, EnumCountType countType, DateTime countDate)
        {
            return supplierUserDAL.GetSupplierCounter(objectID, (int)countType, countDate);
        }

        public static FindPasswordToken GetFindPasswordToken(string companyName, string userName)
        {
            DateTime countDate = DateTime.Now.Date;
            var supplierID = supplierUserDAL.GetSupplierID(companyName, userName);
            if (supplierID <= 0)
            {
                return null;
            }
            var token = supplierUserDAL.GetFindPasswordToken(supplierID);
            if (token == null)
            {
                token = new FindPasswordToken();
                var strToken = StringHelper.GetRandomString(50);
                var expiredTime = DateTime.Now.AddMinutes(60);
                if (supplierUserDAL.SaveFindPasswordToken(supplierID, strToken, expiredTime))
                {
                    token.Token = strToken;
                    token.ExpiredTime = expiredTime;
                }
                else
                {
                    return null;
                }
            }
            if (token != null)
            {
                const string emailFormat = @"<html>
<head>
    <meta charset='utf-8' />
</head>
" +
"<body style=\"font:12px/1.5em 'Microsoft yahei','Arial',Verdana,Helvetica,sans-serif;text-align:left;color:#000;margin:0px;\">" + 
@"
<div style='width:720px;margin:0 auto;'>
	<div style='padding:5px;'>
		親愛的 {0} 管理員：<br /><br />
您已通過健康绿氧商家管理賬號密碼重置的匹配驗證。請點擊以下鏈接重置您的賬號密碼（鏈接60分鐘內有效)：<br />
<a target='_blank' href='{1}'>{1}</a><br />
（如果上面的鏈接無法直接點擊，可以將其復制到瀏覽器地址欄打開）<br /><br />
------------------------------------------------------------------------------------------<br />
如果您錯誤的收到此郵件，可以不處理該郵件，也不要將此郵件轉發給陌生人。此為系統郵件，請勿回復。<br />
------------------------------------------------------------------------------------------<br />
感謝您對健康绿氧網站的支持與配合！
		<div style='text-align:right;'>
			健康绿氧電子商貿有限公司<br />{2}
		</div>
	</div>
</div>
</body>
</html>";
                var content = string.Format(emailFormat, companyName, ConfigHelper.GetSjWebSite + "/Account/FindPassword?token=" + token.Token, DateTime.Now.ToDateTimeString());
                var flag = Mail.SendMailToUser("健康绿氧商家管理賬號密碼重置", content, userName);
                if (flag)
                {
                    supplierUserDAL.IncrementSupplierCounter(supplierID, 1, countDate);
                    return token;
                }
            }
            return null;
        }

        public static FindPasswordToken VerifyFindPasswordToken(string token)
        {
            return supplierUserDAL.VerifyFindPasswordToken(token);
        }

        public static bool UpdatePassWordByToken(string token, string password)
        {
            return supplierUserDAL.UpdatePasswordByToken(token, MD5Hash.GetMd5String(password)) > 0;
        }

        public static PageOf<SupplierUserInfo> GetSupplierUserBySupplierID(int supplierID, PageDTO page)
        {
            return supplierUserDAL.GetSupplierUserBySupplierID(supplierID, page);
        }

        public static SupplierUserInfo GetSupplierUserBySupplierIDAndUserID(int supplierID, int id)
        {
            return supplierUserDAL.GetSupplierUserBySupplierIDAndUserID(supplierID, id);
        }

        public static PageOf<SupplierRoleInfo> GetSupplierRoleBySupplierID(int supplierID, PageDTO page)
        {
            return supplierUserDAL.GetSupplierRoleBySupplierID(supplierID, page);
        }

        public static IList<SupplierUserInfo> GetSupplierUserBySupplierIDAndRoleID(int supplierID, int roleID)
        {
            return supplierUserDAL.GetSupplierUserBySupplierIDAndRoleID(supplierID, roleID);
        }

        public static bool ExistsSupplierUserName(string userName)
        {
            return string.IsNullOrEmpty(userName) || supplierUserDAL.ExistsSupplierUserName(userName);
        }

        public static bool SaveSupplierUserInfo(int supplierID, SupplierUserInfo user)
        {
            try
            {
                user.Password = MD5Hash.GetMd5String(user.Password);
                return supplierUserDAL.SaveSupplierUserInfo(supplierID, user);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }

        public static bool ChangeSupplierUserStatus(int supplierID, int userID, int status)
        {
            return supplierUserDAL.ChangeSupplierUserStatus(supplierID, userID, status);
        }

        public static SupplierRoleInfo GetSupplierRoleInfo(int supplierID, int roleID)
        {
            return supplierUserDAL.GetSupplierRoleInfo(supplierID, roleID);
        }

        public static bool ExistsSupplierRoleName(int supplierID, int roleID, string roleName)
        {
            return string.IsNullOrEmpty(roleName) || supplierUserDAL.ExistsSupplierRoleName(supplierID, roleID, roleName);
        }

        public static bool SaveSupplierRoleInfo(int supplierID, SupplierRoleInfo role)
        {
            return supplierUserDAL.SaveSupplierRoleInfo(supplierID, role);
        }

        public static bool DeleteSupplierRoleInfo(int supplierID, int roleID)
        {
            return supplierUserDAL.DeleteSupplierRoleInfo(supplierID, roleID);
        }
    }
}
