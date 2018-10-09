using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// 待付款
        /// </summary>
        [Description("待付款")]
        WaitingForPay = 0,

        /// <summary>
        /// 待发货
        /// </summary>
        [Description("待发货")]
        WaitingForDeliver = 1,

        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        Delivered = 2,


        /// <summary>
        /// 待确认收货
        /// </summary>
        [Description("已送达")]
        Served = 3,

        /// <summary>
        /// 交易成功
        /// </summary>
        [Description("交易成功")]
        Complete = 4,

        /// <summary>
        /// 交易关闭
        /// </summary>
        [Description("交易关闭")]
        Close = 5,
    }

    /// <summary>
    /// 配送方式
    /// </summary>
    public enum ShippingMethod
    {
        /// <summary>
        /// 顺丰快递
        /// </summary>
        [Description("顺丰快递")]
        SF = 1,
    }
}
