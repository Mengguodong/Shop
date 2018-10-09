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
    /// SkuTempModel
    /// </summary>
    public class SkuTempModel
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
        /// ReportStatus
        /// </summary>
        [DataMember(Name = "ReportStatus")]
        [Display(Name = "ReportStatus")]
        public int ReportStatus { get; set; }

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
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string IsLowStockAlarm { get; set; }

        public int DataSource { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<SkuTempModel> _schema;
        static SkuTempModel()
        {
            _schema = new ObjectSchema<SkuTempModel>();
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

            _schema.AddField(x => x.ReportStatus, "ReportStatus");

            _schema.AddField(x => x.IsOnSaled, "IsOnSaled");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.QtyStatus, "QtyStatus");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");
            _schema.Compile();
        }
    }
}