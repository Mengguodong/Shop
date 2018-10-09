using System.ComponentModel;

namespace SFO2O.Supplier.Models
{
    /// <summary>
    /// 订单状态枚举
    /// </summary>
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

    ///
    /// <summary>
    /// 退款单状态枚举
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        PendingAudit = 1,

        /// <summary>
        /// 上门取件
        /// </summary>
        [Description("上门取件")]
        Pickup = 2,


        /// <summary>
        /// 待退款
        /// </summary>
        [Description("待退款")]
        ToRefunded = 3,

        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        Success = 4,

        /// <summary>
        /// 退款关闭-拒绝退款
        /// </summary>
        [Description("退款关闭-取消退款")]
        Cancel = 5,

        /// <summary>
        /// 退款关闭-拒绝退款
        /// </summary>
        [Description("退款关闭-拒绝退款")]
        Close = 6,
    }

    /// <summary>
    /// 配送方式
    /// </summary>
    public enum ShippingMethod
    {
        /// <summary>
        /// 德邦快递
        /// </summary>
        [Description("德邦快递")]
        SF = 1,
    }

    /// <summary>
    /// 退款方式
    /// </summary>
    public enum RefundType
    {
        /// <summary>
        /// 退款退货
        /// </summary>
        [Description("退款退貨")]
        TH = 1,

        /// <summary>
        /// 退款
        /// </summary>
        [Description("僅退款")]
        TK = 2,

    }

    public enum RefundReason
    {
        /// <summary>
        /// 商品颜色与订单不符
        /// </summary>
        [Description("商品颜色与订单不符")]

        ColorOrderNoMatch = 1,
        /// <summary>
        /// 商品尺寸与订单不符
        /// </summary>
        [Description("商品颜色与订单不符")]
        SizeOrderNoMath = 2,

        /// <summary>
        /// 送达的商品与订单商品不符
        /// </summary>
        [Description("商品颜色与订单不符")]
        ReceiveProductNoMath = 3,

        /// <summary>
        /// 商品已超过使用期限
        /// </summary>
        [Description("商品已超过使用期限")]
        ProductOverdue = 4,

        /// <summary>
        /// 商品不能正常运作
        /// </summary>
        [Description("商品不能正常运作")]
        ProductNoWork = 5,

        /// <summary>
        /// 商品已破损
        /// </summary>
        [Description("商品已破损")]
        ProductBad = 6,

        /// <summary>
        /// 其他商品品质问题(请描述)
        /// </summary>
        [Description("其他商品品质问题(请描述)")]
        OtherProductQuality = 7,

        /// <summary>
        /// 其他原因(请描述)
        /// </summary>
        [Description("其他原因(请描述)")]
        OtherReason = 8
    }


    public enum ProductStatus
    {
        /// <summary>
        /// 未开封
        /// </summary>
        [Description("未开封")]
        Unlatched = 1,

        /// <summary>
        /// 已开封未使用
        /// </summary>
        [Description("已开封未使用")]
        NoUsered = 2,

        /// <summary>
        /// 已开封已使用
        /// </summary>
        [Description("已开封已使用")]
        Usered = 3,
    }

    /// <summary>
    /// 結算單狀態
    /// </summary>
    public enum SettlementStatus
    {
        /// <summary>
        /// 待確認
        /// </summary>
        [Description("待確認")]
        ToBeConfirmed = 1,

        /// <summary>
        /// 待付款
        /// </summary>
        [Description("待付款")]
        ToBePaid = 2,

        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Paid = 3

    }


    public enum PromotionStatus
    {
        /// <summary>
        /// 待審核
        /// </summary>
        [Description("待審核")]
        Auditing  = 0,

        /// <summary>
        /// 未開始
        /// </summary>
        [Description("未開始")]
        NotStarted= 1,

        /// <summary>
        /// 進行中
        /// </summary>
        [Description("進行中")]
        During = 2,

        /// <summary>
        /// 已過期
        /// </summary>
        [Description("已過期")]
        Expired = 3,

        /// <summary>
        /// 已駁回
        /// </summary>
        [Description("已駁回")]
        Rejected = 4,

    }
}
