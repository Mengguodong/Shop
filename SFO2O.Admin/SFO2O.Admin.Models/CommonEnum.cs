using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models
{
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
    /// <summary>
    /// 国家区域
    /// </summary>
    public enum CountryArea
    {
        /// <summary>
        /// 中國大陸
        /// </summary>
        [Description("中國大陸")]
        ChineseMainland = 86,

        /// <summary>
        /// 中國香港
        /// </summary>
        [Description("中國香港")]
        ChineseHongkong = 852


    }
    /// <summary>
    /// 退款单状态
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// 待審核
        /// </summary>
        [Description("待審核")]
        PendingAudit = 1,

        /// <summary>
        /// 上門取件
        /// </summary>
        [Description("上門取件")]
        Pickup = 2,

        /// <summary>
        /// 待退款
        /// </summary>
        [Description("待退款")]
        ToBeRefunded = 3,

        /// <summary>
        /// 退款成功
        /// </summary>
        [Description("退款成功")]
        RefundSuccess = 4,

        /// <summary>
        /// 退款關閉-取消申請
        /// </summary>
        [Description("退款關閉-取消申請")]
        Cancel = 5,

        /// <summary>
        /// 退款關閉-拒絕退款
        /// </summary>
        [Description("退款關閉-拒絕退款")]
        Refused = 6
    }
    /// <summary>
    /// 退款类型
    /// </summary>
    public enum RefundType
    {
        /// <summary>
        /// 未选择
        /// </summary>
        [Description("未选择")]
        NoSelected = 0,
        /// <summary>
        /// 退貨退款
        /// </summary>
        [Description("退貨退款")]
        ReturnRefund = 1,

        /// <summary>
        /// 僅退款
        /// </summary>
        [Description("僅退款")]
        RefundOnly = 2,
    }
    /// <summary>
    /// 退款单商品类型
    /// </summary>
    public enum RefundProductStatus
    {
        /// <summary>
        /// 未開封
        /// </summary>
        [Description("未開封")]
        UnOpened = 1,

        /// <summary>
        /// 已開封未使用
        /// </summary>
        [Description("已開封未使用")]
        UnUsed = 2,
        /// <summary>
        /// 已開封未使用
        /// </summary>
        [Description("已開封已使用")]
        Used = 3,
    }
    /// <summary>
    /// 退款原因
    /// </summary>
    public enum RefundReason
    {
        /// <summary>
        /// 商品顏色與訂單不符
        /// </summary>
        [Description("商品顏色與訂單不符")]
        ColorDifferent = 1,

        /// <summary>
        /// 商品尺寸與訂單不符
        /// </summary>
        [Description("商品尺寸與訂單不符")]
        SizeDifferent = 2,

        /// <summary>
        /// 送達的商品與訂單商品不符
        /// </summary>
        [Description("送達的商品與訂單商品不符")]
        Different = 3,

        /// <summary>
        /// 商品已超過使用期限
        /// </summary>
        [Description("商品已超過使用期限")]
        Overdue = 4,

        /// <summary>
        /// 商品不能正常運作
        /// </summary>
        [Description("商品不能正常運作")]
        Noworked = 5,

        /// <summary>
        /// 商品已破損
        /// </summary>
        [Description("商品已破損")]
        Damaged = 6,

        /// <summary>
        /// 其他商品品質問題(請描述)
        /// </summary>
        [Description("其他商品品質問題(請描述)")]
        OtherProductQuality = 7,

        /// <summary>
        /// 其他原因(請描述)
        /// </summary>
        [Description("其他原因(請描述)")]
        Others = 8,
    }

    /// <summary>
    /// 审批状态
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        AuditPass = 1,

        /// <summary>
        /// 驳回 
        /// </summary>
        [Description("驳回")]
        AuditReject = 2,

        /// <summary>
        /// 系统下架
        /// </summary>
        [Description("系统下架")]
        SysOffShelf = 5,

        /// <summary>
        /// 允许上架
        /// </summary>
        [Description("允许上架")]
        AllowShelf = 4,

    }
}
