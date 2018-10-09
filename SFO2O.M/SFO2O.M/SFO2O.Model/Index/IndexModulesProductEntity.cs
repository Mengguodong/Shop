using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Index
{
    /// <summary>
    /// 首页CMS展示的商品通用实体（包括所在分类）
    /// </summary>
    [Serializable]
    [DataContract]
    public class IndexModulesProductEntity
    {
        /// <summary>
        /// 商品Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 商品分类ID
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        [DataMember(Name = "CategoryName")]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 商品小模块：分类顺序
        /// </summary>
        [DataMember(Name = "IndexValue")]
        [Display(Name = "IndexValue")]
        public int IndexValue { get; set; }

        /// <summary>
        /// 商品小模块内部某个分类内部：产品顺序
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 折扣比例
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 商品最小价
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// 商品图片地址
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// 商品SPU下  Sum(sku库存) - SPU设置的MinForOrder数量,大于0即有库存，小于=0即为已售罄
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<IndexModulesProductEntity> _schema;
        static IndexModulesProductEntity()
        {
            _schema = new ObjectSchema<IndexModulesProductEntity>();

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.CategoryName, "CategoryName");

            _schema.AddField(x => x.IndexValue, "IndexValue");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x=>x.Qty,"Qty");

            _schema.Compile();
        }
    }
}
