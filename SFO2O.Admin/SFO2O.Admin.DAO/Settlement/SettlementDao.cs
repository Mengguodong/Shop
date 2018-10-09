using SFO2O.Admin.Models.Settlement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Admin.Models;
using SFO2O.Admin.ViewModel.Settlement;
using SFO2O.Admin.Models.Payment;
using System.Data.Common;

namespace SFO2O.Admin.DAO.Settlement
{
    public class SettlementDao : BaseDao
    {
        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PagedList<SettlementOrderInfo> getOrderList(SettlementQueryModel queryModel, PagingModel pModel)
        {
            var sql = @"
SELECT  *
FROM    (
            SELECT  ROW_NUMBER() OVER (ORDER BY soi.Createtime DESC) AS RowNumber,
                    soi.SettlementCode,
		            soi.CreateTime,
                    soi.SupplierId,
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
                    DutyAmount = CASE WHEN soi.SettlementStatus=1 THEN sop.TaxAmount ELSE 0.00 END,
					RmbDutyAmount = CASE WHEN soi.SettlementStatus=1 THEN sop.RmbTaxAmount ELSE 0.00 END,
					ProductSettlementAmount = CASE WHEN soi.SettlementStatus=1 THEN (soi.ProductTotalAmount - ISNULL(CASE WHEN soi.RmbProductRefundAmount>0 THEN soi.RmbProductTotalAmount ELSE 0 END,0)*(1-sop.Commission/100)) ELSE 0.00 END,
					RmbProductSettlementAmount= CASE WHEN soi.SettlementStatus=1 THEN (soi.RmbProductTotalAmount - ISNULL(CASE WHEN soi.RmbProductRefundAmount>0 THEN soi.RmbProductTotalAmount ELSE 0 END,0))*(1-sop.Commission/100) ELSE 0.00 END,                  
                    soi.SettlementTime,
                    soi.TradeCode,
                    ISNULL(SupplierPromotionDutyAmount,0) SupplierPromotionDutyAmount,
                    ISNULL(PromotionAmount - SupplierPromotionDutyAmount,0) PromotionDutyAmount,
                    ISNULL(SupplierPromotionDutyAmount,0)*soi.ExchangeRate RmbSupplierPromotionDutyAmount,
                    ISNULL(PromotionAmount - SupplierPromotionDutyAmount,0)*soi.ExchangeRate RmbPromotionDutyAmount
            FROM	SettlementOrderInfo soi(NOLOCK)
		            INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = soi.SupplierId
                    LEFT JOIN SettlementOrderProducts sop(NOLOCK) ON sop.SettlementCode = soi.SettlementCode AND soi.SettlementType = 2
                    LEFT JOIN
                    (
                        SELECT sp.SettlementCode,SUM(ISNULL(OriginalPrice - PromotionPrice,0)*sp.Quantity) PromotionAmount,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) SupplierPromotionDutyAmount
                        FROM SettlementOrderProducts sp(NOLOCK)
                        INNER JOIN SettlementOrderInfo s(NOLOCK) ON sp.SettlementCode = s.SettlementCode
                        INNER JOIN OrderPromotions op ON op.OrderCode=s.OrderCode AND op.Sku=sp.Sku
                        INNER JOIN Promotions p ON p.Id=op.PromotionId
                        WHERE sp.Quantity>0
                        GROUP BY sp.SettlementCode
                    ) t ON t.SettlementCode = soi.SettlementCode
            WHERE	1=1
		            {0}
		            {1}
		            {2}
		            {3}
		            {4}
                    {5}
) a
WHERE   1=1
        {6}
SELECT  TotalCount = Count(soi.SettlementCode),		
		ProductTotalAmount = ISNULL(SUM(ISNULL(soi.ProductTotalAmount,0)),0),
        RmbProductTotalAmount = ISNULL(SUM(ISNULL(soi.RmbProductTotalAmount,0)),0),
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
        SupplierPromotionDutyAmount = ISNULL(SUM(ISNULL(SupplierPromotionDutyAmount,0)),0) ,
        RmbSupplierPromotionDutyAmount = ISNULL(SUM(ISNULL(SupplierPromotionDutyAmount,0)*soi.ExchangeRate),0),
        PromotionDutyAmount = ISNULL(SUM(ISNULL(PromotionAmount - SupplierPromotionDutyAmount,0)),0),
        RmbPromotionDutyAmount = ISNULL(SUM(ISNULL(PromotionAmount - SupplierPromotionDutyAmount,0)*soi.ExchangeRate),0)
FROM	SettlementOrderInfo soi(NOLOCK)
		INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = soi.SupplierId
        LEFT JOIN
        (
            SELECT sp.SettlementCode,SUM(ISNULL(OriginalPrice - PromotionPrice,0)*sp.Quantity) PromotionAmount,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) SupplierPromotionDutyAmount
            FROM SettlementOrderProducts sp(NOLOCK)
            INNER JOIN SettlementOrderInfo s(NOLOCK) ON sp.SettlementCode = s.SettlementCode
            INNER JOIN OrderPromotions op ON op.OrderCode=s.OrderCode AND op.Sku=sp.Sku
            INNER JOIN Promotions p ON p.Id=op.PromotionId
            WHERE sp.Quantity>0
            GROUP BY sp.SettlementCode
        ) t ON t.SettlementCode = soi.SettlementCode
WHERE	1=1
        {0}
		{1}
		{2}
		{3}
		{4}
        {5}
";
            if (string.IsNullOrEmpty(queryModel.SettlementCode))
            {
                sql = string.Format(sql, queryModel.SettlementStatus > -1 ? " AND soi.SettlementStatus = @SettlementStatus " : "",
                    !string.IsNullOrEmpty(queryModel.OrderCode) ? " AND	(soi.OrderCode = @OrderCode OR soi.RefundCode = @OrderCode) " : "",
                    !string.IsNullOrEmpty(queryModel.CompanyName) ? " AND s.CompanyName LIKE '%' +@CompanyName+ '%' " : "",
                    queryModel.StartTime.HasValue ? " AND soi.CreateTime >= @StartTime" : "",
                    queryModel.EndTime.HasValue ? " AND soi.CreateTime <=@EndTime" : "",
                    "",
                    pModel.IsPaging ? " AND a.RowNumber >(@PageIndex-1)*@PageSize AND a.RowNumber <= @PageIndex*@PageSize" : "");
            }
            else
            {
                sql = string.Format(sql, " AND soi.SettlementCode = @SettlementCode ", "", "", "", "", "", pModel.IsPaging ? " AND a.RowNumber >(@PageIndex-1)*@PageSize AND a.RowNumber <= @PageIndex*@PageSize" : "");
            }

            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@SettlementCode", queryModel.SettlementCode);
            parameters.Append("@SettlementStatus", queryModel.SettlementStatus);
            parameters.Append("@OrderCode", queryModel.OrderCode);
            parameters.Append("@CompanyName", queryModel.CompanyName);
            parameters.Append("@StartTime", queryModel.StartTime.Value);
            parameters.Append("@EndTime", queryModel.EndTime.Value.AddDays(1));

            parameters.Append("@PageIndex", pModel.PageIndex);
            parameters.Append("@PageSize", pModel.PageSize);

            var ds = db.ExecuteSqlDataSet(sql, parameters);
            PagedList<SettlementOrderInfo> result = new PagedList<SettlementOrderInfo>();

            result.ContentList = ds.Tables[0].ToList<SettlementOrderInfo>();
            result.CurrentIndex = pModel.PageIndex;
            result.PageSize = pModel.PageSize;
            result.TotalObject = ds.Tables[1].ToList<SettlementOrderInfo>().First();
            result.RecordCount = result.TotalObject.TotalCount;

            return result;
        }
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
        soi.SettlementType,
        SupplierPromotionDutyAmount = ISNULL(SupplierPromotionDutyAmount,0),
        RmbSupplierPromotionDutyAmount = ISNULL(SupplierPromotionDutyAmount*soi.ExchangeRate,0),
        PromotionDutyAmount = ISNULL(PromotionAmount - SupplierPromotionDutyAmount,0),
        RmbPromotionDutyAmount = ISNULL((PromotionAmount - SupplierPromotionDutyAmount)*soi.ExchangeRate,0)
FROM	SettlementOrderInfo soi(NOLOCK)
		INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = soi.SupplierId
        LEFT JOIN
        (
            SELECT sp.SettlementCode,SUM(ISNULL(OriginalPrice - PromotionPrice,0)*sp.Quantity) PromotionAmount,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) SupplierPromotionDutyAmount
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
        /// 获取结算的支付信息
        /// </summary>
        /// <param name="tradeCode"></param>
        /// <returns></returns>
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
		pImg.ImagePath,
		si.BarCode,
        sop.Commission,
        ISNULL(SupplierPromotionDutyAmount,0) PromotionAmount
FROM	SettlementOrderProducts sop(NOLOCK)		
        INNER JOIN ProductInfo p(NOLOCK) ON p.Spu = sop.Spu AND p.LanguageVersion = 2
		INNER JOIN SkuInfo si(NOLOCK) ON si.Spu = sop.Spu AND si.Sku = sop.Sku AND si.SpuId = p.Id		
		LEFT JOIN ProductImage pImg(NOLOCK) ON pImg.Spu = sop.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1
        LEFT JOIN
        (
            SELECT sp.SettlementCode,sp.Sku,SUM(ISNULL((OriginalPrice - PromotionPrice)*sp.Quantity*(p.PromotionCost/100.0),0)) SupplierPromotionDutyAmount
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
        /// 审核结算单
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <param name="settlementAmount"></param>
        /// <param name="rmbSettlementAmount"></param>
        /// <param name="supplierBearDutyAmount"></param>
        /// <param name="rmbSupplierBearDutyAmount"></param>
        /// <param name="bearDutyAmount"></param>
        /// <param name="rmbBearDutyAmount"></param>
        /// <param name="otherAmount"></param>
        /// <param name="rmbOtherAmount"></param>
        /// <param name="auditTime"></param>
        /// <param name="auditor"></param>
        /// <returns></returns>
        public int auditOrder(string settlementCode, decimal settlementAmount, decimal rmbSettlementAmount, decimal supplierBearDutyAmount, decimal rmbSupplierBearDutyAmount, decimal bearDutyAmount, decimal rmbBearDutyAmount, decimal otherAmount, decimal rmbOtherAmount, DateTime auditTime, string auditor)
        {
            var sql = @"
BEGIN TRY
  BEGIN TRANSACTION;
    UPDATE SettlementOrderInfo
       SET  SettlementStatus = 2,  
            RmbSettlementAmount = @RmbSettlementAmount,
            SettlementAmount = @SettlementAmount,
            RmbOtherAmount = @RmbOtherAmount,
            OtherAmount = @OtherAmount,
            RmbSupplierBearDutyAmount = @RmbSupplierBearDutyAmount,
            SupplierBearDutyAmount = @SupplierBearDutyAmount,
            RmbBearDutyAmount = @RmbBearDutyAmount,
            BearDutyAmount = @BearDutyAmount,
            AuditTime = @AuditTime,
	        Auditor = @Auditor
    WHERE	SettlementCode = @SettlementCode
    IF @@ERROR = 0 AND @Result = -1
	BEGIN
        UPDATE SettlementOrderProducts
            SET RmbSettlementAmount = @RmbSettlementAmount,
                SettlementAmount = @SettlementAmount
        WHERE	SettlementCode = @SettlementCode
        IF @@ERROR = 0 AND @Result = -1
		BEGIN
			SET @Result = 1
		END
		ELSE 
		BEGIN
			SET @Result = 0
		END
    END
    ELSE
    BEGIN
		SET @Result = 0
	END
    IF @Result=1 
    BEGIN
         COMMIT TRANSACTION;
    END
    ELSE
    BEGIN
         ROLLBACK TRANSACTION;
    END
END TRY
BEGIN CATCH 
    SET @Result = 0	
    ROLLBACK TRANSACTION;
END CATCH
            ";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@SettlementCode", settlementCode);
            parameters.Append("@RmbSettlementAmount", rmbSettlementAmount);
            parameters.Append("@SettlementAmount", settlementAmount);
            parameters.Append("@RmbOtherAmount", rmbOtherAmount);

            parameters.Append("@OtherAmount", otherAmount);
            parameters.Append("@RmbSupplierBearDutyAmount", rmbSupplierBearDutyAmount);
            parameters.Append("@SupplierBearDutyAmount", supplierBearDutyAmount);

            parameters.Append("@RmbBearDutyAmount", rmbBearDutyAmount);
            parameters.Append("@BearDutyAmount", bearDutyAmount);

            parameters.Append("@AuditTime", auditTime);
            parameters.Append("@Auditor", auditor);

            parameters.Append("@Result", -1, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
            db.ExecuteSqlNonQuery(sql, parameters);
            int result = Convert.ToInt32(parameters["@Result"].Value.ToString());
            return result;
        }
        /// <summary>
        /// 结算单支付
        /// </summary>
        /// <param name="settlementCodes"></param>
        /// <param name="settlementTime"></param>
        /// <param name="payPlatform"></param>
        /// <param name="tradeCode"></param>
        /// <param name="payAmount"></param>
        /// <param name="payer"></param>
        /// <returns></returns>
        public int payOrder(string settlementCodes, DateTime settlementTime, int payPlatform, string tradeCode, decimal payAmount, string payer)
        {            
            var sql = @"
BEGIN TRY
  BEGIN TRANSACTION;
        INSERT INTO OrderPayment(PayCode,TradeCode,UserId,OrderType,OrderCode,PayAmount,PaidAmount,PayPlatform,PayType,PayStatus,PayTerminal,PayCompleteTime,PayBackRemark,Remark,CreateTime,CreateBy)VALUES(@PayCode,@TradeCode,0,3,'',@PayAmount,@PayAmount,2,2,2,5,@SettlementTime,'','',@SettlementTime,@Reckoner)
        IF @@ERROR = 0 AND @Result = -1
		BEGIN
            UPDATE	SettlementOrderInfo
	            SET TradeCode = @TradeCode,
		            SettlementTime = @SettlementTime,
		            Reckoner = @Reckoner,
		            SettlementStatus = 3
            WHERE	SettlementCode IN ({0})
            IF @@ERROR = 0 AND @Result = -1
			BEGIN
				SET @Result = 1
			END
			ELSE 
			BEGIN
				SET @Result = 0
			END
        END
        ELSE
        BEGIN
			SET @Result = 0
		END
        IF @Result=1 
        BEGIN
                COMMIT TRANSACTION;
        END
        ELSE
        BEGIN
                ROLLBACK TRANSACTION;
        END
END TRY
BEGIN CATCH 
    SET @Result = 0	
    ROLLBACK TRANSACTION;
END CATCH
";
            var payCode = new SFO2O.Admin.DAO.Refund.RefundDao().getNewId("settlementpay", 6);
            sql = string.Format(sql, settlementCodes);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@PayCode", payCode);
            parameters.Append("@TradeCode", tradeCode);
            parameters.Append("@SettlementTime", settlementTime);
            parameters.Append("@PayPlatform", payPlatform);
            parameters.Append("@PayAmount", payAmount);
            parameters.Append("@Reckoner", payer);
            parameters.Append("@Result", -1, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
            db.ExecuteSqlNonQuery(sql, parameters);
            int result = Convert.ToInt32(parameters["@Result"].Value.ToString());
            return result;
        }
    }
}
