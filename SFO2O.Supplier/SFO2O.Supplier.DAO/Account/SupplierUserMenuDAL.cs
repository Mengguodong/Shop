using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Models.Account;

namespace SFO2O.Supplier.DAO.Account
{
    public class SupplierUserMenuDAL : BaseDao
    {
        /// <summary>
        /// 获取全部菜单权限信息
        /// </summary>
        /// <param name="supplierType"></param>
        /// <returns></returns>
        public IList<SupplierMenuInfo> GetAllMenuPermission()
        {
            string sql = @"SELECT sm.MenuId,sm.MenuName,sm.MenuUrl,sm.IsShow,sm.DataValue,sm.SortId
    ,sm.MenuDesc,ClassName,sm.Permission,sm.ParentPermission
FROM SupplierMenu(nolock)sm WHERE sm.ParentPermission<>-1
ORDER BY sm.ParentPermission,sm.SortId,sm.Permission";

            var db = DbSFO2ORead;

            return db.ExecuteSqlList<SupplierMenuInfo>(sql);
        }

        /// <summary>
        /// 根据账号Id获取用户全部权限
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IList<SupplierMenuInfo> GetMenuBySupplierUserId(int userId)
        {
            string sql = @"SELECT DISTINCT sm.MenuId,sm.MenuName,sm.MenuUrl,sm.IsShow,sm.DataValue,sm.SortId
	,sm.MenuDesc,ClassName,sm.Permission,sm.ParentPermission
FROM SupplierUser su
INNER JOIN SupplierUserRole(nolock) sur ON su.ID = sur.UserID
INNER JOIN SupplierMenuRole(nolock) smr ON sur.RoleId = smr.RoleId
INNER JOIN SupplierMenu(nolock) sm ON smr.MenuId = sm.MenuId
INNER JOIN SupplierRole(nolock)sr on smr.RoleId=sr.RoleId AND sr.Status=1
WHERE su.ID = @UserId
ORDER BY sm.ParentPermission,sm.Permission";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("UserId", userId);

            return db.ExecuteSqlList<SupplierMenuInfo>(sql, parameters);

        }
    }
}
