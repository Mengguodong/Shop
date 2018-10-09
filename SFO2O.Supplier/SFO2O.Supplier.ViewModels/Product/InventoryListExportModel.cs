using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SFO2O.Supplier.Common;

namespace SFO2O.Supplier.ViewModels.Product
{
    public class InventoryListExportModel
    {
        [Column("商品SPU编号")]
        public String Spu { get; set; }
        [Column("商品名称")]
        public String ProductName { get; set; }
        [Column("条形码")]
        public String BarCode { get; set; }
        [Column("商品SKU编号")]
        public String Sku { get; set; }
        [Column("属性1")]
        public String MainValue { get; set; }
        [Column("属性2")]
        public String SubValue { get; set; }
        [Column("当前库存量")]
        public int Qty { get; set; }
        [Column("库存预警值")]
        public int AlarmStockQty { get; set; }
        [Column("低库存预警")]
        public String IsLowStockAlarm { get; set; }
        [Column("商品状态")]
        public EnumSkuStatus Status { get; set; }
    }

    public enum EnumSkuStatus
    {
        [Description("待上架")]
        WaitFor = 1,
        [Description("已上架")]
        OnSale = 3,
        [Description("已下架")]
        OffSale = 4,
        [Description("系统下架")]
        SystemOffSale = 5,
    }
}
