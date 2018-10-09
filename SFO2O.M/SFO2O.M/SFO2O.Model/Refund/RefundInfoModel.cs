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
    /// 退款/退货详情model
    /// </summary>
    public class RefundInfoModel
    {

        /// <summary>
        /// 退款单号
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [DataMember(Name = "OrderCode")]
        [Display(Name = "OrderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 退款状态：-1：作废 1：待审核 2：上门取件 3：待退款 4：退款成功 5：退款关闭
        /// </summary>
        [DataMember(Name = "RefundStatus")]
        [Display(Name = "RefundStatus")]
        public int RefundStatus { get; set; }

        /// <summary>
        /// 退款类型：1：退货 2：退款
        /// </summary>
        [DataMember(Name = "RefundType")]
        [Display(Name = "RefundType")]
        public int RefundType { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 退款总金额
        /// </summary>
        [DataMember(Name = "RefundTotalAmount")]
        [Display(Name = "RefundTotalAmount")]
        public decimal RefundTotalAmount { get; set; }

        /// <summary>
        /// 退款总金额
        /// </summary>
        [DataMember(Name = "RMBRefundTotalAmount")]
        [Display(Name = "RMBRefundTotalAmount")]
        public decimal RMBRefundTotalAmount { get; set; }


        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// 下单时商品单价
        /// </summary>
        [DataMember(Name = "unitPrice")]
        [Display(Name = "unitPrice")]
        public decimal unitPrice { get; set; }

        /// <summary>
        /// 下单时商品商品税税率
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// sku属性名称
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// sku属性值
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// sku属性名称
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// sku属性值
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        [DataMember(Name = "RefundReason")]
        [Display(Name = "RefundReason")]
        public int RefundReason { get; set; }

        /// <summary>
        /// 退款详情描述
        /// </summary>
        [DataMember(Name = "RefundDescription")]
        [Display(Name = "RefundDescription")]
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款凭证图片
        /// </summary>
        [DataMember(Name = "RefundImagePath")]
        [Display(Name = "RefundImagePath")]
        public string RefundImagePath { get; set; }

        /// <summary>
        /// 申请退款时间
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
        public DateTime? PickupTime { get; set; }

       


        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// 商品税总金额
        /// </summary>
        [DataMember(Name = "DutyAmount")]
        [Display(Name = "DutyAmount")]
        public decimal DutyAmount { get; set; }

        /// <summary>
        /// 商品税总金额
        /// </summary>
        [DataMember(Name = "RmbDutyAmount")]
        [Display(Name = "RmbDutyAmount")]
        public decimal RmbDutyAmount { get; set; }

        /// <summary>
        /// 服务费（平台扣点）
        /// </summary>
        [DataMember(Name = "Commission")]
        [Display(Name = "Commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// 拒绝退款原因
        /// </summary>
        [DataMember(Name = "NoPassReason")]
        [Display(Name = "NoPassReason")]
        public string NoPassReason { get; set; }

        [DataMember(Name = "ProductStatus")]
        [Display(Name = "ProductStatus")]
        public int ProductStatus { get; set; }

        /// <summary>
        /// 退款关闭时间
        /// </summary>
        [DataMember(Name = "CompletionTime")]
        [Display(Name = "CompletionTime")]
        public DateTime CompletionTime { get; set; }

        [DataMember(Name = "ToBePickUpTime")]
        [Display(Name = "ToBePickUpTime")]
        public DateTime ToBePickUpTime { get; set; }

        public decimal HuoLi { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        [DataMember(Name = "Coupon")]
        [Display(Name = "Coupon")]
        public decimal Coupon { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundInfoModel> _schema;
        static RefundInfoModel()
        {
            _schema = new ObjectSchema<RefundInfoModel>();
            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");

            _schema.AddField(x => x.RefundType, "RefundType");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.RefundTotalAmount, "RefundTotalAmount");

            _schema.AddField(x => x.RMBRefundTotalAmount, "RMBRefundTotalAmount");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.unitPrice, "unitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.RefundReason, "RefundReason");

            _schema.AddField(x => x.RefundDescription, "RefundDescription");

            _schema.AddField(x => x.RefundImagePath, "RefundImagePath");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.PickupTime, "PickupTime");

            

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.DutyAmount, "DutyAmount");

            _schema.AddField(x => x.RmbDutyAmount, "RmbDutyAmount");

            _schema.AddField(x=>x.Commission,"Commission");

            

            _schema.AddField(x=>x.NoPassReason,"NoPassReason");

            _schema.AddField(x => x.ProductStatus, "ProductStatus");

            _schema.AddField(x => x.CompletionTime, "CompletionTime");

            _schema.AddField(x => x.ToBePickUpTime, "ToBePickUpTime");

            _schema.AddField(x => x.HuoLi, "HuoLi");

            _schema.AddField(x => x.Coupon, "Coupon");
            _schema.Compile();
        }
    }
}
