using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Product
{
    [DataContract]
    public class ProductInfoModel
    {

        [DataMember(Name = "ProductId")]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [DataMember(Name = "SPU")]
        [Display(Name = "SPU")]
        public string SPU { get; set; }

        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [DataMember(Name = "Brand")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// 产地
        /// </summary>
        [DataMember(Name = "CountryOfManufacture")]
        [Display(Name = "CountryOfManufacture")]
        public string CountryOfManufacture { get; set; }
        /// <summary>
        /// 商品图片
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
        /// 分类id
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        [DataMember(Name = "ParentCategoryId")]
        [Display(Name = "ParentCategoryId")]
        public int ParentCategoryId { get; set; }
        /// <summary>
        /// 是否是节日 1 是，0 不是
        /// </summary>
        public int IsHolidayGoods
        {
            get 
            {
                return ParentCategoryId == ConfigHelper.HolidayCategoryId?1:0;
            }
        }


        /// <summary>
        /// spu库存量
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }
        /// <summary>
        /// 真实库存量
        /// </summary>
        public int ForOrderQty { get; set; }

        /// <summary>
        /// 是否在售
        /// </summary>
        [DataMember(Name = "IsOnSaled")]
        [Display(Name = "IsOnSaled")]
        public int IsOnSaled { get; set; }

        /// <summary>
        /// sku数量
        /// </summary>
        [DataMember(Name = "SkuCount")]
        [Display(Name = "SkuCount")]
        public int SkuCount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "TotalRecord")]
        [Display(Name = "TotalRecord")]
        public int TotalRecord { get; set; }

        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        [DataMember(Name = "SkuList")]
        [Display(Name = "SkuList")]
        public List<string> SkuList { get; set; }

        /// <summary>
        /// 折扣比例
        /// </summary>
        [DataMember(Name = "DiscountRate")]
        [Display(Name = "DiscountRate")]
        public string DiscountRate { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }

        ///<summary>
        ///促销类型
        ///<summary>
        public int PromotionType { get; set; }

        ///<summary>
        ///status
        ///<summary>
        public int PromotionStatus { get; set; }

        ///<summary>
        ///拼团 团人数
        ///<summary>
        public int TuanNumbers { get; set; }
        ///<summary>
        ///Size
        ///<summary>
        public string Size { get; set; }
        ///<summary>
        ///Color
        ///<summary>
        public string Color { get; set; }
        ///<summary>
        ///NetWeight
        ///<summary>
        public decimal NetWeight { get; set; }
        ///<summary>
        ///NetWeightUnit
        ///<summary>
        public string NetWeightUnit { get; set; }

        ///<summary>
        ///Logo
        ///<summary>
        public string Logo { get; set; }

        ///<summary>
        /// 是否报关
        ///<summary>
        public int ReportStatus { get; set; }

        ///<summary>
        /// 是否商品税
        ///<summary>
        public int IsCrossBorderEBTax { get; set; }

        ///<summary>
        /// 
        ///<summary>
        public decimal CBEBTaxRate { get; set; }
        ///<summary>
        /// 
        ///<summary>
        public decimal ConsumerTaxRate { get; set; }
        ///<summary>
        /// 
        ///<summary>
        public decimal VATTaxRate { get; set; }
        ///<summary>
        /// 
        ///<summary>
        public decimal PPATaxRate { get; set; }
        ///<summary>
        /// 2:行邮税 1：综合税
        ///<summary>
        public int realTaxType { get; set; }
        ///<summary>
        /// 税的钱
        ///<summary>
        public decimal taxPrice { get; set; }
        ///<summary>
        /// 拼团的税费
        ///<summary>
        public decimal minRatePrice { get; set; }

        ///<summary>
        /// fiSpu  这件商品是否收藏 
        ///<summary>
        public string fiSpu { get; set; }
       /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductInfoModel> _schema;

        /// <summary>
        /// 商品图片
        /// </summary>
        public IList<ProductInfoModel> Images { get; set; }
        /// <summary>
        /// pid 活动id
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// MainDicValue
        /// </summary>
        public string MainDicValue { get; set; }
        /// <summary>
        /// MainValue
        /// </summary>
        public string MainValue { get; set; }
        /// <summary>
        /// SubDicValue
        /// </summary>
        public string SubDicValue { get; set; }
        /// <summary>
        /// SubValue
        /// </summary>
        public string SubValue { get; set; }

        public int IsDutyOnSeller { get; set; }
        public int minRealTaxType { get; set; }
        public string BrandEN { get; set; }
        public decimal HuoLi { get; set; }

        public decimal Coupon { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 国旗
        /// </summary>
        public string NationalFlag { get; set; }
        /// <summary>
        /// 比较是否已售罄
        /// </summary>
        public int compare { get; set; }
        /// <summary>
        /// Description 描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 折扣价
        /// </summary>
        [DataMember(Name = "DHuoli")]
        [Display(Name = "DHuoli")]
        public decimal DHuoli { get; set; }
        static ProductInfoModel()
        {
            _schema = new ObjectSchema<ProductInfoModel>();

            _schema.AddField(x => x.ProductId, "ProductId");

            _schema.AddField(x => x.SPU, "SPU");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.Brand, "Brand");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.Qty, "Qty");         

            _schema.AddField(x => x.IsOnSaled, "IsOnSaled");

            _schema.AddField(x => x.SkuCount, "SkuCount");

            _schema.AddField(x => x.TotalRecord, "TotalRecord");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.DiscountRate, "DiscountRate");

            _schema.AddField(x => x.PromotionType, "PromotionType");

            _schema.AddField(x => x.PromotionStatus, "PromotionStatus");

            _schema.AddField(x => x.TuanNumbers, "TuanNumbers");

            _schema.AddField(x => x.CountryOfManufacture, "CountryOfManufacture");

            _schema.AddField(x => x.Size, "Size");

            _schema.AddField(x => x.Color, "Color");

            _schema.AddField(x => x.NetWeight, "NetWeight");

            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");

            _schema.AddField(x => x.Logo, "Logo");

            _schema.AddField(x => x.ReportStatus, "ReportStatus");

            _schema.AddField(x => x.IsCrossBorderEBTax, "IsCrossBorderEBTax");

            _schema.AddField(x => x.CBEBTaxRate, "CBEBTaxRate");

            _schema.AddField(x => x.ConsumerTaxRate, "ConsumerTaxRate");

            _schema.AddField(x => x.VATTaxRate, "VATTaxRate");

            _schema.AddField(x => x.PPATaxRate, "PPATaxRate");

            _schema.AddField(x => x.realTaxType, "realTaxType");

            _schema.AddField(x => x.taxPrice, "taxPrice");

            _schema.AddField(x => x.minRatePrice, "minRatePrice");

            _schema.AddField(x => x.Images, "Images");

            _schema.AddField(x => x.pid, "pid");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");
            _schema.AddField(x => x.MainValue, "MainValue");
            _schema.AddField(x => x.SubDicValue, "SubDicValue");
            _schema.AddField(x => x.SubValue, "SubValue");
            _schema.AddField(x => x.IsDutyOnSeller, "IsDutyOnSeller");

            _schema.AddField(x => x.BrandEN, "BrandEN");
            _schema.AddField(x => x.minRealTaxType, "minRealTaxType");
            _schema.AddField(x => x.ForOrderQty, "ForOrderQty");
            _schema.AddField(x => x.HuoLi, "HuoLi");
            _schema.AddField(x => x.Coupon, "Coupon");
            _schema.AddField(x => x.CountryName, "CountryName");
            _schema.AddField(x => x.NationalFlag, "NationalFlag");
            _schema.AddField(x => x.compare, "compare");
            _schema.AddField(x => x.Description, "Description");
            _schema.AddField(x => x.fiSpu, "fiSpu");
            _schema.AddField(x => x.ParentCategoryId, "ParentCategoryId");
            _schema.AddField(x => x.DHuoli, "DHuoli");
            _schema.AddField(x => x.SkuList, "SkuList");
            _schema.Compile();
        }
    }

    /// <summary>
    /// 商品列表页筛选项
    /// </summary>
    public class ProductFilterAttrubile
    {
        public string KeyName { get; set; }

        public string KeyValue { get; set; }
        /// <summary>
        /// 是否sku属性 0不是，1是
        /// </summary>
        public int IsSkuAttr { get; set; }
    }
    public class ListProductInfoModel
    {
       public List<ProductInfoModel> ProductInfoModel { get; set; }
    }
}
