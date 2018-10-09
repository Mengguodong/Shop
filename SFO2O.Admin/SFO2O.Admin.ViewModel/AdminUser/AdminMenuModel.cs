using SFO2O.Admin.Models;
using SFO2O.Admin.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel
{
    public class AdminMenuModel
    {
        //菜单的根菜单
        public AdminMenuInfo root { get; set; }
        //菜单的子菜单
        public List<AdminMenuInfo> children { get; set; }
    }

    public class AdminRolePermissionTreeModel
    {
        public AdminRoleInfo Role { get; set; }

        public List<Tree<AdminMenuInfo>> PermissionTree { get; set; }
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
