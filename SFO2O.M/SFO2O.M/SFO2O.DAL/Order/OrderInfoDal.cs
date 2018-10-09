using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SFO2O.Model.Order;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Utility.Uitl;
using SFO2O.Model.Team;
using SFO2O.DAL.Huoli;
using SFO2O.Model.Huoli;
using SFO2O.DAL.My;
using SFO2O.Model.My;
using SFO2O.DAL.Account;
using SFO2O.Model.Product;
using SFO2O.DAL.GiftCard;

namespace SFO2O.DAL.Order
{
    public class OrderInfoDal : BaseDal
    {
        MyDal myDal = new MyDal();

        public OrderInfoDal()
        { }

        public string GetNewOrderId()
        {
            string sql = @"DECLARE @LastNum bigint,@Currdate int
                            SET @Currdate=CONVERT(varchar(10),GETDATE(),12)
	                        update FrameCode
	                        set @LastNum=LastNum=LastNum+1
	                        where Currdate=@Currdate
	                        AND CType='Order'
                            if(@@RowCount=0)
                            BEGIN                           
                            SET @LastNum=0
                            WHILE(len(@LastNum)!=5 or @LastNum>99000)
                            begin
                            select @LastNum =cast(ceiling(rand() * 100000) as int)
                            end
                            set @LastNum=@LastNum+100000
                            insert INTO FrameCode VALUES(@Currdate,'Order','',@LastNum)
                            end                            
                            SELECT 'S'+cast(@Currdate AS varchar(6))+'11'+Cast(@LastNum as varchar(6))";
            var db = DbSFO2OMain;
            return db.ExecuteScalar(CommandType.Text, sql).ToString();
        }
        public string GetNewTeamOrderId()
        {
            string sql = @"DECLARE @LastNum bigint,@Currdate int
                            SET @Currdate=CONVERT(varchar(10),GETDATE(),12)
	                        update FrameCode
	                        set @LastNum=LastNum=LastNum+1
	                        where Currdate=@Currdate
	                        AND CType='Team'
                            if(@@RowCount=0)
                            BEGIN                           
                            SET @LastNum=0
                            WHILE(len(@LastNum)!=5 or @LastNum>99000)
                            begin
                            select @LastNum =cast(ceiling(rand() * 100000) as int)
                            end
                            set @LastNum=@LastNum+100000
                            insert INTO FrameCode VALUES(@Currdate,'Team','',@LastNum)
                            end                            
                            SELECT 'P'+cast(@Currdate AS varchar(6))+'11'+Cast(@LastNum as varchar(6))";
            var db = DbSFO2OMain;
            return db.ExecuteScalar(CommandType.Text, sql).ToString();
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderInfoEntity">订单基本</param>
        /// <param name="list">订单商品信息</param>
        public void CreateOrder(OrderInfoEntity orderInfoEntity, List<OrderProductsEntity> list,bool insertTeam,TeamInfoEntity teamInfo)
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
                        OrderProductsDal.Add(item, db, tran);

                        //OrderProductsDal.AddOrderPromotion(item, db, tran);//NOTE：增加促销记录

                        StockDal.UpdateByLockedQty(item.Spu, item.Sku, item.Quantity, db, tran);
                    }
                    //if (insertTeam)
                    //{
                    //    AddTeam(teamInfo, db, tran);
                    //}
                    //if (orderInfoEntity.Huoli > 0)
                    //{
                    //    HuoliDal.UpdateByLockedHuoli(orderInfoEntity.UserId, orderInfoEntity.Huoli, db, tran);
                    //}
                    Add(orderInfoEntity, db, tran);

                    string orderCode = string.IsNullOrEmpty(orderInfoEntity.ParentOrderCode) ? orderInfoEntity.OrderCode : orderInfoEntity.ParentOrderCode;
                    //GiftCardDal.ChangeGiftCardStatusByEventType(1, orderInfoEntity.GiftCardId, db, tran, orderCode,orderInfoEntity.OrderCode);
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
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderInfoEntity">订单基本</param>
        /// <param name="list">订单商品信息</param>
        public void NewCreateOrder(OrderInfoEntity orderInfoEntity, List<OrderProductsEntity> list)
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
                        OrderProductsDal.Update(item, db, tran);
                    }
                    Update(orderInfoEntity, db, tran);

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
        }

        /// <summary>
        ///  支付成功后更新订单
        /// </summary>
        /// <param name="orderInfoEntity">订单基本</param>
        /// <param name="list">订单商品信息</param>
        public void OrderPayOk(OrderInfoEntity orderInfoEntity, List<StockEntity> list)
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
                        //得看orderProduct 表里面库存来源 普通订单更新库存
                        StockDal.UpdateByPay(item.Spu, item.Sku, item.Qty, db, tran, item.SQty);
                    }
                    //LogHelper.Error(string.Format("-----普通订单更新开始-----" + orderInfoEntity.OrderCode));
                    bool b1 = UpdateByPay(orderInfoEntity, db, tran);
                    //LogHelper.Error(string.Format("-----普通订单更新结束-----结果：" + b1));

                    ///// 订单用酒豆值支付的情况
                    //if (orderInfoEntity.Huoli > 0)
                    //{

                    //    MyHL myHuoliLog = new MyHL();
                    //    myHuoliLog.userId = orderInfoEntity.UserId;
                    //    myHuoliLog.Direction = 2;
                    //    myHuoliLog.OriginalHuoLi = orderInfoEntity.OriginalHuoLi;
                    //    myHuoliLog.ChangedHuoLi = orderInfoEntity.ChangedHuoLi;
                    //    myHuoliLog.CurrentHuoLi = orderInfoEntity.OriginalHuoLi - orderInfoEntity.ChangedHuoLi;
                    //    myHuoliLog.Description = "购物抵扣";
                    //    myHuoliLog.TradeCode = orderInfoEntity.OrderCode;
                    //    myHuoliLog.addTime = DateTime.Now;

                    //    //LogHelper.Error(string.Format("-----普通订单-----酒豆日志表插入开始"));
                    //    bool logResult = myDal.insertHuoliLog(myHuoliLog);
                    //    //LogHelper.Error(string.Format("-----普通订单-----酒豆日志表插入结束--结果：" + logResult));

                    //    updateHuoLiTotal(orderInfoEntity.Huoli, orderInfoEntity.UserId);
                       
                    //}

                    //LogHelper.Error(string.Format("-----普通订单-----优惠券插入开始"));
                    //LogHelper.Error(string.Format("-----普通订单-----优惠券数值：" + orderInfoEntity.Coupon));
                    // 订单用优惠券支付的情况
                    //if (orderInfoEntity.Coupon > 0)
                    //{
                    //    //LogHelper.Error(string.Format("-----普通订单-----改变状态方法之前：OrderCode：" + (string.IsNullOrEmpty(orderInfoEntity.ParentOrderCode) ? orderInfoEntity.OrderCode : orderInfoEntity.ParentOrderCode)));
                    //    GiftCardDal.ChangeGiftCardStatusByEventType(2, 0, db, tran, string.IsNullOrEmpty(orderInfoEntity.ParentOrderCode) ? orderInfoEntity.OrderCode : orderInfoEntity.ParentOrderCode);
                    //}

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
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(OrderInfoEntity model, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OrderInfo(");
            strSql.Append("OrderCode,UserId,OrderStatus,TotalAmount,Freight,ProductTotalAmount,CustomsDuties,PaidAmount,ExchangeRate,PayType,Receiver,Phone,PassPortType,PassPortNum,ReceiptAddress,ReceiptPostalCode,ReceiptRegion,ReceiptCity,ReceiptProvince,ReceiptCountry,ShippingMethod,CreateTime,PayTime,DeliveryTime,ArrivalTime,OrderCompletionTime,CancelReason,Remark,GatewayCode,ParentOrderCode,TaxType,TeamCode,HuoLi,Coupon,OrderSourceType,OrderSourceValue,DividedAmount,DividedPercent)");
            strSql.Append(" values (");
            strSql.Append("@OrderCode,@UserId,@OrderStatus,@TotalAmount,@Freight,@ProductTotalAmount,@CustomsDuties,@PaidAmount,@ExchangeRate,@PayType,@Receiver,@Phone,@PassPortType,@PassPortNum,@ReceiptAddress,@ReceiptPostalCode,@ReceiptRegion,@ReceiptCity,@ReceiptProvince,@ReceiptCountry,@ShippingMethod,@CreateTime,@PayTime,@DeliveryTime,@ArrivalTime,@OrderCompletionTime,@CancelReason,@Remark,@GatewayCode,@ParentOrderCode,@TaxType,@TeamCode,@HuoLi,@Coupon,@OrderSourceType,@OrderSourceValue,@DividedAmount,@DividedPercent)");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@UserId", model.UserId);
            parameters.Append("@OrderStatus", model.OrderStatus);
            parameters.Append("@TotalAmount", model.TotalAmount);
            parameters.Append("@Freight", model.Freight);
            parameters.Append("@ProductTotalAmount", model.ProductTotalAmount);
            parameters.Append("@CustomsDuties", model.CustomsDuties);
            parameters.Append("@PaidAmount", model.PaidAmount);
            parameters.Append("@ExchangeRate", model.ExchangeRate);
            parameters.Append("@PayType", model.PayType);
            parameters.Append("@Receiver", model.Receiver);
            parameters.Append("@Phone", model.Phone);
            parameters.Append("@PassPortType", model.PassPortType);
            parameters.Append("@PassPortNum", model.PassPortNum);
            parameters.Append("@ReceiptAddress", model.ReceiptAddress);
            parameters.Append("@ReceiptPostalCode", model.ReceiptPostalCode);
            parameters.Append("@ReceiptRegion", model.ReceiptRegion);
            parameters.Append("@ReceiptCity", model.ReceiptCity);
            parameters.Append("@ReceiptProvince", model.ReceiptProvince);
            parameters.Append("@ReceiptCountry", model.ReceiptCountry);
            parameters.Append("@ShippingMethod", model.ShippingMethod);
            parameters.Append("@CreateTime", model.CreateTime);
            parameters.Append("@PayTime", model.PayTime);
            parameters.Append("@DeliveryTime", model.DeliveryTime);
            parameters.Append("@ArrivalTime", model.ArrivalTime);
            parameters.Append("@OrderCompletionTime", model.OrderCompletionTime);
            parameters.Append("@CancelReason", model.CancelReason);
            parameters.Append("@Remark", model.Remark);
            //增加三个属性
            parameters.Append("@GatewayCode", model.GatewayCode);
            parameters.Append("@ParentOrderCode", model.ParentOrderCode);
            parameters.Append("@TaxType", model.TaxType);

            //增加优惠券
            parameters.Append("@Coupon", model.Coupon);
           // parameters.Append("@HuoLi", model.Huoli);
            parameters.Append("@TeamCode", model.TeamCode);
            parameters.Append("@OrderSourceType", model.OrderSourceType);
            parameters.Append("@OrderSourceValue", model.OrderSourceValue);
            parameters.Append("@DividedAmount", model.DividedAmount);
            parameters.Append("@DividedPercent", model.DividedPercent);

            //增加活力（酒豆）
            parameters.Append("@HuoLi", model.Huoli);

            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void AddTeam(TeamInfoEntity model, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TeamInfo(");
            strSql.Append("TeamCode,sku,TeamStatus,UserID,TeamNumbers,CreatTime,PromotionId)");
            strSql.Append(" values (");
            strSql.Append("@TeamCode,@sku,@TeamStatus,@UserID,@TeamNumbers,getdate(),@PromotionId)");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@TeamCode", model.TeamCode.Trim());
            parameters.Append("@sku", model.Sku);
            parameters.Append("@TeamStatus", model.TeamStatus);
            parameters.Append("@UserID", model.UserID);
            parameters.Append("@TeamNumbers", model.TeamNumbers);
            parameters.Append("@PromotionId", model.PromotionId);
            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Update(OrderInfoEntity model, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE OrderInfo ");
            strSql.Append("SET TotalAmount=@TotalAmount, ");
            strSql.Append("CustomsDuties=@CustomsDuties ");
            strSql.Append("WHERE  OrderStatus=1 AND UserId=@UserId AND [OrderCode]=@OrderCode ");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@UserId", model.UserId);
            parameters.Append("@TotalAmount", model.TotalAmount);
            parameters.Append("@CustomsDuties", model.CustomsDuties);

            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 支付完成更新订单支付金额及时间
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByPay(OrderInfoEntity model, Database db, DbTransaction tran)
        {
            string sql = "Update OrderInfo Set PaidAmount=@PaidAmount,PayTime=@PayTime,OrderStatus=1 Where UserId=@UserId AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@UserId", model.UserId);
            parameters.Append("@PaidAmount", model.PaidAmount);
            parameters.Append("@PayTime", DateTime.Now);
            return db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran) > 0;
        }

        /// <summary>
        /// 支付完成更新团订单支付金额及时间
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateTeamOrderByPayForFinished(OrderInfoEntity model, Database db, DbTransaction tran)
        {
            string sql = "Update OrderInfo Set PaidAmount=@PaidAmount,PayTime=@PayTime,OrderStatus=@OrderStatus Where OrderStatus=0 AND UserId=@UserId AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@UserId", model.UserId);
            parameters.Append("@PaidAmount", model.PaidAmount);
            parameters.Append("@PayTime", DateTime.Now);
            parameters.Append("@OrderStatus", model.OrderStatus);
            return db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran) > 0;
        }

        public bool UpdateTeamOrderByPay(OrderInfoEntity model, Database db, DbTransaction tran)
        {
            string sql = "Update OrderInfo Set OrderStatus=@OrderStatus Where OrderStatus=6 AND UserId=@UserId AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@UserId", model.UserId);
            parameters.Append("@PaidAmount", model.PaidAmount);
            //parameters.Append("@PayTime", DateTime.Now);
            parameters.Append("@OrderStatus", model.OrderStatus);
            return db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByPay(OrderInfoEntity model)
        {
            string sql = "Update OrderInfo Set PayTime=@PayTime,OrderStatus=-2 Where OrderStatus=0  AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@PayTime", DateTime.Now);
            return DbSFO2OMain.ExecuteNonQuery(CommandType.Text, sql, parameters) > 0;
        }


        /// <summary>
        /// 获取待付款的订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByCode(string orderCode, int userId)
        {
            const string sql = @"SELECT [OrderCode]
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
                                          ,[CreateTime]
                                          ,[PayTime]
                                          ,[DeliveryTime]
                                          ,[ArrivalTime]
                                          ,[OrderCompletionTime]
                                          ,[CancelReason]
                                          ,[Remark]
                                          ,[Huoli]
                                          ,IsNull([Coupon],0) as Coupon
                                      FROM [OrderInfo] (NOLOCK)
                                      WHERE OrderStatus=0 AND UserId=@UserId AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            parameters.Append("@UserId", userId);

            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }
        /// <summary>
        /// 查询最后一单是否是支付宝支付
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity isAliPay(int userId)
        {
            const string sql = @"SELECT top 1 op.PayPlatform
                                      FROM [OrderInfo] AS oi (NOLOCK)
                                      LEFT JOIN OrderPayment AS op ON op.OrderCode = oi.OrderCode
                                      WHERE oi.UserId=@UserId AND op.PayStatus=2 ORDER BY oi.PayTime DESC ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@UserId", userId);

            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByCodeId(string orderCode, int userId)
        {
            const string sql = @"SELECT [OrderCode]
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
                                          ,[CreateTime]
                                          ,[PayTime]
                                          ,[DeliveryTime]
                                          ,[ArrivalTime]
                                          ,[OrderCompletionTime]
                                          ,[CancelReason]
                                          ,[Remark]
                                          ,[TeamCode]
                                      FROM [OrderInfo] (NOLOCK)
                                      WHERE UserId=@UserId AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            parameters.Append("@UserId", userId);

            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        /// <summary>
        /// 获取待付款的订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByCode(string orderCode)
        {
            const string sql = @"SELECT oi.[OrderCode]
                                          ,oi.[UserId]
                                          ,oi.[OrderStatus]
                                          ,oi.[TotalAmount]
                                          ,oi.[Freight]
                                          ,oi.[ProductTotalAmount]
                                          ,oi.[CustomsDuties]
                                          ,oi.[PaidAmount]
                                          ,oi.[ExchangeRate]
                                          ,oi.[PayType]
                                          ,oi.[Receiver]
                                          ,oi.[Phone]
                                          ,oi.[PassPortType]
                                          ,oi.[PassPortNum]
                                          ,oi.[ReceiptAddress]
                                          ,oi.[ReceiptPostalCode]
                                          ,oi.[ReceiptRegion]
                                          ,oi.[ReceiptCity]
                                          ,oi.[ReceiptProvince]
                                          ,oi.[ReceiptCountry]
                                          ,oi.[ShippingMethod]
                                          ,oi.[CreateTime]
                                          ,oi.[PayTime]
                                          ,oi.[DeliveryTime]
                                          ,oi.[ArrivalTime]
                                          ,oi.[OrderCompletionTime]
                                          ,oi.[CancelReason]
                                          ,oi.[Remark]
                                          ,oi.[HuoLi] AS Huoli
                                          ,oi.[HuoLi] AS ChangedHuoLi
                                          ,hlt.HuoLi AS OriginalHuoLi
                                      FROM [OrderInfo] AS oi (NOLOCK)
                                      LEFT JOIN HuoLiTotal AS hlt ON oi.UserId = hlt.UserId
                                      WHERE oi.OrderStatus=0 AND oi.OrderCode=@OrderCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);

            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }
        /// <summary>
        /// 回写订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfoEntity GetOrderInfoByCodeAndStatus(string orderCode)
        {
            const string sql = @"SELECT oi.[OrderCode]
                                          ,oi.[UserId]
                                          ,oi.[OrderStatus]
                                          ,oi.[TotalAmount]
                                          ,oi.[Freight]
                                          ,oi.[ProductTotalAmount]
                                          ,oi.[CustomsDuties]
                                          ,oi.[PaidAmount]
                                          ,oi.[ExchangeRate]
                                          ,oi.[PayType]
                                          ,oi.[Receiver]
                                          ,oi.[Phone]
                                          ,oi.[PassPortType]
                                          ,oi.[PassPortNum]
                                          ,oi.[ReceiptAddress]
                                          ,oi.[ReceiptPostalCode]
                                          ,oi.[ReceiptRegion]
                                          ,oi.[ReceiptCity]
                                          ,oi.[ReceiptProvince]
                                          ,oi.[ReceiptCountry]
                                          ,oi.[ShippingMethod]
                                          ,oi.[CreateTime]
                                          ,oi.[PayTime]
                                          ,oi.[DeliveryTime]
                                          ,oi.[ArrivalTime]
                                          ,oi.[OrderCompletionTime]
                                          ,oi.[CancelReason]
                                          ,oi.[Remark]
                                          ,oi.[ParentOrderCode]
                                          ,oi.[HuoLi] AS Huoli
                                          ,oi.[HuoLi] AS ChangedHuoLi
                                          ,hlt.HuoLi AS OriginalHuoLi
                                          ,oi.Coupon AS Coupon
                                      FROM [OrderInfo] AS oi (NOLOCK)
                                      LEFT JOIN HuoLiTotal AS hlt ON oi.UserId = hlt.UserId
                                      WHERE  oi.OrderCode=@OrderCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);

            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        public OrderInfoEntity GetOrderInfoByOrderCode(string orderCode)
        {

            const string sql = @"SELECT ISNULL([OrderInfo].ParentOrderCode,'') AS ParentOrderCode
                                    ,[OrderInfo].TaxType AS TaxType
                                    ,[OrderInfo].TeamCode AS TeamCode
                                    ,[OrderStatus]
                                    ,[OrderCode]
                                    FROM [OrderInfo](NOLOCK)
                                    WHERE [OrderInfo].OrderCode = @OrderCode ";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        public OrderInfoEntity GetOrderInfoByPay(string orderCode)
        {

            const string sql = @"SELECT ISNULL(oi.ParentOrderCode,'') AS ParentOrderCode
				                    ,oi.TaxType AS TaxType
				                    ,oi.TeamCode AS TeamCode
				                    ,op.PayPlatform AS PayPlatform
			                    FROM [OrderInfo] AS oi (NOLOCK)
			                    INNER JOIN OrderPayment AS op ON op.OrderCode = oi.OrderCode AND op.PayStatus = 2
			                    WHERE oi.OrderCode = @OrderCode AND oi.OrderStatus = 1";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }
        public OrderInfoEntity GetOrderInfoHuoliByOrderCode(string orderCode)
        {

            const string sql = @"SELECT ISNULL(oi.ParentOrderCode,'') AS ParentOrderCode
				                    ,oi.TaxType AS TaxType
				                    ,oi.TeamCode AS TeamCode
				                    ,op.PayPlatform AS PayPlatform
                                    ,oi.HuoLi
			                    FROM [OrderInfo] AS oi (NOLOCK)
			                    INNER JOIN OrderPayment AS op ON op.OrderCode = oi.OrderCode
			                    WHERE oi.OrderCode = @OrderCode";

            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        public OrderInfoEntity GetOrderCodeByParentOrderCode(string parentOrderCode)
        {
            const string sql = @"SELECT oi.OrderCode,oi.TaxType  
                                    FROM OrderInfo AS oi
                                    WHERE oi.ParentOrderCode = @ParentOrderCode AND OrderStatus=0";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@ParentOrderCode", parentOrderCode);
            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        /// <summary>
        /// 插入推送IBS订单信息
        /// </summary>
        public void AddPushIBSOrderInfo(PushIBSOrderInfoEntity model, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PushIBSOrderInfo(");
            strSql.Append("OrderCode,MailNo,PushStatus,PushFailCount,PushSuccTime,TaxType,GateWayCode,PayType,CreateTime)");
            //strSql.Append("OrderCode,MailNo,PushStatus,PushFailCount,PushSuccTime,TaxType,GateWayCode,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@OrderCode,@MailNo,@PushStatus,@PushFailCount,@PushSuccTime,@TaxType,@GateWayCode,@PayType,@CreateTime)");
            //strSql.Append("@OrderCode,@MailNo,@PushStatus,@PushFailCount,@PushSuccTime,@TaxType,@GateWayCode,@CreateTime)");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@MailNo", model.MailNo);
            parameters.Append("@PushStatus", model.PushStatus);
            parameters.Append("@PushFailCount", model.PushFailCount);
            parameters.Append("@PushSuccTime", model.PushSuccTime);
            parameters.Append("@GateWayCode", model.GateWayCode);
            parameters.Append("@TaxType", model.TaxType);
            parameters.Append("@PayType", model.PayType);
            parameters.Append("@CreateTime", model.CreateTime);

            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 增加推送IBS订单信息
        /// </summary>
        /// <param name="model"></param>
        public void CreatePushIBSOrderInfo(PushIBSOrderInfoEntity model)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    /// 插入推送IBS订单信息
                    AddPushIBSOrderInfo(model, db, tran);

                    tran.Commit();
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ext)
                {
                    LogHelper.Error("-------增加推送IBS订单信息异常------ext:" + ext);
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                    throw ext;
                }
            }
        }

        /// <summary>
        /// 团订单更新
        /// </summary>
        /// <param name="orderInfoEntity"></param>
        /// <param name="list"></param>
        /// <param name="teamInfoEntity"></param>
        public void TeamOrderPayOk(OrderInfoEntity orderInfoEntity, List<StockEntity> list, TeamInfoEntity teamInfoEntity)
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
                        StockDal.UpdateByPay(item.Spu, item.Sku, item.Qty, db, tran);
                    }

                    //LogHelper.Info("--------TeamPayAfter----7-----更新ordreinfo----");
                    bool b1 = UpdateTeamOrderByPayForFinished(orderInfoEntity, db, tran);
                    //LogHelper.Info("--------TeamPayAfter----8-----更新ordreinfo后结果----" + b1);

                    /// 团长更新TeamInfo表
                    if (orderInfoEntity.UserId == teamInfoEntity.UserID)
                    {
                        //LogHelper.Info("--------TeamPayAfter----9-----更新teaminfo----");
                        //LogHelper.Info("--------TeamPayAfter----9.1-----更新团状态" + teamInfoEntity.TeamStatus);
                        bool b2 = UpdateTeamInfoByPay(teamInfoEntity, db, tran);
                        //LogHelper.Info("--------TeamPayAfter----10-----更新teaminfo后结果----" + b2);
                    }

                    /// 订单用酒豆值支付的情况
                    if (orderInfoEntity.Huoli > 0)
                    {

                        MyHL myHuoliLog = new MyHL();
                        myHuoliLog.userId = orderInfoEntity.UserId;
                        myHuoliLog.Direction = 2;
                        myHuoliLog.OriginalHuoLi = orderInfoEntity.OriginalHuoLi;
                        myHuoliLog.ChangedHuoLi = orderInfoEntity.ChangedHuoLi;
                        myHuoliLog.CurrentHuoLi = orderInfoEntity.OriginalHuoLi - orderInfoEntity.ChangedHuoLi;
                        myHuoliLog.Description = "购物抵扣";
                        myHuoliLog.TradeCode = orderInfoEntity.OrderCode;
                        myHuoliLog.addTime = DateTime.Now;

                        //LogHelper.Info(string.Format("-----TeamPayAfter11-----酒豆日志表插入开始"));
                        bool logResult = myDal.insertHuoliLog(myHuoliLog);
                        //LogHelper.Info(string.Format("-----TeamPayAfter11-----酒豆日志表插入结束--结果：" + logResult));

                        updateHuoLiTotal(orderInfoEntity.Huoli, orderInfoEntity.UserId);
                        
                    }

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
        }

        /// <summary>
        /// 更新订单信息表和团信息表状态，不做库存操作
        /// </summary>
        /// <param name="orderInfoEntity"></param>
        /// <param name="teamInfoEntity"></param>
        public void TeamOrderPayOkForStatus(OrderInfoEntity orderInfoEntity, TeamInfoEntity teamInfoEntity)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    //LogHelper.Info("--------TeamPayAfter----4.2-----" + orderInfoEntity.OrderCode +","+ orderInfoEntity.OrderStatus);
                    bool b1 = UpdateTeamOrderByPay(orderInfoEntity, db, tran);
                    //LogHelper.Info("--------TeamPayAfter----4.3-----" + b1);
                    UpdateTeamInfoByPay(teamInfoEntity, db, tran);

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
        }

        /// <summary>
        /// 更新团信息表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool UpdateTeamInfoByPay(TeamInfoEntity model, Database db, DbTransaction tran)
        {
            string sql = "Update TeamInfo Set TeamStatus=@TeamStatus,SuccTeamTime=@SuccTeamTime,StartTime=@StartTime,EndTime=@EndTime Where TeamCode=@TeamCode ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            //LogHelper.Info("--------TeamPayAfter----9.1.1-----更新团状态" + model.TeamStatus);
            parameters.Append("@TeamStatus", model.TeamStatus);
            parameters.Append("@SuccTeamTime", DateTime.Now);
            parameters.Append("@TeamCode", model.TeamCode);
            parameters.Append("@StartTime", DateTime.Now);
            parameters.Append("@EndTime", DateTime.Now.AddHours(24));
            return db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran) > 0;
        }

        public OrderInfoEntity GetTeamOrderInfoByCode(string orderCode)
        {
            const string sql = @"SELECT [OrderCode]
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
                                          ,[CreateTime]
                                          ,[PayTime]
                                          ,[DeliveryTime]
                                          ,[ArrivalTime]
                                          ,[OrderCompletionTime]
                                          ,[CancelReason]
                                          ,[Remark]
                                      FROM [OrderInfo] (NOLOCK)
                                      WHERE OrderStatus=6 AND [OrderCode]=@OrderCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);

            return DbSFO2ORead.ExecuteSqlFirst<OrderInfoEntity>(sql, parameters);
        }

        public IList<OrderInfoEntity> GetNeedCloseOrderInfoByCode(string TeamCode)
        {
            const string sql = @"SELECT oi.[OrderCode]
                                          ,oi.[UserId]
                                          ,oi.[OrderStatus]
                                          ,oi.[TotalAmount]
                                          ,oi.[Freight]
                                          ,oi.[ProductTotalAmount]
                                          ,oi.[CustomsDuties]
                                          ,oi.[PaidAmount]
                                          ,oi.[ExchangeRate]
                                          ,oi.[PayType]
                                          ,oi.[Receiver]
                                          ,oi.[Phone]
                                          ,oi.[PassPortType]
                                          ,oi.[PassPortNum]
                                          ,oi.[ReceiptAddress]
                                          ,oi.[ReceiptPostalCode]
                                          ,oi.[ReceiptRegion]
                                          ,oi.[ReceiptCity]
                                          ,oi.[ReceiptProvince]
                                          ,oi.[ReceiptCountry]
                                          ,oi.[ShippingMethod]
                                          ,oi.[CreateTime]
                                          ,oi.[PayTime]
                                          ,oi.[DeliveryTime]
                                          ,oi.[ArrivalTime]
                                          ,oi.[OrderCompletionTime]
                                          ,oi.[CancelReason]
                                          ,oi.[Remark] 
			                            FROM OrderInfo AS oi
                                        WHERE oi.TeamCode=@TeamCode AND oi.OrderStatus = 0";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@TeamCode", TeamCode);

            return DbSFO2ORead.ExecuteSqlList<OrderInfoEntity>(sql, parameters);
        }

        public bool updateHuoLiTotal(decimal huoli, int userId)
        {
            string sql = @" UPDATE HuoLiTotal SET HuoLi = HuoLi-@huoli,LockedHuoLi = LockedHuoLi-@huoli WHERE UserId=@userId";
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

        public bool insertIntoHuoliDetail(HuoliDetailEntity huoliDetail)
        {
            string sql = @" INSERT INTO HuoLiDetail
                                                (
	                            UserId,
	                            AddHuoLi,
	                            DeadLine
                            )
                            VALUES
                            (
	                             @UserId,
	                             @AddHuoLi,
	                             @DeadLine
                            )";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@UserId", huoliDetail.UserId);
                parameters.Append("@AddHuoLi", huoliDetail.AddHuoLi);
                parameters.Append("@DeadLine", huoliDetail.DeadLine);
                return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 团成员订单订单来源相关值的更新
        /// </summary>
        /// <param name="orderInfoEntity"></param>
        /// <param name="teamInfoEntity"></param>
        public void TeamMemberOrderUpdatePayOK(OrderInfoSourceEntity orderInfoSource)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    bool b1 = UpdateTeamMemberOrderByPay(orderInfoSource, db, tran);
                    //LogHelper.Info("--------TeamMemberOrderUpdate---2----result:" + b1);

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
        }

        /// <summary>
        /// 更新团员订单来源值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool UpdateTeamMemberOrderByPay(OrderInfoSourceEntity orderInfoSource, Database db, DbTransaction tran)
        {
            string sql = @"Update OrderInfo 
                            Set orderSourceType=@orderSourceType
                                ,OrderSourceValue=@OrderSourceValue
                                ,DividedAmount=@DividedAmount
                                ,DividedPercent=@DividedPercent 
                            Where TeamCode=@TeamCode AND OrderStatus = 1 ";
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@orderSourceType", orderInfoSource.OrderSourceType);
            parameters.Append("@OrderSourceValue", orderInfoSource.OrderSourceValue);
            parameters.Append("@DividedAmount", orderInfoSource.DividedAmount);
            parameters.Append("@DividedPercent", orderInfoSource.DividedPercent);
            parameters.Append("@TeamCode", orderInfoSource.TeamCode);
            return db.ExecuteNonQuery(CommandType.Text, sql, parameters, tran) > 0;
        }

        public ProductInfoModel GetOrderImage(string OrderCode)
        {
            const string sql = @"SELECT pi1.Spu
				                        ,pi2.ImagePath
		                        FROM OrderInfo AS oi
		                        INNER JOIN OrderProducts AS op ON op.OrderCode = oi.OrderCode
		                        INNER JOIN ProductInfo AS pi1 ON pi1.Spu = op.Spu AND pi1.LanguageVersion = 1
		                        INNER JOIN ProductImage AS pi2 ON pi2.Spu = pi1.Spu AND pi2.SortValue = 1
		                        WHERE oi.OrderCode=@OrderCode";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", OrderCode);
            return DbSFO2ORead.ExecuteSqlFirst<ProductInfoModel>(sql, parameters);
        }

        public IList<OrderInfoEntity> GetOrderInfoByTeamCode(string teamCode)
        {
            const string sql = @"SELECT [OrderCode]
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
                                      ,[CreateTime]
                                      ,[PayTime]
                                      ,[DeliveryTime]
                                      ,[ArrivalTime]
                                      ,[OrderCompletionTime]
                                      ,[CancelReason]
                                      ,[Remark]
                                      ,[Huoli]
                                FROM OrderInfo AS oi
                                WHERE oi.TeamCode = @TeamCode";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@TeamCode", teamCode);

            return DbSFO2ORead.ExecuteSqlList<OrderInfoEntity>(sql, parameters);
        }

        public IList<OrderInfoEntity> GetOrderInfoStatus(string OrderCode)
        {
            string sql = @"SELECT [OrderStatus]
                           FROM OrderInfo AS oi WHERE oi.ParentOrderCode = (                  
                                SELECT oi.ParentOrderCode
                                FROM OrderInfo AS oi  WHERE oi.OrderCode = @OrderCode)";
            try
            {
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", OrderCode);
                return DbSFO2ORead.ExecuteSqlList<OrderInfoEntity>(sql, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<OrderInfoEntity>();
            }
        }

    }
}
