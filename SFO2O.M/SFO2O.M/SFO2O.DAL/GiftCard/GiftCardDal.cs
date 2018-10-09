using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using SFO2O.Model.GiftCard;
using SFO2O.DAL;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.EntLib.DataExtensions;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace SFO2O.DAL.GiftCard
{
    public class GiftCardDal : BaseDal
    {

        #region 获取某人所有优惠券
        /// <summary>
        /// 获取某人所有优惠券
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<GiftCardEntity> GetAllGiftCard(int userId)
        {
            string sql = @"select * from GiftCard g where g.UserId=@uid";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@uid", userId);
                return DbSFO2ORead.ExecuteSqlList<GiftCardEntity>(sql, parameters).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<GiftCardEntity>();
            }
        }
        #endregion
        
        #region 获取某人 某状态 优惠券 分页数据
        /// <summary>
        /// 获取某人 某状态 优惠券 分页数据
        /// </summary>
        /// <param name="status">
        /// 前台请求优惠券状态：0未使用，1已过期, 2已使用
        /// 数据库状态为（在T-SQL中体现）：1已经使用，0未使用，2冻结</param>
        /// <param name="userId">用户ID</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">每页条数</param>
        /// <returns></returns>
        public List<GiftCardEntity> GetGiftCardList(int status, int userId, int pageindex, int pagesize)
        {
            int startIndex = (pageindex - 1) * pagesize + 1;
            string str = "";
            if (status == 0)
            {
                str = "AND gc.[Status]=0 AND gc.EndTime>=GETDATE() and gc.BeginTime<=GETDATE() ";
            }
            else if (status == 2)
            {
                str = "AND gc.[Status] in (1,2) ";
            }
            else
            {
                str = "AND gc.[Status] =0 AND gc.EndTime<GETDATE() ";
            }
            int endIndex = pageindex * pagesize;
            try
            {

                string sql = @"with sputb
                           AS
                            (SELECT *,ROW_NUMBER() over(ORDER BY a.BeginTimeToString DESC)as rindex
                                                         FROM(
                           SELECT TOP 100 PERCENT  gc.BatchName,gc.CardSum,gc.SatisfyPrice,gc.SatisfyProduct,CONVERT(varchar(100), gc.BeginTime, 102) as BeginTimeToString,gc.CardType,CONVERT(varchar(100), gc.EndTime, 102) as EndTimeToString
                              FROM GiftCard AS gc
                            WHERE gc.UserId=@userId " + str + @" ORDER BY gc.BeginTime desc ) AS a)
                            
                            select *,(select count(1) from sputb) as TotalRecord 
                            from sputb
                            where rindex between @StartIndex and @EndIndex";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@userId", userId);
                parameters.Append("@StartIndex", startIndex);
                parameters.Append("@EndIndex", endIndex);
                var list = DbSFO2ORead.ExecuteSqlList<GiftCardEntity>(sql, parameters);
                return list.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<GiftCardEntity>();
            }
        }
        #endregion

        #region 根据ID获取某个优惠券 面值
        public decimal GetGiftCardValueById(int userId, int id)
        {
            //为了保险起见，防止传过来的id被篡改，进行一下简单的过滤
            string sql = @"select CardSum from GiftCard g where g.UserId=@uid and g.Id=@id and g.[Status]=0 and (GETDATE()>g.BeginTime and GETDATE()<g.EndTime)";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@uid", userId);
                parameters.Append("@id", id);
                return Convert.ToDecimal(DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters));
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return 0M;
            }
        }
        #endregion

        #region 获取某人所有未使用的优惠券
        /// <summary>
        /// 获取某人所有可用优惠券（满足 某个UserID对应优惠券几个条件：
        /// 第一：未使用
        /// 第二：在有效期内
        /// 第三：大于SatisfyPrice最小满额
        /// 第三：商品类型（SatisfyProduct）大于等于选中的商品类型之和
        /// 调用的地方采取：e.g:sum（枚举运算，取出数据库里的值e.g satisfyProduct然后跟商品（|）的结果进行取&，如果结果依然等于商品和sum）那么这个就是我们想要的数据
        /// if(satisfyProduct&sum==sum)那么就得到一个满足条件的优惠券
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="satisfyPrice">最小满减金额</param>
        /// <param name="satisfyProduct">最小|运算结果值   1|2=3   3&2=2</param>
        /// <returns></returns>
        public List<GiftCardEntity> GetAllNotUsedGiftCard(int userId, decimal satisfyPrice, int satisfyProduct)
        {
            //结果集优先按照面值降序，其次按照结束时间降序，最后按照获取时间升序
            string sql = @"select * from GiftCard g where g.UserId=@uid and g.[Status]=0 and (GETDATE()>g.BeginTime and GETDATE()<g.EndTime) and g.SatisfyPrice<=@satisfyPrice and g.SatisfyProduct>=@satisfyProduct order by g.CardSum desc,g.EndTime desc,g.AddTime asc";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@uid", userId);
                parameters.Append("@satisfyPrice", satisfyPrice);
                parameters.Append("@satisfyProduct", satisfyProduct);

                return DbSFO2ORead.ExecuteSqlList<GiftCardEntity>(sql, parameters).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<GiftCardEntity>();
            }
        }
        #endregion

        #region 根据触发类型 更改优惠券状态
        /// <summary>
        /// 根据触发类型 更改优惠券状态
        /// </summary>
        /// <param name="type">1.下单成功事件    2.支付成功事件  3.取消订单事件</param>
        /// <param name="cid">优惠券ID</param>
        /// <returns>True操作成功  False操作失败</returns>
        public static bool ChangeGiftCardStatusByEventType(int type, int cid, Database db, DbTransaction tran, string orderCode = null, string splitOrderCode = null)
        {
            try
            {
                string sql = @"update GiftCard set [Status]=";
                switch (type)
                {
                    case 1://下单成功，冻结优惠券
                        sql += "2,FrozenTime=GETDATE(),OrderCode=@OrderCode " + (orderCode == splitOrderCode ? " where Id=@cid" : ",SplitOrderCode=CASE WHEN Isnull(SplitOrderCode,'') = '' THEN @SplitOrderCode ELSE SplitOrderCode+','+@SplitOrderCode end where Id=@cid");
                        break;
                    case 2://支付成功，优惠券-->已使用
                        sql += "1,UsedTime=case when Isnull(UsedTime,'')='' then GETDATE() else UsedTime end,UsedSum=CardSum where OrderCode=@OrderCode";
                        break;
                    case 3://取消订单，优惠券状态重置为未使用
                        sql += "0, SplitOrderCode=NULL where Id=@cid";
                        break;
                    default://除非传参有问题，一般不会到这个位置，结果会执行False
                        sql += "0 where Id=0";
                        break;
                }
                var parameters = db.CreateParameterCollection();
                
                parameters.Append("@cid", cid);

                if (orderCode!=null)
                {
                    parameters.Append("@OrderCode", orderCode);                
                }

                if (type == 1)
                {
                    if (orderCode != splitOrderCode)
                    {
                        parameters.Append("@SplitOrderCode", splitOrderCode);
                    }
                }
                
                return db.ExecuteNonQuery(CommandType.Text, sql, parameters,tran) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 注册成功后，获得优惠券批次信息
        /// </summary>
        /// <param name="RegisterTime"></param>
        /// <returns></returns>
        public List<GiftCardBatchEntity> GetGiftCardBatchEntityForRegisterSucc(DateTime RegisterTime)
        {
            string sql = @"SELECT gcb.BatchId
						            ,gcb.BatchName
						            ,gcb.CardSum
						            ,gcb.CardNumber
						            ,gcb.CardType
						            ,gcb.BeginTime
						            ,gcb.EndTime
						            ,gcb.ExpiryDays
						            ,gcb.SatisfyPrice
						            ,gcb.SatisfyUser
						            ,gcb.SatisfyProduct
						            ,gcb.Remarks
						            ,gcb.DownloadCounts
						            ,gcb.[Enable]
						            ,gcb.CreateTime
						            ,gcb.Creater 
				            FROM GiftCardBatch AS gcb 
				            WHERE gcb.[Enable] = 1 
						            AND gcb.EndTime >= @RegisterTime AND gcb.BeginTime <= @RegisterTime";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@RegisterTime", RegisterTime);
                return DbSFO2ORead.ExecuteSqlList<GiftCardBatchEntity>(sql, parameters).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<GiftCardBatchEntity>();
            }
        }

        /// <summary>
        /// 添加优惠券
        /// </summary>
        /// <param name="GiftCardEntity"></param>
        /// <returns></returns>
        public bool CreateGiftCard(GiftCardEntity GiftCardEntity)
        {
            bool result = false;
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    // 插入优惠券信息
                    result = SaveGiftCard(GiftCardEntity, db, tran);

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }

            return result;
        }

        /// <summary>
        /// 插入优惠券信息
        /// </summary>
        /// <param name="GiftCardEntity"></param>
        /// <returns></returns>
        public bool SaveGiftCard(GiftCardEntity GiftCardEntity,Database db,DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into GiftCard(");
            strSql.Append("BatchId,BatchName,UserId,CardId,CardSum,CardType,Status,BeginTime,EndTime,SatisfyPrice,SatisfyProduct,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@BatchId,@BatchName,@UserId,@CardId,@CardSum,@CardType,@Status,@BeginTime,@EndTime,@SatisfyPrice,@SatisfyProduct,@AddTime)");

            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@BatchId", GiftCardEntity.BatchId);
            parameters.Append("@BatchName", GiftCardEntity.BatchName);
            parameters.Append("@UserId", GiftCardEntity.UserId);
            parameters.Append("@CardId", GiftCardEntity.CardId);
            parameters.Append("@CardSum", GiftCardEntity.CardSum);
            parameters.Append("@CardType", GiftCardEntity.CardType);
            parameters.Append("@Status", GiftCardEntity.Status);
            parameters.Append("@BeginTime", GiftCardEntity.BeginTime);
            parameters.Append("@EndTime", GiftCardEntity.EndTime);
            parameters.Append("@SatisfyPrice", GiftCardEntity.SatisfyPrice);
            parameters.Append("@SatisfyProduct", GiftCardEntity.SatisfyProduct);
            parameters.Append("@AddTime", GiftCardEntity.AddTime);
            return db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran) > 0;
        }

    }
}
