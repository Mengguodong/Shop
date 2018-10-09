using SFO2O.Admin.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using System.Data;
using SFO2O.Admin.Models;
using SFO2O.Admin.Common;
using SFO2O.Admin.ViewModel;

namespace SFO2O.Admin.DAO.Admin
{
    public class AdminUserDAL : BaseDao
    {
        /// <summary>
        /// 根据Email获取用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public AdminUserInfo GetByUserID(int userID)
        {
            const string sql = @"
            select  id,Email,UserName,password,IsAdmin,TrueName,Status,HaveUpdate,HaveDelet
            ,LastLoginTime,CreateTime,createby,UpdateTime,updateby
            from AdminUser (nolock)
            where id=@ID ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            parameters.Append("ID", userID);


            return db.ExecuteSqlFirst<AdminUserInfo>(sql, parameters);
        }

        public AdminUserInfo GetByPassword(string userName, string passHash)
        {
            const string sql = @"
            select  id,Email,UserName,password,IsAdmin,TrueName,Status,HaveUpdate,HaveDelet
            ,LastLoginTime,CreateTime,createby,UpdateTime,updateby
            from AdminUser (nolock)
            where Status<>-1 AND UserName=@userName ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            parameters.Append("userName", userName);
         

            return db.ExecuteSqlFirst<AdminUserInfo>(sql, parameters);
        }
        public int UpdateUserLastLoginTime(int userId, DateTime datetime)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            string strsql = "UPDATE AdminUser SET LastLoginTime=@LastLoginTime WHERE ID=@ID";
            parameters.Append("ID", userId);
            parameters.Append("LastLoginTime", datetime);

            return db.ExecuteSqlNonQuery(strsql, parameters);
        }

        public string GetPassHashByUserID(int userID)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = "SELECT Password FROM AdminUser(NOLOCK) where status<>-1 and ID=@UserID";
            parameters.Append("UserID", userID);

            return db.ExecuteSqlScalar<String>(strsql, parameters);
        }

        public int UpdatePassWordByUserID(int userID, string passHash)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = "UPDATE AdminUser SET Password=@PassHash where ID=@UserID";
            parameters.Append("UserID", userID);
            parameters.Append("PassHash", passHash);

            return db.ExecuteSqlNonQuery(strsql, parameters);
        }

        public IList<AdminMenuInfo> GetMenuListByUserID(int userID)
        {
            const string sql = @"SELECT DISTINCT am.id,am.ModuleName,am.ModuleURL,am.Domain,am.SortValue,am.IsShow,am.Icon,IsButton
	,am.Permission,am.ParentPermission
FROM AdminUser au
INNER JOIN AdminUserRole(nolock) aur ON au.id = aur.UserID
INNER JOIN AdminRoleMenu(nolock) arm ON aur.RoleID = arm.RoleID
INNER JOIN AdminMenu(nolock) am ON arm.ModuleID = am.id
INNER JOIN AdminRole(nolock) ar on arm.RoleID = ar.id AND ar.Status=1
WHERE au.ID=@ID AND am.ParentPermission != -1
ORDER BY am.SortValue";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("ID", userID);

            return db.ExecuteSqlList<AdminMenuInfo>(sql, parameters);
        }



        public PageOf<AdminUserInfo> GetAdminUserList(PagingModel page)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string QUERY_SQL = "(SELECT * FROM AdminUser(NOLOCK) WHERE IsAdmin=0) AS t";

            string SQL = string.Format(@"select * from (select ROW_NUMBER() OVER(order by t.ID desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<AdminUserInfo>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<AdminUserInfo>(ds)
            };
        }

        public IList<AdminUserInfo> GetAdminUserListByRoleID(int roleID)
        {
            string QUERY_SQL = @"SELECT u.* FROM AdminUser(NOLOCK) u
INNER JOIN AdminUserRole(NOLOCK) ur ON ur.UserID = u.ID
INNER JOIN AdminRole(NOLOCK) r ON r.id = ur.RoleID AND r.[Status]=1
WHERE 1=1 AND r.id=@RoleID";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("RoleID", roleID);
            var userInfos = db.ExecuteSqlList<AdminUserInfo>(QUERY_SQL, parameters);
            return userInfos;
        }

        public bool ExistsAdminUserName(String userName)
        {
            String sql = "SELECT COUNT(*) FROM AdminUser(NOLOCK) WHERE UserName=@UserName";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("UserName", userName);
            return db.ExecuteSqlScalar<int>(sql, parameters) > 0;
        }

        public bool SaveAdminUserInfo(AdminUserInfo user)
        {
            string UPDATE_SQL = "";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("TrueName", user.TrueName);
            parameters.Append("Password", user.password);

            string Part_Sql = "";
            if (user.RoleIDList != null && user.RoleIDList.Count > 0)
                Part_Sql = "INSERT INTO AdminUserRole(UserID,RoleID,CreateTime)VALUES" + String.Join(",", user.RoleIDList.Select(p => "(@UserID," + p.ToString() + ",GetDate())"));

            if (user.id != 0) //编辑
            {
                UPDATE_SQL = string.Format(@"begin transaction 
                                declare @error int
                                set @error = 0
                                    DELETE ur
                                    FROM AdminUserRole ur
                                    INNER JOIN AdminUser u ON u.ID=ur.UserID
                                    WHERE u.ID=@UserID
                                set @error = @error + @@error 
                                    UPDATE AdminUser SET Password=@Password,TrueName=@TrueName,UpdateTime=GetDate() WHERE ID=@UserID
                                set @error = @error + @@error    
                                    {0} 
                                set @error = @error + @@error	
                                if @error <> 0  
                                rollback transaction   
                                else   
                                commit transaction", Part_Sql);
                parameters.Append("UserID", user.id);

            }
            else
            {
                UPDATE_SQL = string.Format(@"begin transaction 
                                    declare @error int;
                                    declare @UserID int; 
                                    set @error = 0;
                                         --添加用户
                                         INSERT INTO AdminUser(UserName,Password,IsAdmin,TrueName,Status,CreateTime)
                                         VALUES(@UserName,@Password,0,@TrueName,1,GetDate())
                                    set @error = @error + @@error   
                                    set @UserID = ( select Scope_Identity() as UserID)
                                         --添加关系
                                        {0}
                                    set @error = @error + @@error 
                                    if @error <> 0  
                                    rollback transaction   
                                    else   
                                    commit transaction", Part_Sql);
                parameters.Append("UserName", user.UserName);
            }
            try
            {
                return db.ExecuteSqlNonQuery(UPDATE_SQL, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }

        public bool ChangeAdminUserStatus(int userID, int status)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("UserID", userID);
            parameters.Append("Status", status);
            string UPDATE_SQL = "UPDATE AdminUser SET Status=@Status WHERE ID=@UserID";
            return db.ExecuteSqlNonQuery(UPDATE_SQL, parameters) > 0;
        }

        public AdminUserInfo GetAdminUserInfo(int userID)
        {
            string QUERY_SQL = "SELECT [id],[UserName],[TrueName] FROM [AdminUser] (nolock) WHERE id=@UserID";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("UserID", userID);
            var userInfo = db.ExecuteSqlFirst<AdminUserInfo>(QUERY_SQL, parameters);
            if (userInfo != null)
            {
                string _SQL = "SELECT [RoleID] FROM AdminUserRole (NOLOCK) WHERE UserID=" + userID.ToString();
                userInfo.RoleIDList = db.ExecuteSqlStringAccessor<int>(_SQL, new Int32RowMapper("RoleID")).ToList();
            }
            return userInfo;
        }

        public PageOf<AdminRoleInfo> GetAdminRoleList(PagingModel page)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string QUERY_SQL = @"(SELECT r.[id],[RoleName],[IsService],[Status],COUNT(UserID) UserCount
FROM [AdminRole] r (nolock) LEFT JOIN [AdminUserRole] m ON m.RoleID = r.id
WHERE r.[Status]=1
GROUP BY r.[id],[RoleName],[IsService],[Status]) AS t";

            string SQL = string.Format(@"SELECT * from (select ROW_NUMBER() OVER(order by t.id desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<AdminRoleInfo>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<AdminRoleInfo>(ds)
            };
        }

        public AdminRoleInfo GetAdminRoleInfo(int roleID)
        {
            string QUERY_SQL = "SELECT [id],[RoleName],[IsService],[Status] FROM [AdminRole] (nolock) WHERE id=@RoleID AND Status=1";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("RoleID", roleID);
            var roleInfo = db.ExecuteSqlFirst<AdminRoleInfo>(QUERY_SQL, parameters);
            if (roleInfo != null)
            {
                string _SQL = "SELECT [ModuleID] FROM AdminRoleMenu (NOLOCK) WHERE RoleID=" + roleID.ToString();
                roleInfo.ModuleIDList = db.ExecuteSqlStringAccessor<int>(_SQL, new Int32RowMapper("ModuleID")).ToList();
            }
            return roleInfo;
        }

        public bool ExistsAdminRoleName(int roleID, String roleName)
        {
            String sql;
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("RoleName", roleName);
            if (roleID == 0)
            {
                sql = "SELECT COUNT(*) FROM AdminRole(NOLOCK) WHERE RoleName=@RoleName AND Status=1";
            }
            else
            {
                sql = "SELECT COUNT(*) FROM AdminRole(NOLOCK) WHERE id<>@RoleID AND RoleName=@RoleName AND Status=1";
                parameters.Append("RoleID", roleID);
            }
            return db.ExecuteSqlScalar<int>(sql, parameters) > 0;
        }

        public bool SaveAdminRoleInfo(AdminRoleInfo role)
        {
            string UPDATE_SQL = "";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("RoleName", role.RoleName);

            string Part_Sql = "";
            if (role.ModuleIDList != null && role.ModuleIDList.Count > 0)
                Part_Sql = "INSERT INTO AdminRoleMenu(RoleID,ModuleID)VALUES" + String.Join(",", role.ModuleIDList.Select(p => "(@RoleID," + p.ToString() + ")"));

            if (role.id != 0) //编辑
            {
                UPDATE_SQL = string.Format(@"begin transaction 
                                declare @error int
                                set @error = 0
                                    DELETE rm
                                    FROM AdminRoleMenu rm
                                    INNER JOIN AdminRole r ON r.id=rm.RoleID
                                    WHERE r.id=@RoleID
                                set @error = @error + @@error 
                                    update AdminRole set RoleName=@RoleName WHERE id=@RoleID
                                set @error = @error + @@error    
                                    {0} 
                                set @error = @error + @@error	
                                if @error <> 0  
                                rollback transaction   
                                else   
                                commit transaction", Part_Sql);
                parameters.Append("RoleID", role.id);

            }
            else
            {
                UPDATE_SQL = string.Format(@"begin transaction 
                                    declare @error int;
                                    declare @RoleID int; 
                                    set @error = 0;
                                    set @RoleID= 0  
                                         --添加角色
                                         insert into AdminRole(RoleName,Status)values(@RoleName,1)
                                    set @error = @error + @@error   
                                    set @RoleID = ( select Scope_Identity() as RoleID)
                                         --添加关系
                                        {0}
                                    set @error = @error + @@error 
                                    if @error <> 0  
                                    rollback transaction   
                                    else   
                                    commit transaction", Part_Sql);
            }
            try
            {
                return db.ExecuteSqlNonQuery(UPDATE_SQL, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }

        public bool DeleteAdminRoleInfo(int roleID)
        {
            string QUERY_SQL = @"IF NOT EXISTS(SELECT RoleID FROM AdminUserRole WHERE RoleID=@RoleID)
BEGIN
	UPDATE AdminRole SET Status=2 WHERE id=@RoleID
END";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("RoleID", roleID);
            return db.ExecuteSqlNonQuery(QUERY_SQL, parameters) > 0;
        }

        /// <summary>
        /// 获取全部菜单权限信息
        /// </summary>
        /// <param name="supplierType"></param>
        /// <returns></returns>
        public IList<AdminMenuInfo> GetAllMenuPermission()
        {
            string sql = @"SELECT am.id,am.ModuleName,am.ModuleURL,am.Domain,am.SortValue,am.IsShow,am.Icon,IsButton
	,am.Permission,am.ParentPermission
FROM AdminMenu(nolock)am WHERE am.ParentPermission<>-1
ORDER BY am.SortValue";

            var db = DbSFO2ORead;

            return db.ExecuteSqlList<AdminMenuInfo>(sql);
        }

        public class Int32RowMapper : IRowMapper<Int32>
        {
            string ColumnName;
            public Int32RowMapper(string columnName)
            {
                ColumnName = columnName;
            }
            public Int32 MapRow(IDataRecord row)
            {
                return Convert.ToInt32(row[ColumnName]);
            }
        }
    }
}
