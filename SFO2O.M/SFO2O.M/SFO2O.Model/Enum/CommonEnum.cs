using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Enum
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusEnum
    {
        // 订单状态：-2 异常订单 -1：无效 0：待付款 1：待发货 2：已发货 3：前台：待收货（后台：已送达）4：交易成功 5：交易关闭 6:已付款待确认
        [Description("默认值")]
        Default = -3,

        [Description("异常订单")]
        Exception = -2,

        [Description("无效")]
        Invalid = -1,

        [Description("待付款")]
        NonPayment = 0,

        [Description("待发货")]
        UnDelivered = 1,

        [Description("已发货")]
        Shipped = 2,

        [Description("已送达")]
        Received = 3,

        [Description("交易成功")]
        Complete = 4,

        [Description("交易关闭")]
        Closed = 5,

        [Description("已付款 待确认")]
        SureOrder = 6,

    }

    public enum IndexModuleType
    {
        /// <summary>
        /// 顶部轮播图片
        /// </summary>
        TopPricturs = 0,
        /// <summary>
        /// 分类模块1
        /// </summary>
        CategroyModule1 = 1,
        /// <summary>
        /// 分类模块2
        /// </summary>
        CategroyModule2 = 2,
        /// <summary>
        /// 销量
        /// </summary>
        ProductTopSale = 3,
        /// <summary>
        /// 精品
        /// </summary>
        ProductCompetitive = 4,
        /// <summary>
        /// 公告
        /// </summary>
        Notice = 5
    }
    /// <summary>
    /// 证件类型
    /// </summary>
    public enum CertificateType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        IdCard = 1,
        /// <summary>
        /// 护照
        /// </summary>
        Passport = 2,
        /// <summary>
        /// 其他
        /// </summary>
        Other = 3
    }
    /// <summary>
    /// 退款/退货状态
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        WaitAudit = 1,
        /// <summary>
        /// 上门取件
        /// </summary>
        Pickup = 2,
        /// <summary>
        /// 待退款
        /// </summary>
        WaitRefund = 3,
        /// <summary>
        /// 退款成功
        /// </summary>
        RefundSuccess = 4,
        /// <summary>
        /// 退款关闭，取消退款
        /// </summary>
        CancelRefund = 5,
        /// <summary>
        /// 退款关闭，拒绝退款
        /// </summary>
        DenyRefund = 6
    }
    /// <summary>
    /// 退款类型
    /// </summary>
    public enum RefundType
    {
        /// <summary>
        /// 退款退货
        /// </summary>
        ReturnOfGoods = 1,
        /// <summary>
        /// 仅退款
        /// </summary>
        ReturnMoney = 2
    }
    /// <summary>
    /// 退款原因
    /// </summary>
    public enum RefundReason
    {
        /// <summary>
        /// 商品颜色与订单不符
        /// </summary>
        ColorOrderNoMatch = 1,
        /// <summary>
        /// 商品尺寸与订单不符
        /// </summary>
        SizeOrderNoMath = 2,
        /// <summary>
        /// 送达的商品与订单商品不符
        /// </summary>
        ReceiveProductNoMath = 3,
        /// <summary>
        /// 商品已超过使用期限
        /// </summary>
        ProductOverdue = 4,
        /// <summary>
        /// 商品不能正常运作
        /// </summary>
        ProductNoWork = 5,
        /// <summary>
        /// 商品已破损
        /// </summary>
        ProductBad = 6,
        /// <summary>
        /// 其他商品品质问题(请描述)
        /// </summary>
        OtherProductQuality = 7,
        /// <summary>
        /// 其他原因(请描述)
        /// </summary>
        OtherReason = 8
    }
    public enum RefundProductStatus
    {
        /// <summary>
        /// 未开封
        /// </summary>
        UnOpen = 1,
        /// <summary>
        /// 已开封未使用
        /// </summary>
        OpendUnUse = 2,
        /// <summary>
        /// 已开封已使用
        /// </summary>
        OpendUsed = 3
    }
    /// <summary>
    /// 各种优惠类型 0x01 0x02 0x04 0x08 0x10 0x20 
    /// if((model.SatisfyProduct & PromotionType.Normal) == PromotionType.Normal) //判断是否包含某个枚举
    ///  model.SatisfyProduct|=PromotionType.Normal  //加入枚举值normal
    ///  model.SatisfyProduct|=PromotionType.GroupBuy  //加入枚举值GroupBuy
    /// </summary>
    [Flags]
    public enum PromotionType
    {
        /// <summary>
        /// 全部
        /// </summary>
        All = None | Promotion | GroupBuy,
        /// <summary>
        /// 原價商品
        /// </summary>
        None = 0x01,
        /// <summary>
        /// 促銷價商品
        /// </summary>
        Promotion = 0x02,
        /// <summary>
        /// 拼生活商品
        /// </summary>
        GroupBuy = 0x04,
    }
    /// <summary>
    /// 来源:(1.M站   2.PC  3.APP)默认值1
    /// </summary>
    public enum DeviceSource
    {
        MSite = 1,
        PC = 2,
        APP = 3
    }
}
