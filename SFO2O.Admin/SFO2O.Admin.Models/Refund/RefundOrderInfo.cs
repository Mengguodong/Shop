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
    /// RefundOrderInfo
    /// </summary>
    public class RefundOrderInfo
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
        /// IsQualityProblem
        /// </summary>
        [DataMember(Name = "IsQualityProblem")]
        [Display(Name = "IsQualityProblem")]
        public int IsQualityProblem { get; set; }

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
        public DateTime? PickupTime { get; set; }

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
        /// CollectionCode
        /// </summary>
        [DataMember(Name = "CollectionCode")]
        [Display(Name = "CollectionCode")]
        public string CollectionCode { get; set; }

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
        /// OrderCustomsDuties
        /// </summary>
        [DataMember(Name = "OrderCustomsDuties")]
        [Display(Name = "OrderCustomsDuties")]
        public decimal OrderCustomsDuties { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundOrderInfo> _schema;
        static RefundOrderInfo()
        {
            _schema = new ObjectSchema<RefundOrderInfo>();
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

            _schema.AddField(x => x.IsQualityProblem, "IsQualityProblem");

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

            _schema.AddField(x => x.CollectionCode, "CollectionCode");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.RegionCode, "RegionCode");

            _schema.AddField(x => x.ProductStatus, "ProductStatus");

            _schema.AddField(x => x.OrderCustomsDuties, "OrderCustomsDuties");
            _schema.Compile();
        }
    }
}