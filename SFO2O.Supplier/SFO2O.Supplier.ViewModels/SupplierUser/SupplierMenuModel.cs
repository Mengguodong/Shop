using SFO2O.Supplier.Models;
using SFO2O.Supplier.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels
{
    public class SupplierMenuModel
    {
        //菜单的根菜单
        public SupplierMenuInfo root { get; set; }
        //菜单的子菜单
        public List<SupplierMenuInfo> children { get; set; }
    }

    public class SupplierPermissionModel
    {
        public List<SupplierMenuModel> MenuList { get; set; }

        public HashSet<EnumPermission> PermissionSet { get; set; }
    }

    public class SupplierRolePermissionModel
    {
        public SupplierRoleInfo Role { get; set; }

        public List<Tree<SupplierMenuInfo>> PermissionTree { get; set; }
    }

    public class Tree<T>
    {
        public T Node { get; set; }

        public bool? IsActive { get; set; }

        public List<Tree<T>> SubNodeList { get; set; }

        public bool Exists(Predicate<Tree<T>> match, bool falg = false)
        {
            if (falg && match(this))
            {
                return true;
            }
            return SubNodeList == null ? false : SubNodeList.Exists(p => p.Exists(match, true));
        }
    }
}
