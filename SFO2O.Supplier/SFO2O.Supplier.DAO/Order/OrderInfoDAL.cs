
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Supplier.Common;
using SFO2O.Supplier.Models;

namespace SFO2O.Supplier.DAO.Order
{
    public class OrderInfoDAL : BaseDao
    {
        public Models.PageOf<Models.OrderListInfoModel> GetOrderList(Models.OrderQueryInfo queryInfo, Models.PageDTO page, LanguageEnum languageVersion, int receiptCountry)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            var sql = @"(  select o.OrderCode,s.MainDicValue,s.SubDicValue,s.MainValue,s.SubValue,o.CreateTime,p.Name,op.UnitPrice,op.PayUnitPrice,op.Quantity,o.TotalAmount,
                              o.OrderStatus,o.ExchangeRate,p.Spu,pImg.ImagePath as ProductImagePath,op.Sku,op.RefundQuantity,
                              (select top 1 expresslist from OrderExpress where OrderCode = o.OrderCode) as  expresslist,
                               (select top 1 ExpressCompany from OrderExpress where OrderCode = o.OrderCode) as  ExpressCompany,pr.ProvinceName+' | '+ci.CityName+' | '+ ar.AreaName+' | '+ o.ReceiptAddress AS ReceiptAddress,o.Phone,o.Receiver 
                              from OrderInfo o(NOLOCK)
                              inner join OrderProducts op(NOLOCK) on o.OrderCode= op.OrderCode
                              inner join SkuInfo s(NOLOCK) on s.Sku= op.Sku 
                              inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
	                          left join ProductImage pImg(NOLOCK) ON pImg.Spu = p.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1 
                              LEFT JOIN  city(NOLOCK) AS ci ON ci.CityId=o.ReceiptCity AND ci.LanguageVersion = 1
	                          LEFT JOIN Province(NOLOCK) AS pr ON pr.ProvinceId = o.ReceiptProvince AND pr.LanguageVersion = 1
	                          LEFT JOIN Area AS ar ON ar.areaid=o.ReceiptRegion AND ar.LanguageVersion = 1	
                              where p.SupplierId= @SupplierId ";
            if (receiptCountry != 0)
            {
                sql = sql + " and o.ReceiptCountry=@ReceiptCountry";
            }

            var query = BindQuery(queryInfo, parameters);

            sql = sql + query;

            sql += ") pp ";

            string SQL = string.Format(@" select * from (select ROW_NUMBER() OVER(order by pp.CreateTime desc) AS RowNum,* from {0}
										) as A where A.RowNum BETWEEN (@PageIndex-1)* @PageSize+1 AND @PageIndex*@PageSize ORDER BY RowNum;",
                                  sql);

            SQL += string.Format(@" SELECT COUNT(1),COUNT(distinct pp.OrderCode) FROM {0};", sql);

            parameters.Append("LanguageVersion", (int)languageVersion);
            parameters.Append("SupplierId", queryInfo.SupplierId);
            parameters.Append("ReceiptCountry", receiptCountry);
            parameters.Append("PageIndex", page.PageIndex);
            parameters.Append("PageSize", page.PageSize);

            DataSet ds = db.ExecuteSqlDataSet(SQL, parameters);

            return new PageOf<OrderListInfoModel>()
            {
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                Total = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][1]),
                Items = DataMapHelper.DataSetToList<OrderListInfoModel>(ds)
            };
        }

        public OrderTotalInfo GetOrderTotal(OrderQueryInfo queryInfo, LanguageEnum languageEnum, int receiptCountry)
        {
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            var sql = @"  select COUNT(distinct o.UserId) as BuyerCount,COUNT(distinct o.OrderCode) OrderCount,COUNT(op.Quantity) ProductCount,SUM(op.UnitPrice * op.Quantity) OrderAmountTotal,sum(o.Freight) as FreightTotal
                          from OrderInfo o(NOLOCK)
                          inner join OrderProducts op(NOLOCK) on o.OrderCode= op.OrderCode
                          inner join SkuInfo s(NOLOCK) on s.Sku= op.Sku 
                          inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
                          where p.SupplierId= @SupplierId";
            if (receiptCountry != 0)
            {
                sql = sql + " and o.ReceiptCountry=@ReceiptCountry";
                parameters.Append("ReceiptCountry", receiptCountry);
            }
            var query = BindQuery(queryInfo, parameters);

            sql = sql + query;

            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("SupplierId", queryInfo.SupplierId);

            return db.ExecuteSqlFirst<OrderTotalInfo>(sql, parameters);
        }

        /// <summary>
        /// 构造查询条件
        /// </summary>
        private string BindQuery(OrderQueryInfo queryInfo, ParameterCollection dbParameters)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(queryInfo.orderCode))
            {
                stringBuilder.Append(" and  o.OrderCode=@OrderCode");
                dbParameters.Append("OrderCode", queryInfo.orderCode);
            }
            else
            {
                if (queryInfo.orderSatus != -1)
                {
                    stringBuilder.Append(" and  o.OrderStatus=@OrderStatus");
                    dbParameters.Append("OrderStatus", queryInfo.orderSatus);
                }
                if (queryInfo.startTime != null)
                {
                    stringBuilder.Append(" and  o.CreateTime> @StartTime");
                    dbParameters.Append("StartTime", queryInfo.startTime.ToString("yyyy-MM-dd 00:00:00"));
                }
                if (queryInfo.endTime != null)
                {
                    stringBuilder.Append(" and  o.CreateTime< @EndTime");
                    dbParameters.Append("EndTime", queryInfo.endTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
                }
            }

            return stringBuilder.ToString();
        }

        public IList<Models.OrderProduct> GetOrderProducts(string orderCode, int supplierId, LanguageEnum languageEnum)
        {
            var sql = @"select o.OrderCode,s.Sku,p.Spu,s.MainDicValue,s.SubDicValue,s.MainValue,s.SubValue,p.Name,s.BarCode,op.PayUnitPrice,op.TaxAmount,op.Quantity,op.UnitPrice,
                        o.OrderStatus,o.ExchangeRate,pImg.ImagePath as ProductImagePath,op.RefundQuantity,ISNULL(opr.OriginalPrice - opr.PromotionPrice,0) PromotionPrice
                        from OrderInfo o(NOLOCK)
                        inner join OrderProducts op(NOLOCK) on o.OrderCode= op.OrderCode
                        inner join SkuInfo s(NOLOCK) on s.Sku= op.Sku 
                        inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
                        inner join customer c(NOLOCK) on c.ID=o.UserId
                        left join ProductImage pImg(NOLOCK) ON pImg.Spu = p.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1
                        LEFT JOIN OrderPromotions opr ON opr.OrderCode=op.OrderCode AND opr.Sku=s.Sku
                        where o.OrderCode=@OrderCode and p.SupplierId= @SupplierId";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("SupplierId", supplierId);
            parameters.Append("OrderCode", orderCode);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            return DataMapHelper.DataSetToList<OrderProduct>(ds);
        }

        public Models.Order GetOrderModel(string orderCode, int supplierId, LanguageEnum languageEnum)
        {

            var sql = @"select o.OrderCode,o.UserId,c.UserName,c.Mobile,c.NickName,c.Email,o.Receiver,o.ReceiptAddress,o.PayTime,o.DeliveryTime,o.ArrivalTime,o.OrderCompletionTime,o.Phone,o.ShippingMethod,o.CreateTime,o.OrderStatus,o.ExchangeRate,
                        sum(op.PayAmount) as PaidAmount,sum(op.TaxAmount) as CustomsDuties,sum(op.PayUnitPrice*op.Quantity) as TotalAmount,opay.PayCode,(case when opay.PayPlatform = 1 then '易票联' else '' end) as PayPlatform
                        ,pro.ProvinceName,ci.CityName,a.AreaName,(select top 1 expresslist from OrderExpress where OrderCode = o.OrderCode) as  expresslist,(select top 1 ExpressCompany from OrderExpress where OrderCode = o.OrderCode) as  ExpressCompany
                        from OrderInfo o(NOLOCK)
                        left join OrderPayment opay(NOLOCK) on o.OrderCode=opay.OrderCode and opay.PayStatus=2
                        inner join Province pro(NOLOCK) on pro.ProvinceId=o.ReceiptProvince and pro.LanguageVersion=@LanguageVersion
                        inner join City ci(NOLOCK) on ci.CityId= o.ReceiptCity and ci.LanguageVersion=@LanguageVersion
                        inner join Area a(NOLOCK) on a.AreaId= o.ReceiptRegion and a.LanguageVersion=@LanguageVersion
                        inner join OrderProducts op(NOLOCK) on o.OrderCode=op.OrderCode
                        inner join customer c(NOLOCK) on c.ID=o.UserId
                        inner join SkuInfo s(NOLOCK) on s.Sku= op.Sku 
                        inner join ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=@LanguageVersion
                        where o.OrderCode=@OrderCode and p.SupplierId=@SupplierId
                        group by o.OrderCode,o.UserId,c.UserName,c.Mobile,c.NickName,c.Email,o.Receiver,o.ReceiptAddress,o.PayTime,o.DeliveryTime,o.ArrivalTime,o.Phone,o.ShippingMethod,o.OrderCompletionTime,
                        o.CreateTime,o.OrderStatus,o.ExchangeRate,opay.PayCode,opay.PayPlatform,pro.ProvinceName,ci.CityName,a.AreaName";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("LanguageVersion", (int)languageEnum);
            parameters.Append("SupplierId", supplierId);
            parameters.Append("OrderCode", orderCode);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            return DataMapHelper.DataSetToList<Models.Order>(ds)[0];
        }

        public decimal GetOrderTotalTaxRate(string orderCode, int supplierId)
        {
            var sql = @"DECLARE @totalAmount decimal(18,2)
                        SELECT @totalAmount = CustomsDuties FROM OrderInfo(NOLOCK) WHERE  OrderCode=@OrderCode 

                        IF(@totalAmount >0)
                        BEGIN
	                        SELECT @totalAmount = SUM(TaxAmount) FROM OrderProducts(NOLOCK)  op
	                        INNER JOIN ProductInfo(NOLOCK) p ON op.Spu=p.Spu AND p.LanguageVersion=2 
	                        WHERE OrderCode=@OrderCode AND IsBearDuty=0 AND p.SupplierId=@SupplierId

                        END

                        SELECT @totalAmount";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            parameters.Append("SupplierId", supplierId);
            parameters.Append("OrderCode", orderCode);

            var ds = db.ExecuteSqlDataSet(sql, parameters);
            var result = decimal.Zero;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                result = ds.Tables[0].Rows[0][0] == DBNull.Value ? decimal.Zero : Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString());
            }

            return result;
        }

        public IList<OrderLogistics> GetOrderLogistics(string orderCode, int supplierId)
        {
            var sql = @"select ol.OrderCode,oe.ExpressCompany,oe.ExpressList,ol.Id,ol.LogisticsTime,ol.Remark,ol.Status,ol.CreateTime from  
                        OrderExpress oe(NOLOCK) inner join OrderLogistics ol(NOLOCK) on oe.OrderCode=ol.OrderCode 
                        where oe.OrderCode=@OrderCode
                        order by ol.CreateTime desc ";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("OrderCode", orderCode);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            return DataMapHelper.DataSetToList<OrderLogistics>(ds);
        }

        /// <summary>
        /// 商家确认发货，更新订单信息
        /// </summary>
        /// <param name="expressCompany"></param>
        /// <param name="expressCode"></param>
        /// <param name="freight"></param>
        /// <returns></returns>
        public bool ComfirmSendGoods(string orderCode, string expressCompany, string expressCode, string freight)
        {
            var db = DbSFO2OMain;
            var connection = db.CreateConnection();
            connection.Open();

            bool result = false;
            using (var tran = connection.BeginTransaction())
            {
                try
                {
                    string sql_update_orderinfo = "update OrderInfo set OrderStatus=@OrderStatus,DeliveryTime=getdate(),Freight=@Freight where OrderCode=@OrderCode ";
                    string sql_insert_orderExpress = "insert into OrderExpress values(@orderCode,@ExpressCompany,@ExpressList,1)";

                    var parameters_orderinfo = db.CreateParameterCollection();
                    var parameters_orderExpress = db.CreateParameterCollection();

                    parameters_orderinfo.Append("OrderStatus", 2);
                    parameters_orderinfo.Append("OrderCode", orderCode);
                    parameters_orderinfo.Append("Freight", freight);

                    parameters_orderExpress.Append("OrderCode", orderCode);
                    parameters_orderExpress.Append("ExpressCompany", expressCompany);
                    parameters_orderExpress.Append("ExpressList", expressCode);

                    bool update_result = db.ExecuteNonQuery(CommandType.Text, sql_update_orderinfo, parameters_orderinfo, tran) > 0;
                    bool insert_result = db.ExecuteNonQuery(CommandType.Text, sql_insert_orderExpress, parameters_orderExpress, tran) > 0;

                    result = update_result && insert_result;
                    if (result)
                    {
                        tran.Commit();
                        connection.Close();
                        connection.Dispose();
                    }
                    else
                    {
                        tran.Rollback();
                        connection.Close();
                        connection.Dispose();
                    }
                }
                catch (Exception ext)
                {
                    tran.Rollback();
                    connection.Close();
                    connection.Dispose();
                }
                return result;
            }
        }
        /// <summary>
        /// 商家编辑订单状态（运单号、物流公司）
        /// </summary>
        /// <param name="orderCode"></param>
        /// <param name="expressCompany"></param>
        /// <param name="expressCode"></param>
        /// <param name="freight"></param>
        /// <returns></returns>
        public bool UpdateOrderInfo(string orderCode, string expressCompany, string expressCode)
        {
            try
            {
                string sql_update_orderExpress = "UPDATE [OrderExpress] SET [ExpressCompany] = @ExpressCompany,[ExpressList] = @ExpressList WHERE [OrderCode] = @OrderCode";

                var db = DbSFO2OMain;
                var parameters_orderExpress = db.CreateParameterCollection();
                parameters_orderExpress.Append("OrderCode", orderCode);
                parameters_orderExpress.Append("ExpressCompany", expressCompany);
                parameters_orderExpress.Append("ExpressList", expressCode);

                bool result = db.ExecuteNonQuery(CommandType.Text, sql_update_orderExpress, parameters_orderExpress) > 0;
                return result;
            }
            catch (Exception ext)
            {
                return false;
            }
        }

        public OrderExpressEntity GetOrderExpress(string orderId)
        {
            var sql = @"select * from OrderExpress where OrderCode=@OrderCode";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();

            parameters.Append("OrderCode", orderId);

            var ds = db.ExecuteSqlFirst<OrderExpressEntity>(sql, parameters);

            return ds;
            //var ds = db.ExecuteSqlDataSet(sql, parameters);

           // return DataMapHelper.DataSetToList<OrderExpressEntity>(ds)[0];
        }
    }
}
