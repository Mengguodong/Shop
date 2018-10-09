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
    /// RefundProductEntity
    /// </summary>
    public class RefundProductEntity
    {

        /// <summary>
        /// 自增id 主键
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

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
        /// 退货数量
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 商品单价（人民币）
        /// </summary>
        [DataMember(Name = "RMBUnitPrice")]
        [Display(Name = "RMBUnitPrice")]
        public decimal RMBUnitPrice { get; set; }

        /// <summary>
        /// 下单时税率
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// 是否承担商品税 1商家承担
        /// </summary>
        [DataMember(Name = "IsBearDuty")]
        [Display(Name = "IsBearDuty")]
        public int IsBearDuty { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

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

        /// <summary>
        /// 商品图片
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        [DataMember(Name = "NetWeightUnit")]
        [Display(Name = "NetWeightUnit")]
        public string NetWeightUnit { get; set; }

        [DataMember(Name = "NetContentUnit")]
        [Display(Name = "NetContentUnit")]
        public string NetContentUnit { get; set; }
        /// <summary>
        /// 酒豆
        /// </summary>
        public decimal HuoLi { get; set; }

        public decimal Coupon { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundProductEntity> _schema;
        static RefundProductEntity()
        {
            _schema = new ObjectSchema<RefundProductEntity>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.RMBUnitPrice, "RMBUnitPrice");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.NetContentUnit, "NetContentUnit");

            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");

            _schema.AddField(x => x.HuoLi, "HuoLi");

            _schema.AddField(x => x.Coupon, "Coupon");
            _schema.Compile();
        }
    }
}