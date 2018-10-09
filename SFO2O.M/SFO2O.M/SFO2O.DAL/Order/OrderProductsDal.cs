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
using SFO2O.Model.Shopping;

namespace SFO2O.DAL.Order
{
    public class OrderProductsDal : BaseDal
    {
        public OrderProductsDal()
        { }
        /// <summary>
        /// 增加促销信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool AddOrderPromotion(OrderProductsEntity model, Database db, DbTransaction tran)
        {
            if (model.PromotionId > 0)
            {
                string sql =
                    @"INSERT INTO  [OrderPromotions]([OrderCode],[Spu],[Sku],[PromotionId],[PromotionPrice],[OriginalPrice],[OriginalRMBPrice])
                        VALUES (@OrderCode,@Spu,@Sku,@PromotionId,@PromotionPrice,@OriginalPrice,@OriginalRMBPrice)";
                var parameters = db.CreateParameterCollection();
                parameters.Append("@OrderCode", model.OrderCode);
                parameters.Append("@Spu", model.Spu);
                parameters.Append("@Sku", model.Sku);
                parameters.Append("@PromotionId", model.PromotionId);
                parameters.Append("@PromotionPrice", model.UnitPrice);
                parameters.Append("@OriginalPrice", model.OriginalPrice);
                parameters.Append("@OriginalRMBPrice", model.OriginalRMBPrice);

                
                return db.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters, tran) > 0;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void Add(OrderProductsEntity model, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OrderProducts(");
            strSql.Append("OrderCode,Spu,Sku,Quantity,UnitPrice,PayUnitPrice,TaxRate,Commission,PayAmount,TaxAmount,IsBearDuty,RefundQuantity,PayTaxAmountRMB,PayTaxAmonutHKD,VATTaxRate,CBEBTaxRate,PPATaxRate,ConsumerTaxRate,Huoli,Coupon)");
            strSql.Append(" values (");
            strSql.Append("@OrderCode,@Spu,@Sku,@Quantity,@UnitPrice,@PayUnitPrice,@TaxRate,@Commission,@PayAmount,@TaxAmount,@IsBearDuty,@RefundQuantity,@PayTaxAmountRMB,@PayTaxAmonutHKD,@VATTaxRate,@CBEBTaxRate,@PPATaxRate,@ConsumerTaxRate,@Huoli,@Coupon)");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@Spu", model.Spu);
            parameters.Append("@Sku", model.Sku);
            parameters.Append("@Quantity", model.Quantity);
            parameters.Append("@UnitPrice", model.UnitPrice);
            parameters.Append("@PayUnitPrice", model.PayUnitPrice);
            parameters.Append("@TaxRate", model.TaxRate);
            parameters.Append("@Commission", model.Commission);
            parameters.Append("@PayAmount", model.PayAmount);
            parameters.Append("@TaxAmount", model.TaxAmount);
            parameters.Append("@IsBearDuty", model.IsBearDuty);
            parameters.Append("@RefundQuantity", model.RefundQuantity);
            parameters.Append("@PayTaxAmountRMB", model.PayTaxAmonutRMB);
            parameters.Append("@PayTaxAmonutHKD", model.PayTaxAmonutHKD);
            parameters.Append("@VATTaxRate", model.VATTaxRate);
            parameters.Append("@CBEBTaxRate", model.CBEBTaxRate);
            parameters.Append("@PPATaxRate", model.PPATaxRate);
            parameters.Append("@ConsumerTaxRate", model.ConsumerTaxRate);
            parameters.Append("@Huoli", model.Huoli);
            parameters.Append("@Coupon", model.GiftCard);
            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void Update(OrderProductsEntity model, Database db, DbTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE OrderProducts ");
            strSql.Append("SET TaxRate=@TaxRate,TaxAmount=@TaxAmount ");
            strSql.Append("WHERE Spu=@Spu AND Sku=@Sku AND OrderCode=@OrderCode ");
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@Spu", model.Spu);
            parameters.Append("@Sku", model.Sku);
            parameters.Append("@TaxRate", model.TaxRate);
            parameters.Append("@TaxAmount", model.TaxAmount);

            db.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters, tran);
        }

        /// <summary>
        /// 获取订单商品列表
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public IList<OrderProductsEntity> GetOrderProductsByCode(string orderCode)
        {
            const string sql = @"SELECT    OrderProducts.OrderCode, OrderProducts.Spu, OrderProducts.Sku, OrderProducts.Quantity, 
                                           OrderProducts.UnitPrice, OrderProducts.PayUnitPrice,OrderProducts.TaxRate, 
                                           OrderProducts.PayAmount, OrderProducts.TaxAmount, OrderProducts.IsBearDuty, OrderProducts.PayTaxAmountRMB as PayTaxAmonutRMB,
                                           OrderProducts.PayTaxAmonutHKD,OrderProducts.VATTaxRate,OrderProducts.CBEBTaxRate,OrderProducts.PPATaxRate,OrderProducts.ConsumerTaxRate,
                                           OrderProducts.RefundQuantity,isnull(OrderProducts.SFQty,0) as SFQty,isnull(OrderProducts.MQty,0) as MQty 
                                FROM       OrderProducts (NOLOCK) 
                                WHERE       OrderProducts.OrderCode=@OrderCode ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);

            return DbSFO2ORead.ExecuteSqlList<OrderProductsEntity>(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public IList<OrderProductsEntity> productList(string orderCode)
        {
                const string sql = @"SELECT     op.Sku AS Sku , op.VATTaxRate,op.CBEBTaxRate,op.ConsumerTaxRate,op.PPATaxRate

                               FROM       OrderProducts op (NOLOCK) LEFT OUTER JOIN SkuCustomsReport (NOLOCK) 
                                          ON op.Sku = SkuCustomsReport.Sku   WHERE  op.OrderCode =@OrderCode";
                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", orderCode);

                return DbSFO2ORead.ExecuteSqlList<OrderProductsEntity>(sql, parameters);
        }

        /// <summary>
        /// 获取购物车列表(选择提交订单的)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="salesTerritory"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public IList<ShoppingCartItemEntity> GetOrderProductsByCode(string orderCode, int salesTerritory, int language)
        {
            string sql = @"  SELECT p.Id AS ProductId, p.Spu, p.SupplierId, p.Name,p.IsDutyOnSeller,
							p.MinPrice, p.LanguageVersion,
                            s.Sku, s.Price,   s.CreateTime, s.AuditTime,
                            s.[Status], s.IsOnSaled,
                            s.MainDicValue,s.SubDicValue ,
                            s.MainValue,s.SubValue,
                            sk.ForOrderQty 
                            ,sc.UnitPrice AS CartUnitPrice
                            , sc.Quantity AS CartQuantity
                            , sc.TaxRate/100 AS CartTaxRate
                            ,sc.TaxAmount AS CartTaxAmount
                            , 0 AS CartExchangeRate
                            ,'' as ShoppingCartId
                            ,1 as IsChecked
                            ,pim.ImagePath AS ImagePath
                            ,s.ReportStatus
                            ,s.IsCrossBorderEBTax
                            ,s.PPATaxRate/100 AS PPATaxRate
                            ,s.CBEBTaxRate/100 AS CBEBTaxRate
                            ,s.ConsumerTaxRate/100 AS ConsumerTaxRate
                            ,s.VATTaxRate/100 AS VATTaxRate
                            from OrderProducts  sc (NOLOCK)
                            INNER JOIN skuinfo s ON s.Sku=sc.Sku 
                            INNER JOIN  productInfo p (NOLOCK) ON p.id=s.SpuId AND (p.SalesTerritory=@SalesTerritory or p.SalesTerritory=3)
                            INNER JOIN stock sk ON s.Sku=sk.Sku
                            LEFT JOIN ProductImage AS pim ON pim.spu=p.spu AND pim.sortValue=1
                            WHERE p.LanguageVersion=@LanguageVersion  AND sc.OrderCode=@OrderCode ";
            try
            {

                var parameters = DbSFO2ORead.CreateParameterCollection();
                parameters.Append("@OrderCode", orderCode);
                parameters.Append("@SalesTerritory", salesTerritory);
                parameters.Append("@LanguageVersion", language);

                return DbSFO2ORead.ExecuteSqlList<ShoppingCartItemEntity>(sql, parameters);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new List<ShoppingCartItemEntity>();
            }
        }
        /// <summary>
        /// 更新orderProduct表
        /// </summary>
        public bool UpdateOrderProduct(OrderProductsEntity model)
        {
            string strSql = " UPDATE OrderProducts SET SFQty =@SFQty,MQty =@MQty WHERE OrderCode=@OrderCode AND Spu=@Spu AND Sku=@Sku ";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", model.OrderCode);
            parameters.Append("@Spu", model.Spu);
            parameters.Append("@MQty", model.MQty);
            parameters.Append("@SFQty", model.SFQty);
            parameters.Append("@Sku", model.Sku);
            return DbSFO2OMain.ExecuteSqlNonQuery(strSql, parameters) > 0;
        }
    }
}
