using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Refund
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// 退款单列表
    /// </summary>
    public class RefundModel
    {

        /// <summary>
        /// 退款单编号
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
        public string RefundTotalAmount { get; set; }

        /// <summary>
        /// 退款总金额
        /// </summary>
        [DataMember(Name = "RMBTotalAmount")]
        [Display(Name = "RMBTotalAmount")]
        public string RMBTotalAmount { get; set; }

        

        /// <summary>
        /// 下单时商品单价
        /// </summary>
        [DataMember(Name = "unitPrice")]
        [Display(Name = "unitPrice")]
        public string unitPrice { get; set; }
        /// <summary>
        /// 下单时商品单价
        /// </summary>
        [DataMember(Name = "RMBUnitPrice")]
        [Display(Name = "RMBUnitPrice")]
        public string RMBUnitPrice { get; set; }

        /// <summary>
        /// 下单时商品商品税比例
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
        /// 订单总金额
        /// </summary>
        [DataMember(Name = "OrderTotalAmount")]
        [Display(Name = "OrderTotalAmount")]
        public string OrderTotalAmount { get; set; }

        /// <summary>
        /// sku属性名称：如颜色，尺码等
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// sku属性值：如黑色，XL
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// sku属性名称：如颜色，尺码等
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// sku属性值：如黑色，XL
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public int TotalRecord { get; set; }


        [DataMember(Name = "NetWeightUnit")]
        [Display(Name = "NetWeightUnit")]
        public string NetWeightUnit { get; set; }

        [DataMember(Name = "NetContentUnit")]
        [Display(Name = "NetContentUnit")]
        public string NetContentUnit { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundModel> _schema;
        static RefundModel()
        {
            _schema = new ObjectSchema<RefundModel>();
            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.OrderCode, "OrderCode");

            _schema.AddField(x => x.RefundStatus, "RefundStatus");

            _schema.AddField(x => x.RefundType, "RefundType");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.RefundTotalAmount, "RefundTotalAmount");

            _schema.AddField(x => x.unitPrice, "unitPrice");

            _schema.AddField(x => x.RMBUnitPrice, "RMBUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");


            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.OrderTotalAmount, "OrderTotalAmount");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.TotalRecord, "TotalRecord");

            _schema.AddField(x => x.RMBTotalAmount, "RMBTotalAmount");

            _schema.AddField(x => x.NetContentUnit, "NetContentUnit");


            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");
            _schema.Compile();
        }
    }
}