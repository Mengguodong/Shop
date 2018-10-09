using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Product
{
    public class SkuDto
    {

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

        /// <summary>
        /// 商家承担运费
        /// </summary>
        [DataMember(Name = "IsDutyOnSeller")]
        [Display(Name = "IsDutyOnSeller")]
        public int IsDutyOnSeller { get; set; }
    }
}
