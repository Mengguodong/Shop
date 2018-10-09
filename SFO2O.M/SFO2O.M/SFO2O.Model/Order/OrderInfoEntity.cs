using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Order
{
    [Serializable]
    [DataContract]
    public class OrderInfoEntity
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
        /// Receiver
        /// </summary>
        [DataMember(Name = "Receiver")]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [DataMember(Name = "Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// PassPortType
        /// </summary>
        [DataMember(Name = "PassPortType")]
        [Display(Name = "PassPortType")]
        public int PassPortType { get; set; }

        /// <summary>
        /// PassPortNum
        /// </summary>
        [DataMember(Name = "PassPortNum")]
        [Display(Name = "PassPortNum")]
        public string PassPortNum { get; set; }

        /// <summary>
        /// ReceiptAddress
        /// </summary>
        [DataMember(Name = "ReceiptAddress")]
        [Display(Name = "ReceiptAddress")]
        public string ReceiptAddress { get; set; }

        /// <summary>
        /// ReceiptPostalCode
        /// </summary>
        [DataMember(Name = "ReceiptPostalCode")]
        [Display(Name = "ReceiptPostalCode")]
        public string ReceiptPostalCode { get; set; }

        /// <summary>
        /// ReceiptRegion
        /// </summary>
        [DataMember(Name = "ReceiptRegion")]
        [Display(Name = "ReceiptRegion")]
        public string ReceiptRegion { get; set; }

        /// <summary>
        /// ReceiptCity
        /// </summary>
        [DataMember(Name = "ReceiptCity")]
        [Display(Name = "ReceiptCity")]
        public string ReceiptCity { get; set; }

        /// <summary>
        /// ReceiptProvince
        /// </summary>
        [DataMember(Name = "ReceiptProvince")]
        [Display(Name = "ReceiptProvince")]
        public string ReceiptProvince { get; set; }

        /// <summary>
        /// ReceiptCountry
        /// </summary>
        [DataMember(Name = "ReceiptCountry")]
        [Display(Name = "ReceiptCountry")]
        public string ReceiptCountry { get; set; }

        /// <summary>
        /// ShippingMethod
        /// </summary>
        [DataMember(Name = "ShippingMethod")]
        [Display(Name = "ShippingMethod")]
        public int ShippingMethod { get; set; }

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
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// DeliveryTime
        /// </summary>
        [DataMember(Name = "DeliveryTime")]
        [Display(Name = "DeliveryTime")]
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// ArrivalTime
        /// </summary>
        [DataMember(Name = "ArrivalTime")]
        [Display(Name = "ArrivalTime")]
        public DateTime? ArrivalTime { get; set; }

        /// <summary>
        /// OrderCompletionTime
        /// </summary>
        [DataMember(Name = "OrderCompletionTime")]
        [Display(Name = "OrderCompletionTime")]
        public DateTime? OrderCompletionTime { get; set; }

        /// <summary>
        /// CancelReason
        /// </summary>
        [DataMember(Name = "CancelReason")]
        [Display(Name = "CancelReason")]
        public string CancelReason { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DataMember(Name = "Remark")]
        [Display(Name = "Remark")]
        public string Remark { get; set; }

        //增加父订单号
        public string ParentOrderCode { get; set; }

        /// <summary>
        /// GatewayCode
        /// </summary>
        public int GatewayCode { get; set; }

        /// <summary>
        /// TaxType
        /// </summary>
        public int TaxType { get; set; }

        //增加团编码
        public string TeamCode { get; set; }

        //增加优惠券
        public decimal Coupon { get; set; }

        public decimal Huoli { get; set; }

        public int OrderSourceType { get; set; }

        public string OrderSourceValue { get; set; }

        public decimal DividedAmount { get; set; }

        public decimal DividedPercent { get; set; }

        public decimal ChangedHuoLi { get; set; }

        public decimal OriginalHuoLi { get; set; }

        public decimal CurrentHuoLi { get; set; }

        public int PayPlatform { get; set; }

        /// <summary>
        /// 2016.6.1 优惠券ID，如果使用了的话会传过来券ID
        /// </summary>
        public int GiftCardId { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderInfoEntity> _schema;
        static OrderInfoEntity()
        {
            _schema = new ObjectSchema<OrderInfoEntity>();
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

            _schema.AddField(x => x.Receiver, "Receiver");

            _schema.AddField(x => x.Phone, "Phone");

            _schema.AddField(x => x.PassPortType, "PassPortType");

            _schema.AddField(x => x.PassPortNum, "PassPortNum");

            _schema.AddField(x => x.ReceiptAddress, "ReceiptAddress");

            _schema.AddField(x => x.ReceiptPostalCode, "ReceiptPostalCode");

            _schema.AddField(x => x.ReceiptRegion, "ReceiptRegion");

            _schema.AddField(x => x.ReceiptCity, "ReceiptCity");

            _schema.AddField(x => x.ReceiptProvince, "ReceiptProvince");

            _schema.AddField(x => x.ReceiptCountry, "ReceiptCountry");

            _schema.AddField(x => x.ShippingMethod, "ShippingMethod");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.PayTime, "PayTime");

            _schema.AddField(x => x.DeliveryTime, "DeliveryTime");

            _schema.AddField(x => x.ArrivalTime, "ArrivalTime");

            _schema.AddField(x => x.OrderCompletionTime, "OrderCompletionTime");

            _schema.AddField(x => x.CancelReason, "CancelReason");

            _schema.AddField(x => x.Remark, "Remark");

            _schema.AddField(x => x.ParentOrderCode, "ParentOrderCode");

            _schema.AddField(x => x.GatewayCode, "GatewayCode");

            _schema.AddField(x => x.TaxType, "TaxType");

            _schema.AddField(x => x.TeamCode, "TeamCode");

            _schema.AddField(x => x.Coupon, "Coupon");

            _schema.AddField(x => x.Huoli, "Huoli");

            _schema.AddField(x => x.OrderSourceType, "OrderSourceType");
            _schema.AddField(x => x.OrderSourceValue, "OrderSourceValue");
            _schema.AddField(x => x.DividedAmount, "DividedAmount");
            _schema.AddField(x => x.DividedPercent, "DividedPercent");
            _schema.AddField(x => x.ChangedHuoLi, "ChangedHuoLi");
            _schema.AddField(x => x.OriginalHuoLi, "OriginalHuoLi");
            _schema.AddField(x => x.CurrentHuoLi, "CurrentHuoLi");
            _schema.AddField(x => x.PayPlatform, "PayPlatform");

            _schema.Compile();
        }

    }
}