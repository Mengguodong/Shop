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
    /// 结算产品表
    /// </summary>
    public class SettleProductEntity
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// 结算单号
        /// </summary>
        [DataMember(Name = "SettlementCode")]
        [Display(Name = "SettlementCode")]
        public string SettlementCode { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 单价（人民币）
        /// </summary>
        [DataMember(Name = "RmbUnitPrice")]
        [Display(Name = "RmbUnitPrice")]
        public decimal RmbUnitPrice { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// 总金额（人民币）
        /// </summary>
        [DataMember(Name = "RmbAmount")]
        [Display(Name = "RmbAmount")]
        public decimal RmbAmount { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember(Name = "Amount")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 税金（人民币）
        /// </summary>
        [DataMember(Name = "RmbTaxAmount")]
        [Display(Name = "RmbTaxAmount")]
        public decimal RmbTaxAmount { get; set; }

        /// <summary>
        /// 税金
        /// </summary>
        [DataMember(Name = "TaxAmount")]
        [Display(Name = "TaxAmount")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// 结算金额（人民币）
        /// </summary>
        [DataMember(Name = "RmbSettlementAmount")]
        [Display(Name = "RmbSettlementAmount")]
        public decimal RmbSettlementAmount { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        [DataMember(Name = "SettlementAmount")]
        [Display(Name = "SettlementAmount")]
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// 是否商家承担商品税：1：商家承担 0：商家不承担（OrderProducts.IsBearDuty）
        /// </summary>
        [DataMember(Name = "IsBearDuty")]
        [Display(Name = "IsBearDuty")]
        public int IsBearDuty { get; set; }

        [DataMember(Name = "Commission")]
        [Display(Name = "Commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SettleProductEntity> _schema;
        static SettleProductEntity()
        {
            _schema = new ObjectSchema<SettleProductEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SettlementCode, "SettlementCode");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.RmbUnitPrice, "RmbUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.RmbAmount, "RmbAmount");

            _schema.AddField(x => x.Amount, "Amount");

            _schema.AddField(x => x.RmbTaxAmount, "RmbTaxAmount");

            _schema.AddField(x => x.TaxAmount, "TaxAmount");

            _schema.AddField(x => x.RmbSettlementAmount, "RmbSettlementAmount");

            _schema.AddField(x => x.SettlementAmount, "SettlementAmount");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.Commission, "Commission");

            _schema.Compile();
        }
    }
}