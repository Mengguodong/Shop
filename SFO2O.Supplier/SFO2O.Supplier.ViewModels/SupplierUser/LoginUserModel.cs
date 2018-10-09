using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels
{
    public class LoginUserModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public SupplierUserInfo SupplierUserInfo { get; set; }
        /// <summary>
        /// 用户拥有的菜单(一级)
        /// </summary>
        public IList<SupplierMenuModel> MenuList { get; set; }
        /// <summary>
        /// 用户拥有的權限
        /// </summary>
        public HashSet<EnumPermission> PermissionSet { get; set; }
    }
}
