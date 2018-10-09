using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel
{
    public class LoginUserModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public AdminUserInfo AdminUserInfo { get; set; }
        /// <summary>
        /// 用户拥有的菜单(一级)
        /// </summary>
        public List<AdminMenuModel> MenuList { get; set; }
        /// <summary>
        /// 用户拥有的權限
        /// </summary>
        public HashSet<EnumPermission> PermissionSet { get; set; }
    }
}
