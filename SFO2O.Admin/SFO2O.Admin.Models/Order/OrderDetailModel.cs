using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Order
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// OrderDetailModel
    /// </summary>
    public class OrderDetailModel
    {

        /// <summary>
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }


        /// <summary>
        /// spu
        /// </summary>
        [DataMember(Name = "spu")]
        [Display(Name = "spu")]
        public string Spu { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        [Display(Name = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// PayUnitPrice
        /// </summary>
        [DataMember(Name = "PayUnitPrice")]
        [Display(Name = "PayUnitPrice")]
        public decimal PayUnitPrice { get; set; }

        /// <summary>
        /// PayAmount
        /// </summary>
        [DataMember(Name = "PayAmount")]
        [Display(Name = "PayAmount")]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }      

        /// <summary>
        /// RefundQuantity
        /// </summary>
        [DataMember(Name = "RefundQuantity")]
        [Display(Name = "RefundQuantity")]
        public int RefundQuantity { get; set; }

        /// <summary>
        /// TaxAmount
        /// </summary>
        [DataMember(Name = "TaxAmount")]
        [Display(Name = "TaxAmount")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        [DataMember(Name = "CompanyName")]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// TotalAmount
        /// </summary>
        [DataMember(Name = "TotalAmount")]
        [Display(Name = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember(Name = "ExchangeRate")]
        [Display(Name = "ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// MainDicValue
        /// </summary>
        [DataMember(Name = "MainDicValue")]
        [Display(Name = "MainDicValue")]
        public string MainDicValue { get; set; }

        /// <summary>
        /// MainValue
        /// </summary>
        [DataMember(Name = "MainValue")]
        [Display(Name = "MainValue")]
        public string MainValue { get; set; }

        /// <summary>
        /// SubDicValue
        /// </summary>
        [DataMember(Name = "SubDicValue")]
        [Display(Name = "SubDicValue")]
        public string SubDicValue { get; set; }

        /// <summary>
        /// SubValue
        /// </summary>
        [DataMember(Name = "SubValue")]
        [Display(Name = "SubValue")]
        public string SubValue { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// PromotionAmount
        /// </summary>
        [DataMember(Name = "PromotionAmount")]
        [Display(Name = "PromotionAmount")]
        public decimal PromotionAmount { get; set; }


        /// <summary>
        /// OriginalPrice
        /// </summary>
        [DataMember(Name = "OriginalPrice")]
        [Display(Name = "OriginalPrice")]
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<OrderDetailModel> _schema;
        static OrderDetailModel()
        {
            _schema = new ObjectSchema<OrderDetailModel>();
            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.Spu, "spu");

            _schema.AddField(x => x.Sku, "sku");

            _schema.AddField(x => x.Name, "Name");

            _schema.AddField(x => x.PayUnitPrice, "PayUnitPrice");

            _schema.AddField(x => x.PayAmount, "PayAmount");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.TaxAmount, "TaxAmount");

            _schema.AddField(x => x.CompanyName, "CompanyName");

            _schema.AddField(x => x.TotalAmount, "TotalAmount");

            _schema.AddField(x => x.ExchangeRate, "ExchangeRate");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.RefundQuantity, "RefundQuantity");

            _schema.AddField(x => x.PromotionAmount, "PromotionAmount");

            _schema.AddField(x => x.OriginalPrice, "OriginalPrice");
            _schema.Compile();
        }
    }
}