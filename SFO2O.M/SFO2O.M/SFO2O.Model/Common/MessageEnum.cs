using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Common
{
    public enum MessageType
    {
        /// <summary>
        /// 手机短信
        /// </summary>
        [Description("手机短信")]
        ShortMessage = 1
        
    }
    public enum SendWay
    {
        /// <summary>
        /// 手动实时
        /// </summary>
        [Description("手动实时")]
        Ordinary = 1,
        /// <summary>
        /// 手动定时
        /// </summary>
        [Description("手动定时")]
        Timing = 2,
        /// <summary>
        /// 自动发送
        /// </summary>
        [Description("实时发送")]
        Auto = 3,
    }
    public enum SendType
    {
        /// <summary>
        /// 按手机号发送
        /// </summary>
        [Description("按手机号发送")]
        ByMobile = 1,
        /// <summary>
        /// 按类别发送
        /// </summary>
        [Description("按类别发送")]
        ByType = 2,
        /// <summary>
        /// 按商家ID号
        /// </summary>
        [Description("按商家ID号")]
        BySupplierID = 3,
    }

    public enum MessageStatus
    {
        /// <summary>
        /// 待发送
        /// </summary>
        [Description("待发送")]
        Wait = 0,
        /// <summary>
        /// 已发送
        /// </summary>
        [Description("已发送")]
        Success = 1,
        /// <summary>
        /// 发送失败
        /// </summary>
        [Description("发送失败")]
        Fail = 2,
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Cancel = 3,
    }
    public enum MessageReceiveStatus
    {
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = -1,
        /// <summary>
        /// 未读
        /// </summary>
        [Description("未读")]
        UnRead = 0,
        /// <summary>
        /// 已读
        /// </summary>
        [Description("已读")]
        Read = 1
    }
    public enum MessageContentType
    {
        [Description("其它")]
        Other = 0,
        [Description("订单")]
        Order = 1
    }
}
