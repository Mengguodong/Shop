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
    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductAuditingListModel
    /// </summary>
    public class ProductAuditingListModel
    {

        /// <summary>
        /// Spu
        /// </summary>
        [DataMember(Name = "Spu")]
        [Display(Name = "Spu")]
        public string Spu { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [DataMember(Name = "ProductName")]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// SupplierName
        /// </summary>
        [DataMember(Name = "SupplierName")]
        [Display(Name = "SupplierName")]
        public string SupplierName { get; set; }

        /// <summary>
        /// Createtime
        /// </summary>
        [DataMember(Name = "Createtime")]
        [Display(Name = "Createtime")]
        public DateTime Createtime { get; set; }

        /// <summary>
        /// SaleArea
        /// </summary>
        [DataMember(Name = "SaleArea")]
        [Display(Name = "SaleArea")]
        public string SaleArea { get; set; }

        /// <summary>
        /// SalesTerritory
        /// </summary>
        [DataMember(Name = "SalesTerritory")]
        [Display(Name = "SalesTerritory")]
        public string SalesTerritory { get; set; }

        /// <summary>
        /// DataSource
        /// </summary>
        [DataMember(Name = "DataSource")]
        [Display(Name = "DataSource")]
        public string DataSource { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        [DataMember(Name = "Sku")]
        [Display(Name = "Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// ReportStatus
        /// </summary>
        [DataMember(Name = "ReportStatus")]
        [Display(Name = "ReportStatus")]
        public string ReportStatus { get; set; }

        /// <summary>
        /// CategoryName
        /// </summary>
        [DataMember(Name = "CategoryName")]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// InventoryStatus
        /// </summary>
        [DataMember(Name = "InventoryStatus")]
        [Display(Name = "InventoryStatus")]
        public int InventoryStatus { get; set; }

        /// <summary>
        /// QTY
        /// </summary>
        [DataMember(Name = "QTY")]
        [Display(Name = "QTY")]
        public int QTY { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary>
        [DataMember(Name = "MinForOrder")]
        [Display(Name = "MinForOrder")]
        public int MinForOrder { get; set; }

        /// <summary>
        /// IsOnSaled
        /// </summary>
        [DataMember(Name = "IsOnSaled")]
        [Display(Name = "IsOnSaled")]
        public bool IsOnSaled { get; set; }

        public int SkuOrderQuantity { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductAuditingListModel> _schema;
        static ProductAuditingListModel()
        {
            _schema = new ObjectSchema<ProductAuditingListModel>();
            _schema.AddField(x => x.Spu, "Spu");

            _schema.AddField(x => x.ProductName, "ProductName");

            _schema.AddField(x => x.SupplierName, "SupplierName");

            _schema.AddField(x => x.Createtime, "Createtime");

            _schema.AddField(x => x.SalesTerritory, "SalesTerritory");

            _schema.AddField(x => x.DataSource, "DataSource");

            _schema.AddField(x => x.Sku, "Sku");

            _schema.AddField(x => x.ReportStatus, "ReportStatus");

            _schema.AddField(x => x.CategoryName, "CategoryName");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.InventoryStatus, "InventoryStatus");

            _schema.AddField(x => x.IsOnSaled, "IsOnSaled");

            _schema.AddField(x => x.QTY, "QTY");

            _schema.AddField(x => x.MinForOrder, "MinForOrder");

            _schema.AddField(x => x.SkuOrderQuantity, "SkuOrderQuantity");
            _schema.Compile();
        }



       
    }
}