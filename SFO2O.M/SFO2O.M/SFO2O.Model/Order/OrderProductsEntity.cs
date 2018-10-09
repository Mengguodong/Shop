using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Order
{
    public class OrderProductsEntity
    {
        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

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
        /// PayUnitPrice
        /// </summary>
        [DataMember(Name = "PayUnitPrice")]
        [Display(Name = "PayUnitPrice")]
        public decimal PayUnitPrice { get; set; }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// NewTaxRate
        /// </summary>
        [DataMember(Name = "NewTaxRate")]
        [Display(Name = "NewTaxRate")]
        public decimal NewTaxRate { get; set; }

        /// <summary>
        /// TaxRate
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

        public int PromotionId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal OriginalRMBPrice { get; set; }
        
        //增加父订单号
        public string ParentOrderCode { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        public decimal PayTaxAmonutRMB { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        public decimal PayTaxAmonutHKD { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        public decimal VATTaxRate { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        public decimal CBEBTaxRate { get; set; }

        public decimal ConsumerTaxRate { get; set; }

        public decimal PPATaxRate { get; set; }

        public decimal Huoli { get; set; }

        /// <summary>
        /// 2016.6.1一种sku(可能多个)分摊的优惠券金额
        /// </summary>
        public decimal GiftCard { get; set; }
        public int SFQty { get; set; }
        public int MQty { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderProductsEntity> _schema;
        static OrderProductsEntity()
        {
            _schema = new ObjectSchema<OrderProductsEntity>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.PayUnitPrice, "PayUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.NewTaxRate, "NewTaxRate");

            _schema.AddField(x => x.Commission, "Commission");

            _schema.AddField(x => x.PayAmount, "PayAmount");

            _schema.AddField(x => x.TaxAmount, "TaxAmount");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.RefundQuantity, "RefundQuantity");

            _schema.AddField(x => x.ParentOrderCode, "ParentOrderCode");

            _schema.AddField(x => x.PayTaxAmonutRMB, "PayTaxAmonutRMB");

            _schema.AddField(x => x.PayTaxAmonutHKD, "PayTaxAmonutHKD");

            _schema.AddField(x => x.VATTaxRate, "VATTaxRate");

            _schema.AddField(x => x.CBEBTaxRate, "CBEBTaxRate");

            _schema.AddField(x => x.ConsumerTaxRate, "ConsumerTaxRate");

            _schema.AddField(x => x.PPATaxRate, "PPATaxRate");

            _schema.AddField(x=>x.GiftCard,"GiftCard");

            _schema.AddField(x => x.SFQty, "SFQty");

            _schema.AddField(x => x.MQty, "MQty");

            _schema.Compile();
        }

    }
}
