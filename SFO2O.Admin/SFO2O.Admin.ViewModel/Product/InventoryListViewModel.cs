using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.ViewModel
{
    public class InventoryListViewModel
    {
        public InventoryListViewModel()
        {
            PageIndex = 1;
        }
        [Display(Name = "所屬商家")]
        public int? SupplierID { get; set; }
        [Display(Name = "SPU編號")]
        public string Spu { get; set; }
        [Display(Name = "SKU編號")]
        public string Sku { get; set; }
        [Display(Name = "低庫存預警")]
        public bool? IsLowStockAlarm { get; set; }
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; }
        [Display(Name = "條形碼")]
        public string BarCode { get; set; }
        [Display(Name = "商品狀態")]
        public int? SkuStatus { get; set; }
        public int PageIndex { get; set; }
    }
}
