﻿using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Product
{
    /// <summary>
    /// ProductSkuEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class ProductSkuEntity
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
        /// ProductPrice
        /// </summary>
        [DataMember(Name = "ProductPrice")]
        [Display(Name = "ProductPrice")]
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember(Name = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

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
        /// IsDutyOnSeller
        /// </summary>
        [DataMember(Name = "IsDutyOnSeller")]
        [Display(Name = "IsDutyOnSeller")]
        public int IsDutyOnSeller { get; set; }

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
        /// 净重
        /// </summary>
        [DataMember(Name = "NetWeightUnit")]
        [Display(Name = "NetWeightUnit")]
        public string NetWeightUnit { get; set; }

        /// <summary>
        /// 净含量
        /// </summary>
        [DataMember(Name = "NetContentUnit")]
        [Display(Name = "NetContentUnit")]
        public string NetContentUnit { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

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
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// IsOnSaled
        /// </summary>
        [DataMember(Name = "IsOnSaled")]
        [Display(Name = "IsOnSaled")]
        public bool IsOnSaled { get; set; }

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
        /// Qty
        /// </summary>
        [DataMember(Name = "Qty")]
        [Display(Name = "Qty")]
        public int Qty { get; set; }
        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// <summary>
        /// BrandId
        /// </summary>
        [DataMember(Name = "BrandId")]
        [Display(Name = "BrandId")]
        public int BrandId { get; set; }

        /// <summary>
        /// NameCN
        /// </summary>
        [DataMember(Name = "NameCN")]
        [Display(Name = "NameCN")]
        public string NameCN { get; set; }

        /// <summary>
        /// NameHK
        /// </summary>
        [DataMember(Name = "NameHK")]
        [Display(Name = "NameHK")]
        public string NameHK { get; set; }

        /// <summary>
        /// NameEN
        /// </summary>
        [DataMember(Name = "NameEN")]
        [Display(Name = "NameEN")]
        public string NameEN { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        [DataMember(Name = "Logo")]
        [Display(Name = "Logo")]
        public string Logo { get; set; }

        /// <summary>
        /// CountryName
        /// </summary>
        [DataMember(Name = "CountryName")]
        [Display(Name = "CountryName")]
        public string CountryName { get; set; }

        /// <summary>
        /// IntroductionCN
        /// </summary>
        [DataMember(Name = "IntroductionCN")]
        [Display(Name = "IntroductionCN")]
        public string IntroductionCN { get; set; }

        /// <summary>
        /// ReportStatus
        /// </summary>
        [DataMember(Name = "ReportStatus")]
        [Display(Name = "ReportStatus")]
        public int ReportStatus { get; set; }

        /// <summary>
        /// IsCrossBorderEBTax
        /// </summary>
        [DataMember(Name = "IsCrossBorderEBTax")]
        [Display(Name = "IsCrossBorderEBTax")]
        public int IsCrossBorderEBTax { get; set; }

        /// <summary>
        /// PPATaxRate
        /// </summary>
        [DataMember(Name = "PPATaxRate")]
        [Display(Name = "PPATaxRate")]
        public decimal PPATaxRate { get; set; }

        /// <summary>
        /// CBEBTaxRate
        /// </summary>
        [DataMember(Name = "CBEBTaxRate")]
        [Display(Name = "CBEBTaxRate")]
        public decimal CBEBTaxRate { get; set; }

        /// <summary>
        /// ConsumerTaxRate
        /// </summary>
        [DataMember(Name = "ConsumerTaxRate")]
        [Display(Name = "ConsumerTaxRate")]
        public decimal ConsumerTaxRate { get; set; }

        /// <summary>
        /// ConsumerTaxRate
        /// </summary>
        [DataMember(Name = "VATTaxRate")]
        [Display(Name = "VATTaxRate")]
        public decimal VATTaxRate { get; set; }

        [DataMember(Name = "DiscountPrice")]
        [Display(Name = "DiscountPrice")]
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// 增加折扣
        /// </summary>
        public decimal PromotionCost { get; set; }
        /// <summary>
        /// 国旗
        /// </summary>
        public string NationalFlag { get; set; }
        /// <summary>
        /// 单品规格
        /// </summary>
        [DataMember(Name = "Specifications")]
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// 单品净重
        /// </summary>
        [DataMember(Name = "NetWeight")]
        [Display(Name = "NetWeight")]
        public decimal NetWeight { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductSkuEntity> _schema;
        static ProductSkuEntity()
        {
            _schema = new ObjectSchema<ProductSkuEntity>();
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

            _schema.AddField(x => x.IsExchangeInCHINA, "IsExchangeInCHINA");

            _schema.AddField(x => x.IsExchangeInHK, "IsExchangeInHK");

            _schema.AddField(x => x.IsDutyOnSeller, "IsDutyOnSeller");

            _schema.AddField(x => x.IsReturn, "IsReturn");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");

            _schema.AddField(x => x.NetContentUnit, "NetContentUnit");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.AlarmStockQty, "AlarmStockQty");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.ShelvesTime, "ShelvesTime");

            _schema.AddField(x => x.RemovedTime, "RemovedTime");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.IsOnSaled, "IsOnSaled");

            _schema.AddField(x => x.MainDicKey, "MainDicKey");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicKey, "SubDicKey");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainKey, "MainKey");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubKey, "SubKey");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.Qty, "Qty");

            _schema.AddField(x => x.ImagePath, "ImagePath");
            

            _schema.AddField(x => x.BrandId, "BrandId");

            _schema.AddField(x => x.NameCN, "NameCN");

            _schema.AddField(x => x.NameHK, "NameHK");

            _schema.AddField(x => x.NameEN, "NameEN");

            _schema.AddField(x => x.Logo, "Logo");

            _schema.AddField(x => x.CountryName, "CountryName");

            _schema.AddField(x => x.IntroductionCN, "IntroductionCN");

            _schema.AddField(x => x.ReportStatus, "ReportStatus");

            _schema.AddField(x => x.IsCrossBorderEBTax, "IsCrossBorderEBTax");

            _schema.AddField(x => x.PPATaxRate, "PPATaxRate");

            _schema.AddField(x => x.CBEBTaxRate, "CBEBTaxRate");

            _schema.AddField(x => x.ConsumerTaxRate, "ConsumerTaxRate");

            _schema.AddField(x => x.VATTaxRate, "VATTaxRate");

            _schema.AddField(x => x.DiscountPrice, "DiscountPrice");

            _schema.AddField(x => x.PromotionCost, "PromotionCost");

            _schema.AddField(x => x.NationalFlag, "NationalFlag");

            _schema.AddField(x => x.Specifications, "Specifications");

            _schema.AddField(x => x.NetWeight, "NetWeight");

            _schema.Compile();
        }
    }
}