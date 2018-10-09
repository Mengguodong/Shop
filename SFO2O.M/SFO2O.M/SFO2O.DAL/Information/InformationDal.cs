using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Common;
using SFO2O.Utility.Uitl;
using System.Data;
using SFO2O.Model.Information;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace SFO2O.DAL.Information
{
    public class InformationDal : BaseDal
    {
        /// <summary>
        /// 获取某人的系统消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns>消息List</returns>
        public List<InformationEntity> GetSysInfoList(int userId, int pageindex, int pagesize)
        {
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;
            string sql = @"with msg as
                                    (
                                    select i.Id,ir.UserId as ReadUserId,ROW_NUMBER() over(order by i.createTime desc)as RIndex,i.Title,i.Content,i.CreateTime
                                    from Information i
                                    left join InformationRead ir
                                    on i.Id=ir.InformationId and ir.UserId=@UserID
                                    where i.CreateTime>=(select c.CreateTime from Customer c where ID=@UserID) and (i.SendUserId is null or i.SendUserId=0 or i.SendUserId=@UserID) and i.[Type]=1 and i.WebInnerType in(1,4) and i.SendDest in((select case RegionCode when '86' then 1 else 2 end as 'SendID' from Customer where ID=@UserID),3)
                                    )
                                    select (select count(1) from msg) as TotalRecord,* from msg 
                                    where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@UserID", userId);
            var list = DbSFO2ORead.ExecuteSqlList<InformationEntity>(sql, parameters);
            return list.ToList();
        }

        /// <summary>
        /// 获取某人的订单消息列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns>消息List</returns>
        public List<InformationEntity> GetOrderInfoList(int userId, int pageindex, int pagesize)
        {
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;
            string sql = @"with msg as
                                (
                                select i.Id
					                    ,ir.UserId as ReadUserId
					                    ,ROW_NUMBER() over(order by i.createTime desc)as RIndex
					                    ,i.Title,i.Content,i.CreateTime
					                    ,i.ImagePath,i.LinkUrl
                                from Information i
                                left join InformationRead ir on i.Id=ir.InformationId AND ir.UserId = @UserID 
                                where (i.SendUserId=@UserID) and i.[Type]=1 and i.WebInnerType=3
                                )
                                select (select count(1) from msg) as TotalRecord,* from msg 
                                where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@UserID", userId);
            var list = DbSFO2ORead.ExecuteSqlList<InformationEntity>(sql, parameters);
            return list.ToList();
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<InformationEntity> GetActivityInfoList(int userId, int pageindex, int pagesize)
        {
            int startIndex = (pageindex - 1) * pagesize + 1;
            int endIndex = pageindex * pagesize;
            string sql = @"with msg as
                                (
                                select ir.UserId as ReadUserId
                                        ,i.Id 
                                        ,i.Title,i.Content,i.CreateTime
                                        ,i.ImagePath,i.LinkUrl
                                        ,i.StartTime
                                from InformationToCustomer itc
                                LEFT JOIN Information AS i on i.Id=itc.InformationId and i.[Type]=1 and i.WebInnerType=2
                                LEFT JOIN InformationRead AS ir ON itc.InformationId = ir.InformationId AND ir.UserId = @UserID
                                where itc.UserId = @UserID 
                                )
                                select * from (select (select count(1) from msg where StartTime<=GETDATE()) as TotalRecord
                                ,ROW_NUMBER() over(order by StartTime desc)as RIndex,* from msg 
                                where StartTime<=GETDATE()) as a where rindex between @StartIndex and @EndIndex";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            parameters.Append("@UserID", userId);
            var list = DbSFO2ORead.ExecuteSqlList<InformationEntity>(sql, parameters);
            return list.ToList();
        }

        /// <summary>
        ///根据系统消息ID获取消息 
        /// </summary>
        /// <param name="infoid">消息id</param>
        /// <returns>消息对象</returns>
        public InformationEntity GetSysInfoById(int infoid)
        {
            string sql = @"select SendUserId,Title,Content,CreateTime,SendUserId from Information where [Id]=@InfoId";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@InfoId", infoid);
            var model = DbSFO2ORead.ExecuteSqlFirst<InformationEntity>(sql,parameters);
            return model;
        }

        /// <summary>
        /// 执行阅读某条消息操作
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="infoid">消息id</param>
        public void ReadMessage(int userid,int infoid)
        {
            try
            {
                string sql = @"select count(1) from InformationRead where UserId=@userid and InformationId=@InfoId";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@userid", userid);
                parameters.Append("@InfoId", infoid);
                bool isRead = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters).ToString() == "0" ? false : true;
                //如果已读表不存在词条数据，则插入
                if (!isRead)
                {
                    sql = @"INSERT INTO [InformationRead]([UserId],[InformationId],[CreateTime])VALUES (@userid,@InfoId,GETDATE())";
                    parameters.Clear();
                    parameters.Append("@userid", userid);
                    parameters.Append("@InfoId", infoid);
                    DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
		
        /// <summary>
        /// 获得消息最后一条的信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<InformationEntity> GetInformationLast(int UserId)
        {
            try
            {
                string sql = @"  SELECT * FROM 
		                        (SELECT TOP 1 i.Id 
					                        ,i.[Type] AS InfoType
					                        ,i.WebInnerType
					                        ,i.Title
					                        ,i.CreateTime
		                        FROM Information AS i
		                        WHERE i.WebInnerType = 1 and i.CreateTime>=(select c.CreateTime from Customer c where ID=@UserId)
                                and i.SendDest in((select case RegionCode when '86' then 1 else 2 end as 'SendID' from Customer where ID=@UserId),3)
		                        ORDER BY i.CreateTime DESC) AS SystemInfo
		                        UNION ALL
		                        SELECT * FROM 
		                        (SELECT TOP 1 i.Id 
					                        ,i.[Type] AS InfoType
					                        ,i.WebInnerType
					                        ,i.Title
					                        ,i.StartTime
		                        FROM InformationToCustomer AS itc
		                        INNER JOIN Information AS i ON i.Id = itc.InformationId and i.StartTime<=GETDATE()
		                        WHERE itc.UserId = @UserId AND itc.Visible = 1
		                        ORDER BY i.StartTime DESC) AS ActivityInfo
		                        UNION ALL
		                        SELECT * FROM 
		                        (SELECT TOP 1 i.Id 
					                        ,i.[Type] AS InfoType
					                        ,i.WebInnerType
					                        ,i.Title
					                        ,i.CreateTime
		                        FROM Information AS i
		                        LEFT JOIN InformationRead AS ir ON ir.InformationId = i.Id AND ir.UserId = @UserId
		                        WHERE i.WebInnerType = 3 AND i.SendUserId = @UserId
		                        ORDER BY i.CreateTime DESC) AS OrderInfo
		                        UNION ALL
		                        SELECT * FROM 
		                        (SELECT TOP 1 i.Id 
					                        ,i.[Type] AS InfoType
					                        ,i.WebInnerType
					                        ,i.Title
					                        ,i.CreateTime
		                        FROM Information AS i
		                        WHERE i.WebInnerType = 4 AND i.SendUserId = @UserId
		                        ORDER BY i.CreateTime DESC) AS WebInnerSystemInfo";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", UserId);
                return DbSFO2ORead.ExecuteSqlList<InformationEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得未读消息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<InformationEntity> GetNotReadInfomation(int UserId)
        {
            try
            {
                string sql = @"  SELECT i.WebInnerType
				                         ,COUNT(i.Id) AS NotReadInfoCount
		                        FROM Information AS i 
		                        WHERE i.WebInnerType = 1 AND i.SendDest in((select case RegionCode when '86' then 1 else 2 end as 'SendID' from Customer where ID=@UserId),3) 
                                    AND i.CreateTime>=(select c.CreateTime from Customer c where ID=@UserId)
                                    AND NOT EXISTS(
			                        SELECT * FROM InformationRead AS ir WHERE ir.UserId = @UserId 
								                        AND ir.InformationId = i.Id)
		                        GROUP BY i.WebInnerType
		                        UNION ALL
		                        SELECT i.WebInnerType
				                        ,COUNT(i.Id) AS NotReadInfoCount
		                        FROM Information AS i
		                        INNER JOIN InformationToCustomer AS itc ON i.Id = itc.InformationId AND itc.UserId = @UserId AND itc.Visible = 1
		                        WHERE i.WebInnerType = 2 AND i.StartTime<=GETDATE() AND NOT EXISTS(
			                        SELECT * FROM InformationRead AS ir WHERE itc.UserId = ir.UserId 
								                        AND itc.InformationId = ir.InformationId)
		                        GROUP BY i.WebInnerType
		                        UNION ALL
		                        SELECT i.WebInnerType
				                        ,COUNT(i.Id) AS NotReadInfoCount
		                        FROM Information AS i
		                        WHERE i.WebInnerType = 3 AND i.SendUserId = @UserId AND NOT EXISTS(
			                        SELECT * FROM InformationRead AS ir WHERE i.SendUserId = ir.UserId 
								                        AND i.Id = ir.InformationId)
		                        GROUP BY i.WebInnerType
		                        UNION ALL
		                        SELECT i.WebInnerType
				                        ,COUNT(i.Id) AS NotReadInfoCount
		                        FROM Information AS i
		                        WHERE i.WebInnerType = 4 AND i.SendUserId = @UserId AND NOT EXISTS(
			                        SELECT * FROM InformationRead AS ir WHERE i.SendUserId = ir.UserId 
								                        AND i.Id = ir.InformationId)
		                        GROUP BY i.WebInnerType";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", UserId);

                return DbSFO2ORead.ExecuteSqlList<InformationEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        public void AddInformationRead(int UserId,int InformationId)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    LogHelper.Info("------已读消息回写1------insert方法");
                    /// 
                    InsertInformationRead(UserId, InformationId, db, tran);

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    LogHelper.Info("------已读消息回写1------ext:" + ext);
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }

        public void InsertInformationRead(int UserId, int InformationId, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into InformationRead(");
            strSql.Append("UserId,InformationId,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@InformationId,@CreateTime)");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@UserId", UserId);
            parameters.Append("@InformationId", InformationId);
            parameters.Append("@CreateTime", DateTime.Now);

            LogHelper.Info("------已读消息回写2------insert sql:" + strSql);
            int aa = db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
            LogHelper.Info("------已读消息回写2------insert 结果:" + aa);
        }

        public int OrderReadMessage(int userid, int infoid)
        {
            try
            {
                int result = 0;
                string sql = @"select count(1) from InformationRead where UserId=@userid and InformationId=@InfoId";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@userid", userid);
                parameters.Append("@InfoId", infoid);
                bool isRead = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters).ToString() == "0" ? false : true;
                //如果已读表不存在词条数据，则插入
                if (!isRead)
                {
                    sql = @"INSERT INTO [InformationRead]([UserId],[InformationId],[CreateTime])VALUES (@userid,@InfoId,GETDATE())";
                    parameters.Clear();
                    parameters.Append("@userid", userid);
                    parameters.Append("@InfoId", infoid);
                    result = DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters);
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return -1;
            }
        }

        public void InsertInformation(InformationEntity InformationEntity, Database db, DbTransaction tran)
        {
            string sql = @"insert into Information
                            (Type,WebInnerType,SendDest,SendUserId,TradeCode,Title,[Content]
                            ,ImagePath,Summary,LinkUrl,StartTime,EndTime,LongTerm,CreateTime) 
                            values (@Type,@WebInnerType,@SendDest,@SendUserId,@TradeCode,@Title
                                    ,@Content,@ImagePath,@Summary,@LinkUrl,@StartTime
                                    ,@EndTime,@LongTerm,@CreateTime)";
            var parameters = db.CreateParameterCollection();
            parameters.Append("@Type", InformationEntity.InfoType);
            parameters.Append("@WebInnerType", InformationEntity.WebInnerType);
            parameters.Append("@SendDest", InformationEntity.SendDest);
            parameters.Append("@SendUserId", InformationEntity.SendUserId);
            parameters.Append("@TradeCode", InformationEntity.TradeCode);
            parameters.Append("@Title", InformationEntity.Title);
            parameters.Append("@Content", InformationEntity.InfoContent);
            parameters.Append("@ImagePath", InformationEntity.ImagePath);
            parameters.Append("@Summary", InformationEntity.Summary);
            parameters.Append("@LinkUrl", InformationEntity.LinkUrl);
            parameters.Append("@StartTime", InformationEntity.StartTime);
            parameters.Append("@EndTime", InformationEntity.EndTime);
            parameters.Append("@LongTerm", InformationEntity.LongTerm);
            parameters.Append("@CreateTime", InformationEntity.CreateTime);

            //LogHelper.Error("------添加消息信息2------insert sql:" + sql);
            int aa = db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran);
            //LogHelper.Error("------添加消息信息2------insert 结果:" + aa);
        }

        public void AddInformation(InformationEntity InformationEntity)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    //LogHelper.Error("------添加消息信息1------insert方法");
                     
                    InsertInformation(InformationEntity, db, tran);

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    LogHelper.Error("------添加消息信息1异常------ext:" + ext);
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }

        public List<InformationEntity> GetActivityListForRegister(int userId)
        {
            string sql = @"with msg as
                                    (
                                    select i.Id
				                           ,i.StartTime
                                    from Information i
                                    where (i.SendUserId is null or i.SendUserId=0) 
                                    and i.[Type]=1 and i.WebInnerType=2 
                                    and i.SendDest in(
                                        (select case RegionCode when '86' then 1 else 2 end as 'SendID' 
                                            from Customer where ID=@UserID),3)
                                    )
                                    select * from msg";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserID", userId);
            var list = DbSFO2ORead.ExecuteSqlList<InformationEntity>(sql, parameters);
            return list.ToList();
        }

        public int MutiInsertToInfoCustomer(DataTable dt)
        {
            string str = ConfigurationManager.ConnectionStrings["ConnStringSFO2OMain"].ConnectionString;

            int result = 0;
            SqlConnection sqlConn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["ConnStringSFO2OMain"].ConnectionString);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn);
            bulkCopy.DestinationTableName = "InformationToCustomer";
            bulkCopy.BatchSize = dt.Rows.Count;

            try
            {
                sqlConn.Open();
                if (dt != null && dt.Rows.Count != 0)
                {
                    bulkCopy.WriteToServer(dt);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = -1;
                LogHelper.Info("----------MutiInsertToInfoCustomer---------执行完成捕获异常日志：" + ex);
            }
            finally
            {
                sqlConn.Close();
                if (bulkCopy != null)
                {
                    bulkCopy.Close();
                }
            }

            return result;
        }


    }
}
