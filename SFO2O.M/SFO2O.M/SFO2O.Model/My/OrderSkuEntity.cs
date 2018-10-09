using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.My
{
    /// <summary>
    /// OrderSkuEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class OrderSkuEntity
    {

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// OrderStatus
        /// </summary>
        [DataMember(Name = "OrderStatus")]
        [Display(Name = "OrderStatus")]
        public int OrderStatus { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Freight
        /// </summary>
        [DataMember(Name = "Freight")]
        [Display(Name = "Freight")]
        public decimal Freight { get; set; }

        /// <summary>
        /// ProductTotalAmount
        /// </summary>
        [DataMember(Name = "ProductTotalAmount")]
        [Display(Name = "ProductTotalAmount")]
        public decimal ProductTotalAmount { get; set; }

        /// <summary>
        /// CustomsDuties
        /// </summary>
        [DataMember(Name = "CustomsDuties")]
        [Display(Name = "CustomsDuties")]
        public decimal CustomsDuties { get; set; }

        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember(Name = "PaidAmount")]
        [Display(Name = "PaidAmount")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        [DataMember(Name = "PayType")]
        [Display(Name = "PayType")]
        public int PayType { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// PayTime
        /// </summary>
        [DataMember(Name = "PayTime")]
        [Display(Name = "PayTime")]
        public DateTime PayTime { get; set; }

        /// <summary>
        /// DeliveryTime
        /// </summary>
        [DataMember(Name = "DeliveryTime")]
        [Display(Name = "DeliveryTime")]
        public DateTime DeliveryTime { get; set; }

        /// <summary>
        /// ArrivalTime
        /// </summary>
        [DataMember(Name = "ArrivalTime")]
        [Display(Name = "ArrivalTime")]
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// OrderCompletionTime
        /// </summary>
        [DataMember(Name = "OrderCompletionTime")]
        [Display(Name = "OrderCompletionTime")]
        public DateTime OrderCompletionTime { get; set; }

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
        /// PayUnitPrice
        /// </summary>
        [DataMember(Name = "PayUnitPrice")]
        [Display(Name = "PayUnitPrice")]
        public decimal PayUnitPrice { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        [DataMember(Name = "Commission")]
        [Display(Name = "Commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// PayAmount
        /// </summary>
        [DataMember(Name = "PayAmount")]
        [Display(Name = "PayAmount")]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// TaxAmount
        /// </summary>
        [DataMember(Name = "TaxAmount")]
        [Display(Name = "TaxAmount")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// IsBearDuty
        /// </summary>
        [DataMember(Name = "IsBearDuty")]
        [Display(Name = "IsBearDuty")]
        public int IsBearDuty { get; set; }

        /// <summary>
        /// RefundQuantity
        /// </summary>
        [DataMember(Name = "RefundQuantity")]
        [Display(Name = "RefundQuantity")]
        public int RefundQuantity { get; set; }

        /// <summary>
        /// PayStatus
        /// </summary>
        [DataMember(Name = "PayStatus")]
        [Display(Name = "PayStatus")]
        public int PayStatus { get; set; }

        /// <summary>
        /// TradeCode
        /// </summary>
        [DataMember(Name = "TradeCode")]
        [Display(Name = "TradeCode")]
        public string TradeCode { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }
        /// <summary>
        /// PromotionId
        /// </summary>
        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

        /// <summary>
        /// PromotionPrice
        /// </summary>
        [DataMember(Name = "PromotionPrice")]
        [Display(Name = "PromotionPrice")]
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// OriginalPrice
        /// </summary>
        [DataMember(Name = "OriginalPrice")]
        [Display(Name = "OriginalPrice")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// OriginalRMBPrice
        /// </summary>
        [DataMember(Name = "OriginalRMBPrice")]
        [Display(Name = "OriginalRMBPrice")]
        public decimal OriginalRMBPrice { get; set; }

        /// <summary>
        /// PromotionCost
        /// </summary>
        [DataMember(Name = "PromotionCost")]
        [Display(Name = "PromotionCost")]
        public int PromotionCost { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderSkuEntity> _schema;
        static OrderSkuEntity()
        {
            _schema = new ObjectSchema<OrderSkuEntity>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.OrderStatus, "OrderStatus");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.Freight, "Freight");

            _schema.AddField(x => x.ProductTotalAmount, "ProductTotalAmount");

            _schema.AddField(x => x.CustomsDuties, "CustomsDuties");

            _schema.AddField(x => x.PaidAmount, "PaidAmount");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.PayType, "PayType");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.PayTime, "PayTime");

            _schema.AddField(x => x.DeliveryTime, "DeliveryTime");

            _schema.AddField(x => x.ArrivalTime, "ArrivalTime");

            _schema.AddField(x => x.OrderCompletionTime, "OrderCompletionTime");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.PayUnitPrice, "PayUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.Commission, "Commission");

            _schema.AddField(x => x.PayAmount, "PayAmount");

            _schema.AddField(x => x.TaxAmount, "TaxAmount");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.RefundQuantity, "RefundQuantity");

            _schema.AddField(x => x.PayStatus, "PayStatus");

            _schema.AddField(x => x.TradeCode, "TradeCode");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.PromotionId, "PromotionId");

            _schema.AddField(x => x.PromotionPrice, "PromotionPrice");

            _schema.AddField(x => x.OriginalPrice, "OriginalPrice");

            _schema.AddField(x => x.OriginalRMBPrice, "OriginalRMBPrice");

            _schema.AddField(x => x.PromotionCost, "PromotionCost");

            _schema.Compile();
        }
    }
}