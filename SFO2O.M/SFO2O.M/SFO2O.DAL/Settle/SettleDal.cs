using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Settle;
using SFO2O.EntLib.DataExtensions;
using System.Data;

namespace SFO2O.DAL.Settle
{
    public class SettleDal:BaseDal
    {
        /// <summary>
        /// 新增结算单
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool AddSettle(SettleOrderInfoEntity e)
        {
            string sql = @"insert into SettlementOrderInfo(SettlementCode,OrderCode,RefundCode,SettlementStatus,SettlementType,SupplierId,ExchangeRate,RmbProductTotalAmount,
                            ProductTotalAmount,RmbProductRefundAmount,ProductRefundAmount,RmbSettlementAmount,SettlementAmount,
                           RmbSupplierBearDutyAmount,SupplierBearDutyAmount,RmbBearDutyAmount,BearDutyAmount,CreateTime,CreateBy)
                            Values(@SettlementCode,@OrderCode,@RefundCode,@SettlementStatus,@SettlementType,@SupplierId,@ExchangeRate,
                            @RmbProductTotalAmount,@ProductTotalAmount,@RmbProductRefundAmount,@ProductRefundAmount,@RmbSettlementAmount,@SettlementAmount,
                            @RmbSupplierBearDutyAmount,@SupplierBearDutyAmount,@RmbBearDutyAmount,@BearDutyAmount,@CreateTime,@CreateBy);";

            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@SettlementCode",e.SettlementCode);
            parameters.Append("@OrderCode",e.OrderCode);
            parameters.Append("@RefundCode",e.RefundCode);
            parameters.Append("@SettlementStatus",e.SettlementStatus);
            parameters.Append("@SettlementType",e.SettlementType);
            parameters.Append("@SupplierId",e.SupplierId);
            parameters.Append("@ExchangeRate",e.ExchangeRate);
            parameters.Append("@RmbProductTotalAmount",e.RmbProductTotalAmount);
            parameters.Append("@ProductTotalAmount",e.ProductTotalAmount);
            parameters.Append("@RmbProductRefundAmount",e.RmbProductRefundAmount);
            parameters.Append("@ProductRefundAmount",e.ProductRefundAmount);
            parameters.Append("@RmbSettlementAmount",e.RmbSettlementAmount);
            parameters.Append("@SettlementAmount",e.SettlementAmount);
            parameters.Append("@RmbSupplierBearDutyAmount",e.RmbSupplierBearDutyAmount);
            parameters.Append("@SupplierBearDutyAmount",e.SupplierBearDutyAmount);
            parameters.Append("@RmbBearDutyAmount",e.RmbBearDutyAmount);
            parameters.Append("@BearDutyAmount",e.BearDutyAmount);
            parameters.Append("@CreateTime",e.CreateTime);
            parameters.Append("@CreateBy",e.CreateBy);

            return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
        }
        /// <summary>
        /// 新增结算商品
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool AddSettleProduct(SettleProductEntity e)
        {
            string sql = @"Insert into SettlementOrderProducts(SettlementCode,spu,Sku,Quantity,UnitPrice,RmbUnitPrice,TaxRate,RmbAmount,Amount,RmbTaxAmount,TaxAmount,RmbSettlementAmount,SettlementAmount,IsBearDuty,Commission)
                        Values(@SettlementCode,@spu,@Sku,@Quantity,@UnitPrice,@RmbUnitPrice,@TaxRate,@RmbAmount,@Amount,@RmbTaxAmount,@TaxAmount,@RmbSettlementAmount,@SettlementAmount,@IsBearDuty,@Commission);";
            
            var parameters = DbSFO2OMain.CreateParameterCollection();
            parameters.Append("@SettlementCode", e.SettlementCode);
            parameters.Append("@spu",e.Spu);
            parameters.Append("@Sku",e.Sku);
            parameters.Append("@Quantity",e.Quantity);
            parameters.Append("@UnitPrice",e.UnitPrice);
            parameters.Append("@RmbUnitPrice",e.RmbUnitPrice);
            parameters.Append("@TaxRate",e.TaxRate);
            parameters.Append("@RmbAmount",e.RmbAmount);
            parameters.Append("@Amount",e.Amount);
            parameters.Append("@RmbTaxAmount",e.RmbTaxAmount);
            parameters.Append("@TaxAmount",e.TaxAmount);
            parameters.Append("@RmbSettlementAmount",e.RmbSettlementAmount);
            parameters.Append("@SettlementAmount",e.SettlementAmount);
            parameters.Append("@IsBearDuty",e.IsBearDuty);
            parameters.Append("@Commission", e.Commission);
            return DbSFO2OMain.ExecuteSqlNonQuery(sql, parameters) > 0;
        }
        /// <summary>
        /// 生成结算单顺序号
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public int GetSettleCodeNo(string orderCode)
        {
            string sql = "Select Count(1) From SettlementOrderInfo Where OrderCode=@OrderCode";
            var parameters = DbSFO2ORead.CreateParameterCollection();
            parameters.Append("@OrderCode", orderCode);
            object obj = DbSFO2ORead.ExecuteScalar(CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// 取消退款，并生成结算单
        /// </summary>
        /// <param name="refundCode"></param>
        /// <param name="refuseTime"></param>
        /// <param name="refuseBy"></param>
        public int CancelRefund(string refundCode, string settleCode, DateTime refuseTime, string refuseBy)
        {

            var sql = @"
BEGIN TRY
      BEGIN TRANSACTION;
        UPDATE RefundOrderInfo 
	        SET RefundStatus  = 5,
		        CompletionTime = @CompletionTime
        WHERE	RefundCode = @RefundCode				
		IF @@ERROR = 0 AND @Result = -1
		BEGIN		
				INSERT INTO SettlementOrderInfo(SettlementCode,OrderCode,RefundCode,SettlementStatus,SettlementType,SupplierId,ExchangeRate,RmbProductTotalAmount,ProductTotalAmount,RmbProductRefundAmount,ProductRefundAmount,RmbSettlementAmount,
				SettlementAmount,RmbOtherAmount,OtherAmount,RmbSupplierBearDutyAmount,SupplierBearDutyAmount,RmbBearDutyAmount,BearDutyAmount,CreateTime,CreateBy,AuditTime,Auditor) 
				SELECT  @SettlementCode,roi.OrderCode,roi.RefundCode,2,2,roi.SupplierId,ISNULL(roi.ExchangeRate,1),rop.RMBUnitPrice * rop.Quantity,rop.UnitPrice * rop.Quantity,0.00,0.00,
				(rop.RmbUnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END),
				(rop.UnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END),0.00,0.00,
				CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END,CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END,
				0,0,
				@CompletionTime,@CreateBy,@CompletionTime,@CreateBy
				FROM    RefundOrderInfo roi
						INNER JOIN RefundOrderProducts rop ON rop.RefundCode = roi.RefundCode
				WHERE	roi.RefundCode=@RefundCode				
				IF @@ERROR = 0 AND @Result = -1
				BEGIN
				
						INSERT INTO SettlementOrderProducts(SettlementCode,Spu,Sku,Quantity,UnitPrice,RmbUnitPrice,TaxRate,RmbAmount,Amount,RmbTaxAmount,TaxAmount,RmbSettlementAmount,SettlementAmount,IsBearDuty,Commission)
						SELECT  @SettlementCode,rop.Spu,rop.Sku,rop.Quantity,rop.UnitPrice,rop.RmbUnitPrice,rop.TaxRate,rop.RmbUnitPrice * rop.Quantity,rop.UnitPrice * rop.Quantity,rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity,
						rop.TaxRate/100 * rop.UnitPrice * rop.Quantity,(rop.RmbUnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.RmbUnitPrice * rop.Quantity ELSE 0 END),
						(rop.UnitPrice * rop.Quantity)*(1- roi.Commission/100)-(CASE WHEN rop.IsBearDuty = 1 THEN rop.TaxRate/100 * rop.UnitPrice * rop.Quantity ELSE 0 END),ISNULL(rop.IsBearDuty,0),roi.Commission
						FROM    RefundOrderInfo roi
								INNER JOIN RefundOrderProducts rop ON rop.RefundCode = roi.RefundCode
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
;";
            var db = DbSFO2OMain;
            var parameters = db.CreateParameterCollection();
            parameters.Append("@RefundCode", refundCode);
            parameters.Append("@SettlementCode", settleCode);
            parameters.Append("@CompletionTime", refuseTime);
            parameters.Append("@CreateBy", refuseBy);
            parameters.Append("@Result", -1, System.Data.DbType.Int32, System.Data.ParameterDirection.InputOutput);
            db.ExecuteSqlNonQuery(sql, parameters);
            int result = Convert.ToInt32(parameters["@Result"].Value.ToString());
            return result;
        }
    }
}
