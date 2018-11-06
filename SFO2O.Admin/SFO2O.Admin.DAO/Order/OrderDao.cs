using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Admin.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Admin.ViewModel.Order;
using SFO2O.Admin.ViewModel;
using SFO2O.Admin.Common;
using System.Data;

namespace SFO2O.Admin.DAO.Order
{
    public class OrderDao : BaseDao
    {
        /// <summary>
        /// 根据订单号获取订单信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public OrderInfo getOrderInfo(string orderCode)
        {
            var sql = @"
SELECT	oi.OrderCode,
		oi.UserId,
		oi.OrderStatus,
		oi.TotalAmount,
		oi.Freight,
		oi.ProductTotalAmount,
		oi.CustomsDuties,
		oi.PaidAmount,
		oi.ExchangeRate,
		oi.PayType,
		oi.Receiver,
		oi.Phone,
		oi.PassPortType,
		oi.PassPortNum,
		oi.ReceiptAddress,
		oi.ReceiptPostalCode,
		oi.ReceiptRegion,
		a.AreaName,
		oi.ReceiptCity,
		ci.CityName,
		oi.ReceiptProvince,
		p.ProvinceName,
		oi.ReceiptCountry,
		oi.ShippingMethod,
		oi.CreateTime,
		oi.PayTime,
		oi.DeliveryTime,
		oi.ArrivalTime,
		oi.OrderCompletionTime,
		oi.CancelReason,
		oi.Remark,
        BuyerName = c.UserName
FROM	OrderInfo oi(NOLOCK)
		INNER JOIN Customer c(NOLOCK) ON c.ID = oi.UserId
		INNER JOIN Province p(NOLOCK) ON oi.ReceiptProvince = p.ProvinceId AND p.LanguageVersion = 2
		INNER JOIN City ci(NOLOCK) ON oi.ReceiptCity = ci.CityId AND ci.LanguageVersion = 2
		INNER JOIN Area a(NOLOCK) ON oi.ReceiptRegion = a.AreaId AND a.LanguageVersion = 2
WHERE	oi.OrderCode = @OrderCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return db.ExecuteSqlFirst<OrderInfo>(sql, parameters);
        }

        public bool UpdateOrderStatus(int orderStatus, string orderCode)
        {
            var sql = @"update OrderInfo set OrderStatus=@orderStatus where OrderCode=@orderCode;";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@orderStatus", orderStatus);
            parameters.Append("@orderCode", orderCode);
            return db.ExecuteNonQuery(CommandType.Text,sql, parameters)>0;
        }

        public OrderInfoModel GetOrderMainInfo(string orderCode)
        {
            const string sql = @"
               SELECT o.OrderCode,o.UserId,c.UserName,c.Mobile,c.NickName,c.Email,o.Receiver,o.ReceiptAddress,o.PayTime,o.DeliveryTime
                ,o.Phone,o.ShippingMethod,o.CreateTime,o.OrderCompletionTime,o.OrderStatus,o.ExchangeRate,o.PaidAmount,o.TotalAmount,o.CustomsDuties,o.ProductTotalAmount
                ,CASE o.PayType WHEN 1 THEN '易票聯' WHEN 2 THEN '電匯' WHEN 3 THEN '支付寶' WHEN 4 THEN '微信支付' END AS PayType
                ,op.PayCode,ct.CityName,pv.ProvinceName,ar.AreaName
                FROM OrderInfo o(NOLOCK)
                INNER JOIN customer c(NOLOCK) on c.ID=o.UserId
                LEFT JOIN OrderPayment(NOLOCK) op 
                ON op.OrderCode=o.OrderCode AND op.PayStatus=2
                LEFT JOIN City AS ct 
                ON ct.CityId=o.ReceiptCity AND ct.LanguageVersion=2
		        LEFT JOIN Province AS pv 
                ON pv.ProvinceId=o.ReceiptProvince  AND pv.LanguageVersion=2
		        LEFT JOIN Area AS ar 
                ON ar.AreaId=o.ReceiptRegion AND ar.LanguageVersion=2

                WHERE o.OrderCode=@OrderCode
            ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return db.ExecuteSqlFirst<OrderInfoModel>(sql, parameters);
        }

        public List<OrderDetailModel> GetOrderDetail(string orderCode)
        {
            const string sql = @"
               WITH IMGS AS
                (
	                SELECT spu,ImagePath 
					FROM (
						SELECT rownum = ROW_NUMBER() OVER (PARTITION BY spu ORDER BY sortValue), spu,ImagePath 
						FROM 	ProductImage 
						WHERE	ImageType = 1
					)b
					WHERE rownum<2
                )
                SELECT si.BarCode,op.spu,op.sku,p.Name,op.PayUnitPrice,op.PayAmount,op.Quantity,TaxAmount,s.CompanyName,oi.TotalAmount,oi.ExchangeRate,op.RefundQuantity 
                ,si.MainDicValue,si.MainValue,SubDicValue,SubValue,IMGS.ImagePath,ISNULL(opr.PromotionPrice,0) PromotionAmount,ISNULL(opr.OriginalPrice,0) OriginalPrice
                FROM OrderProducts(NOLOCK) AS op
                INNER JOIN OrderInfo(NOLOCK) AS oi
                ON oi.OrderCode = op.OrderCode
                INNER JOIN SkuInfo(NOLOCK) AS si
                ON si.Sku=op.Sku
                INNER JOIN ProductInfo(NOLOCK) AS p
                ON p.id=si.SpuId AND p.LanguageVersion=2
                INNER JOIN Supplier(NOLOCK) AS s
                ON s.SupplierID=p.SupplierId
                INNER JOIN IMGS
                ON IMGS.Spu = op.Spu
                LEFT JOIN OrderPromotions opr ON opr.OrderCode=op.OrderCode AND opr.Sku=si.Sku
                WHERE op.OrderCode=@OrderCode
            ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return db.ExecuteSqlList<OrderDetailModel>(sql, parameters).ToList();
        }

        public OrderLogisticsModel GetOrderExpress(String orderCode)
        {
            String sql = "SELECT TOP 1 ExpressCompany,ExpressList FROM OrderExpress(NOLOCK) WHERE OrderCode=@OrderCode";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return db.ExecuteSqlFirst<OrderLogisticsModel>(sql, parameters);
        }

        public List<OrderLogisticsModel> GetOrderLogistics(string orderCode)
        {
            const string sql = @"
                SELECT ol.OrderCode,oe.ExpressCompany,oe.ExpressList,ol.Id,ol.LogisticsTime,ol.Remark,ol.Status,ol.CreateTime 
                FROM  OrderExpress(NOLOCK) AS  oe
                INNER JOIN OrderLogistics(NOLOCK) AS ol
                ON oe.OrderCode=ol.OrderCode 
                WHERE oe.OrderCode=@OrderCode
                ORDER BY ol.CreateTime DESC
            ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            return db.ExecuteSqlList<OrderLogisticsModel>(sql, parameters).ToList();
        }

        public OrderListTotalModel GetOrderCount(OrderListQueryModel query)
        {
            const string sql = @"
                SELECT COUNT(OrderCode) AS OrderCount,SUM(Quantity) AS SkuCount
                ,SUM(PaidAmount) AS PaidAmount
                ,SUM(CustomsDuties) AS CustomsDuties
                ,MIN(ExchangeRate) ExchangeRate
                FROM
                (
	                SELECT o.OrderCode,o.CustomsDuties,o.ExchangeRate,SUM(op.Quantity) AS Quantity
	                ,SUM(op.UnitPrice*op.Quantity) AS PaidAmount
	                FROM OrderInfo o(NOLOCK)
	                INNER JOIN OrderProducts op(NOLOCK) on o.OrderCode=op.OrderCode
	                INNER JOIN customer c(NOLOCK) on c.ID=o.UserId
	                INNER JOIN SkuInfo s(NOLOCK) on s.Sku= op.Sku 
	                INNER JOIN ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=1
                    WHERE 1=1 {0} {1}
	                GROUP BY o.OrderCode,o.CustomsDuties,o.ExchangeRate
                ) t";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            var recCountry = "";
            if (query.CountryCode > 0)
            {
                parameters.Append("ReceiptCountry", query.CountryCode);
                recCountry = "AND ReceiptCountry=@ReceiptCountry";
            }

            var conditions = OrderListConditions(query, ref parameters);

            return db.ExecuteSqlFirst<OrderListTotalModel>(String.Format(sql, recCountry, conditions), parameters);
        }

        public PageOf<OrderListModel> GetOrderList(OrderListQueryModel query, int pageSize, int pageIndex)
        {
            const string sql = @"
                WITH IMGS AS
                (
	                SELECT spu,ImagePath 
					FROM (
						SELECT rownum = ROW_NUMBER() OVER (PARTITION BY spu ORDER BY sortValue), spu,ImagePath 
						FROM 	ProductImage 
						WHERE	ImageType = 1
					)b
					WHERE rownum<2
                ),
                orderMain AS
                (
                    SELECT * FROM (
		                    SELECT ROW_NUMBER() OVER (ORDER BY a.Createtime desc) AS RowNumber,* FROM (
                            SELECT distinct o.OrderCode,o.CreateTime,c.UserName,o.OrderStatus,o.ExchangeRate,SUM(op.RefundQuantity) AS RefundQuantity,CustomsDuties
                            FROM OrderInfo o(NOLOCK)
                            INNER JOIN OrderProducts op(NOLOCK) on o.OrderCode=op.OrderCode
                            INNER JOIN customer c(NOLOCK) on c.ID=o.UserId
                            INNER JOIN SkuInfo s(NOLOCK) on s.Sku= op.Sku 
                            INNER JOIN ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=1
                            WHERE 1=1 {0} {1}
                            GROUP BY o.OrderCode,o.CreateTime,c.UserName,o.OrderStatus,o.ExchangeRate,o.CustomsDuties
                    ) a)b WHERE b.RowNumber>(@PageIndex-1)*@PageSize AND b.RowNumber <= @PageIndex*@PageSize 
                    
                )
                SELECT om.OrderCode,om.CreateTime,om.UserName,p.Name,si.MainDicValue,si.MainValue,si.SubDicValue,si.SubValue,om.OrderStatus,op.Sku,op.Spu,op.PayUnitPrice,op.Quantity,op.IsBearDuty,s.CompanyName,op.TaxAmount,op.IsBearDuty,om.CustomsDuties
                ,SUM(op.PayUnitPrice*op.Quantity) AS TotalAmount,SUM(op.UnitPrice*op.Quantity) AS TotalAmount1,om.ExchangeRate,IMGS.ImagePath
                FROM OrderProducts(NOLOCK) AS op
                INNER JOIN orderMain AS om ON op.OrderCode = om.OrderCode
                INNER JOIN SkuInfo si(NOLOCK) ON si.Sku= op.Sku 
                INNER JOIN ProductInfo p(NOLOCK) ON p.Id=si.SpuId and p.LanguageVersion=1
                INNER JOIN Supplier(NOLOCK) AS s ON s.SupplierID = p.SupplierId
                LEFT JOIN IMGS ON IMGS.Spu=op.Spu
                WHERE 1=1 {2}
                GROUP BY om.OrderCode,om.CreateTime,om.UserName,op.Sku,op.Spu,op.PayUnitPrice,op.Quantity,s.CompanyName,p.Name,si.MainDicValue,si.MainValue,si.SubDicValue,si.SubValue,om.OrderStatus,om.ExchangeRate,IMGS.ImagePath,op.TaxAmount,op.IsBearDuty,om.CustomsDuties
                ORDER BY om.CreateTime DESC
                
                SELECT COUNT(DISTINCT o.ordercode)
                FROM OrderInfo o(NOLOCK)
                INNER JOIN OrderProducts op(NOLOCK) on o.OrderCode=op.OrderCode
                INNER JOIN customer c(NOLOCK) on c.ID=o.UserId
                INNER JOIN SkuInfo s(NOLOCK) on s.Sku= op.Sku 
                INNER JOIN ProductInfo p(NOLOCK) on p.Id=s.SpuId and p.LanguageVersion=1
                WHERE 1=1 {0} {1}
                ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("PageSize", pageSize);
            parameters.Append("PageIndex", pageIndex);

            var recCountry = "";
            if (query.CountryCode > 0)
            {
                parameters.Append("ReceiptCountry", query.CountryCode);
                recCountry = "AND ReceiptCountry=@ReceiptCountry";
            }


            var conditions = OrderListConditions(query, ref parameters);
            var conditions1 = "";

            if (query.SellerId > 0)
            {
                conditions1 = " AND p.SupplierId=@SupplierId1";
                parameters.Append("SupplierId1", query.SellerId);
            }
            if (!String.IsNullOrWhiteSpace(query.SKU))
            {
                conditions1 += " AND op.Sku=@SKU1";
                parameters.Append("SKU1", query.SKU);
            }

            var finalSql = String.Format(sql, recCountry, conditions, conditions1);


            var ds = db.ExecuteSqlDataSet(finalSql, parameters);

            return new PageOf<OrderListModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                RowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]),
                Items = DataMapHelper.DataSetToList<OrderListModel>(ds)
            };
        }

        private string OrderListConditions(OrderListQueryModel query, ref ParameterCollection paras)
        {
            StringBuilder sb = new StringBuilder(1000);

            if (!String.IsNullOrWhiteSpace(query.OrderCode))
            {
                sb.Append(" AND o.OrderCode=@OrderCode");
                paras.Append("OrderCode", query.OrderCode);
            }
            else
            {
                sb.Append(" AND o.CreateTime>=@StartTime AND o.CreateTime<=@EndTime");
                paras.Append("StartTime", new DateTime(query.CreateTimeStart.Year, query.CreateTimeStart.Month, query.CreateTimeStart.Day, 0, 0, 0));
                paras.Append("EndTime", new DateTime(query.CreateTimeEnd.Year, query.CreateTimeEnd.Month, query.CreateTimeEnd.Day, 23, 59, 59, 999));

                if (query.OrderStatus != -2)
                {
                    sb.Append(" AND o.OrderStatus=@Status");
                    paras.Append("Status", query.OrderStatus);
                }

                if (!String.IsNullOrWhiteSpace(query.SKU))
                {
                    sb.Append(" AND op.Sku=@SKU");
                    paras.Append("SKU", query.SKU);
                }

                if (!String.IsNullOrWhiteSpace(query.BuyerAccount))
                {
                    sb.Append(" AND c.UserName=@UserName");
                    paras.Append("UserName", query.BuyerAccount);
                }

                if (query.SellerId > 0)
                {
                    sb.Append(" AND p.SupplierId=@SupplierId");
                    paras.Append("SupplierId", query.SellerId);
                }

                if (query.IsExcludeCloseOrder == 1)
                {
                    sb.Append(" AND o.OrderStatus NOT IN (5)");
                }
            }

            return sb.ToString();
        }

        public List<OrderStockOutModel> GetOrderStockOutInfos(DateTime startTime, DateTime endTime)
        {
            const string sql = @"
                SELECT oi.OrderCode,oi.Receiver as CompanyName,p.ProvinceName,c.CityName,a.AreaName,
                (p.ProvinceName +c.CityName + a.AreaName + oi.ReceiptAddress) as ReceiptAddress, (oi.TotalAmount / oi.ExchangeRate) as TotalAmountHK,
                op.UnitPrice,si.BarCode,pinfo.Name,si.MainValue,si.SubValue,op.Quantity,oi.Receiver,oi.Phone,oi.Phone as Mobile
                FROM OrderInfo(NOLOCK) oi
                INNER JOIN OrderProducts(NOLOCK) op
                    ON oi.OrderCode = op.OrderCode
                INNER JOIN SkuInfo(NOLOCK) AS si
                    ON si.Sku=op.Sku
                INNER JOIN ProductInfo(NOLOCK) AS pinfo
                    ON pinfo.id=si.SpuId AND pinfo.LanguageVersion=1
                INNER JOIN Province(NOLOCK) p
                    ON p.ProvinceId = oi.ReceiptProvince and p.LanguageVersion=1
                INNER JOIN City(NOLOCK) c 
                    ON c.CityId = oi.ReceiptCity AND c.LanguageVersion=1
                INNER JOIN Area(NOLOCK) a 
                    ON a.AreaId = oi.ReceiptRegion AND a.LanguageVersion=1
                WHERE oi.OrderStatus=1 AND  oi.PayTime is not null 
                    AND oi.PayTime BETWEEN @StartTime AND @EndTime ";

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("StartTime", startTime);
            parameters.Append("EndTime", endTime);

            return db.ExecuteSqlList<OrderStockOutModel>(sql, parameters).ToList();
        }
    }
}
