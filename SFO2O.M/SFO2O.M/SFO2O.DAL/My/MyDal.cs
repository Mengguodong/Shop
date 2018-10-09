using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Enum;
using SFO2O.Model.My;
using SFO2O.Model.Shopping;
using SFO2O.Utility.Extensions;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Order;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SFO2O.DAL.Order;
using SFO2O.DAL.Huoli;
using SFO2O.Model.Account;
using SFO2O.Model.Refund;
using SFO2O.DAL.GiftCard;
namespace SFO2O.DAL.My
{
    public class MyDal : BaseDal
    {
        /// <summary>
        /// 获取我的首页订单统计信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        public IList<MyOrderInfoEntity> GetMyOrderIndex(int userId, int country, int language)
        {
            string sql = @"SELECT DISTINCT oi.[OrderCode]
                          ,[UserId]
                          ,[OrderStatus]
                          ,[TotalAmount]
                          ,[Freight]
                          ,[ProductTotalAmount]
                          ,[CustomsDuties]
                          ,[PaidAmount]
                          ,[ExchangeRate]
                          ,[PayType]
                          ,[Receiver]
                          ,[Phone]
                          ,[PassPortType]
                          ,[PassPortNum]
                          ,[ReceiptAddress]
                          ,[ReceiptPostalCode]
                          ,[ReceiptRegion]
                          ,[ReceiptCity]
                          ,[ReceiptProvince]
                          ,[ReceiptCountry]
                          ,[ShippingMethod]
                          ,oi.[CreateTime]
                          ,[PayTime]
                          ,[DeliveryTime]
                          ,[ArrivalTime]
                          ,[OrderCompletionTime]
                          ,[CancelReason]
                          ,[Remark],oi.[Huoli]  FROM OrderInfo AS oi
                            INNER JOIN OrderProducts AS op ON op.OrderCode=oi.OrderCode
                               INNER JOIN ProductInfo AS p ON op.Spu=p.Spu AND p.LanguageVersion=@LanguageVersion
                               INNER JOIN SkuInfo AS s ON s.Sku=op.Sku AND s.SpuId=p.Id 
                            WHERE oi.UserId=@UserId AND oi.ReceiptCountry=@CountryId ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CountryId", country);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2ORead.ExecuteSqlList<MyOrderInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<MyOrderInfoEntity>();
            }
        }


        /// <summary>
        /// 根据状态获取订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IList<MyOrderSkuInfoEntity> GetMyOrderProductInfos(int userId, int country, int language, int pageIndex, int pageSize, Model.Enum.OrderStatusEnum status)
        {


            string sql = @"                        
--DECLARE @PageSize INT
--DECLARE @PageIndex INT  
--SET @PageSize=100
--SET @PageIndex=1
  
;WITH tbAll([OrderCode],[UserId],[OrderStatus],[TotalAmount],[Freight],[ProductTotalAmount],[CustomsDuties],[CreateTime],[TeamCode],[Huoli],RowNum,Coupon) AS
(
    SELECT  [OrderCode]
            ,[UserId]
            ,[OrderStatus]
            ,[TotalAmount]
            ,[Freight]
            ,[ProductTotalAmount]
            ,[CustomsDuties] 
            ,[CreateTime] 
            ,[TeamCode]
            ,[Huoli]
            ,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNum 
            ,IsNull(Coupon,0) as Coupon
	FROM     OrderInfo AS oi  
	WHERE   oi.UserId=@UserId  
            AND oi.ReceiptCountry=@ReceiptCountry 
            {0}
)

SELECT  ta.[OrderCode],
        ta.[UserId],
        [OrderStatus],
        [TotalAmount],
        [Freight],
        [ProductTotalAmount],
        [CustomsDuties],
        ta.[CreateTime],
        rowsCount= (select max(rownum) from tbAll),
        op.RefundQuantity,
        op.Spu, 
        op.Sku,
        p.Name,
        s.MainDicValue, 
        s.MainValue,
        s.SubDicValue,
        s.SubValue, 
        op.Quantity, 
        op.PayUnitPrice,
        ta.TeamCode as TeamCode,ta.Huoli as HuoLi,ta.Coupon as Coupon,
        pim.ImagePath,p.NetWeightUnit,p.NetContentUnit,
        ti.UserID AS TeamUserId,
        ti.StartTime AS TeamStartTime,
        ti.EndTime AS TeamEndTime,
        ti.TeamStatus AS TeamStatus,
        oe.ExpressList AS ExpressList,
        oe.ExpressCompany AS ExpressCompany
from    tbAll as ta
        INNER JOIN OrderProducts AS op ON op.OrderCode=ta.OrderCode
        INNER JOIN ProductInfo AS p ON op.Spu=p.Spu AND p.LanguageVersion=@LanguageVersion
        INNER JOIN SkuInfo AS s ON s.Sku=op.Sku AND s.SpuId=p.Id
        INNER JOIN ProductImage AS pim ON pim.Spu=op.Spu AND pim.SortValue=1
        LEFT  JOIN TeamInfo AS ti ON ti.TeamCode=ta.TeamCode
        LEFT  JOIN OrderExpress as oe ON oe.OrderCode=ta.OrderCode 
where   rownum>(@PageSize*(@PageIndex-1)) and rownum <=(@PageSize*@PageIndex) ORDER BY ta.[CreateTime] DESC
";
            try
            {
                var parameters = DbSFO2OMain.CreateParameterCollection();


                sql = string.Format(sql, status != OrderStatusEnum.Default ? " and oi.OrderStatus=@OrderStatus " : " AND 1=1 ");

                parameters.Append("@OrderStatus", status.ToInt());
                parameters.Append("@UserId", userId);
                parameters.Append("@ReceiptCountry", country);
                parameters.Append("@LanguageVersion", language);
                parameters.Append("@PageSize", pageSize);
                parameters.Append("@PageIndex", pageIndex);
                return DbSFO2OMain.ExecuteSqlList<MyOrderSkuInfoEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<MyOrderSkuInfoEntity>();
            }
        }

        /// <summary>
        /// 获取订单详情信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public IList<MyOrderSkuInfoEntity> GetMyOrderInfoByParentOrderCode(string ParentOrderCode)
        {
            string sql =
                @"SELECT oi.OrderCode as OrderCode FROM OrderInfo AS oi WHERE oi.ParentOrderCode=@ParentOrderCode ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@ParentOrderCode", ParentOrderCode);
                return DbSFO2OMain.ExecuteSqlList<MyOrderSkuInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<MyOrderSkuInfoEntity>();
            }
        }

        /// <summary>
        /// 获取申诉的订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="country"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public IList<RefundInfoModel> GetMyOrderInfoAndRefund(string orderCode)
        {
            string sql =
                @"SELECT
                    	RefundCode,
                    	OrderCode,
                    	UserId,
                    	RefundType,
                    	RefundReason,
                    	RefundDescription,
                    	RefundStatus,
                    	TotalAmount,
                    	RMBTotalAmount,
                    	DutyAmount,
                    	RmbDutyAmount,
                    	IsReturnDuty,
                    	IsQualityProblem,
                    	Commission,
                    	CollectionCode,
                    	CreateTime,
                    	AuditTime,
                    	TobePickUpTime,
                    	PickupTime,
                    	CreateBy,
                    	Auditor,
                    	Pickupper,
                    	ExpressCompany,
                    	ExpressList,
                    	NoPassReason,
                    	CancelReason,
                    	ImagePath,
                    	ExchangeRate,
                    	SupplierId,
                    	RegionCode,
                    	ProductStatus,
                    	CompletionTime FROM 
                    RefundOrderInfo  WHERE OrderCode=@orderCode ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@orderCode", orderCode);
                return DbSFO2OMain.ExecuteSqlList<RefundInfoModel>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<RefundInfoModel>();
            }
        }

        public IList<MyOrderSkuInfoEntity> GetMyOrderInfo(int userId, int country, int language, string orderCode)
        {
            string sql =
                @"SELECT ti.StartTime as TeamStartTime,ti.EndTime as TeamEndTime,ti.TeamStatus,ti.UserID AS TeamUserId,oi.TeamCode,oi.HuoLi,oi.[OrderCode],oi.[UserId],[OrderStatus],[TotalAmount],[Freight],[ProductTotalAmount],[CustomsDuties]
	              ,oi.[PaidAmount],[ExchangeRate],opay.PayPlatform as PayType,[Receiver],[Phone],[PassPortType],[PassPortNum],[ReceiptAddress]
	              ,[ReceiptPostalCode],[TaxType]
				  ,ar.AreaName AS  [ReceiptRegion]
				  ,ct.CityName AS [ReceiptCity]
				  ,pv.ProvinceName AS [ReceiptProvince]			  
				  ,[ReceiptCountry]
				  ,[ShippingMethod]
	              ,oi.[CreateTime],[PayTime],[DeliveryTime],[ArrivalTime],[OrderCompletionTime],p.IsReturn
	              ,RowsCount=1
	              ,op.RefundQuantity,op.Spu, op.Sku,p.Name,s.MainDicValue, s.MainValue,s.SubDicValue,s.SubValue, op.Quantity, op.UnitPrice,op.PayUnitPrice,pim.ImagePath
	              ,opay.PayStatus,op.TaxRate,p.SupplierId,op.IsBearDuty,op.Commission,p.NetWeightUnit,p.NetContentUnit,oi.Huoli,gc.CardSum,ISNULL(oi.Coupon,0) AS Coupon,ISNULL(gc.Id,0) AS CouponId,oe.ExpressList ExpressList,oe.ExpressCompany ExpressCompany
                  
		            FROM   [OrderInfo] oi  
		            LEFT JOIN OrderProducts AS op ON op.OrderCode=oi.OrderCode
		            INNER JOIN ProductInfo AS p ON op.Spu=p.Spu AND p.LanguageVersion=@LanguageVersion
		            INNER JOIN SkuInfo AS s ON s.Sku=op.Sku AND s.SpuId=p.Id
		            INNER JOIN ProductImage AS pim ON pim.Spu=op.Spu AND pim.SortValue=1 
		            LEFT JOIN OrderPayment AS opay ON oi.[OrderCode]=opay.OrderCode  AND opay.PayStatus=2
		            
		            LEFT JOIN City AS ct ON ct.CityId=oi.ReceiptCity AND ct.LanguageVersion=@LanguageVersion
		            LEFT JOIN Province AS pv ON pv.ProvinceId=oi.ReceiptProvince  AND pv.LanguageVersion=@LanguageVersion
		            LEFT JOIN Area AS ar ON ar.AreaId=oi.ReceiptRegion AND ar.LanguageVersion=@LanguageVersion
                    LEFT JOIN TeamInfo AS ti ON  ti.TeamCode=oi.TeamCode
                    LEFT JOIN GiftCard AS gc ON gc.OrderCode= CASE WHEN ISNULL(oi.ParentOrderCode,'')='' THEN oi.OrderCode ELSE oi.ParentOrderCode END 
                    LEFT JOIN OrderExpress oe ON oe.OrderCode=oi.OrderCode
                    WHERE  oi.UserId=@UserId  AND oi.OrderCode=@OrderCode ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@OrderCode", orderCode);
                //parameters.Append("@ReceiptCountry", country);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2OMain.ExecuteSqlList<MyOrderSkuInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<MyOrderSkuInfoEntity>();
            }
        }

        /// <summary>
        /// 获取订单SKU详细信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="language"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>

        public IList<OrderSkuEntity> GetOrderSkuEntities(int userId, int language, string orderCode)
        {

            string sql =
                @"SELECT oi.[OrderCode],oi.[UserId],[OrderStatus],[TotalAmount],[Freight],[ProductTotalAmount],[CustomsDuties]
                      ,oi.[PaidAmount],[ExchangeRate],oi.[PayType]
                      ,oi.[CreateTime],[PayTime],[DeliveryTime],[ArrivalTime],[OrderCompletionTime] 
                      ,op.Spu, op.Sku, op.Quantity, op.UnitPrice, op.PayUnitPrice
                      ,op.TaxRate, op.Commission, op.PayAmount, op.TaxAmount
                      ,op.IsBearDuty,op.RefundQuantity
                      ,opay.PayStatus,
                      p.SupplierId,
                        op2.PromotionId
                              ,op2.PromotionPrice
                              ,op2.OriginalPrice
                              ,op2.OriginalRMBPrice
                              ,p2.PromotionCost
                      
                FROM   [OrderInfo] oi  
                LEFT JOIN OrderProducts AS op ON op.OrderCode=oi.OrderCode
                INNER JOIN ProductInfo AS p ON op.Spu=p.Spu AND p.LanguageVersion=1
                INNER JOIN SkuInfo AS s ON s.Sku=op.Sku AND s.SpuId=p.Id  
                LEFT JOIN OrderPayment AS opay ON oi.[OrderCode]=opay.OrderCode  AND opay.PayStatus=2
                LEFT JOIN OrderPromotions AS op2 ON oi.OrderCode=op2.OrderCode AND op.Sku = op2.Sku
						        LEFT JOIN Promotions AS p2 ON op2.PromotionId=p2.Id 
                WHERE  oi.UserId=@UserId   AND oi.OrderCode=@OrderCode";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@OrderCode", orderCode);
                parameters.Append("@LanguageVersion", language);
                return DbSFO2OMain.ExecuteSqlList<OrderSkuEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<OrderSkuEntity>();
            }

        }

        //        /// <summary>
        //        /// 获取订单物流信息
        //        /// </summary>
        //        /// <param name="orderCode"></param>
        //        /// <returns></returns>
        //        public IList<OrderLogisticsEntity> GetOrderLogistics(string orderCode)
        //        {
        //            string sql = @"SELECT oe.Id, oe.OrderCode, oe.ExpressCompany, oe.ExpressList, oe.ExPressStatus,ol.LogisticsTime,
        //                           ol.[Status], ol.Remark, ol.CreateTime, ol.CreateBy
        //                           FROM OrderExpress AS oe 
        //                           LEFT JOIN OrderLogistics AS ol ON oe.OrderCode=ol.OrderCode AND ol.[Status]=1 
        //                           WHERE ol.OrderCode=@OrderCode
        //                           ORDER BY ol.CreateTime DESC  ";
        //            try
        //            {
        //                var parameters = DbSFO2ORead.CreateParameterCollection();
        //                parameters.Append("@OrderCode", orderCode);
        //                return DbSFO2OMain.ExecuteSqlList<OrderLogisticsEntity>(sql, parameters);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error(ex);
        //                return new List<OrderLogisticsEntity>();
        //            }
        //        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool CancelOrder(string orderCode, List<StockEntity> list, int userId, decimal huoli, int couponId, bool IsUpdateGiftStatus)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        StockDal.UpdateByForOrderQty(item.Spu, item.Sku, item.Qty, db, tran);
                    }
                    if (huoli > 0)
                    {
                        HuoliDal.UpdateByLockedForHuoli(userId, huoli, db, tran);
                    }
                    
                    CancelOrder(orderCode, db, tran);
                    if (couponId > 0)
                    {
                        // 是否更新优惠券状态
                        if (IsUpdateGiftStatus)
                        {
                            //查询优惠券时间
                            GiftCardDal.ChangeGiftCardStatusByEventType(3, couponId, db, tran);
                        }
                    }
                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                    return true;
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    LogHelper.Error(ext);
                    return false;
                }
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool CancelOrder(string orderCode, Database db, DbTransaction tran)
        {
            string sql = @" UPDATE OrderInfo 
                            SET  	
                            OrderStatus =@OrderStatus ,OrderCompletionTime=GETDATE(),CancelReason='未支付取消'
                            WHERE OrderCode=@OrderCode ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", orderCode);
                parameters.Append("@OrderStatus", OrderStatusEnum.Closed);
                return db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public bool ConfirmOrder(string orderCode)
        {
            string sql = @" UPDATE OrderInfo 
                            SET  	
                            OrderStatus =@OrderStatus 
                            ,OrderCompletionTime=GETDATE() 
                            WHERE OrderCode=@OrderCode ";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", orderCode);
                parameters.Append("@OrderStatus", OrderStatusEnum.Complete);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }

            //对为申诉的商品生成结算单


        }
        /// <summary>
        /// 取运单号
        /// </summary>
        /// <param name="orderCode">订单号</param>
        /// <returns></returns>
        public string GetExpressCodeByOrderCode(string orderCode)
        {
            string sql = @"Select  top 1 ExpressList from orderexpress nolock where OrderCode=@OrderCode order by id asc;";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", orderCode);
                return DbSFO2OMain.ExecuteSqlScalar<string>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 获得我的酒豆
        /// </summary>
        /// <param name="orderCode">订单号</param>
        /// <returns></returns>
        public MyHL getMyHL(int userId)
        {
            string sql = @"SELECT hls.HuoLi as countHL,hls.LockedHuoLi as freezeHL,hls.HuoLiCurrent as usableHL
                            FROM dbo.HuoLiTotal AS hls WHERE hls.UserId=@userId";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@userId", userId);
            return DbSFO2OMain.ExecuteSqlFirst<MyHL>(sql, parameters);
        }
        ///
        /// /// <summary>
        /// 我的酒豆详情
        /// </summary>
        public List<MyHL> HLDetail(int userId, int typeName, int PageSize, int PageIndex)
        {
            string type = null;
            int startIndex = (PageIndex - 1) * PageSize + 1;
            int endIndex = PageIndex * PageSize;
            if (typeName == 0)
            {
                type = " and hll.Direction in (1,2)";
            }
            else if (typeName == 1)
            {
                type = " and hll.Direction = 1";
            }
            else if (typeName == 2)
            {
                type = " and hll.Direction = 2";
            }
            string sql = @"with sputb
                            AS
                              (SELECT hll.UserId,hll.TradeCode,hll.[Description],hll.Direction,hll.ChangedHuoLi,hll.CurrentHuoLi,CONVERT(VARCHAR(23),hll.CreateTime,120) as CreateTime,ROW_NUMBER() OVER (ORDER BY hll.Id DESC ) as rindex
                              FROM  HuoLiLog  AS hll WHERE hll.UserId=@userId " + type + ") select *,(select count(1) from sputb) as TotalRecord from sputb  where rindex between @StartIndex and @EndIndex";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@userId", userId);
            parameters.Append("@StartIndex", startIndex);
            parameters.Append("@EndIndex", endIndex);
            var list = DbSFO2OMain.ExecuteSqlList<MyHL>(sql, parameters);
            return list.ToList();
        }
        public bool updateIsPush(int userId,int type) 
        {
            string sql = @"UPDATE Customer SET  IsPushingInfo =@type WHERE ID=@userId";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@userId", userId);
                parameters.Append("@type", type);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 更新我的酒豆详情
        /// </summary>
        public bool updateHuoLiTotal(decimal huoli,int userId)
        {
            string sql = @" UPDATE HuoLiTotal SET HuoLi = HuoLi+@huoli WHERE UserId=@userId";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@huoli", huoli);
                parameters.Append("@userId", userId);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 插入我的酒豆log
        /// </summary>
        public bool insertHuoliLog(MyHL myHL)
        {
            string sql = @" INSERT INTO HuoLiLog
                                                (
	                            UserId,
	                            Direction,
	                            OriginalHuoLi,
	                            ChangedHuoLi,
	                            CurrentHuoLi,
	                            [Description],
	                            TradeCode,
	                            CreateTime
                            )
                            VALUES
                            (
	                             @UserId,
	                             @Direction,
	                             @OriginalHuoLi,
	                             @ChangedHuoLi,
	                             @CurrentHuoLi,
	                             @Description,
	                             @TradeCode,
	                             @CreateTime
                            )";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", myHL.userId);
                parameters.Append("@Direction", myHL.Direction);
                parameters.Append("@OriginalHuoLi", myHL.OriginalHuoLi);
                parameters.Append("@ChangedHuoLi", myHL.ChangedHuoLi);
                parameters.Append("@CurrentHuoLi", myHL.CurrentHuoLi);
                parameters.Append("@Description", myHL.Description);
                parameters.Append("@TradeCode", myHL.TradeCode);
                parameters.Append("@CreateTime", myHL.addTime);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        public bool insertHuoLiTotal(int userId)
        {
            string sql = @"INSERT INTO HuoLiTotal
                            (
	                            -- Id -- this column value is auto-generated
	                            UserId,
	                            HuoLi,
	                            LockedHuoLi,
	                            -- HuoLiCurrent -- this column value is auto-generated
	                            CreateTime,
	                            CreateBy
                            )
                            VALUES
                            (
	                            @UserId,
	                            0,
	                            0,
	                            @CreateTime,
	                            @CreateBy
                            )";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", userId);
                parameters.Append("@CreateTime", DateTime.Now);
                parameters.Append("@CreateBy","system");
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        public CustomerEntity getUserInfo(int userId)
        {
            string sql = @"SELECT * FROM Customer AS c WHERE c.ID=@userId";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@userId", userId);
            return DbSFO2OMain.ExecuteSqlFirst<CustomerEntity>(sql, parameters);
        }
        public bool getOrderInfoCount(int userId,string startTime,string endTime) 
        {
            string sql = @"SELECT count(1) FROM OrderInfo AS oi LEFT JOIN TeamInfo AS ti ON ti.TeamCode = oi.TeamCode AND ti.UserId = oi.UserId
WHERE ti.TeamStatus=3 AND ti.UserID=@userId AND ti.StartTime>=@startTime AND ti.StartTime<=@endTime AND oi.OrderStatus=4";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@userId", userId);
            parameters.Append("@startTime", startTime);
            parameters.Append("@endTime", endTime);
            object returnValue = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters);
            return returnValue == null ? false : Convert.ToInt32(returnValue) > 0;
        }

        public bool updateActivityInfoVisible(int userId, int visible, DateTime currentTime)
        {
            string sql = @"UPDATE InformationToCustomer
					        SET    InformationToCustomer.Visible = @visible
					        FROM   InformationToCustomer
						            INNER JOIN Information
							            ON Information.Id = InformationToCustomer.InformationId
					        WHERE  InformationToCustomer.UserId = @userId
						            AND Information.StartTime > @currentTime";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@userId", userId);
                parameters.Append("@visible", visible);
                parameters.Append("@currentTime", currentTime);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) >= 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
    }
}
