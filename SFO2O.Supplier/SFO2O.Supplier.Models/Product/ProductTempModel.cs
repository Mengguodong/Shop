using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Supplier.Models.Product
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductTempModel
    /// </summary>
    public class ProductTempModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        [DataMember(Name = "CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary>
        [DataMember(Name = "SupplierId")]
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        [DataMember(Name = "Tag")]
        [Display(Name = "Tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DataMember(Name = "Price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// BrandId
        /// </summary>
        [DataMember(Name = "BrandId")]
        [Display(Name = "BrandId")]
        public int BrandId { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        [DataMember(Name = "Brand")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// CountryOfManufacture
        /// </summary>
        [DataMember(Name = "CountryOfManufacture")]
        [Display(Name = "CountryOfManufacture")]
        public string CountryOfManufacture { get; set; }

        /// <summary>
        /// SalesTerritory
        /// </summary>
        [DataMember(Name = "SalesTerritory")]
        [Display(Name = "SalesTerritory")]
        public int SalesTerritory { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [DataMember(Name = "Unit")]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// IsExchangeInCHINA
        /// </summary>
        [DataMember(Name = "IsExchangeInCHINA")]
        [Display(Name = "IsExchangeInCHINA")]
        public decimal IsExchangeInCHINA { get; set; }

        /// <summary>
        /// IsExchangeInHK
        /// </summary>
        [DataMember(Name = "IsExchangeInHK")]
        [Display(Name = "IsExchangeInHK")]
        public decimal IsExchangeInHK { get; set; }

        /// <summary>
        /// IsReturn
        /// </summary>
        [DataMember(Name = "IsReturn")]
        [Display(Name = "IsReturn")]
        public int IsReturn { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary>
        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        /// <summary>
        /// MinPrice
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// NetWeight
        /// </summary>
        [DataMember(Name = "NetWeight")]
        [Display(Name = "NetWeight")]
        public decimal NetWeight { get; set; }

        /// <summary>
        /// NetWeightUnit
        /// </summary>
        [DataMember(Name = "NetWeightUnit")]
        [Display(Name = "NetWeightUnit")]
        public string NetWeightUnit { get; set; }

        /// <summary>
        /// NetContent
        /// </summary>
        [DataMember(Name = "NetContent")]
        [Display(Name = "NetContent")]
        public decimal NetContent { get; set; }

        /// <summary>
        /// NetContentUnit
        /// </summary>
        [DataMember(Name = "NetContentUnit")]
        [Display(Name = "NetContentUnit")]
        public string NetContentUnit { get; set; }

        /// <summary>
        /// IsDutyOnSeller
        /// </summary>
        [DataMember(Name = "IsDutyOnSeller")]
        [Display(Name = "IsDutyOnSeller")]
        public int IsDutyOnSeller { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "DataSource")]
        [Display(Name = "DataSource")]
        public int DataSource { get; set; }

        /// <summary>
        /// Createtime
        /// </summary>
        [DataMember(Name = "Createtime")]
        [Display(Name = "Createtime")]
        public DateTime Createtime { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [DataMember(Name = "CreateBy")]
        [Display(Name = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        /// ModifyTime
        /// </summary>
        [DataMember(Name = "ModifyTime")]
        [Display(Name = "ModifyTime")]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// ModifyBy
        /// </summary>
        [DataMember(Name = "ModifyBy")]
        [Display(Name = "ModifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>
        /// AuditingTime
        /// </summary>
        [DataMember(Name = "AuditingTime")]
        [Display(Name = "AuditingTime")]
        public DateTime AuditingTime { get; set; }

        /// <summary>
        /// AuditingBy
        /// </summary>
        [DataMember(Name = "AuditingBy")]
        [Display(Name = "AuditingBy")]
        public string AuditingBy { get; set; }

        /// <summary>
        /// PreOnSaleTime
        /// </summary>
        [DataMember(Name = "PreOnSaleTime")]
        [Display(Name = "PreOnSaleTime")]
        public DateTime? PreOnSaleTime { get; set; }


        /// <summary>
        /// CommissionInCHINA
        /// </summary>
        [DataMember(Name = "CommissionInCHINA")]
        [Display(Name = "CommissionInCHINA")]
        public decimal CommissionInCHINA { get; set; }

        /// <summary>
        /// CommissionInHK
        /// </summary>
        [DataMember(Name = "CommissionInHK")]
        [Display(Name = "CommissionInHK")]
        public decimal CommissionInHK { get; set; }

        #region 扩展属性
        public String CategoryName { get; set; }
        public String AuditReason { get; set; }
        public IList<ProductTempImageModel> ImageList { get; set; }
        public ProductExtTempModel ExpandInfo { get; set; }
        public Dictionary<String, String> ExpandDic { get; set; }
        public IList<SkuTempModel> SkuInfoList { get; set; }
        public Dictionary<String, String> DeliveryDic { get; set; }
        public string FirstImage { get; set; }
        #endregion

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductTempModel> _schema;
        static ProductTempModel()
        {
            _schema = new ObjectSchema<ProductTempModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.Tag, "Tag");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.Brand, "Brand");

            _schema.AddField(x => x.BrandId, "BrandId");

            _schema.AddField(x => x.CountryOfManufacture, "CountryOfManufacture");

            _schema.AddField(x => x.SalesTerritory, "SalesTerritory");

            _schema.AddField(x => x.Unit, "Unit");

            _schema.AddField(x => x.IsExchangeInCHINA, "IsExchangeInCHINA");

            _schema.AddField(x => x.IsExchangeInHK, "IsExchangeInHK");

            _schema.AddField(x => x.IsReturn, "IsReturn");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.NetWeight, "NetWeight");

            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");

            _schema.AddField(x => x.NetContent, "NetContent");

            _schema.AddField(x => x.NetContentUnit, "NetContentUnit");

            _schema.AddField(x => x.IsDutyOnSeller, "IsDutyOnSeller");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.DataSource, "DataSource");

            _schema.AddField(x => x.Createtime, "Createtime");

            _schema.AddField(x => x.CreateBy, "CreateBy");

            _schema.AddField(x => x.ModifyTime, "ModifyTime");

            _schema.AddField(x => x.ModifyBy, "ModifyBy");

            _schema.AddField(x => x.AuditingTime, "AuditingTime");

            _schema.AddField(x => x.AuditingBy, "AuditingBy");

            _schema.AddField(x => x.PreOnSaleTime, "PreOnSaleTime");

            _schema.AddField(x => x.CommissionInCHINA, "CommissionInCHINA");

            _schema.AddField(x => x.CommissionInHK, "CommissionInHK");
            _schema.Compile();
        }
    }
}