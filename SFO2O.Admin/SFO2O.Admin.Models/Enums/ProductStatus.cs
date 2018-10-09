using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Enums
{
    public enum ProductStatus
    {
        /// <summary>
        /// 待上架
        /// </summary>
        [Description("待上架")]
        WaitingForShelve = 1,

        /// <summary>
        /// 已上架
        /// </summary>
        [Description("已上架")]
        Shelved = 3,

        /// <summary>
        /// 已下架
        /// </summary>
        [Description("已下架")]
        OffShelf = 4,

        /// <summary>
        /// 系統下架
        /// </summary>
        [Description("系統下架")]
        SysOffShelf = 5,

        ///// <summary>
        ///// 系統下架
        ///// </summary>
        //[Description("允许上架")]
        //AllowShelf = 6,
    }

    public enum InventoryStatus
    {

        /// <summary>
        /// 有库存
        /// </summary>
        [Description("有库存")]
        HaveInventory = 1,

        /// <summary>
        /// 缺库存
        /// </summary>
        [Description("缺库存")]
        NoInventory = 0,
    };
}
