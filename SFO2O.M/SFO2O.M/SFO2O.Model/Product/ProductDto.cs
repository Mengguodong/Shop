using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.Model.Promotion;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;

namespace SFO2O.Model.Product
{
    public class ProductDto
    {
        /// <summary>
        /// Id
        /// </summary> 
        public int Id { get; set; }

        /// <summary>
        /// Spu
        /// </summary> 
        public string Spu { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary> 
        public int CategoryId { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary> 
        public int SupplierId { get; set; }

        /// <summary>
        /// Name
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Tag
        /// </summary> 
        public string Tag { get; set; }

        /// <summary>
        /// ProductPrice
        /// </summary> 
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Description
        /// </summary> 
        public string Description { get; set; }

        /// <summary>
        /// Brand
        /// </summary> 
        public string Brand { get; set; }

        /// <summary>
        /// CountryOfManufacture
        /// </summary> 
        public string CountryOfManufacture { get; set; }

        /// <summary>
        /// SalesTerritory
        /// </summary> 
        public int SalesTerritory { get; set; }

        /// <summary>
        /// Unit
        /// </summary> 
        public string Unit { get; set; }

        /// <summary>
        /// IsExchange
        /// </summary> 
        public int IsExchangeCN { get; set; }

        public int IsExchangeHK { get; set; }
        /// <summary>
        /// 商家承担运费
        /// </summary>
        public int IsDutyOnSeller { get; set; }

        /// <summary>
        /// IsReturn
        /// </summary> 
        public int IsReturn { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary> 
        public int MinForOrder { get; set; }

        /// <summary>
        /// MinPrice
        /// </summary> 
        public decimal MinPrice { get; set; }
        public decimal MinPriceOriginal { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary> 
        public int LanguageVersion { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public ProductImage[] Images { get; set; }

        public SkuDto[] SkuDtos { get; set; }
        public int SkuForOrder { get; set; }

        public decimal PromotionDiscountPrice { get; set; }
        public PromotionEntity Promotion { get; set; }
         
        public int BrandId { get; set; }
         
        public string NameCN { get; set; }
         
        public string NameHK { get; set; }
         
        public string NameEN { get; set; }
         
        public string Logo { get; set; }
         
        public string CountryName { get; set; }
         
        public string IntroductionCN { get; set; }

        /// <summary>
        /// 判断这个商品是不是拼生活的商品，用于单品页 添加拼团链接
        /// </summary>
        public int isTrue { get; set; }

        public int PromotionId { get; set; }

        public int PromotionType { get; set; }
        public bool isflag { get; set; }
        public string NationalFlag { get; set; }

        //净重单位
        public string NetWeightUnit { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ItemViewSupporter
    {
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        [DataMember(Name = "Name")]
        [Display(Name = "Name")] 
        public string Name { get; set; }

        [DataMember(Name = "Tag")]
        [Display(Name = "Tag")]
        public string Tag { get; set; }

        [DataMember(Name = "ProductPrice")]
        [Display(Name = "ProductPrice")]
        public decimal ProductPrice { get; set; }

        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Brand")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [DataMember(Name = "CountryOfManufacture")]
        [Display(Name = "CountryOfManufacture")]
        public string CountryOfManufacture { get; set; }

        [DataMember(Name = "SalesTerritory")]
        [Display(Name = "SalesTerritory")]
        public int SalesTerritory { get; set; }

        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [DataMember(Name = "IsExchangeCN")]
        [Display(Name = "IsExchangeCN")]
        public int IsExchangeCN { get; set; }

        [DataMember(Name = "IsExchangeHK")]
        [Display(Name = "IsExchangeHK")]
        public int IsExchangeHK { get; set; }

        [DataMember(Name = "IsDutyOnSeller")]
        [Display(Name = "IsDutyOnSeller")]
        public int IsDutyOnSeller { get; set; }

        [DataMember(Name = "IsReturn")]
        [Display(Name = "IsReturn")]
        public int IsReturn { get; set; }

        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        [DataMember(Name = "MinPriceOriginal")]
        [Display(Name = "MinPriceOriginal")]
        public decimal MinPriceOriginal { get; set; }

        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        [DataMember(Name = "Images")]
        [Display(Name = "Images")]
        public ProductImage[] Images { get; set; }

        [DataMember(Name = "SkuDtos")]
        [Display(Name = "SkuDtos")]
        public SkuDto[] SkuDtos { get; set; }

        [DataMember(Name = "SkuForOrder")]
        [Display(Name = "SkuForOrder")]
        public int SkuForOrder { get; set; }

        [DataMember(Name = "PromotionDiscountPrice")]
        [Display(Name = "PromotionDiscountPrice")]
        public decimal PromotionDiscountPrice { get; set; }

        [DataMember(Name = "Promotion")]
        [Display(Name = "Promotion")]
        public PromotionEntity Promotion { get; set; }

        [DataMember(Name = "BrandId")]
        [Display(Name = "BrandId")]
        public int BrandId { get; set; }

        [DataMember(Name = "NameCN")]
        [Display(Name = "NameCN")]
        public string NameCN { get; set; }


        [DataMember(Name = "NameHK")]
        [Display(Name = "NameHK")]
        public string NameHK { get; set; }


        [DataMember(Name = "NameEN")]
        [Display(Name = "NameEN")]
        public string NameEN { get; set; }


        [DataMember(Name = "Logo")]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [DataMember(Name = "CountryName")]
        [Display(Name = "CountryName")]
        public string CountryName { get; set; }

        [DataMember(Name = "IntroductionCN")]
        [Display(Name = "IntroductionCN")]
        public string IntroductionCN { get; set; }

        [DataMember(Name = "isTrue")]
        [Display(Name = "isTrue")]
       
        public int isTrue { get; set; }


        [DataMember(Name = "PromotionId")]
        [Display(Name = "PromotionId")]
        public int PromotionId { get; set; }

        [DataMember(Name = "PromotionType")]
        [Display(Name = "PromotionType")]
        public int PromotionType { get; set; }

        [DataMember(Name = "isflag")]
        [Display(Name = "isflag")]
        public bool isflag { get; set; }

        [DataMember(Name = "NationalFlag")]
        [Display(Name = "NationalFlag")]
        public string NationalFlag { get; set; }

        [DataMember(Name = "NetWeightUnit")]
        [Display(Name = "NetWeightUnit")]

        public string NetWeightUnit { get; set; }

        [DataMember(Name = "ProductSkuEntity")]
        [Display(Name = "ProductSkuEntity")]
        public List<ProductSkuEntity> ProductSkuEntity { get; set; }

        private static readonly ObjectSchema<ItemViewSupporter> _schema;
        static ItemViewSupporter()
        {
            _schema = new ObjectSchema<ItemViewSupporter>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Spu, "Spu");
            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.Tag, "Tag");

            _schema.AddField(x => x.ProductPrice, "ProductPrice");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.Brand, "Brand");

            _schema.AddField(x => x.CountryOfManufacture, "CountryOfManufacture");

            _schema.AddField(x => x.SalesTerritory, "SalesTerritory");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.IsExchangeCN, "IsExchangeCN");

            _schema.AddField(x => x.IsExchangeHK, "IsExchangeHK");

            _schema.AddField(x => x.IsDutyOnSeller, "IsDutyOnSeller");

            _schema.AddField(x => x.IsReturn, "IsReturn");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.MinPriceOriginal, "MinPriceOriginal");

            //_schema.AddField(x => x.NetContentUnit, "NetContentUnit");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");

            _schema.AddField(x => x.Images, "Images");

            _schema.AddField(x => x.SkuDtos, "SkuDtos");

            _schema.AddField(x => x.SkuForOrder, "SkuForOrder");

            _schema.AddField(x => x.PromotionDiscountPrice, "PromotionDiscountPrice");

            _schema.AddField(x => x.Promotion, "Promotion");

            _schema.AddField(x => x.BrandId, "BrandId");

            _schema.AddField(x => x.NameCN, "NameCN");

            _schema.AddField(x => x.NameHK, "NameHK");

            _schema.AddField(x => x.NameEN, "NameEN");

            _schema.AddField(x => x.Logo, "Logo");

            _schema.AddField(x => x.CountryName, "CountryName");

            _schema.AddField(x => x.IntroductionCN, "IntroductionCN");

            _schema.AddField(x => x.isTrue, "isTrue");

            _schema.AddField(x => x.PromotionId, "PromotionId");

            _schema.AddField(x => x.PromotionType, "PromotionType");

            _schema.AddField(x => x.isflag, "isflag");

            _schema.AddField(x => x.NationalFlag, "NationalFlag");

            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");

            _schema.AddField(x => x.ProductSkuEntity, "ProductSkuEntity");

            _schema.Compile();
        }
       
    }
}
