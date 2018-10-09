﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SFO2O.Admin.Common;

namespace SFO2O.Admin.ViewModels.Product
{
    public class InventoryListExportModel
    {
        [Column("所屬商家")]
        public String CompanyName { get; set; }
        [Column("商品SPU編號")]
        public String Spu { get; set; }
        [Column("商品名稱")]
        public String ProductName { get; set; }
        [Column("條形碼")]
        public String BarCode { get; set; }
        [Column("商品SKU編號")]
        public String Sku { get; set; }
        [Column("屬性1")]
        public String MainValue { get; set; }
        [Column("屬性2")]
        public String SubValue { get; set; }
        [Column("當前庫存量")]
        public int Qty { get; set; }
        [Column("庫存預警值")]
        public int AlarmStockQty { get; set; }
        [Column("低庫存預警")]
        public String IsLowStockAlarm { get; set; }
        [Column("商品狀態")]
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
        [Description("系統下架")]
        SystemOffSale = 5,
    }
}
