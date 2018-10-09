using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Product
{
    public class ImportSpuModel
    {
        /// <summary>
        /// SPU以及SPUExtend信息
        /// </summary>
        public List<ProductAndExpandInfo> ProductInfo { get; set; }

        /// <summary>
        /// SKU信息
        /// </summary>
        public List<SkuInfo> SkuInfos { get; set; }

        /// <summary>
        /// SKU报关信息
        /// </summary>
        public List<SkuCustomInfo> CustomInfos { get; set; }

        /// <summary>
        /// SPU图片信息
        /// </summary>
        public List<SpuImgsInfo> Imgs { get; set; }
    }

    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductAndExpandInfo
    /// </summary>
    public class ProductAndExpandInfo
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
        public int IsExchangeInCHINA { get; set; }

        /// <summary>
        /// IsExchangeInHK
        /// </summary>
        [DataMember(Name = "IsExchangeInHK")]
        [Display(Name = "IsExchangeInHK")]
        public int IsExchangeInHK { get; set; }

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
        /// DataSource
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
        public DateTime PreOnSaleTime { get; set; }

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

        /// <summary>
        /// SpuId
        /// </summary>
        [DataMember(Name = "SpuId")]
        [Display(Name = "SpuId")]
        public int SpuId { get; set; }

        /// <summary>
        /// Materials
        /// </summary>
        [DataMember(Name = "Materials")]
        [Display(Name = "Materials")]
        public string Materials { get; set; }

        /// <summary>
        /// Pattern
        /// </summary>
        [DataMember(Name = "Pattern")]
        [Display(Name = "Pattern")]
        public string Pattern { get; set; }

        /// <summary>
        /// Flavour
        /// </summary>
        [DataMember(Name = "Flavour")]
        [Display(Name = "Flavour")]
        public string Flavour { get; set; }

        /// <summary>
        /// Ingredients
        /// </summary>
        [DataMember(Name = "Ingredients")]
        [Display(Name = "Ingredients")]
        public string Ingredients { get; set; }

        /// <summary>
        /// StoragePeriod
        /// </summary>
        [DataMember(Name = "StoragePeriod")]
        [Display(Name = "StoragePeriod")]
        public string StoragePeriod { get; set; }

        /// <summary>
        /// StoringTemperature
        /// </summary>
        [DataMember(Name = "StoringTemperature")]
        [Display(Name = "StoringTemperature")]
        public string StoringTemperature { get; set; }

        /// <summary>
        /// SkinType
        /// </summary>
        [DataMember(Name = "SkinType")]
        [Display(Name = "SkinType")]
        public string SkinType { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [DataMember(Name = "Gender")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// AgeGroup
        /// </summary>
        [DataMember(Name = "AgeGroup")]
        [Display(Name = "AgeGroup")]
        public string AgeGroup { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        [DataMember(Name = "Model")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        /// <summary>
        /// BatteryTime
        /// </summary>
        [DataMember(Name = "BatteryTime")]
        [Display(Name = "BatteryTime")]
        public string BatteryTime { get; set; }

        /// <summary>
        /// Voltage
        /// </summary>
        [DataMember(Name = "Voltage")]
        [Display(Name = "Voltage")]
        public string Voltage { get; set; }

        /// <summary>
        /// Power
        /// </summary>
        [DataMember(Name = "Power")]
        [Display(Name = "Power")]
        public string Power { get; set; }

        /// <summary>
        /// Warranty
        /// </summary>
        [DataMember(Name = "Warranty")]
        [Display(Name = "Warranty")]
        public string Warranty { get; set; }

        /// <summary>
        /// SupportedLanguage
        /// </summary>
        [DataMember(Name = "SupportedLanguage")]
        [Display(Name = "SupportedLanguage")]
        public string SupportedLanguage { get; set; }

        /// <summary>
        /// PetType
        /// </summary>
        [DataMember(Name = "PetType")]
        [Display(Name = "PetType")]
        public string PetType { get; set; }

        /// <summary>
        /// PetAgeUnit
        /// </summary>
        [DataMember(Name = "PetAgeUnit")]
        [Display(Name = "PetAgeUnit")]
        public string PetAgeUnit { get; set; }

        /// <summary>
        /// PetAge
        /// </summary>
        [DataMember(Name = "PetAge")]
        [Display(Name = "PetAge")]
        public string PetAge { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember(Name = "Location")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        [DataMember(Name = "Weight")]
        [Display(Name = "Weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// WeightUnit
        /// </summary>
        [DataMember(Name = "WeightUnit")]
        [Display(Name = "WeightUnit")]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        [DataMember(Name = "Volume")]
        [Display(Name = "Volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// VolumeUnit
        /// </summary>
        [DataMember(Name = "VolumeUnit")]
        [Display(Name = "VolumeUnit")]
        public string VolumeUnit { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        [DataMember(Name = "Length")]
        [Display(Name = "Length")]
        public decimal Length { get; set; }

        /// <summary>
        /// LengthUnit
        /// </summary>
        [DataMember(Name = "LengthUnit")]
        [Display(Name = "LengthUnit")]
        public string LengthUnit { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [DataMember(Name = "Width")]
        [Display(Name = "Width")]
        public decimal Width { get; set; }

        /// <summary>
        /// WidthUnit
        /// </summary>
        [DataMember(Name = "WidthUnit")]
        [Display(Name = "WidthUnit")]
        public string WidthUnit { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [DataMember(Name = "Height")]
        [Display(Name = "Height")]
        public decimal Height { get; set; }

        /// <summary>
        /// HeightUnit
        /// </summary>
        [DataMember(Name = "HeightUnit")]
        [Display(Name = "HeightUnit")]
        public string HeightUnit { get; set; }

        /// <summary>
        /// Flavor
        /// </summary>
        [DataMember(Name = "Flavor")]
        [Display(Name = "Flavor")]
        public string Flavor { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductAndExpandInfo> _schema;
        static ProductAndExpandInfo()
        {
            _schema = new ObjectSchema<ProductAndExpandInfo>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.CategoryId, "CategoryId");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.Tag, "Tag");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.Description, "Description");

            _schema.AddField(x => x.BrandId, "BrandId");

            _schema.AddField(x => x.Brand, "Brand");

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

            _schema.AddField(x => x.SpuId, "SpuId");

            _schema.AddField(x => x.Materials, "Materials");

            _schema.AddField(x => x.Pattern, "Pattern");

            _schema.AddField(x => x.Flavour, "Flavour");

            _schema.AddField(x => x.Ingredients, "Ingredients");

            _schema.AddField(x => x.StoragePeriod, "StoragePeriod");

            _schema.AddField(x => x.StoringTemperature, "StoringTemperature");

            _schema.AddField(x => x.SkinType, "SkinType");

            _schema.AddField(x => x.Gender, "Gender");

            _schema.AddField(x => x.AgeGroup, "AgeGroup");

            _schema.AddField(x => x.Model, "Model");

            _schema.AddField(x => x.BatteryTime, "BatteryTime");

            _schema.AddField(x => x.Voltage, "Voltage");

            _schema.AddField(x => x.Power, "Power");

            _schema.AddField(x => x.Warranty, "Warranty");

            _schema.AddField(x => x.SupportedLanguage, "SupportedLanguage");

            _schema.AddField(x => x.PetType, "PetType");

            _schema.AddField(x => x.PetAgeUnit, "PetAgeUnit");

            _schema.AddField(x => x.PetAge, "PetAge");

            _schema.AddField(x => x.Location, "Location");

            _schema.AddField(x => x.Weight, "Weight");

            _schema.AddField(x => x.WeightUnit, "WeightUnit");

            _schema.AddField(x => x.Volume, "Volume");

            _schema.AddField(x => x.VolumeUnit, "VolumeUnit");

            _schema.AddField(x => x.Length, "Length");

            _schema.AddField(x => x.LengthUnit, "LengthUnit");

            _schema.AddField(x => x.Width, "Width");

            _schema.AddField(x => x.WidthUnit, "WidthUnit");

            _schema.AddField(x => x.Height, "Height");

            _schema.AddField(x => x.HeightUnit, "HeightUnit");

            _schema.AddField(x => x.Flavor, "Flavor");
            _schema.Compile();
        }
    }

    [Serializable]
    [DataContract]
    /// <summary>
    /// SkuInfo
    /// </summary>
    public class SkuInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// SpuId
        /// </summary>
        [DataMember(Name = "SpuId")]
        [Display(Name = "SpuId")]
        public int SpuId { get; set; }

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
        /// MainDicKey
        /// </summary>
        [DataMember(Name = "MainDicKey")]
        [Display(Name = "MainDicKey")]
        public string MainDicKey { get; set; }

        /// <summary>
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// SubDicKey
        /// </summary>
        [DataMember(Name = "SubDicKey")]
        [Display(Name = "SubDicKey")]
        public string SubDicKey { get; set; }

        /// <summary>
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// MainKey
        /// </summary>
        [DataMember(Name = "MainKey")]
        [Display(Name = "MainKey")]
        public string MainKey { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubKey
        /// </summary>
        [DataMember(Name = "SubKey")]
        [Display(Name = "SubKey")]
        public string SubKey { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// NetWeight
        /// </summary>
        [DataMember(Name = "NetWeight")]
        [Display(Name = "NetWeight")]
        public decimal NetWeight { get; set; }

        /// <summary>
        /// NetContent
        /// </summary>
        [DataMember(Name = "NetContent")]
        [Display(Name = "NetContent")]
        public decimal NetContent { get; set; }

        /// <summary>
        /// Specifications
        /// </summary>
        [DataMember(Name = "Specifications")]
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        [DataMember(Name = "Size")]
        [Display(Name = "Size")]
        public string Size { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [DataMember(Name = "Color")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        /// <summary>
        /// AlcoholPercentage
        /// </summary>
        [DataMember(Name = "AlcoholPercentage")]
        [Display(Name = "AlcoholPercentage")]
        public string AlcoholPercentage { get; set; }

        /// <summary>
        /// Smell
        /// </summary>
        [DataMember(Name = "Smell")]
        [Display(Name = "Smell")]
        public string Smell { get; set; }

        /// <summary>
        /// CapacityRestriction
        /// </summary>
        [DataMember(Name = "CapacityRestriction")]
        [Display(Name = "CapacityRestriction")]
        public string CapacityRestriction { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DataMember(Name = "Price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        /// <summary>
        /// AlarmStockQty
        /// </summary>
        [DataMember(Name = "AlarmStockQty")]
        [Display(Name = "AlarmStockQty")]
        public int AlarmStockQty { get; set; }

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
        /// ShelvesTime
        /// </summary>
        [DataMember(Name = "ShelvesTime")]
        [Display(Name = "ShelvesTime")]
        public DateTime ShelvesTime { get; set; }

        /// <summary>
        /// RemovedTime
        /// </summary>
        [DataMember(Name = "RemovedTime")]
        [Display(Name = "RemovedTime")]
        public DateTime RemovedTime { get; set; }

        /// <summary>
        /// IsOnSaled
        /// </summary>
        [DataMember(Name = "IsOnSaled")]
        [Display(Name = "IsOnSaled")]
        public bool IsOnSaled { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// QtyStatus
        /// </summary>
        [DataMember(Name = "QtyStatus")]
        [Display(Name = "QtyStatus")]
        public int QtyStatus { get; set; }

        /// <summary>
        /// ReportStatus
        /// </summary>
        [DataMember(Name = "ReportStatus")]
        [Display(Name = "ReportStatus")]
        public int ReportStatus { get; set; }

        /// <summary>
        /// DataSource
        /// </summary>
        [DataMember(Name = "DataSource")]
        [Display(Name = "DataSource")]
        public int DataSource { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        [DataMember(Name = "CompanyName")]
        [Display(Name = "CompanyName")]
        public String CompanyName { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public String ProductName { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }

        public string IsLowStockAlarm { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SkuInfo> _schema;
        static SkuInfo()
        {
            _schema = new ObjectSchema<SkuInfo>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SpuId, "SpuId");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.MainDicKey, "MainDicKey");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicKey, "SubDicKey");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainKey, "MainKey");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubKey, "SubKey");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.NetWeight, "NetWeight");

            _schema.AddField(x => x.NetContent, "NetContent");

            _schema.AddField(x => x.Specifications, "Specifications");

            _schema.AddField(x => x.Size, "Size");

            _schema.AddField(x => x.Color, "Color");

            _schema.AddField(x => x.AlcoholPercentage, "AlcoholPercentage");

            _schema.AddField(x => x.Smell, "Smell");

            _schema.AddField(x => x.CapacityRestriction, "CapacityRestriction");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.AlarmStockQty, "AlarmStockQty");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.ShelvesTime, "ShelvesTime");

            _schema.AddField(x => x.RemovedTime, "RemovedTime");

            _schema.AddField(x => x.IsOnSaled, "IsOnSaled");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.QtyStatus, "QtyStatus");

            _schema.AddField(x => x.ReportStatus, "ReportStatus");

            _schema.AddField(x => x.DataSource, "DataSource");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.ProductName, "ProductName");

            _schema.AddField(x => x.Qty, "Qty");
            _schema.Compile();
        }
    }

    [Serializable]
    [DataContract]
    /// <summary>
    /// SpuImgsInfo
    /// </summary>
    public class SpuImgsInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// SPU
        /// </summary>
        [DataMember(Name = "SPU")]
        [Display(Name = "SPU")]
        public string SPU { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// ImageType
        /// </summary>
        [DataMember(Name = "ImageType")]
        [Display(Name = "ImageType")]
        public int ImageType { get; set; }

        /// <summary>
        /// SortValue
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Createby
        /// </summary>
        [DataMember(Name = "Createby")]
        [Display(Name = "Createby")]
        public string Createby { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SpuImgsInfo> _schema;
        static SpuImgsInfo()
        {
            _schema = new ObjectSchema<SpuImgsInfo>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SPU, "SPU");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.ImageType, "ImageType");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Createby, "Createby");
            _schema.Compile();
        }
    }

    [Serializable]
    [DataContract]
    /// <summary>
    /// SkuCustomInfo
    /// </summary>
    public class SkuCustomInfo
    {

        /// <summary>
        /// id
        /// </summary>
        [DataMember(Name = "id")]
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// CustomsUnit
        /// </summary>
        [DataMember(Name = "CustomsUnit")]
        [Display(Name = "CustomsUnit")]
        public string CustomsUnit { get; set; }

        /// <summary>
        /// InspectionNo
        /// </summary>
        [DataMember(Name = "InspectionNo")]
        [Display(Name = "InspectionNo")]
        public string InspectionNo { get; set; }

        /// <summary>
        /// HSCode
        /// </summary>
        [DataMember(Name = "HSCode")]
        [Display(Name = "HSCode")]
        public string HSCode { get; set; }

        /// <summary>
        /// UOM
        /// </summary>
        [DataMember(Name = "UOM")]
        [Display(Name = "UOM")]
        public string UOM { get; set; }

        /// <summary>
        /// PrepardNo
        /// </summary>
        [DataMember(Name = "PrepardNo")]
        [Display(Name = "PrepardNo")]
        public string PrepardNo { get; set; }

        /// <summary>
        /// GnoCode
        /// </summary>
        [DataMember(Name = "GnoCode")]
        [Display(Name = "GnoCode")]
        public string GnoCode { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// TaxCode
        /// </summary>
        [DataMember(Name = "TaxCode")]
        [Display(Name = "TaxCode")]
        public string TaxCode { get; set; }

        /// <summary>
        /// ModelForCustoms
        /// </summary>
        [DataMember(Name = "ModelForCustoms")]
        [Display(Name = "ModelForCustoms")]
        public string ModelForCustoms { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SkuCustomInfo> _schema;
        static SkuCustomInfo()
        {
            _schema = new ObjectSchema<SkuCustomInfo>();
            _schema.AddField(x => x.id, "id");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.CustomsUnit, "CustomsUnit");

            _schema.AddField(x => x.InspectionNo, "InspectionNo");

            _schema.AddField(x => x.HSCode, "HSCode");

            _schema.AddField(x => x.UOM, "UOM");

            _schema.AddField(x => x.PrepardNo, "PrepardNo");

            _schema.AddField(x => x.GnoCode, "GnoCode");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.TaxCode, "TaxCode");

            _schema.AddField(x => x.ModelForCustoms, "ModelForCustoms");
            _schema.Compile();
        }
    }
}
