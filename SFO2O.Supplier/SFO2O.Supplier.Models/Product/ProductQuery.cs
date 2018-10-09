using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models.Product
{
    public class ProductListQueryInfo
    {
        public int SupplierID { get; set; }
        public DateTime? UploadTime { get; set; }
        public int[] SkuStatus { get; set; }
        public bool? HasInventory { get; set; }
        public bool? IsOnSaled { get; set; }
        public string Sku { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
    }
    public class AuditProductListQueryInfo
    {
        public int SupplierID { get; set; }
        public DateTime? EditTime { get; set; }
        public int[] SkuStatus { get; set; }
        public int? EditType { get; set; }
        public string Sku { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
    }
    public class InventoryListQueryInfo
    {
        public int SupplierID { get; set; }
        public string ProductName { get; set; }
        public string Spu { get; set; }
        public string Sku { get; set; }
        public string BarCode { get; set; }
        public int? SkuStatus { get; set; }
        public bool? IsLowStockAlarm { get; set; }
    }

    public class ChangeSkuStatusRequest
    {
        public int SupplierID { get; set; }
        public string Sku { get; set; }
        public int Status { get; set; }
    }
}
