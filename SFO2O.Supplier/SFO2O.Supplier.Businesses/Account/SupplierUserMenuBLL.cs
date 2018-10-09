using SFO2O.Supplier.Common;
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
    public class SupplierUserMenuBLL
    {
        private readonly SupplierUserMenuDAL supplierUserMenuDAL = new SupplierUserMenuDAL();

        /// <summary>
        /// 获取全部菜单信息
        /// </summary>
        /// <returns></returns>
        public IList<SupplierMenuInfo> GetAllMenuPermission()
        {
            return supplierUserMenuDAL.GetAllMenuPermission();

        }

        /// <summary>
        /// 获取全部菜单权限
        /// </summary>
        /// <returns></returns>
        public SupplierPermissionModel GetAllPermissionInfo()
        {
            return ConvertToPermissionModel(GetAllMenuPermission());

        }

        public SupplierPermissionModel GetMenuBySupplierUserId(int userId)
        {
            //获取菜单
            IList<SupplierMenuInfo> lstMenu = supplierUserMenuDAL.GetMenuBySupplierUserId(userId);
            return ConvertToPermissionModel(lstMenu);
        }

        private SupplierPermissionModel ConvertToPermissionModel(IList<SupplierMenuInfo> lstMenu)
        {
            SupplierPermissionModel model = new SupplierPermissionModel();
            var lstMenuRoot = new List<SupplierMenuModel>();
            if (lstMenu != null && lstMenu.Count > 0)
            {
                //添加主菜单
                lstMenuRoot.AddRange(lstMenu.Where(root => root.IsShow && root.ParentPermission == EnumPermission.None).Select(p => new SupplierMenuModel
                {
                    root = p,
                    //添加子菜单
                    children = lstMenu.Where(child => child.IsShow && child.ParentPermission == p.Permission).ToList()
                }));
                var lstPermission = lstMenu.Select(p => p.Permission).ToList();
                model.MenuList = lstMenuRoot;
                model.PermissionSet = new HashSet<EnumPermission>(lstPermission);
            }
            return model;
        }
    }
}
