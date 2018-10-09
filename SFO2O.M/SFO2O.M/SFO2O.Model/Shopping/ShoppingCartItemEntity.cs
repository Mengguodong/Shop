using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Model.Shopping
{
    /// <summary>
    /// ShoppingCartItemEntity
    /// </summary>
    [Serializable]
    [DataContract]
    public class ShoppingCartItemEntity
    {

        /// <summary>
        /// ProductId
        /// </summary>
        [DataMember(Name = "ProductId")]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

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
        /// IsDutyOnSeller
        /// </summary>
        [DataMember(Name = "IsDutyOnSeller")]
        [Display(Name = "IsDutyOnSeller")]
        public int IsDutyOnSeller { get; set; }

        /// <summary>
        /// MinPrice
        /// </summary>
        [DataMember(Name = "MinPrice")]
        [Display(Name = "MinPrice")]
        public decimal MinPrice { get; set; }

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
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// ForOrderQty
        /// </summary>
        [DataMember(Name = "ForOrderQty")]
        [Display(Name = "ForOrderQty")]
        public int ForOrderQty { get; set; }

        /// <summary>
        /// CartUnitPrice
        /// </summary>
        [DataMember(Name = "CartUnitPrice")]
        [Display(Name = "CartUnitPrice")]
        public decimal CartUnitPrice { get; set; }

        /// <summary>
        /// CartQuantity
        /// </summary>
        [DataMember(Name = "CartQuantity")]
        [Display(Name = "CartQuantity")]
        public int CartQuantity { get; set; }

        /// <summary>
        /// CartTaxRate
        /// </summary>
        [DataMember(Name = "CartTaxRate")]
        [Display(Name = "CartTaxRate")]
        public decimal CartTaxRate { get; set; }

        /// <summary>
        /// CartExchangeRate
        /// </summary>
        [DataMember(Name = "CartExchangeRate")]
        [Display(Name = "CartExchangeRate")]
        public decimal CartExchangeRate { get; set; }

        /// <summary>
        /// ShoppingCartId
        /// </summary>
        [DataMember(Name = "ShoppingCartId")]
        [Display(Name = "ShoppingCartId")]
        public string ShoppingCartId { get; set; }

        /// <summary>
        /// IsChecked
        /// </summary>
        [DataMember(Name = "IsChecked")]
        [Display(Name = "IsChecked")]
        public int IsChecked { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        
        /// <summary>
        /// CartDiscountPrice
        /// </summary>
        [DataMember(Name = "CartDiscountPrice")]
        [Display(Name = "CartDiscountPrice")]
        public decimal CartDiscountPrice { get; set; }


         ///<summary>
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
            /// CommissionInHK
            /// </summary>
            [DataMember(Name = "NetWeightUnit")]
            [Display(Name = "NetWeightUnit")]
            public string NetWeightUnit { get; set; }

         /// <summary>
            /// CommissionInHK
            /// </summary>
            [DataMember(Name = "NetContentUnit")]
            [Display(Name = "NetContentUnit")]
            public string NetContentUnit { get; set; }


            /// <summary>
            /// 是否报备
            /// </summary>
            [DataMember(Name = "ReportStatus")]
            [Display(Name = "ReportStatus")]
            public int ReportStatus { get; set; }

            /// <summary>
            /// 是否电商税
            /// </summary>
            [DataMember(Name = "IsCrossBorderEBTax")]
            [Display(Name = "IsCrossBorderEBTax")]
            public int IsCrossBorderEBTax { get; set; }

            /// <summary>
            /// 是否电商税
            /// </summary>
            [DataMember(Name = "SortTime")]
            [Display(Name = "SortTime")]
            public DateTime SortTime { get; set; }

            /// <summary>
            /// 是否电商税
            /// </summary>
            [DataMember(Name = "PPATaxRate")]
            [Display(Name = "PPATaxRate")]
            public decimal PPATaxRate { get; set; }

            /// <summary>
            /// 商品税
            /// </summary>
            [DataMember(Name = "CBEBTaxRate")]
            [Display(Name = "CBEBTaxRate")]
            public decimal CBEBTaxRate { get; set; }

            /// <summary>
            /// 消费税率
            /// </summary>
            [DataMember(Name = "ConsumerTaxRate")]
            [Display(Name = "ConsumerTaxRate")]
            public decimal ConsumerTaxRate { get; set; }

            /// <summary>
            /// 增值税率
            /// </summary>
            [DataMember(Name = "VATTaxRate")]
            [Display(Name = "VATTaxRate")]
            public decimal VATTaxRate { get; set; }

            /// <summary>
            /// 活力[酒豆]
            /// </summary>
            [DataMember(Name = "Huoli")]
            [Display(Name = "Huoli")]
            public decimal Huoli { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ShoppingCartItemEntity> _schema;
        static ShoppingCartItemEntity()
        {
            _schema = new ObjectSchema<ShoppingCartItemEntity>();
            _schema.AddField(x => x.ProductId, "ProductId");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.SupplierId, "SupplierId");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.IsDutyOnSeller, "IsDutyOnSeller");

            _schema.AddField(x => x.MinPrice, "MinPrice");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Price, "Price");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.AuditTime, "AuditTime");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.IsOnSaled, "IsOnSaled");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.ForOrderQty, "ForOrderQty");

            _schema.AddField(x => x.CartUnitPrice, "CartUnitPrice");

            _schema.AddField(x => x.CartQuantity, "CartQuantity");

            _schema.AddField(x => x.CartTaxRate, "CartTaxRate");

            _schema.AddField(x => x.CartExchangeRate, "CartExchangeRate");

            _schema.AddField(x => x.ShoppingCartId, "ShoppingCartId");

            _schema.AddField(x => x.IsChecked, "IsChecked");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.CartDiscountPrice, "CartDiscountPrice");

            _schema.AddField(x => x.CommissionInCHINA, "CommissionInCHINA");

            _schema.AddField(x => x.CommissionInHK, "CommissionInHK");
            _schema.AddField(x => x.NetContentUnit, "NetContentUnit");
            _schema.AddField(x => x.NetWeightUnit, "NetWeightUnit");
            _schema.AddField(x => x.ReportStatus, "ReportStatus");
            _schema.AddField(x => x.IsCrossBorderEBTax, "IsCrossBorderEBTax");
            _schema.AddField(x => x.SortTime, "SortTime");
            _schema.AddField(x => x.PPATaxRate, "PPATaxRate");
            _schema.AddField(x => x.CBEBTaxRate, "CBEBTaxRate");
            _schema.AddField(x => x.VATTaxRate, "VATTaxRate");
            _schema.AddField(x => x.ConsumerTaxRate, "ConsumerTaxRate");
            _schema.AddField(x => x.Huoli, "Huoli");
            

            _schema.Compile();
        }
    }
}