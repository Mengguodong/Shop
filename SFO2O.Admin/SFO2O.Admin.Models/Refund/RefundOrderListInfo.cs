using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Refund
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// RefundOrderListInfo
    /// </summary>
    public class RefundOrderListInfo
    {

        /// <summary>
        /// RefundCode
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

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
        /// RefundDescription
        /// </summary>
        [DataMember(Name = "RefundDescription")]
        [Display(Name = "RefundDescription")]
        public string RefundDescription { get; set; }

        /// <summary>
        /// RefundStatus
        /// </summary>
        [DataMember(Name = "RefundStatus")]
        [Display(Name = "RefundStatus")]
        public int RefundStatus { get; set; }

        /// <summary>
        /// RmbTotalAmount
        /// </summary>
        [DataMember(Name = "RmbTotalAmount")]
        [Display(Name = "RmbTotalAmount")]
        public decimal RmbTotalAmount { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// RmbDutyAmount
        /// </summary>
        [DataMember(Name = "RmbDutyAmount")]
        [Display(Name = "RmbDutyAmount")]
        public decimal RmbDutyAmount { get; set; }

        /// <summary>
        /// DutyAmount
        /// </summary>
        [DataMember(Name = "DutyAmount")]
        [Display(Name = "DutyAmount")]
        public decimal DutyAmount { get; set; }

        /// <summary>
        /// IsReturnDuty
        /// </summary>
        [DataMember(Name = "IsReturnDuty")]
        [Display(Name = "IsReturnDuty")]
        public int IsReturnDuty { get; set; }

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
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// TobePickUpTime
        /// </summary>
        [DataMember(Name = "TobePickUpTime")]
        [Display(Name = "TobePickUpTime")]
        public DateTime TobePickUpTime { get; set; }

        /// <summary>
        /// PickupTime
        /// </summary>
        [DataMember(Name = "PickupTime")]
        [Display(Name = "PickupTime")]
        public DateTime PickupTime { get; set; }

        /// <summary>
        /// CompletionTime
        /// </summary>
        [DataMember(Name = "CompletionTime")]
        [Display(Name = "CompletionTime")]
        public DateTime CompletionTime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// Auditor
        /// </summary>
        [DataMember(Name = "Auditor")]
        [Display(Name = "Auditor")]
        public string Auditor { get; set; }

        /// <summary>
        /// Pickupper
        /// </summary>
        [DataMember(Name = "Pickupper")]
        [Display(Name = "Pickupper")]
        public string Pickupper { get; set; }

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
        /// NoPassReason
        /// </summary>
        [DataMember(Name = "NoPassReason")]
        [Display(Name = "NoPassReason")]
        public string NoPassReason { get; set; }

        /// <summary>
        /// CancelReason
        /// </summary>
        [DataMember(Name = "CancelReason")]
        [Display(Name = "CancelReason")]
        public string CancelReason { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// RegionCode
        /// </summary>
        [DataMember(Name = "RegionCode")]
        [Display(Name = "RegionCode")]
        public int RegionCode { get; set; }

        /// <summary>
        /// ProductStatus
        /// </summary>
        [DataMember(Name = "ProductStatus")]
        [Display(Name = "ProductStatus")]
        public int ProductStatus { get; set; }

        /// <summary>
        /// BuyerName
        /// </summary>
        [DataMember(Name = "BuyerName")]
        [Display(Name = "BuyerName")]
        public string BuyerName { get; set; }

        /// <summary>
        /// SellerName
        /// </summary>
        [DataMember(Name = "SellerName")]
        [Display(Name = "SellerName")]
        public string SellerName { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// ProductImgPath
        /// </summary>
        [DataMember(Name = "ProductImgPath")]
        [Display(Name = "ProductImgPath")]
        public string ProductImgPath { get; set; }

        /// <summary>
        /// RmbProductAmount
        /// </summary>
        [DataMember(Name = "RmbProductAmount")]
        [Display(Name = "RmbProductAmount")]
        public decimal RmbProductAmount { get; set; }

        /// <summary>
        /// ProductAmount
        /// </summary>
        [DataMember(Name = "ProductAmount")]
        [Display(Name = "ProductAmount")]
        public decimal ProductAmount { get; set; }

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
        /// TotalCount
        /// </summary>
        [DataMember(Name = "TotalCount")]
        [Display(Name = "TotalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// ProductQuantity
        /// </summary>
        [DataMember(Name = "ProductQuantity")]
        [Display(Name = "ProductQuantity")]
        public int ProductQuantity { get; set; }

        /// <summary>
        /// IsReturn
        /// </summary>
        [DataMember(Name = "IsReturn")]
        [Display(Name = "IsReturn")]
        public int IsReturn { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundOrderListInfo> _schema;
        static RefundOrderListInfo()
        {
            _schema = new ObjectSchema<RefundOrderListInfo>();
            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.RefundType, "RefundType");

            _schema.AddField(x => x.RefundReason, "RefundReason");

            _schema.AddField(x => x.RefundDescription, "RefundDescription");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");

            _schema.AddField(x => x.RmbTotalAmount, "RmbTotalAmount");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.RmbDutyAmount, "RmbDutyAmount");

            _schema.AddField(x => x.DutyAmount, "DutyAmount");

            _schema.AddField(x => x.IsReturnDuty, "IsReturnDuty");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.TobePickUpTime, "TobePickUpTime");

            _schema.AddField(x => x.PickupTime, "PickupTime");

            _schema.AddField(x => x.CompletionTime, "CompletionTime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.Auditor, "Auditor");

            _schema.AddField(x => x.Pickupper, "Pickupper");

            _schema.AddField(x => x.ExpressCompany, "ExpressCompany");

            _schema.AddField(x => x.ExpressList, "ExpressList");

            _schema.AddField(x => x.NoPassReason, "NoPassReason");

            _schema.AddField(x => x.CancelReason, "CancelReason");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.RegionCode, "RegionCode");

            _schema.AddField(x => x.ProductStatus, "ProductStatus");

            _schema.AddField(x => x.BuyerName, "BuyerName");

            _schema.AddField(x => x.SellerName, "SellerName");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.ProductName, "ProductName");

            _schema.AddField(x => x.ProductImgPath, "ProductImgPath");

            _schema.AddField(x => x.RmbProductAmount, "RmbProductAmount");

            _schema.AddField(x => x.ProductAmount, "ProductAmount");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.TotalCount, "TotalCount");

            _schema.AddField(x => x.ProductQuantity, "ProductQuantity");

            _schema.AddField(x => x.IsReturn, "IsReturn");

            _schema.Compile();
        }
    }
}