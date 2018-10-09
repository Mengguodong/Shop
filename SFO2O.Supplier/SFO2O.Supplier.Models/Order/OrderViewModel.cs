using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Supplier.Models
{
    [Serializable]
    [DataContract]
    public class Order
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
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Mobile
        /// </summary>
        [DataMember(Name = "Mobile")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember(Name = "Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

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
        public int ReceiptRegion { get; set; }

        /// <summary>
        /// ReceiptCity
        /// </summary>
        [DataMember(Name = "ReceiptCity")]
        [Display(Name = "ReceiptCity")]
        public int ReceiptCity { get; set; }

        /// <summary>
        /// ReceiptProvince
        /// </summary>
        [DataMember(Name = "ReceiptProvince")]
        [Display(Name = "ReceiptProvince")]
        public int ReceiptProvince { get; set; }

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

        /// <summary>
        /// PayCode
        /// </summary>
        [DataMember(Name = "PayCode")]
        [Display(Name = "PayCode")]
        public string PayCode { get; set; }


        /// <summary>
        /// PayPlatform
        /// </summary>
        [DataMember(Name = "PayPlatform")]
        [Display(Name = "PayPlatform")]
        public string PayPlatform { get; set; }



        /// <summary>
        /// ProvinceName
        /// </summary>
        [DataMember(Name = "ProvinceName")]
        [Display(Name = "ProvinceName")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// CityName
        /// </summary>
        [DataMember(Name = "CityName")]
        [Display(Name = "CityName")]
        public string CityName { get; set; }

        /// <summary>
        /// AreaName
        /// </summary>
        [DataMember(Name = "AreaName")]
        [Display(Name = "AreaName")]
        public string AreaName { get; set; }

        /// <summary>
        /// ExpressList  
        /// </summary>
        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }

        /// <summary>
        /// ExpressList  
        /// </summary>
        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }   

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<Order> _schema;
        static Order()
        {
            _schema = new ObjectSchema<Order>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.Mobile, "Mobile");

            _schema.AddField(x => x.Email, "Email");

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

            _schema.AddField(x => x.PayCode, "PayCode");

            _schema.AddField(x => x.PayPlatform, "PayPlatform");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");

            _schema.Compile();
        }



    }

    [Serializable]
    [DataContract]
    public class OrderProduct
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
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

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
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

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
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// ProductImagePath
        /// </summary>
        [DataMember(Name = "ProductImagePath")]
        [Display(Name = "ProductImagePath")]
        public string ProductImagePath { get; set; }

        /// <summary>
        /// PromotionPrice
        /// </summary>
        [DataMember(Name = "PromotionPrice")]
        [Display(Name = "PromotionPrice")]
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderProduct> _schema;
        static OrderProduct()
        {
            _schema = new ObjectSchema<OrderProduct>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.Spu, "Spu");     // Name

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.PayAmount, "PayAmount");

            _schema.AddField(x => x.TaxAmount, "TaxAmount");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.RefundQuantity, "RefundQuantity");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.ProductImagePath, "ProductImagePath");

            _schema.AddField(x => x.PromotionPrice, "PromotionPrice");

            _schema.Compile();
        }

    }

    [Serializable]
    [DataContract]
    public class OrderLogistics
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }


        /// <summary>
        /// ExpressCompany
        /// </summary>
        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }


        /// <summary>
        /// ExpressList
        /// </summary>
        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }

        /// <summary>
        /// LogisticsTime
        /// </summary>
        [DataMember(Name = "LogisticsTime")]
        [Display(Name = "LogisticsTime")]
        public DateTime LogisticsTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DataMember(Name = "Remark")]
        [Display(Name = "Remark")]
        public string Remark { get; set; }

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
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderLogistics> _schema;
        static OrderLogistics()
        {
            _schema = new ObjectSchema<OrderLogistics>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.LogisticsTime, "LogisticsTime");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.Remark, "Remark");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.Compile();
        }
    }

    public class OrderViewModel
    {
        public Order order { get; set; }

        public IList<OrderProduct> orderProducts { get; set; }

    }


}
