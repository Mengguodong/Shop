using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels
{
    public class PublishedListModel
    {
        public PublishedListModel()
        {
            PageIndex = 1;
        }
        [Display(Name = "上传时间")]
        public DateTime? UploadTime { get; set; }
        [Display(Name = "商品状态")]
        public int? SkuStatus { get; set; }
        [Display(Name = "库存状态")]
        public bool? HasInventory { get; set; }
        [Display(Name = "是否在售")]
        public bool? IsOnSaled { get; set; }
        [Display(Name = "SKU编号")]
        public string Sku { get; set; }
        [Display(Name = "条形码")]
        public string BarCode { get; set; }
        [Display(Name = "商品名称")]
        public string ProductName { get; set; }
        public int PageIndex { get; set; }
    }
}
