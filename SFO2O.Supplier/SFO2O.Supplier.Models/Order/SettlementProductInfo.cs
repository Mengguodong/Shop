using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// SettlementProductInfo
    /// </summary>
    public class SettlementProductInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// SettlementCode
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
        /// Quantity
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// RmbUnitPrice
        /// </summary>
        [DataMember(Name = "RmbUnitPrice")]
        [Display(Name = "RmbUnitPrice")]
        public decimal RmbUnitPrice { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// RmbAmount
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
        /// RmbTaxAmount
        /// </summary>
        [DataMember(Name = "RmbTaxAmount")]
        [Display(Name = "RmbTaxAmount")]
        public decimal RmbTaxAmount { get; set; }

        /// <summary>
        /// TaxAmount
        /// </summary>
        [DataMember(Name = "TaxAmount")]
        [Display(Name = "TaxAmount")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// RmbSettlementAmount
        /// </summary>
        [DataMember(Name = "RmbSettlementAmount")]
        [Display(Name = "RmbSettlementAmount")]
        public decimal RmbSettlementAmount { get; set; }

        /// <summary>
        /// SettlementAmount
        /// </summary>
        [DataMember(Name = "SettlementAmount")]
        [Display(Name = "SettlementAmount")]
        public decimal SettlementAmount { get; set; }

        /// <summary>
        /// IsBearDuty
        /// </summary>
        [DataMember(Name = "IsBearDuty")]
        [Display(Name = "IsBearDuty")]
        public int IsBearDuty { get; set; }

        /// <summary>
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// ProductImagePath
        /// </summary>
        [DataMember(Name = "ProductImagePath")]
        [Display(Name = "ProductImagePath")]
        public string ProductImagePath { get; set; }

        /// <summary>
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        [DataMember(Name = "Commission")]
        [Display(Name = "Commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// PromotionAmount
        /// </summary>
        [DataMember(Name = "PromotionAmount")]
        [Display(Name = "PromotionAmount")]

        public decimal PromotionAmount { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SettlementProductInfo> _schema;
        static SettlementProductInfo()
        {
            _schema = new ObjectSchema<SettlementProductInfo>();
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

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.ProductName, "ProductName");

            _schema.AddField(x => x.ProductImagePath, "ProductImagePath");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.Commission, "Commission");

            _schema.AddField(x => x.PromotionAmount, "PromotionAmount");

            _schema.Compile();
        }
    }
}