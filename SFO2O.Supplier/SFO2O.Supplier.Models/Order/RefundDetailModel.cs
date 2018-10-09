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
    /// <summary>
    /// RefundDetailModel.cs
    /// </summary>
    public class RefundDetailModel
    {
        /// <summary>
        /// OrderCode
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// RefundCode
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

        /// <summary>
        /// RefundStatus
        /// </summary>
        [DataMember(Name = "RefundStatus")]
        [Display(Name = "RefundStatus")]
        public int RefundStatus { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// RefundType
        /// </summary>
        [DataMember(Name = "RefundType")]
        [Display(Name = "RefundType")]
        public int RefundType { get; set; }

        /// <summary>
        /// RefundReason
        /// </summary>
        [DataMember(Name = "RefundReason")]
        [Display(Name = "RefundReason")]
        public int RefundReason { get; set; }

        /// <summary>
        /// ProductStatus
        /// </summary>
        [DataMember(Name = "ProductStatus")]
        [Display(Name = "ProductStatus")]
        public int ProductStatus { get; set; }

        /// <summary>
        /// RefundDescription
        /// </summary>
        [DataMember(Name = "RefundDescription")]
        [Display(Name = "RefundDescription")]
        public string RefundDescription { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// AuditTime
        /// </summary>
        [DataMember(Name = "AuditTime")]
        [Display(Name = "AuditTime")]
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// PickupTime
        /// </summary>
        [DataMember(Name = "PickupTime")]
        [Display(Name = "PickupTime")]
        public DateTime? PickupTime { get; set; }

        /// <summary>
        /// CompletionTime
        /// </summary>
        [DataMember(Name = "CompletionTime")]
        [Display(Name = "CompletionTime")]
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Receiver
        /// </summary>
        [DataMember(Name = "Receiver")]
        [Display(Name = "Receiver")]
        public string Receiver { get; set; }

        /// <summary>
        /// ReceiptAddress
        /// </summary>
        [DataMember(Name = "ReceiptAddress")]
        [Display(Name = "ReceiptAddress")]
        public string ReceiptAddress { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [DataMember(Name = "Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

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
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

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
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// RMBUnitPrice
        /// </summary>
        [DataMember(Name = "RMBUnitPrice")]
        [Display(Name = "RMBUnitPrice")]
        public decimal RMBUnitPrice { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// RmbTotalAmount
        /// </summary>
        [DataMember(Name = "RmbTotalAmount")]
        [Display(Name = "RmbTotalAmount")]
        public decimal RmbTotalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }


        /// <summary>
        /// ProductImagePath
        /// </summary>
        [DataMember(Name = "ProductImagePath")]
        [Display(Name = "ProductImagePath")]
        public string ProductImagePath { get; set; }

        /// <summary>
        /// RmbDutyAmount
        /// </summary>
        [DataMember(Name = "RmbDutyAmount")]
        [Display(Name = "RmbDutyAmount")]
        public decimal RmbDutyAmount { get; set; }


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
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundDetailModel> _schema;
        static RefundDetailModel()
        {
            _schema = new ObjectSchema<RefundDetailModel>();
            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.RefundType, "RefundType");

            _schema.AddField(x => x.RefundReason, "RefundReason");

            _schema.AddField(x => x.ProductStatus, "ProductStatus");

            _schema.AddField(x => x.RefundDescription, "RefundDescription");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.PickupTime, "PickupTime");

            _schema.AddField(x => x.CompletionTime, "CompletionTime");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.UserName, "UserName");

            _schema.AddField(x => x.Receiver, "Receiver");

            _schema.AddField(x => x.ReceiptAddress, "ReceiptAddress");

            _schema.AddField(x => x.Phone, "Phone");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.RMBUnitPrice, "RMBUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.RmbTotalAmount, "RmbTotalAmount");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.ProductImagePath, "ProductImagePath");

            _schema.AddField(x => x.RmbDutyAmount, "RmbDutyAmount");

            _schema.AddField(x => x.ProvinceName, "ProvinceName");

            _schema.AddField(x => x.CityName, "CityName");

            _schema.AddField(x => x.AreaName, "AreaName");

            _schema.Compile();
        }

     
    }
}