using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Refund
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// 退款单实体
    /// </summary>
    public class RefundOrderEntity
    {

        /// <summary>
        /// 退款单号
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember(Name = "UserId")]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// 退款类型：1：退货 2：退款
        /// </summary>
        [DataMember(Name = "RefundType")]
        [Display(Name = "RefundType")]
        public int RefundType { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        [DataMember(Name = "RefundReason")]
        [Display(Name = "RefundReason")]
        public int RefundReason { get; set; }

        /// <summary>
        /// 退款详细描述
        /// </summary>
        [DataMember(Name = "RefundDescription")]
        [Display(Name = "RefundDescription")]
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款状态：-1：作废 1：待审核 2：上门取件 3：待退款 4：退款成功 5：退款关闭
        /// </summary>
        [DataMember(Name = "RefundStatus")]
        [Display(Name = "RefundStatus")]
        public int RefundStatus { get; set; }

        /// <summary>
        /// 退款总金额
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 退款总金额（人民币）
        /// </summary>
        [DataMember(Name = "RMBTotalAmount")]
        [Display(Name = "RMBTotalAmount")]
        public decimal RMBTotalAmount { get; set; }

        /// <summary>
        /// 退款申请时间
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [DataMember(Name = "AuditTime")]
        [Display(Name = "AuditTime")]
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 取件时间
        /// </summary>
        [DataMember(Name = "PickupTime")]
        [Display(Name = "PickupTime")]
        public DateTime PickupTime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [DataMember(Name = "Auditor")]
        [Display(Name = "Auditor")]
        public string Auditor { get; set; }

        /// <summary>
        /// 取件人
        /// </summary>
        [DataMember(Name = "Pickupper")]
        [Display(Name = "Pickupper")]
        public string Pickupper { get; set; }

        /// <summary>
        /// 物流公司
        /// </summary>
        [DataMember(Name = "ExpressCompany")]
        [Display(Name = "ExpressCompany")]
        public string ExpressCompany { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [DataMember(Name = "ExpressList")]
        [Display(Name = "ExpressList")]
        public string ExpressList { get; set; }

        /// <summary>
        /// 审核未通过原因
        /// </summary>
        [DataMember(Name = "NoPassReason")]
        [Display(Name = "NoPassReason")]
        public string NoPassReason { get; set; }

        /// <summary>
        ///取消退款原因
        /// </summary>
        [DataMember(Name = "CancelReason")]
        [Display(Name = "CancelReason")]
        public string CancelReason { get; set; }

        /// <summary>
        /// 退款凭证图片
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// 下单时汇率
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
        /// 区域代码
        /// </summary>
        [DataMember(Name = "RegionCode")]
        [Display(Name = "RegionCode")]
        public int RegionCode { get; set; }

        /// <summary>
        /// 商品状态：1未开封，2已开封未使用，3已开封已使用
        /// </summary>
        [DataMember(Name="ProductStatus")]
        [Display(Name="ProductStatus")]
        public int ProductStatus { get; set; }

        [DataMember(Name = "Commision")]
        [Display(Name = "Commision")]
        public decimal Commision { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundOrderEntity> _schema;
        static RefundOrderEntity()
        {
            _schema = new ObjectSchema<RefundOrderEntity>();
            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.UserId, "UserId");

            _schema.AddField(x => x.RefundType, "RefundType");

            _schema.AddField(x => x.RefundReason, "RefundReason");

            _schema.AddField(x => x.RefundDescription, "RefundDescription");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.RMBTotalAmount, "RMBTotalAmount");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.PickupTime, "PickupTime");

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

            _schema.AddField(x => x.Commision, "Commision");

            _schema.AddField(x => x.ProductStatus, "ProductStatus");

            _schema.Compile();
        }
    }
}