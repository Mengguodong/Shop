using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using SFO2O.Supplier.Models;

namespace SFO2O.Supplier.DAO.Order
{
    public class SettlementDAL : BaseDao
    {
        /// <summary>
        /// 获取结算单信息
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <returns></returns>
        public SettlementOrderInfo getOrderInfo(string settlementCode)
        {
            var sql = @"
SELECT  soi.SettlementCode,
		soi.CreateTime,
		OrderCode = ISNULL(soi.RefundCode,soi.OrderCode),
		s.CompanyName,
		soi.RmbProductTotalAmount,
		CASE WHEN soi.RmbProductRefundAmount>0 THEN soi.RmbProductTotalAmount ELSE 0 END RmbProductRefundAmount,
		soi.RmbSupplierBearDutyAmount,
		soi.RmbBearDutyAmount,
		soi.RmbOtherAmount,
		soi.RmbSettlementAmount,
        soi.ProductTotalAmount,
		CASE WHEN soi.ProductRefundAmount>0 THEN soi.ProductTotalAmount ELSE 0 END ProductRefundAmount,
		soi.SupplierBearDutyAmount,
		soi.BearDutyAmount,
		soi.OtherAmount,
		soi.SettlementAmount,
		soi.ExchangeRate,
		soi.SettlementStatus,
		soi.AuditTime,
		soi.SettlementTime,
        soi.TradeCode,
        ISNULL(PromotionAmount,0) PromotionAmount,
        ISNULL(SupplierPromotionDutyAmount,0) RmbPromotionDutyAmount,
        ISNULL(SupplierPromotionDutyAmount*soi.ExchangeRate,0) RmbSupplierPromotionDutyAmount,
        ISNULL(PromotionAmount - SupplierPromotionDutyAmount,0) PromotionDutyAmount,
        ISNULL((PromotionAmount - SupplierPromotionDutyAmount)*soi.ExchangeRate,0) RmbPromotionDutyAmount
FROM	SettlementOrderInfo soi(NOLOCK)
		INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = soi.SupplierId
        LEFT JOIN
        (
            SELECT sp.SettlementCode,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) PromotionAmount,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) SupplierPromotionDutyAmount
            FROM SettlementOrderProducts sp(NOLOCK)
            INNER JOIN SettlementOrderInfo s(NOLOCK) ON sp.SettlementCode = s.SettlementCode
            INNER JOIN OrderPromotions op ON op.OrderCode=s.OrderCode AND op.Sku=sp.Sku
            INNER JOIN Promotions p ON p.Id=op.PromotionId
            WHERE sp.Quantity>0 AND sp.SettlementCode = @SettlementCode
            GROUP BY sp.SettlementCode
        ) t ON t.SettlementCode = soi.SettlementCode
WHERE   soi.SettlementCode = @SettlementCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@SettlementCode", settlementCode);
            return db.ExecuteSqlFirst<SettlementOrderInfo>(sql, parameters);
        }

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <returns></returns>
        public List<SettlementProductInfo> getOrderProducts(string settlementCode)
        {
            var sql = @"
SELECT  sop.SettlementCode,
        sop.Spu,
        sop.Sku,
        sop.Quantity,
		sop.RmbUnitPrice,
        sop.UnitPrice,
        sop.TaxRate,
		sop.RmbAmount,
        sop.Amount,
		sop.RmbTaxAmount,
        sop.TaxAmount,
        sop.IsBearDuty,
        sop.RmbSettlementAmount,
        sop.SettlementAmount,
		si.MainDicValue,
		si.MainValue,
		si.SubDicValue,
		si.SubValue,
		ProductName = p.Name,
		pImg.ImagePath as ProductImagePath,
		si.BarCode,
        sop.Commission,
        ISNULL(PromotionAmount,0) PromotionAmount
FROM	SettlementOrderProducts sop(NOLOCK)		
        INNER JOIN ProductInfo p(NOLOCK) ON p.Spu = sop.Spu AND p.LanguageVersion = 2
		INNER JOIN SkuInfo si(NOLOCK) ON si.Spu = sop.Spu AND si.Sku = sop.Sku AND si.SpuId = p.Id		
		LEFT JOIN ProductImage pImg(NOLOCK) ON pImg.Spu = sop.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1
        LEFT JOIN
        (
            SELECT sp.SettlementCode,sp.Sku,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) PromotionAmount
            FROM SettlementOrderProducts sp(NOLOCK)
            INNER JOIN SettlementOrderInfo s(NOLOCK) ON sp.SettlementCode = s.SettlementCode
            INNER JOIN OrderPromotions op ON op.OrderCode=s.OrderCode AND op.Sku=sp.Sku
            INNER JOIN Promotions p ON p.Id=op.PromotionId
            WHERE sp.Quantity>0 AND sp.SettlementCode = @SettlementCode
            GROUP BY sp.SettlementCode,sp.Sku
        ) t ON t.SettlementCode = sop.SettlementCode AND t.Sku=sop.Sku 
WHERE   sop.SettlementCode = @SettlementCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@SettlementCode", settlementCode);
            return db.ExecuteSqlList<SettlementProductInfo>(sql, parameters).ToList();
        }


        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="pageDTO"></param>
        /// <returns></returns>
        public PageOf<SettlementOrderInfo> getSettlementList(SettlementQueryModel queryModel, PageDTO pageDTO)
        {
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();

            var sql = @"SELECT  soi.SettlementCode,
		            soi.CreateTime,
		            OrderCode = ISNULL(soi.RefundCode,soi.OrderCode),
                    soi.RmbProductTotalAmount,
		            CASE WHEN soi.RmbProductRefundAmount>0 THEN soi.RmbProductTotalAmount ELSE 0 END RmbProductRefundAmount,
		            soi.RmbSupplierBearDutyAmount,
		            soi.RmbBearDutyAmount,
		            soi.RmbOtherAmount,
		            soi.RmbSettlementAmount,
		            soi.ProductTotalAmount,
		            CASE WHEN soi.ProductRefundAmount>0 THEN soi.ProductTotalAmount ELSE 0 END ProductRefundAmount,
		            soi.SupplierBearDutyAmount,
		            soi.BearDutyAmount,
		            soi.OtherAmount,
		            soi.SettlementAmount,
		            soi.ExchangeRate,
		            soi.SettlementStatus,
                    ISNULL(t.PromotionAmount,0) PromotionAmount
            FROM	SettlementOrderInfo soi(NOLOCK)
            LEFT JOIN
            (
                SELECT sp.SettlementCode,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) PromotionAmount
                FROM SettlementOrderProducts sp(NOLOCK)
                INNER JOIN SettlementOrderInfo s(NOLOCK) ON sp.SettlementCode = s.SettlementCode
                INNER JOIN OrderPromotions op ON op.OrderCode=s.OrderCode AND op.Sku=sp.Sku
                INNER JOIN Promotions p ON p.Id=op.PromotionId
                WHERE sp.Quantity>0
                GROUP BY sp.SettlementCode
            ) t ON t.SettlementCode = soi.SettlementCode
            WHERE	1=1 and soi.SupplierId=@SupplierId {0}
            order by soi.CreateTime desc;

            SELECT  TotalCount = Count(soi.SettlementCode),		
		            ProductTotalAmount = ISNULL(SUM(ISNULL(soi.ProductTotalAmount,0)),0),
		            ProductRefundAmount = ISNULL(SUM(ISNULL(CASE WHEN soi.ProductRefundAmount>0 THEN soi.ProductTotalAmount ELSE 0 END,0)),0),
		            SupplierBearDutyAmount = ISNULL(SUM(ISNULL(soi.SupplierBearDutyAmount,0)),0),
		            BearDutyAmount = ISNULL(SUM(ISNULL(soi.BearDutyAmount,0)),0),
		            OtherAmount = ISNULL(SUM(ISNULL(soi.OtherAmount,0)),0),
		            SettlementAmount = ISNULL(SUM(ISNULL(soi.SettlementAmount,0)),0),
                    RmbProductTotalAmount = ISNULL(SUM(ISNULL(soi.RmbProductTotalAmount,0)),0),
		            RmbProductRefundAmount = ISNULL(SUM(ISNULL(CASE WHEN soi.RmbProductRefundAmount>0 THEN soi.RmbProductTotalAmount ELSE 0 END,0)),0),
		            RmbSupplierBearDutyAmount = ISNULL(SUM(ISNULL(soi.RmbSupplierBearDutyAmount,0)),0),
		            RmbBearDutyAmount = ISNULL(SUM(ISNULL(soi.RmbBearDutyAmount,0)),0),
		            RmbOtherAmount = ISNULL(SUM(ISNULL(soi.RmbOtherAmount,0)),0),
		            RmbSettlementAmount = ISNULL(SUM(ISNULL(soi.RmbSettlementAmount,0)),0),
                    PromotionAmount = ISNULL(SUM(ISNULL(t.PromotionAmount,0)),0)
            FROM	SettlementOrderInfo soi(NOLOCK)
		            INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = soi.SupplierId
            LEFT JOIN
            (
                SELECT sp.SettlementCode,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) PromotionAmount
                FROM SettlementOrderProducts sp(NOLOCK)
                INNER JOIN SettlementOrderInfo s(NOLOCK) ON sp.SettlementCode = s.SettlementCode
                INNER JOIN OrderPromotions op ON op.OrderCode=s.OrderCode AND op.Sku=sp.Sku
                INNER JOIN Promotions p ON p.Id=op.PromotionId
                WHERE sp.Quantity>0
                GROUP BY sp.SettlementCode
            ) t ON t.SettlementCode = soi.SettlementCode
            WHERE	1=1 and soi.SupplierId=@SupplierId {1}";

            var query = BindQuery(queryModel, parameters);
            sql = string.Format(sql, query, query);

            parameters.Append("@SupplierId", queryModel.SupplierId);
            parameters.Append("@PageIndex", pageDTO.PageIndex);
            parameters.Append("@PageSize", pageDTO.PageSize);

            var ds = db.ExecuteSqlDataSet(sql, parameters);

            PageOf<SettlementOrderInfo> result = new PageOf<SettlementOrderInfo>();
            result.Items = ds.Tables[0].ToList<SettlementOrderInfo>();
            result.PageIndex = pageDTO.PageIndex;
            result.PageSize = pageDTO.PageSize;
            result.TotalObject = ds.Tables[1].ToList<SettlementOrderInfo>().First();
            result.Total = result.TotalObject.TotalCount;

            return result;
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        private string BindQuery(SettlementQueryModel queryInfo, ParameterCollection dbParameters)
        {
            var stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(queryInfo.SettlementCode))
            {
                stringBuilder.Append(" and soi.SettlementCode=@SettlementCode");
                dbParameters.Append("SettlementCode", queryInfo.SettlementCode);
            }
            else
            {

                if (!string.IsNullOrEmpty(queryInfo.OrderCode))
                {
                    stringBuilder.Append(" and (soi.OrderCode=@OrderCode or soi.RefundCode=@OrderCode)");
                    dbParameters.Append("OrderCode", queryInfo.OrderCode);
                }
                if (queryInfo.SettlementStatus != -0)
                {
                    stringBuilder.Append(" and soi.SettlementStatus=@SettlementStatus");
                    dbParameters.Append("SettlementStatus", queryInfo.SettlementStatus);
                }

                if (queryInfo.StartTime != null)
                {
                    stringBuilder.Append(" and soi.CreateTime> @StartTime");
                    dbParameters.Append("StartTime", queryInfo.StartTime.ToString("yyyy-MM-dd 00:00:00"));
                }
                if (queryInfo.EndTime != null)
                {
                    stringBuilder.Append(" and soi.CreateTime< @EndTime");
                    dbParameters.Append("EndTime", queryInfo.EndTime.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
                }
            }

            return stringBuilder.ToString();
        }


        public OrderPaymentInfo getPaymentInfo(string tradeCode)
        {
            var sql = @"
SELECT  op.PayCode,
        op.TradeCode,
        op.UserId,
        op.OrderType,
        op.OrderCode,
        op.PayAmount,
        op.PaidAmount,
        op.PayPlatform,
        op.PayType,
        op.PayStatus,
        op.PayTerminal,
        op.PayCompleteTime,
        op.PayBackRemark,
        op.Remark,
        op.CreateTime,
        op.CreateBy
FROM    OrderPayment op(NOLOCK)
WHERE   op.TradeCode = @TradeCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@TradeCode", tradeCode);
            return db.ExecuteSqlFirst<OrderPaymentInfo>(sql, parameters);
        }
    }
}
