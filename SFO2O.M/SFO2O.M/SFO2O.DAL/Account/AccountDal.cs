using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Account;
using SFO2O.Utility.Uitl;
using SFO2O.Utility.Security;
using SFO2O.Model.Huoli;


namespace SFO2O.DAL.Account
{
    public class AccountDal : BaseDal
    {
        /// <summary>
        /// 根据用户名密码获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public CustomerEntity GetCustomerEntity(string userName, string pwd, string regionCode)
        {
            try
            {
                const string sql = @"SELECT [ID]
                                  ,[UserName]
                                  ,[NickName]
                                  ,[Password]
                                  ,[ImageUrl]
                                  ,[Mobile]
                                  ,[RegionCode]
                                  ,[Gender]
                                  ,[PayPassword]
                                  ,[Email]
                                  ,[Type]
                                  ,[Status]
                                  ,[FirstOrderAuthorize]
                                  ,[LastLoginTime]
                                  ,[CreateTime]
                                  ,[UpdateBy]
                                  ,[UpdateTime]
                                  ,IsPushingInfo
                                  ,SourceValue
                                  ,SourceType
                                  ,ChannelId
                              FROM  [Customer] (NOLOCK)
                    WHERE [UserName]=@UserName AND [Password]=@Password  AND RegionCode=@RegionCode";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserName", userName);
                parameters.Append("@Password", pwd);
                parameters.Append("@RegionCode", regionCode);


                return DbSFO2ORead.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 用户名是否已经存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsExistsUserName(string userName, string regionCode)
        {
            try
            {
                string sql = "Select Count(0) From Customer (nolock) Where UserName=@UserName And RegionCode=@RegionCode";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserName", userName);
                parameters.Append("@RegionCode", regionCode);
                object obj = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters);
                return int.Parse(obj.ToString()) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        public CustomerEntity GetCustomerEntity(int userId)
        {
            try
            {
                const string sql = @"SELECT [ID]
                          ,[UserName]
                          ,[NickName]
                          ,[Password]
                          ,[Mobile]
                          ,[ImageUrl]
                          ,[RegionCode]
                          ,[Gender]
                          ,[PayPassword]
                          ,[Email]
                          ,[Type]
                          ,[Status]
                          ,[LastLoginTime]
                          ,[CreateTime]
                          ,[UpdateBy]
                          ,[UpdateTime]
                          ,[SourceType]
                      FROM  [Customer] (NOLOCK)
                    WHERE [ID]=@ID  ";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@ID", userId);

                return DbSFO2ORead.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(CustomerEntity entity)
        {
            string sql = @"Insert Into Customer(UserName,Password,Mobile,RegionCode,Gender,Email,Type,Status,CreateTime,IsPushingInfo,SourceValue,SourceType,ChannelId) Values(
                            @UserName,@Password,@Mobile,@RegionCode,@Gender,@Email,@Type,@Status,getdate(),1,@SourceValue,@SourceType,@ChannelId);
                            select @@IDENTITY";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@UserName", entity.Mobile);
            parameters.Append("@Password", MD5Hash.Md5Encrypt(entity.Password));
            parameters.Append("@Mobile", entity.Mobile);
            parameters.Append("@RegionCode", entity.RegionCode);
            parameters.Append("@Gender", entity.Gender);
            parameters.Append("@Email", entity.Email);
            parameters.Append("@Type", entity.RegionCode == "86" ? 0 : 1);
            parameters.Append("@Status", 1);
            parameters.Append("@SourceValue", entity.SourceValue);
            parameters.Append("@SourceType", entity.SourceType);
            parameters.Append("@ChannelId", entity.ChannelId);
            return Convert.ToInt32(DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters));
        }
        /// <summary>
        /// 添加新用户酒豆
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertHuoli(CustomerEntity entity, int UserId, decimal huoli, decimal lockHuoli)
        {
            string sql = @"INSERT INTO [HuoLiTotal] ([UserId],[HuoLi],[LockedHuoLi],[CreateTime],[CreateBy]) Values(
                            @UserId,@HuoLi,@LockedHuoLi,getdate(),@CreateBy)";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@UserId", UserId);
            parameters.Append("@HuoLi", huoli);
            parameters.Append("@LockedHuoLi", lockHuoli);
            parameters.Append("@CreateBy", 0);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdatePassword(CustomerEntity entity)
        {
            string sql = "Update Customer Set Password=@Password Where UserName=@UserName";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@Password", MD5Hash.Md5Encrypt(entity.Password));
            parameters.Append("@UserName", entity.UserName);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }

        /// <summary>
        /// 更新是否首次下单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateFirstOrderAuthorize(string userName)
        {
            string sql = "Update Customer Set FirstOrderAuthorize=1 Where UserName=@UserName";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@UserName", userName);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFirstOrderAuthorize(string userName)
        {
            string sql = @"SELECT FirstOrderAuthorize FROM Customer WHERE UserName=@UserName ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@UserName", userName);
            return DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters).ToString() == "1" ? true : false;
        }

        public HuoliEntity GetHuoliEntityByUerId(int UserId)
        {
            try
            {
                const string sql = @"SELECT hlt.UserId,hlt.HuoLi,hlt.LockedHuoLi,HuoLiCurrent FROM HuoLiTotal AS hlt WHERE hlt.UserId = @ID ";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@ID", UserId);
                return DbSFO2ORead.ExecuteSqlFirst<HuoliEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        public CustomerEntity GetUserInfoByUserName(string UserName)
        {
            try
            {
                const string sql = @"SELECT c.ID,c.UserName
		                                FROM Customer AS c
		                                WHERE c.UserName = @UserName";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserName", UserName);
                return DbSFO2ORead.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
        public bool UpdateVisitedTimes(string StationSource, int ChannelId)
        {
            string sql = "UPDATE DividedPercentStation SET VisitedTimes=VisitedTimes+1 WHERE DPID=@ChannelId AND ZDID=@StationSource";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@StationSource", StationSource);
            parameters.Append("@ChannelId", ChannelId);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }

        public bool GetDividedPercentStationZDID(int DPID, string ZDID)
        {
            try
            {
                const string sql = @"SELECT
					                        dps.Id,
					                        dps.DPID,
					                        dps.ZDID,
					                        dps.ZDName,
					                        dps.ZDNO,
					                        dps.ZDAddress,
					                        dps.VisitedTimes,
					                        dps.CreateTime,
					                        dps.CreateBy
				                        FROM
					                        DividedPercentStation AS dps
				                        WHERE dps.DPID = @DPID AND dps.ZDID = @ZDID";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@DPID", DPID);
                parameters.Append("@ZDID", ZDID);
                return DbSFO2ORead.ExecuteSqlFirst<CustomerEntity>(sql, parameters) != null;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        // 通过StationSource  ChannelId 查询 DividedPercentStationVisitedLog 表中是否有记录
        public int selectVisitedLog(string StationSource, int ChannelId)
        {
            string sql = "SELECT COUNT(*) FROM DividedPercentStationVisitedLog AS dpsvl WHERE dpsvl.DPID=@ChannelId AND dpsvl.ZDID=@StationSource ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StationSource", StationSource);
            parameters.Append("@ChannelId", ChannelId);
            return Convert.ToInt32(DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters));
        }
        //插入 DividedPercentStationVisitedLog 
        public int InsertVisitedLog(string StationSource, int ChannelId)
        {
            string sql = @"INSERT INTO DividedPercentStationVisitedLog
                            (
	                            -- Id -- this column value is auto-generated
	                            DPID,
	                            ZDID,
	                            VisitedDate,
	                            VisitedCount
                            )
                            VALUES
                            (
	                            @DPID,
	                            @ZDID,
	                            @VisitedDate,
	                            @VisitedCount
                            )";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@DPID", ChannelId);
            parameters.Append("@ZDID", StationSource);
            parameters.Append("@VisitedDate", DateTime.Now);
            parameters.Append("@VisitedCount", 1);
            return Convert.ToInt32(DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters));
        }
        //通过StationSource ChannelId 更新 DividedPercentStationVisitedLog 表的记录
        public bool UpdateVisitedLog(string StationSource, int ChannelId)
        {
            string sql = "UPDATE DividedPercentStationVisitedLog SET VisitedCount = VisitedCount + 1 WHERE DPID=@ChannelId AND ZDID=@StationSource ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@StationSource", StationSource);
            parameters.Append("@ChannelId", ChannelId);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }

        public int InsertTemp(string UserName, string pwd, int UserType)
        {
            string sql = @"INSERT INTO CustomerTemp(UserName,UserPwd,UserType) VALUES(@UserName,@UserPwd,@UserType)";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@UserName", UserName);
            parameters.Append("@UserPwd", pwd);
            parameters.Append("@UserType", UserType);
            return Convert.ToInt32(DbSFO2OMain.ExecuteScalar(CommandType.Text, sql, parameters));
        }

        public int GetUserTempByUserName(string UserName)
        {
            string sql = "SELECT COUNT(1) FROM CustomerTemp AS c WHERE c.UserName = @UserName ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserName", UserName);
            return Convert.ToInt32(DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters));
        }

        public IList<CustomerEntity> GetUserInfo()
        {
            try
            {
                const string sql = @"SELECT c.ID,c.UserName FROM Customer AS c WHERE c.CreateTime>'2016-07-27 00:00:00' AND c.CreateTime<'2016-07-28 12:00:00'";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                return DbSFO2ORead.ExecuteSqlList<CustomerEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据用户UnionId获取用户信息
        /// </summary>
        /// <param name="UnionId"></param>
        /// <returns></returns>
        public CustomerEntity GetCustomerEntityByUnionId(string UnionId)
        {
            try
            {
                const string sql = @"SELECT [ID]
                          ,[UserName]
                          ,[NickName]
                          ,[Password]
                          ,[Mobile]
                          ,[ImageUrl]
                          ,[RegionCode]
                          ,[Gender]
                          ,[PayPassword]
                          ,[Email]
                          ,[Type]
                          ,[Status]
                          ,[LastLoginTime]
                          ,[CreateTime]
                          ,[UpdateBy]
                          ,[UpdateTime]
                      FROM  [Customer] (NOLOCK)
                    WHERE [UnionId]=@UnionId  ";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UnionId", UnionId);

                return DbSFO2ORead.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 插入物流速递用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CustomerEntity InsertSFExpressCustomer(CustomerEntity entity)
        {
            string sql = @"	EXEC [dbo].[sp_InsertSFExpressUser]
                                    @Password = @Password,
			                        @SourceType = @SourceType,
                                    @SFId = @SFId,
                                    @UnionId = @UnionId";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@Password", entity.Password);
            parameters.Append("@SourceType", entity.SourceType);
            parameters.Append("@SFId", entity.SFId);
            parameters.Append("@UnionId", entity.UnionId);

            return DbSFO2ORead.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
        }
    }
}
