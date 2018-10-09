using SFO2O.Admin.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Common.EntLib.DataExtensions;
using SFO2O.Admin.Models.Refund;
using SFO2O.Admin.ViewModel.Refund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Admin.Models.Payment;
using System.Data;

namespace SFO2O.Admin.DAO.Refund
{
    public class RefundDao : BaseDao
    {
        /// <summary>
        /// 获取退款单列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PagedList<RefundOrderListInfo> getOrderList(RefundQueryModel queryModel, PagingModel pModel)
        {
            var sql = @"
SELECT *
FROM	(
			SELECT	RowNumber = ROW_NUMBER() OVER (ORDER BY roi.Createtime DESC),
					roi.OrderCode,
					BuyerName = c.UserName,		
					roi.RefundCode,
					roi.CreateTime,
					roi.RefundType,
					roi.ImagePath,
					SellerName = s.CompanyName,
					rop.Sku,
					ProductName = p.Name,
					ProductImgPath = pimg.ImagePath,
					RmbProductAmount = ISNULL(rop.RmbUnitPrice,0) * ISNULL(Quantity,0),
					ProductAmount = ISNULL(UnitPrice,0) *  ISNULL(Quantity,0),
					roi.RmbDutyAmount,
					roi.DutyAmount,
                    roi.RmbTotalAmount,
					roi.TotalAmount,				
					roi.RefundStatus,
					si.MainDicValue,
					si.MainValue,
					si.SubDicValue,
					si.SubValue,
                    p.IsReturn,
                    rop.Spu
			FROM	RefundOrderInfo roi(NOLOCK)
					INNER JOIN RefundOrderProducts rop(NOLOCK) ON rop.RefundCode = roi.RefundCode
					INNER JOIN ProductInfo p(NOLOCK) ON p.Spu = rop.Spu AND p.LanguageVersion = 2
					INNER JOIN SkuInfo si(NOLOCK) ON si.Sku = rop.Sku AND si.Spu = rop.Spu AND si.SpuId = p.Id
					LEFT JOIN ProductImage pimg(NOLOCK)  ON pimg.Spu = rop.Spu AND pimg.SortValue = 1		
					INNER JOIN Customer c(NOLOCK) ON c.ID = roi.UserId
					INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = roi.SupplierId
			WHERE	1=1
					AND roi.RegionCode = @RegionCode
					{0}
					{1}
					{2}
					{3}
					{4}
					{5}
					{6}
					{7}
					{8}
                    {9}
) a
WHERE	1=1
		{10}
SELECT	TotalCount = COUNT(roi.RefundCode),		
		RmbProductAmount = SUM(ISNULL(ISNULL(rop.RmbUnitPrice,0) * ISNULL(Quantity,0),0)),
		ProductAmount = SUM(ISNULL(ISNULL(UnitPrice,0) *  ISNULL(Quantity,0),0)),
		RmbDutyAmount = SUM(ISNULL(roi.RmbDutyAmount,0)),
		DutyAmount = SUM(ISNULL(roi.DutyAmount,0)),
		RmbTotalAmount = SUM(ISNULL(roi.RmbTotalAmount,0)),
		TotalAmount = SUM(ISNULL(roi.TotalAmount,0)),
        ProductQuantity = SUM(ISNULL(rop.Quantity,0))
FROM	RefundOrderInfo roi(NOLOCK)
		INNER JOIN RefundOrderProducts rop(NOLOCK) ON rop.RefundCode = roi.RefundCode
		INNER JOIN ProductInfo p(NOLOCK) ON p.Spu = rop.Spu AND p.LanguageVersion = 2
		INNER JOIN SkuInfo si(NOLOCK) ON si.Sku = rop.Sku AND si.Spu = rop.Spu AND si.SpuId = p.Id
		LEFT JOIN ProductImage pimg(NOLOCK)  ON pimg.Spu = rop.Spu AND pimg.SortValue = 1		
		INNER JOIN Customer c(NOLOCK) ON c.ID = roi.UserId
		INNER JOIN Supplier s(NOLOCK) ON s.SupplierID = roi.SupplierId
WHERE	1=1	
        AND roi.RegionCode = @RegionCode	
		{0}
		{1}
		{2}
		{3}
		{4}
		{5}
		{6}
		{7}
		{8}
        {9}
";

            sql = string.Format(sql, queryModel.StartTime != null ? " AND roi.CreateTime >=@StartTime" : "",
                                     queryModel.EndTime != null ? " AND roi.CreateTime <=@EndTime" : "",
                                     queryModel.RefundStatus > -1 ? " AND roi.RefundStatus = @RefundStatus " : "",
                                     queryModel.RefundType > -1 ? " AND roi.RefundType = @RefundType " : "",
                                     !string.IsNullOrEmpty(queryModel.BuyerName) ? " AND c.UserName LIKE '%'+@BuyerName+'%' " : "",
                                     !(queryModel.SellerName <=0) ? " AND (s.SupplierID=@SellerName) " : "",
                                     !string.IsNullOrEmpty(queryModel.Sku) ? " AND si.Sku LIKE '%'+@Sku+'%' " : "",
                                     !string.IsNullOrEmpty(queryModel.OrderCode) ? " AND roi.OrderCode LIKE '%'+@OrderCode+'%' " : "",
                                     !string.IsNullOrEmpty(queryModel.RefundCode) ? " AND roi.RefundCode LIKE '%'+@RefundCode+'%' " : "",
                                     queryModel.IsFinance == 1 ? " AND roi.RefundStatus IN (3,4) " : "",
                                     pModel.IsPaging ? " AND a.RowNumber >(@PageIndex-1)*@PageSize AND a.RowNumber <= @PageIndex*@PageSize" : "");


            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RegionCode", queryModel.RegionCode);
            parameters.Append("@StartTime", queryModel.StartTime);
            parameters.Append("@EndTime", queryModel.EndTime.AddDays(1));
            parameters.Append("@RefundStatus", queryModel.RefundStatus);
            parameters.Append("@RefundType", queryModel.RefundType);
            parameters.Append("@BuyerName", queryModel.BuyerName);
            parameters.Append("@SellerName", queryModel.SellerName);
            parameters.Append("@Sku", queryModel.Sku);
            parameters.Append("@RefundCode", queryModel.RefundCode);
            parameters.Append("@OrderCode", queryModel.OrderCode);
            parameters.Append("@PageIndex", pModel.PageIndex);
            parameters.Append("@PageSize", pModel.PageSize);

            var ds = db.ExecuteSqlDataSet(sql, parameters);
            PagedList<RefundOrderListInfo> result = new PagedList<RefundOrderListInfo>();

            result.ContentList = ds.Tables[0].ToList<RefundOrderListInfo>();
            result.CurrentIndex = pModel.PageIndex;
            result.PageSize = pModel.PageSize;
            result.TotalObject = ds.Tables[1].ToList<RefundOrderListInfo>().First();
            result.RecordCount = result.TotalObject.TotalCount;

            return result;
        }

        /// <summary>
        /// 获取退款单信息
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public RefundOrderInfo getOrderInfo(string refundCode)
        {
            var sql = @"
SELECT	roi.RefundCode,
		roi.RefundStatus,
		roi.TotalAmount,
        roi.RmbTotalAmount,
		roi.RefundType,
		roi.RefundReason,
		roi.RefundDescription,
		roi.ImagePath,
		roi.CreateTime,
		roi.AuditTime,
		roi.PickupTime,
		roi.CompletionTime,
		roi.UserId,
		roi.SupplierId,
        roi.OrderCode,
        roi.ProductStatus,
        o.CustomsDuties as OrderCustomsDuties
FROM	RefundOrderInfo roi(NOLOCK)
inner join OrderInfo o(NOLOCK) on o.OrderCode=roi.OrderCode
where roi.RefundCode = @RefundCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);
            return db.ExecuteSqlFirst<RefundOrderInfo>(sql, parameters);
        }

        /// <summary>
        /// 获取退款单明细
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <returns></returns>
        public List<RefundProductInfo> getOrderProducts(string refundCode)
        {
            var sql = @"
SELECT  ProductName = p.Name,
		ProductImgPath = pImg.ImagePath,
		si.BarCode,
		si.MainDicValue,
		si.MainValue,
		si.SubDicValue,
		si.SubValue,
		rop.Sku,
		rop.Spu,
		rop.UnitPrice,
		rop.RmbUnitPrice,
		rop.Quantity,
		ProductAmount = rop.Quantity * rop.UnitPrice,
		RmpProductAmount = rop.Quantity * rop.RmbUnitPrice,
		rop.RefundCode,
		rop.TaxRate,
		rop.Id,
        rop.IsBearDuty,
        p.IsReturn
FROM	RefundOrderProducts rop(NOLOCK)		
        INNER JOIN ProductInfo p(NOLOCK) ON p.Spu = rop.Spu AND p.LanguageVersion = 2
		INNER JOIN SkuInfo si(NOLOCK) ON si.Spu = rop.Spu AND si.Sku = rop.Sku AND si.SpuId = p.Id		
		LEFT JOIN ProductImage pImg(NOLOCK) ON pImg.Spu = rop.Spu AND pImg.SortValue = 1 AND pImg.ImageType = 1 		
WHERE   rop.RefundCode = @RefundCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);
            return db.ExecuteSqlList<RefundProductInfo>(sql, parameters).ToList();
        }

        /// <summary>
        /// 拒绝退款，并生成结算单
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="refuseTime"></param>
        /// <param name="refuseBy"></param>
        public int refuseRefund(string refundCode, string refuseReason, DateTime refuseTime, string refuseBy)
        {

            var sql = @"
BEGIN TRY
      BEGIN TRANSACTION;
        UPDATE RefundOrderInfo 
	        SET RefundStatus  = 6,
		        NoPassReason = @NoPassReason,
		        CompletionTime = @CompletionTime
        WHERE	RefundCode = @RefundCode				
		IF @@ERROR = 0 AND @Result = -1
		BEGIN		
				INSERT INTO SettlementOrderInfo(SettlementCode,OrderCode,RefundCode,SettlementStatus,SettlementType,SupplierId,ExchangeRate,RmbProductTotalAmount,ProductTotalAmount,RmbProductRefundAmount,ProductRefundAmount,RmbSettlementAmount,
				SettlementAmount,RmbOtherAmount,OtherAmount,RmbSupplierBearDutyAmount,SupplierBearDutyAmount,RmbBearDutyAmount,BearDutyAmount,CreateTime,CreateBy,AuditTime,Auditor) 
				SELECT  @SettlementCode,roi.OrderCode,roi.RefundCode,2,2,roi.SupplierId,ISNULL(roi.ExchangeRate,1),rop.RMBUnitPrice * rop.Quantity,rop.UnitPrice * rop.Quantity,0.00,0.00,
				(rop.RmbUnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END)
+ISNULL((rop.Quantity)*(op2.OriginalRMBPrice-op.PayUnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),
(rop.UnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END)
+ISNULL((rop.Quantity)*(op2.OriginalPrice-op.UnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),0.00,0.00,
				CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END,CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END,
				0,0,
				@CompletionTime,@CreateBy,@CompletionTime,@CreateBy
				FROM    RefundOrderInfo roi
						INNER JOIN RefundOrderProducts rop ON rop.RefundCode = roi.RefundCode
		INNER JOIN OrderProducts op ON op.OrderCode=roi.OrderCode AND op.Sku = rop.Sku
        LEFT JOIN OrderPromotions op2 ON op2.OrderCode=roi.OrderCode AND op2.Sku = rop.Sku
        LEFT JOIN Promotions p2 ON p2.Id=op2.PromotionId
				WHERE	roi.RefundCode=@RefundCode				
				IF @@ERROR = 0 AND @Result = -1
				BEGIN
				
						INSERT INTO SettlementOrderProducts(SettlementCode,Spu,Sku,Quantity,UnitPrice,RmbUnitPrice,TaxRate,RmbAmount,Amount,RmbTaxAmount,TaxAmount,RmbSettlementAmount,SettlementAmount,IsBearDuty,Commission)
						SELECT  @SettlementCode,rop.Spu,rop.Sku,rop.Quantity,rop.UnitPrice,rop.RmbUnitPrice,rop.TaxRate,rop.RmbUnitPrice * rop.Quantity,rop.UnitPrice * rop.Quantity,rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity,
						rop.TaxRate/100 * rop.UnitPrice * rop.Quantity,(rop.RmbUnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END)
+ISNULL((rop.Quantity)*(op2.OriginalRMBPrice-op.PayUnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),
						(rop.UnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END)
+ISNULL((rop.Quantity)*(op2.OriginalPrice-op.UnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),ISNULL(rop.IsBearDuty,0),roi.Commission
						FROM    RefundOrderInfo roi
								INNER JOIN RefundOrderProducts rop ON rop.RefundCode = roi.RefundCode
		INNER JOIN OrderProducts op ON op.OrderCode=roi.OrderCode AND op.Sku = rop.Sku
        LEFT JOIN OrderPromotions op2 ON op2.OrderCode=roi.OrderCode AND op2.Sku = rop.Sku
        LEFT JOIN Promotions p2 ON p2.Id=op2.PromotionId
						WHERE	roi.RefundCode = @RefundCode
						SELECT @@ROWCOUNT
				END
				ELSE 
				BEGIN
					SET @Result = 0
				END
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
            var settlementCode = getSettlementCode(refundCode);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);
            parameters.Append("@NoPassReason", refuseReason);
            parameters.Append("@SettlementCode", settlementCode);
            parameters.Append("@CompletionTime", refuseTime);
            parameters.Append("@CreateBy", refuseBy);
            parameters.Append("@Result", -1, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
            db.ExecuteSqlNonQuery(sql, parameters);
            int result = Convert.ToInt32(parameters["@Result"].Value.ToString());
            return result;
        }
        /// <summary>
        /// 退款审核通过
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int agreeRefund(RefundOrderInfo order)
        {
            var sql = @"
UPDATE RefundOrderInfo 
	SET IsQualityProblem  = @IsQualityProblem,
		RefundType = @RefundType,
		IsReturnDuty = @IsReturnDuty,
        RmbTotalAmount = @RmbTotalAmount,
        TotalAmount = @TotalAmount,
        RmbDutyAmount = @RmbDutyAmount,
        DutyAmount = @DutyAmount,
        CollectionCode = @CollectionCode,
        TobePickUpTime = @TobePickUpTime,
        AuditTime = @AuditTime,
        Auditor = @Auditor,
        RefundStatus = @RefundStatus
WHERE	RefundCode = @RefundCode			
		
";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@IsQualityProblem", order.IsQualityProblem);
            parameters.Append("@RefundType", order.RefundType);
            parameters.Append("@IsReturnDuty", order.IsReturnDuty);
            parameters.Append("@RmbTotalAmount", order.RmbTotalAmount);
            parameters.Append("@TotalAmount", order.TotalAmount);
            parameters.Append("@RmbDutyAmount", order.RmbDutyAmount);
            parameters.Append("@DutyAmount", order.DutyAmount);
            parameters.Append("@CollectionCode", order.CollectionCode);
            parameters.Append("@TobePickUpTime", order.TobePickUpTime);
            parameters.Append("@AuditTime", order.AuditTime);
            parameters.Append("@Auditor", order.Auditor);
            parameters.Append("@RefundStatus", order.RefundStatus);
            parameters.Append("@RefundCode", order.RefundCode);

            return db.ExecuteSqlNonQuery(sql, parameters);
        }
        /// <summary>
        /// 上门取件
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="expressCompany"></param>
        /// <param name="expressList"></param>
        /// <param name="pickupTime"></param>
        /// <param name="pickupper"></param>
        /// <returns></returns>
        public int refundPickUp(string refundCode, string expressCompany, string expressList, DateTime pickupTime, string pickupper)
        {
            var sql = @"
UPDATE RefundOrderInfo 
	SET ExpressCompany  = @ExpressCompany,
		ExpressList = @ExpressList,
		Pickupper = @Pickupper,
        PickupTime = @PickupTime,       
        RefundStatus = @RefundStatus
WHERE	RefundCode = @RefundCode			
		
";

            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundStatus", 3);
            parameters.Append("@PickupTime", pickupTime);
            parameters.Append("@Pickupper", pickupper);
            parameters.Append("@ExpressList", expressList);
            parameters.Append("@ExpressCompany", expressCompany);
            parameters.Append("@RefundCode", refundCode);

            return db.ExecuteSqlNonQuery(sql, parameters);
        }
        public string getSettlementCode(string refundCode)
        {
            var sql = @"
SELECT  @OrderCode =  OrderCode 
FROM    RefundOrderInfo roi(NOLOCK) WHERE roi.RefundCode = @RefundCode
SELECT  TOP 1 soi.SettlementCode 
FROM    SettlementOrderInfo soi(NOLOCK) 
WHERE   soi.OrderCode = @OrderCode
ORDER BY soi.SettlementCode DESC
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);
            parameters.Append("@OrderCode", "", System.Data.DbType.String, System.Data.ParameterDirection.InputOutput, 30);
            var maxSettlementCode = db.ExecuteSqlScalar<string>(sql, parameters);
            var orderCode = parameters["@OrderCode"].Value.ToString();
            var currentCode = string.Format("C{0}-{1}", orderCode.Replace("S", ""), string.IsNullOrEmpty(maxSettlementCode) ? "001" : (Convert.ToInt32(maxSettlementCode.Split('-')[1]) + 1).ToString().PadLeft(3, '0'));
            return currentCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="startNum"></param>
        /// <returns></returns>
        public string getNewId(string type, int startNum)
        {
            var sql = @"
DECLARE @LastNum BIGINT;
DECLARE @Currdate INT
SET @Currdate=CONVERT(VARCHAR(10),GETDATE(),12)
UPDATE FrameCode
    SET @LastNum=LastNum=LastNum+1
WHERE   Currdate=@Currdate
        AND CType='{0}'
IF(@@RowCount=0)
BEGIN
    SET @LastNum=100001;
    INSERT INTO FrameCode VALUES(@Currdate,'{0}','',@LastNum)
END
SELECT '{1}'+CAST(@Currdate AS VARCHAR(6))+CAST(@LastNum AS VARCHAR(6))";
            sql = string.Format(sql, type, startNum);
            var db = DbSFO2OMain;
            return db.ExecuteSqlScalar<string>(sql);
        }
        /// <summary>
        /// 退款单退款-修改退款单状态，写支付记录，生成结算单
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="rmbTotalAmount"></param>
        /// <param name="payPlatform"></param>
        /// <param name="settlementTime"></param>
        /// <param name="tradeCode"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int returnRefund(string refundCode, decimal rmbTotalAmount, int payPlatform, DateTime completionTime, string tradeCode, string payer)
        {

            var sql = @"
BEGIN TRY
      BEGIN TRANSACTION;
        UPDATE RefundOrderInfo 
	        SET RefundStatus  = 4,
		        CompletionTime = @CompletionTime
        WHERE	RefundCode = @RefundCode				
		IF @@ERROR = 0 AND @Result = -1
		BEGIN		
                INSERT INTO OrderPayment(PayCode,TradeCode,UserId,OrderType,OrderCode,PayAmount,PaidAmount,PayPlatform,PayType,PayStatus,PayTerminal,PayCompleteTime,PayBackRemark,Remark,CreateTime,CreateBy)
                VALUES(@PayCode,@TradeCode,0,2,@RefundCode,@PayAmount,@PayAmount,@PayPlatform,2,2,5,@CreateTime,'','',@CreateTime,@Payer)
                IF @@ERROR = 0 AND @Result = -1
                BEGIN
				    INSERT INTO SettlementOrderInfo(SettlementCode,OrderCode,RefundCode,SettlementStatus,SettlementType,SupplierId,ExchangeRate,RmbProductTotalAmount,ProductTotalAmount,RmbProductRefundAmount,ProductRefundAmount,
                    RmbSettlementAmount,SettlementAmount,RmbOtherAmount,OtherAmount,RmbSupplierBearDutyAmount,SupplierBearDutyAmount,RmbBearDutyAmount,BearDutyAmount,CreateTime,CreateBy,AuditTime,Auditor) 
				    SELECT  @SettlementCode,roi.OrderCode,roi.RefundCode,CASE WHEN roi.IsQualityProblem = 1 THEN 1 ELSE 2 END,2,roi.SupplierId,ISNULL(roi.ExchangeRate,1),
                    rop.RMBUnitPrice * rop.Quantity,
                    rop.UnitPrice * rop.Quantity,
                    roi.RMBTotalAmount,
                    roi.TotalAmount,
                    CASE WHEN roi.IsQualityProblem = 0 THEN (rop.RmbUnitPrice * rop.Quantity-roi.RmbTotalAmount+roi.RmbDutyAmount)*(1- roi.Commission/100)
                    -(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END) ELSE 0 END
+ISNULL((rop.Quantity)*(op2.OriginalRMBPrice-op.PayUnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),
                    CASE WHEN roi.IsQualityProblem = 0 THEN (rop.UnitPrice * rop.Quantity-roi.TotalAmount+roi.DutyAmount)*(1- roi.Commission/100)
                    -(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END) ELSE 0 END
+ISNULL((rop.Quantity)*(op2.OriginalPrice-op.UnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),
                    0.00,
                    0.00,
                    CASE WHEN roi.IsQualityProblem = 0 AND rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END ,
                    CASE WHEN roi.IsQualityProblem = 0 AND rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END, 

                    CASE WHEN roi.RmbDutyAmount>0 AND rop.IsBearDuty = 0 THEN roi.RmbDutyAmount ELSE 0 END,
                    CASE WHEN roi.DutyAmount>0 AND rop.IsBearDuty = 0 THEN roi.DutyAmount ELSE 0 END,
                    @CreateTime,@Payer,CASE WHEN roi.IsQualityProblem = 2 THEN @CreateTime ELSE null END,CASE WHEN roi.IsQualityProblem = 2 THEN @Payer ELSE null END
				    FROM    RefundOrderInfo roi
						    INNER JOIN RefundOrderProducts rop ON rop.RefundCode = roi.RefundCode
		INNER JOIN OrderProducts op ON op.OrderCode=roi.OrderCode AND op.Sku = rop.Sku
        LEFT JOIN OrderPromotions op2 ON op2.OrderCode=roi.OrderCode AND op2.Sku = rop.Sku
        LEFT JOIN Promotions p2 ON p2.Id=op2.PromotionId
				    WHERE	roi.RefundCode=@RefundCode				
				    IF @@ERROR = 0 AND @Result = -1
				    BEGIN				
						    INSERT INTO SettlementOrderProducts(SettlementCode,Spu,Sku,Quantity,UnitPrice,RmbUnitPrice,TaxRate,RmbAmount,Amount,RmbTaxAmount,TaxAmount,RmbSettlementAmount,SettlementAmount,IsBearDuty,Commission)
						    SELECT  @SettlementCode,rop.Spu,rop.Sku,rop.Quantity,rop.UnitPrice,rop.RmbUnitPrice,rop.TaxRate,rop.RmbUnitPrice * rop.Quantity,rop.UnitPrice * rop.Quantity,rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity,
						    rop.TaxRate/100 * rop.UnitPrice * rop.Quantity,
                            CASE WHEN roi.IsQualityProblem = 0 THEN (rop.RmbUnitPrice * rop.Quantity-roi.RmbTotalAmount+roi.RmbDutyAmount)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END) ELSE 0 END
+ISNULL((rop.Quantity)*(op2.OriginalRMBPrice-op.PayUnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),
                            CASE WHEN roi.IsQualityProblem = 0 THEN (rop.UnitPrice * rop.Quantity-roi.TotalAmount+roi.DutyAmount)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END) ELSE 0 END
+ISNULL((rop.Quantity)*(op2.OriginalPrice-op.UnitPrice)*(1-p2.PromotionCost/100.0)*(1- roi.Commission/100),0),ISNULL(rop.IsBearDuty,0),roi.Commission
						    FROM    RefundOrderInfo roi
								    INNER JOIN RefundOrderProducts rop ON rop.RefundCode = roi.RefundCode
		INNER JOIN OrderProducts op ON op.OrderCode=roi.OrderCode AND op.Sku = rop.Sku
        LEFT JOIN OrderPromotions op2 ON op2.OrderCode=roi.OrderCode AND op2.Sku = rop.Sku
        LEFT JOIN Promotions p2 ON p2.Id=op2.PromotionId
						    WHERE	roi.RefundCode = @RefundCode
				    END
					ELSE
					BEGIN
						SET @Result = 0	
					END
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
            var settlementCode = getSettlementCode(refundCode);
            var payCode = getNewId("Refund", 7);
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);
            parameters.Append("@TradeCode", tradeCode);
            parameters.Append("@SettlementCode", settlementCode);
            parameters.Append("@CompletionTime", completionTime);
            parameters.Append("@PayPlatform", payPlatform);
            parameters.Append("@Payer", payer);
            parameters.Append("@PayAmount", rmbTotalAmount);
            parameters.Append("@CreateTime", DateTime.Now);
            parameters.Append("@PayCode", payCode);
            parameters.Append("@Result", -1, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
            db.ExecuteSqlNonQuery(sql, parameters);
            int result = Convert.ToInt32(parameters["@Result"].Value.ToString());
            return result;
        }
        /// <summary>
        /// 获取退款支付信息
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public OrderPaymentInfo getRefundPaymentInfo(string refundCode)
        {
            var sql = @"
SELECT  op.Id,
        op.PayCode,
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
WHERE   op.OrderCode = @RefundCode
";
            var db = DbSFO2ORead;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);

            return db.ExecuteSqlFirst<OrderPaymentInfo>(sql, parameters);
        }

    }
}
