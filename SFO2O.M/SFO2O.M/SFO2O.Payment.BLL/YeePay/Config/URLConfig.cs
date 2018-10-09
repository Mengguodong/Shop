

/// <summary>
/// 请求路径集合
/// </summary>
public class URLConfig
{
    /// <summary>
    /// 获取请求路径
    /// </summary>
    static URLConfig()
    {

        //支付请求地址
        //https://ok.yeepay.com/paymobile/api/pay/request       一键支付
        //https://ok.yeepay.com/paymobile/payapi/request        PC  扫码  一键支付
        payUrl = "https://ok.yeepay.com/paymobile/payapi/request";


        //查询地址
        queryOrderUrl = "https://ok.yeepay.com/merchant/query_server/pay_single";


        //消费对账文件下载地址
        orderFileDownloadUrld="https://ok.yeepay.com/merchant/query_server/pay_clear_data";


        //订单退款
        refundUrl = "https://ok.yeepay.com/merchant/query_server/direct_refund";



        //退款查询
        queryRefundUrl = "https://ok.yeepay.com/merchant/query_server/refund_single";



        //退款文件下载
        refundFileDownloadUrl = "https://ok.yeepay.com/merchant/query_server/refund_clear_data";


        //银行卡查询
        findBankCardUrl = "https://ok.yeepay.com/payapi/api/bankcard/check";

        //绑卡列表查询
        queryBindCardList="https://ok.yeepay.com/payapi/api/bankcard/bind/list";

        //解绑接口
        unBindCardUrl = "https://ok.yeepay.com/payapi/api/bankcard/unbind";



    }
    /// <summary>
    /// 银行卡信息查询
    /// </summary>
    public static string findBankCardUrl { get; private set; }
    /// <summary>
    /// 消费对账文件下载
    /// </summary>
    public static string orderFileDownloadUrld { get; private set; }
    /// <summary>
    /// 支付地址
    /// </summary>
    public static string payUrl { get; private set; }

    /// <summary>
    /// 绑卡列表查询
    /// </summary>
    public static string queryBindCardList { get; private set; }

    /// <summary>
    /// 订单查询
    /// </summary>
    public static string queryOrderUrl { get; private set; }
    /// <summary>
    /// 退款订单查询
    /// </summary>
    public static string queryRefundUrl { get; private set; }
    /// <summary>
    /// 退款对账文件下载
    /// </summary>
    public static string refundFileDownloadUrl { get; private set; }
    /// <summary>
    /// 退款请求地址
    /// </summary>
    public static string refundUrl { get; private set; }
    /// <summary>
    /// 解绑接口
    /// </summary>
    public static string unBindCardUrl { get; private set; }
}