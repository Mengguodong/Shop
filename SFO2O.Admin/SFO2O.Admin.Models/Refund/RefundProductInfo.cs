using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SFO2O.Admin.Models.Refund
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// RefundProductInfo
    /// </summary>
    public class RefundProductInfo
    {

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// ProductImgPath
        /// </summary>
        [DataMember(Name = "ProductImgPath")]
        [Display(Name = "ProductImgPath")]
        public string ProductImgPath { get; set; }

        /// <summary>
        /// BarCode
        /// </summary>
        [DataMember(Name = "BarCode")]
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

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
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [DataMember(Name = "UnitPrice")]
        [Display(Name = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// RmbUnitPrice
        /// </summary>
        [DataMember(Name = "RmbUnitPrice")]
        [Display(Name = "RmbUnitPrice")]
        public decimal RmbUnitPrice { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [DataMember(Name = "Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// ProductAmount
        /// </summary>
        [DataMember(Name = "ProductAmount")]
        [Display(Name = "ProductAmount")]
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// RmpProductAmount
        /// </summary>
        [DataMember(Name = "RmpProductAmount")]
        [Display(Name = "RmpProductAmount")]
        public decimal RmpProductAmount { get; set; }

        /// <summary>
        /// RefundCode
        /// </summary>
        [DataMember(Name = "RefundCode")]
        [Display(Name = "RefundCode")]
        public string RefundCode { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember(Name = "TaxRate")]
        [Display(Name = "TaxRate")]
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// IsBearDuty
        /// </summary>
        [DataMember(Name = "IsBearDuty")]
        [Display(Name = "IsBearDuty")]
        public int IsBearDuty { get; set; }

        /// <summary>
        /// IsReturn
        /// </summary>
        [DataMember(Name = "IsReturn")]
        [Display(Name = "IsReturn")]
        public int IsReturn { get; set; }
        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<RefundProductInfo> _schema;
        static RefundProductInfo()
        {
            _schema = new ObjectSchema<RefundProductInfo>();
            _schema.AddField(x => x.ProductName, "ProductName");

            _schema.AddField(x => x.ProductImgPath, "ProductImgPath");

            _schema.AddField(x => x.BarCode, "BarCode");

            _schema.AddField(x => x.MainDicValue, "MainDicValue");

            _schema.AddField(x => x.MainValue, "MainValue");

            _schema.AddField(x => x.SubDicValue, "SubDicValue");

            _schema.AddField(x => x.SubValue, "SubValue");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.UnitPrice, "UnitPrice");

            _schema.AddField(x => x.RmbUnitPrice, "RmbUnitPrice");

            _schema.AddField(x => x.Quantity, "Quantity");

            _schema.AddField(x => x.ProductAmount, "ProductAmount");

            _schema.AddField(x => x.RmpProductAmount, "RmpProductAmount");

            _schema.AddField(x => x.RefundCode, "RefundCode");

            _schema.AddField(x => x.TaxRate, "TaxRate");

            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.IsBearDuty, "IsBearDuty");

            _schema.AddField(x => x.IsReturn, "IsReturn");
            
            _schema.Compile();
        }
    }
}