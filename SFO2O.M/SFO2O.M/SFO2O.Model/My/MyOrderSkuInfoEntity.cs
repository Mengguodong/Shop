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
    /// MyOrderSkuInfoEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class MyOrderSkuInfoEntity
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
        public int ReceiptCountry { get; set; }

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
        /// RowsCount
        /// </summary>
        [DataMember(Name = "RowsCount")]
        [Display(Name = "RowsCount")]
        public int RowsCount { get; set; }

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
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

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
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// PayStatus
        /// </summary>
        [DataMember(Name = "PayStatus")]
        [Display(Name = "PayStatus")]
        public int PayStatus { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }


        /// <summary>
        /// RefundQuantity
        /// </summary>
        [DataMember(Name = "RefundQuantity")]
        [Display(Name = "RefundQuantity")]
        public int RefundQuantity { get; set; }

        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// 是否承担商品税 1商家承担
        /// </summary>
        [DataMember(Name = "IsBearDuty")]
        [Display(Name = "IsBearDuty")]
        public int IsBearDuty { get; set; }

        [DataMember(Name = "Commission")]
        [Display(Name = "Commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// 是否允许退货 1可以退，0不退
        /// </summary>
        [DataMember(Name = "IsReturn")]
        [Display(Name = "IsReturn")]
        public int IsReturn { get; set; }
         
            
        /// <summary>
        /// NetWeightUnit
        /// </summary>
        [DataMember(Name = "NetWeightUnit")]
        [Display(Name = "NetWeightUnit")]
        public string NetWeightUnit { get; set; }

          /// <summary>
        /// NetContentUnit
        /// </summary>
        [DataMember(Name = "NetContentUnit")]
        [Display(Name = "NetContentUnit")]
        public string NetContentUnit { get; set; }

        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }


        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }

        /// <summary>
        /// 父订单号
        /// </summary>
        public string ParentOrderCode { get; set; }

        /// <summary>
        /// [TeamCode]
        /// </summary>
        public string TeamCode { get; set; }
        /// <summary>
        /// 增加酒豆字段
        /// </summary>
        /// <summary>
        /// NetContentUnit
        /// </summary>
        [DataMember(Name = "HuoLi")]
        [Display(Name = "HuoLi")]
        public decimal HuoLi { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<MyOrderSkuInfoEntity> _schema;

        //增加税金类型
        public int TaxType { get; set; }
        /// <summary>
        /// TeamUserId
        /// </summary>
        public int TeamUserId { get; set; }
        /// <summary>
        /// TeamStatus
        /// </summary>
        public int TeamStatus { get; set; }

        public DateTime TeamStartTime { get; set; }
        public DateTime TeamEndTime { get; set; }
        /// <summary>
        /// 优惠券面值
        /// </summary>
        public decimal CardSum { get; set; }

        public int RefundStatus { get; set; }

        /// <summary>
        /// 优惠券Coupon
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 优惠券id
        /// </summary>
        public int CouponId { get; set; }
        static MyOrderSkuInfoEntity()
        {
            _schema = new ObjectSchema<MyOrderSkuInfoEntity>();

            _schema.AddField(x => x.TeamStartTime, "TeamStartTime");

            _schema.AddField(x => x.TeamEndTime, "TeamEndTime");

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

            _schema.AddField(x => x.RowsCount, "RowsCount");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.PayUnitPrice, "PayUnitPrice");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.PayStatus, "PayStatus");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.RefundQuantity, "RefundQuantity");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.Commission, "Commission");

            _schema.AddField(x=>x.IsReturn,"IsReturn");

            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");

            _schema.AddField(x => x.NetContentUnit, "NetContentUnit");

            _schema.AddField(x => x.ParentOrderCode, "ParentOrderCode");

            _schema.AddField(x => x.TaxType, "TaxType");

            _schema.AddField(x => x.TeamCode, "TeamCode");

            _schema.AddField(x => x.HuoLi, "HuoLi");

            _schema.AddField(x => x.TeamUserId, "TeamUserId");

            _schema.AddField(x => x.TeamStatus, "TeamStatus");

            _schema.AddField(x => x.CardSum, "CardSum");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");

            _schema.AddField(x => x.CouponId, "CouponId");
            
            _schema.AddField(x => x.Coupon, "Coupon");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");
            
            _schema.Compile();
        }
    }
}