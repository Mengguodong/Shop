using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Account;
using SFO2O.Model.Common;
using SFO2O.Model.Product;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Category;
using SFO2O.Model.Team;

namespace SFO2O.DAL.Product
{
    public class TeamDal : BaseDal
    {
        
        public TeamInfoEntity GetTeamInfoEntity(string OrderCode)
        {
            if (string.IsNullOrEmpty(OrderCode))
            {
                throw new ArgumentException("OrderCode");
            }
            try
            {

                string sql = @" SELECT ti.TeamCode,ti.sku,ti.TeamStatus,ti.StartTime,ti.EndTime
		                                ,ti.UserID,ti.TeamNumbers,ti.CreatTime,ti.SuccTeamTime
                                FROM OrderInfo AS oi 
                                INNER JOIN TeamInfo AS ti ON ti.TeamCode = oi.TeamCode
                                WHERE oi.OrderCode=@OrderCode ";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", OrderCode);
                return DbSFO2ORead.ExecuteSqlFirst<TeamInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }

        }

        /// <summary>
        /// 获取团详情信息
        /// </summary>
        /// <param name="TeamCode"></param>
        /// <returns></returns>
        public IList<TeamDetailEntity> GetTeamDetailByCode(string TeamCode)
        {
            const string sql = @"SELECT oi.OrderCode 
		                                ,oi.OrderStatus
		                                ,oi.TeamCode
                                        ,ps.DiscountPrice
		                                ,oi.PaidAmount
		                                ,oi.PayTime
                                        ,oi.orderSourceType AS OrderSourceType
                                        ,oi.OrderSourceValue
                                        ,oi.DividedAmount
                                        ,oi.DividedPercent
		                                ,c.ID AS UserId
		                                ,c.ImageUrl
		                                ,c.Mobile
		                                ,ti.UserID AS TeamHead
		                                ,ti.TeamNumbers
                                        ,ti.TeamStatus
                                        ,ti.StartTime
			                            ,ti.EndTime
                                        ,ti.PromotionId
		                                ,si.Sku
		                                ,pi2.ImagePath
		                                ,pi1.Name AS ProductName
                                        ,ISNULL(si.MainValue,'') as MainValue
                                        ,ISNULL(si.SubValue,'') as SubValue
                                        ,ISNULL(si.MainDicValue,'') as MainDicValue
                                        ,ISNULL(si.SubDicValue,'') as SubDicValue
                                        ,pi1.NetWeightUnit
                                 FROM OrderInfo AS oi
                                INNER JOIN OrderProducts AS op ON op.OrderCode = oi.OrderCode
                                INNER JOIN OrderPayment AS op2 ON op2.OrderCode = oi.OrderCode AND op2.PayStatus = 2
	                            INNER JOIN ProductInfo AS pi1 ON pi1.Spu = op.Spu AND pi1.LanguageVersion = 1
	                            INNER JOIN ProductImage AS pi2 ON pi2.Spu = op.Spu AND pi2.SortValue = 1
	                            INNER JOIN TeamInfo AS ti ON ti.TeamCode = oi.TeamCode
	                            INNER JOIN Promotions AS p ON p.Id = ti.PromotionId
	                            INNER JOIN SkuInfo AS si ON si.Sku = op.Sku AND si.SpuId = pi1.Id
	                            INNER JOIN PromotionSku AS ps ON ps.PromotionId = p.Id AND ps.Sku = si.Sku
                                INNER JOIN Promotions AS p2 ON p2.Id = ps.PromotionId AND p2.PromotionType = 2
	                            INNER JOIN Customer AS c ON c.ID = oi.UserId
                                WHERE oi.TeamCode=@TeamCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@TeamCode", TeamCode);

            return DbSFO2ORead.ExecuteSqlList<TeamDetailEntity>(sql, parameters);
        }

        public int GetTeamUserId(string OrderCode, string TeamCode)
        {
            try
            {
                string sql = @" SELECT oi.UserId
		                        FROM OrderInfo AS oi
		                        INNER JOIN TeamInfo AS ti ON ti.TeamCode = oi.TeamCode
                                WHERE oi.TeamCode=@TeamCode AND oi.OrderCode=@OrderCode";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@TeamCode", TeamCode);
                parameters.Append("@OrderCode", OrderCode);
                return DbSFO2ORead.ExecuteSqlScalar<int>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                throw ex;
            }
        }
        public TeamInfoEntity GetTeamInfoByTeamCode(string TeamCode)
        {
            if (string.IsNullOrEmpty(TeamCode))
            {
                throw new ArgumentException("TeamCode");
            }
            try
            {
                string sql = @"SELECT ti.TeamCode,ti.sku,ti.TeamStatus,ti.StartTime,ti.EndTime,ti.UserID,ti.TeamNumbers,ti.SuccTeamTime,ti.PromotionId FROM TeamInfo AS ti WHERE ti.TeamCode = @TeamCode";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@TeamCode", TeamCode);
                return DbSFO2ORead.ExecuteSqlFirst<TeamInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }

        public TeamInfoEntity GetTeamInfoByPid(int Pid)
        {
            if (Pid==0)
            {
                throw new ArgumentException("pid");
            }
            try
            {
                string sql = @"SELECT ti.TeamCode,ti.sku,ti.TeamStatus,ti.StartTime,ti.EndTime,ti.UserID,ti.TeamNumbers,ti.SuccTeamTime,ti.PromotionId FROM TeamInfo AS ti WHERE ti.PromotionId = @Pid";

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@Pid", Pid);
                return DbSFO2ORead.ExecuteSqlFirst<TeamInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return null;
            }
        }
    }
}
