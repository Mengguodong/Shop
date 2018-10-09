using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Models.Account;
using SFO2O.Supplier.Models;
using System.Data;
using SFO2O.Supplier.Common;

namespace SFO2O.Supplier.DAO.Account
{
    public class SupplierUserDAL : BaseDao
    {
        private const string GETBYID_SQL = @"
    select  su.ID,su.ID as UserID,Su.SupplierID,SU.UserName,Password,SU.Status,IsAdmin,ss.StoreName,s.Status AS SupplierStatus,
    NickName,Gender,ImageUrl,Su.CreateTime,Su.CreateBy,Su.UpdateTime,Su.UpdateBy,Su.LastLoginTime,s.CompanyName,S.TrueName
    from SupplierUser (nolock) su 
    inner join Supplier(nolock) s on su.SupplierId=s.SupplierId
    left join SupplierStore ss(NOLOCK) on ss.SupplierID = su.SupplierID ";

        public SupplierUserInfo GetUserByUserId(int userId)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = GETBYID_SQL + " where s.status<>-1 and su.status<>-1 and su.ID=@ID";
            parameters.Append("ID", userId);

            return db.ExecuteSqlFirst<SupplierUserInfo>(strsql, parameters);
        }
        public String GetSupplierLogo(int supplierID)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = @"SELECT StoreLogoPath FROM SupplierStore(nolock) WHERE SupplierID=@SupplierID";
            parameters.Append("SupplierID", supplierID);

            return db.ExecuteSqlScalar<String>(strsql, parameters);
        }

        public int GetSupplierUserID(string userName)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = @"DECLARE @SupplierUserID int
SET @SupplierUserID = 0
SELECT @SupplierUserID=su.[ID] FROM [SupplierUser](NOLOCK) su
INNER JOIN Supplier(nolock) s on su.SupplierId=s.SupplierId
WHERE  s.status<>-1 and su.status<>-1 and su.[UserName]=@userName
SELECT @SupplierUserID";
            parameters.Append("UserName", userName);
            return db.ExecuteSqlScalar<int>(strsql, parameters);
        }

        public SupplierUserInfo GetUserInfoByLogin(string userName)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = GETBYID_SQL + " where s.status<>-1 and su.status<>-1 and su.UserName=@userName";
            parameters.Append("userName", userName);

            return db.ExecuteSqlFirst<SupplierUserInfo>(strsql, parameters);
        }
        public int UpdateUserLastLoginTime(int userId, DateTime datetime)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            string strsql = "UPDATE SupplierUser SET LastLoginTime=@LastLoginTime WHERE ID=@ID";
            parameters.Append("ID", userId);
            parameters.Append("LastLoginTime", datetime);

            return db.ExecuteSqlNonQuery(strsql, parameters);
        }

        public string GetPassHashByUserID(int userID)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = "SELECT Password FROM SupplierUser(NOLOCK)su where su.status<>-1 and su.ID=@UserID";
            parameters.Append("UserID", userID);

            return db.ExecuteSqlScalar<String>(strsql, parameters);
        }

        public int UpdatePassWordByUserID(int userID, string passHash)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = "UPDATE SupplierUser SET Password=@PassHash where ID=@UserID";
            parameters.Append("UserID", userID);
            parameters.Append("PassHash", passHash);

            return db.ExecuteSqlNonQuery(strsql, parameters);
        }

        public int GetSupplierID(string companyName, string userName)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = @"DECLARE @SupplierID int
SET @SupplierID = 0
SELECT @SupplierID=[SupplierID] FROM [Supplier](NOLOCK) WHERE [CompanyName]=@CompanyName AND [UserName]=@UserName
SELECT @SupplierID";
            parameters.Append("CompanyName", companyName);
            parameters.Append("UserName", userName);
            return db.ExecuteSqlScalar<int>(strsql, parameters);
        }

        public SupplierCounter GetSupplierCounter(int objectID, int countType, DateTime countDate)
        {
            String sql = "SELECT * FROM [SupplierCounter](NOLOCK) WHERE ObjectID=@ObjectID AND CountType=@CountType AND CountDate=@CountDate";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("ObjectID", objectID);
            parameters.Append("CountType", countType);
            parameters.Append("CountDate", countDate);
            var ds = db.ExecuteSqlDataSet(sql, parameters);
            return DataMapHelper.DataSetToObject<SupplierCounter>(ds); ;
        }

        public bool IncrementSupplierCounter(int objectID, int countType, DateTime countDate)
        {
            String sql = @"BEGIN TRANSACTION
DECLARE @error int
SET @error = 0
IF EXISTS (SELECT Value FROM [SupplierCounter] WHERE ObjectID=@ObjectID AND CountType=@CountType AND CountDate=@CountDate)
	UPDATE [SupplierCounter] SET UpdateTime=GETDATE(),Value=Value+1 WHERE ObjectID=@ObjectID AND CountType=@CountType AND CountDate=@CountDate
ELSE
    INSERT INTO [SupplierCounter]([ObjectID],[CountType],[CountDate],[UpdateTime],[Value])VALUES(@ObjectID,@CountType,@CountDate,GETDATE(),1)
SET @error = @error + @@error
IF @error <> 0
ROLLBACK TRANSACTION
ELSE
COMMIT TRANSACTION";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("ObjectID", objectID);
            parameters.Append("CountType", countType);
            parameters.Append("CountDate", countDate);
            return db.ExecuteSqlNonQuery(sql, parameters) > 0;
        }

        public FindPasswordToken GetFindPasswordToken(int supplierID)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

                string strsql = @"SELECT [Token],[ExpiredTime]
FROM [SupplierToken](NOLOCK) WHERE ObjectID=@SupplierID AND [TokenType]=1 AND [Status]=1 AND [ExpiredTime]>GetDate()";
                parameters.Append("SupplierID", supplierID);
                DataSet ds2 = db.ExecuteSqlDataSet(strsql, parameters);
                var token = DataMapHelper.DataSetToObject<FindPasswordToken>(ds2);
            return token;
        }

        public bool SaveFindPasswordToken(int supplierId, string token, DateTime expiredTime)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = "INSERT INTO [SupplierToken]([ObjectID],[TokenType],[Token],[ExpiredTime],[Status])VALUES(@ObjectID,1,@Token,@ExpiredTime,1)";
            parameters.Append("ObjectID", supplierId);
            parameters.Append("Token", token);
            parameters.Append("ExpiredTime", expiredTime);

            return db.ExecuteSqlNonQuery(strsql, parameters) > 0;
        }

        public FindPasswordToken VerifyFindPasswordToken(string token)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = @"SELECT s.[SupplierID],[UserName],[CompanyName],st.[Token],st.[ExpiredTime]
FROM [Supplier](NOLOCK) s
INNER JOIN [SupplierToken](NOLOCK) st ON s.SupplierID = st.ObjectID AND st.[TokenType]=1
WHERE st.[ExpiredTime]>GetDate() AND st.[Status]=1 AND st.[Token]=@Token";
            parameters.Append("Token", token);

            DataSet ds = db.ExecuteSqlDataSet(strsql, parameters);
            return DataMapHelper.DataSetToObject<FindPasswordToken>(ds);
        }

        public int UpdatePasswordByToken(string token, string passHash)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string strsql = @"begin transaction
declare @SupplierID int
declare @TokenID int
declare @error int
set @SupplierID = 0
set @error = 0
    SELECT @TokenID=ID,@SupplierID=ISNULL(ObjectID,0) FROM SupplierToken
    WHERE Token=@token AND TokenType=1 AND [Status]=1 AND ExpiredTime>GETDATE()
if(@SupplierID>0)
begin
	UPDATE SupplierToken SET [Status]=2 WHERE ID=@TokenID
	set @error = @error + @@error 
	UPDATE SupplierUser SET Password=@PassHash WHERE SupplierID=@SupplierID AND IsAdmin=1
	set @error = @error + @@error 
end
if @error <> 0  
rollback transaction   
else   
commit transaction";
            parameters.Append("Token", token);
            parameters.Append("PassHash", passHash);

            return db.ExecuteSqlNonQuery(strsql, parameters);
        }

        public PageOf<SupplierUserInfo> GetSupplierUserBySupplierID(int supplierID, PageDTO page)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string QUERY_SQL = "(" + GETBYID_SQL + " where s.SupplierId=@SupplierID AND su.IsAdmin=0) AS t";
            parameters.Append("SupplierID", supplierID);

            string SQL = string.Format(@"select * from (select ROW_NUMBER() OVER(order by t.ID desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<SupplierUserInfo>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<SupplierUserInfo>(ds)
            };
        }

        public SupplierUserInfo GetSupplierUserBySupplierIDAndUserID(int supplierID, int id)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string SQL = "SELECT * FROM SupplierUser(NOLOCK) where SupplierID=@SupplierID AND IsAdmin=0 AND ID=@ID";
            parameters.Append("SupplierID", supplierID);
            parameters.Append("ID", id);

            var user = db.ExecuteSqlFirst<SupplierUserInfo>(SQL, parameters);
            if (user != null)
            {
                string _SQL = "SELECT [RoleID] FROM SupplierUserRole (NOLOCK) WHERE UserID=" + id.ToString();
                user.RoleIDList = db.ExecuteSqlStringAccessor<int>(_SQL, new Int32RowMapper("RoleID")).ToList();
            }
            return user;
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

        public IList<SupplierUserInfo> GetSupplierUserBySupplierIDAndRoleID(int supplierID, int roleID)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string SQL = @"SELECT su.* FROM SupplierUser(NOLOCK) su
INNER JOIN SupplierUserRole(NOLOCK) sur ON sur.UserID = su.ID
INNER JOIN SupplierRole(NOLOCK) r ON r.RoleID = sur.RoleID AND su.SupplierID = r.SupplierID AND r.[Status]=1
WHERE r.SupplierID=@SupplierID AND r.RoleID=@RoleID";
            parameters.Append("SupplierID", supplierID);
            parameters.Append("RoleID", roleID);

           return db.ExecuteSqlList<SupplierUserInfo>(SQL, parameters);
        }

        public PageOf<SupplierRoleInfo> GetSupplierRoleBySupplierID(int supplierID, PageDTO page)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            string QUERY_SQL = @"(SELECT r.[RoleID],[RoleName],[Status],[SupplierID],COUNT(UserID) UserCount
FROM [SupplierRole] r (nolock) LEFT JOIN [SupplierUserRole] m ON m.RoleID = r.RoleID
WHERE r.[Status]=1 AND SupplierID=@SupplierID
GROUP BY r.[RoleID],[RoleName],[Status],[SupplierID]) AS t";
            parameters.Append("SupplierID", supplierID);

            string SQL = string.Format(@"select * from (select ROW_NUMBER() OVER(order by t.RoleID desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;
                                        SELECT COUNT(1) FROM {0};", QUERY_SQL);

            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<SupplierRoleInfo>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<SupplierRoleInfo>(ds)
            };
        }

        public bool ExistsSupplierUserName(String userName)
        {
            String sql = "SELECT COUNT(*) FROM SupplierUser(NOLOCK) WHERE UserName=@UserName";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("UserName", userName);
            return db.ExecuteSqlScalar<int>(sql, parameters) > 0;
        }

        public bool SaveSupplierUserInfo(int supplierID, SupplierUserInfo user)
        {
            string UPDATE_SQL = "";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", supplierID);
            parameters.Append("Password", user.Password);
            parameters.Append("NickName", user.NickName);

            string Part_Sql = "";
            if (user.RoleIDList != null && user.RoleIDList.Count > 0)
                Part_Sql = "INSERT INTO SupplierUserRole(UserID,RoleID)VALUES" + String.Join(",", user.RoleIDList.Select(p => "(@UserID," + p.ToString() + ")"));

            if (user.ID != 0) //编辑
            {
                UPDATE_SQL = string.Format(@"begin transaction
                                declare @error int
                                set @error = 0
                                    DELETE ur
                                    FROM SupplierUserRole ur
                                    INNER JOIN SupplierUser u ON u.ID=ur.UserID
                                    WHERE u.ID=@UserID AND u.SupplierID=@SupplierID
                                set @error = @error + @@error 
                                    UPDATE SupplierUser SET Password=@Password,NickName=@NickName,UpdateTime=GetDate() WHERE SupplierID=@SupplierID AND ID=@UserID
                                set @error = @error + @@error    
                                    {0} 
                                set @error = @error + @@error	
                                if @error <> 0  
                                rollback transaction   
                                else   
                                commit transaction", Part_Sql);
                parameters.Append("UserID", user.ID);

            }
            else
            {
                UPDATE_SQL = string.Format(@"begin transaction
                                    declare @error int;
                                    declare @UserID int; 
                                    set @error = 0;
                                         --添加用户
                                         INSERT INTO SupplierUser(SupplierID,UserName,Password,Status,IsAdmin,NickName,CreateTime)
                                         VALUES(@SupplierID,@UserName,@Password,1,0,@NickName,GetDate())
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

        public bool ChangeSupplierUserStatus(int supplierID, int userID, int status)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", supplierID);
            parameters.Append("UserID", userID);
            parameters.Append("Status", status);
            string UPDATE_SQL = "UPDATE SupplierUser SET Status=@Status WHERE SupplierID=@SupplierID AND ID=@UserID";
            return db.ExecuteSqlNonQuery(UPDATE_SQL, parameters) > 0;
        }

        public SupplierRoleInfo GetSupplierRoleInfo(int supplierID, int roleID)
        {
            string QUERY_SQL = "SELECT [RoleID],[RoleName],[Status],[SupplierID] FROM [SupplierRole] (nolock) WHERE SupplierID=@SupplierID AND RoleID=@RoleID AND Status=1";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", supplierID);
            parameters.Append("RoleID", roleID);
            var roleInfo = db.ExecuteSqlFirst<SupplierRoleInfo>(QUERY_SQL, parameters);
            if (roleInfo != null)
            {
                string _SQL = "SELECT [MenuId] FROM SupplierMenuRole (NOLOCK) WHERE RoleID=" + roleID.ToString();
                roleInfo.MenuIdList = db.ExecuteSqlStringAccessor<int>(_SQL, new Int32RowMapper("MenuId")).ToList();
            }
            return roleInfo;
        }

        public bool ExistsSupplierRoleName(int supplierID, int roleID, String roleName)
        {
            String sql;
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", supplierID);
            parameters.Append("RoleName", roleName);
            if (roleID == 0)
            {
                sql = "SELECT COUNT(*) FROM SupplierRole(NOLOCK) WHERE SupplierID=@SupplierID AND RoleName=@RoleName AND Status=1";
            }
            else
            {
                sql = "SELECT COUNT(*) FROM SupplierRole(NOLOCK) WHERE SupplierID=@SupplierID AND RoleID<>@RoleID AND RoleName=@RoleName AND Status=1";
                parameters.Append("RoleID", roleID);
            }
            return db.ExecuteSqlScalar<int>(sql, parameters) > 0;
        }

        public bool SaveSupplierRoleInfo(int supplierID, SupplierRoleInfo role)
        {
            string UPDATE_SQL = "";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", supplierID);
            parameters.Append("RoleName", role.RoleName);

            string Part_Sql = "";
            if (role.MenuIdList != null && role.MenuIdList.Count > 0)
                Part_Sql = "INSERT INTO SupplierMenuRole(MenuId,RoleID)VALUES" + String.Join(",", role.MenuIdList.Select(p => "(" + p.ToString() + ",@RoleID)"));

            if (role.RoleID != 0) //编辑
            {
                UPDATE_SQL = string.Format(@"begin transaction
                                declare @error int
                                set @error = 0
                                    DELETE mr
                                    FROM SupplierMenuRole mr
                                    INNER JOIN SupplierRole r ON r.RoleID=mr.RoleID
                                    WHERE r.RoleID=@RoleID AND r.SupplierID=@SupplierID
                                set @error = @error + @@error 
                                    update SupplierRole set RoleName=@RoleName WHERE SupplierID=@SupplierID AND RoleID=@RoleID
                                set @error = @error + @@error    
                                    {0} 
                                set @error = @error + @@error	
                                if @error <> 0  
                                rollback transaction   
                                else   
                                commit transaction", Part_Sql);
                parameters.Append("RoleID", role.RoleID);

            }
            else
            {
                UPDATE_SQL = string.Format(@"begin transaction
                                    declare @error int;
                                    declare @RoleID int; 
                                    set @error = 0;
                                    set @RoleID= 0  
                                         --添加角色
                                         insert into SupplierRole(RoleName,Status,SupplierID)values(@RoleName,1,@SupplierID)
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

        public bool DeleteSupplierRoleInfo(int supplierID, int roleID)
        {
            string QUERY_SQL = @"IF NOT EXISTS(SELECT RoleID FROM SupplierUserRole WHERE RoleID=@RoleID)
BEGIN
	UPDATE SupplierRole SET Status=2 WHERE SupplierID=@SupplierID AND RoleID=@RoleID
END";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("SupplierID", supplierID);
            parameters.Append("RoleID", roleID);
            return db.ExecuteSqlNonQuery(QUERY_SQL, parameters) > 0;
        }
    }
}
