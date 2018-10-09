using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Settle
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// 结算单实体
    /// </summary>
    public class SettleOrderInfoEntity
    {

        /// <summary>
        /// 结算单号
        /// </summary>
        [DataMember(Name = "SettlementCode")]
        [Display(Name = "SettlementCode")]
        public string SettlementCode { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

        /// <summary>
        /// 结算单状态：1：待确认 2：待付款 3：付款完成
        /// </summary>
        [DataMember(Name = "SettlementStatus")]
        [Display(Name = "SettlementStatus")]
        public int SettlementStatus { get; set; }

        /// <summary>
        /// 结算单类型：1订单，2退款退货，3仅退款
        /// </summary>
        [DataMember(Name = "SettlementType")]
        [Display(Name = "SettlementType")]
        public int SettlementType { get; set; }

        /// <summary>
        /// 商家id
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// 商品总金额(人民币)
        /// </summary>
        [DataMember(Name = "RmbProductTotalAmount")]
        [Display(Name = "RmbProductTotalAmount")]
        public decimal RmbProductTotalAmount { get; set; }

        /// <summary>
        /// 商品总金额
        /// </summary>
        [DataMember(Name = "ProductTotalAmount")]
        [Display(Name = "ProductTotalAmount")]
        public decimal ProductTotalAmount { get; set; }

        /// <summary>
        /// 商品退款总金额（人民币）
        /// </summary>
        [DataMember(Name = "RmbProductRefundAmount")]
        [Display(Name = "RmbProductRefundAmount")]
        public decimal RmbProductRefundAmount { get; set; }

        /// <summary>
        /// 商品退款总金额
        /// </summary>
        [DataMember(Name = "ProductRefundAmount")]
        [Display(Name = "ProductRefundAmount")]
        public decimal ProductRefundAmount { get; set; }

        /// <summary>
        /// 结算总金额（人民币）
        /// </summary>
        [DataMember(Name = "RmbSettlementAmount")]
        [Display(Name = "RmbSettlementAmount")]
        public decimal RmbSettlementAmount { get; set; }

        /// <summary>
        /// 结算总金额
        /// </summary>
        [DataMember(Name = "SettlementAmount")]
        [Display(Name = "SettlementAmount")]
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// RmbOtherAmount
        /// </summary>
        [DataMember(Name = "RmbOtherAmount")]
        [Display(Name = "RmbOtherAmount")]
        public decimal RmbOtherAmount { get; set; }

        /// <summary>
        /// OtherAmount
        /// </summary>
        [DataMember(Name = "OtherAmount")]
        [Display(Name = "OtherAmount")]
        public decimal OtherAmount { get; set; }

        /// <summary>
        /// 商家承担商品税总金额（人民币）
        /// </summary>
        [DataMember(Name = "RmbSupplierBearDutyAmount")]
        [Display(Name = "RmbSupplierBearDutyAmount")]
        public decimal RmbSupplierBearDutyAmount { get; set; }

        /// <summary>
        /// 商家承担商品税总金额
        /// </summary>
        [DataMember(Name = "SupplierBearDutyAmount")]
        [Display(Name = "SupplierBearDutyAmount")]
        public decimal SupplierBearDutyAmount { get; set; }

        /// <summary>
        /// RmbBearDutyAmount
        /// </summary>
        [DataMember(Name = "RmbBearDutyAmount")]
        [Display(Name = "RmbBearDutyAmount")]
        public decimal RmbBearDutyAmount { get; set; }

        /// <summary>
        /// BearDutyAmount
        /// </summary>
        [DataMember(Name = "BearDutyAmount")]
        [Display(Name = "BearDutyAmount")]
        public decimal BearDutyAmount { get; set; }

        /// <summary>
        /// TradeCode
        /// </summary>
        [DataMember(Name = "TradeCode")]
        [Display(Name = "TradeCode")]
        public string TradeCode { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// AuditTime
        /// </summary>
        [DataMember(Name = "AuditTime")]
        [Display(Name = "AuditTime")]
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// Auditor
        /// </summary>
        [DataMember(Name = "Auditor")]
        [Display(Name = "Auditor")]
        public string Auditor { get; set; }

        /// <summary>
        /// SettlementTime
        /// </summary>
        [DataMember(Name = "SettlementTime")]
        [Display(Name = "SettlementTime")]
        public DateTime SettlementTime { get; set; }

        /// <summary>
        /// Reckoner
        /// </summary>
        [DataMember(Name = "Reckoner")]
        [Display(Name = "Reckoner")]
        public string Reckoner { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SettleOrderInfoEntity> _schema;
        static SettleOrderInfoEntity()
        {
            _schema = new ObjectSchema<SettleOrderInfoEntity>();
            _schema.AddField(x => x.SettlementCode, "SettlementCode");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.SettlementStatus, "SettlementStatus");

            _schema.AddField(x => x.SettlementType, "SettlementType");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.RmbProductTotalAmount, "RmbProductTotalAmount");

            _schema.AddField(x => x.ProductTotalAmount, "ProductTotalAmount");

            _schema.AddField(x => x.RmbProductRefundAmount, "RmbProductRefundAmount");

            _schema.AddField(x => x.ProductRefundAmount, "ProductRefundAmount");

            _schema.AddField(x => x.RmbSettlementAmount, "RmbSettlementAmount");

            _schema.AddField(x => x.SettlementAmount, "SettlementAmount");

            _schema.AddField(x => x.RmbOtherAmount, "RmbOtherAmount");

            _schema.AddField(x => x.OtherAmount, "OtherAmount");

            _schema.AddField(x => x.RmbSupplierBearDutyAmount, "RmbSupplierBearDutyAmount");

            _schema.AddField(x => x.SupplierBearDutyAmount, "SupplierBearDutyAmount");

            _schema.AddField(x => x.RmbBearDutyAmount, "RmbBearDutyAmount");

            _schema.AddField(x => x.BearDutyAmount, "BearDutyAmount");

            _schema.AddField(x => x.TradeCode, "TradeCode");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.Auditor, "Auditor");

            _schema.AddField(x => x.SettlementTime, "SettlementTime");

            _schema.AddField(x => x.Reckoner, "Reckoner");
            _schema.Compile();
        }
    }
}