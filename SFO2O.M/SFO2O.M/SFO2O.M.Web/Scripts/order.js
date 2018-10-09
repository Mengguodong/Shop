var cancelUrl,
		ordercode,
		payUrl,
		checkOrderUrl = window.hostname + "";
function refreshPage() {
    window.location.reload();
}
$().ready(function () {
    $("#MContainer").off("click", "a.gotoPay").on("click", "a.gotoPay", function (event) {
        ////支付 gotoPay
        payUrl = $(this).attr("href");
        ordercode = $(this).attr("data-ordercode");
        FSH.EventUtil.preventDefault(event);
        if (ajaxFlag) {
            ajaxFlag = false;
            FSH.Ajax({
                type: "get",
                url: "/Team/checkTeamStatus?OrderCode=" + ordercode,
                dataType: 'json',
                data: { "ordercode": ordercode },
                jsonp: 'callback',
                jsonpCallback: "success_jsonpCallback",
                success: function (json) {
                    console.log(json);
                    if (json.Type == 1 && (json.status == 1 || json.status == 0)) {
                        window.location.href = window.hostname + payUrl;
                    } else {
                        FSH.commonDialog(1, ['抱歉，该团人数已满，不能再参加，下次记得提前哦~'], '', 'refreshPage', '知道了');
                    }
                },
                error: function (err) {
                    FSH.commonDialog(1, ['请求超时，请刷新页面']);
                }
            })
        } else {
            FSH.smallPrompt("请勿重复提交")
        }
    })
    $("#MContainer").off("click", "a.confirmReceiptBtn").on("click", "a.confirmReceiptBtn", function (event) {
        //确认收货 按钮上 data-num 标识需返还的酒豆数 如果为空则直接弹确认收货弹窗 否则弹提示返还酒豆确认收货弹窗
        cancelUrl = $(this).attr("href");
        ordercode = $(this).attr("data-ordercode");
        var num = $(this).attr("data-num");
        FSH.EventUtil.preventDefault(event);
        if (num) {
            FSH.commonDialog(2, ['确认收货后，返 <span class="FontColor6">' + num + '</span> 酒豆', '<span style="line-height:160%;">稍后返到酒豆账户，请耐心等待…<br>交易成功后不能再申请退货</span>'], ".confirmReceiptBtn", "cancelFun", "完成");
        } else {
            FSH.commonDialog(2, ['是否要确认收货？', '交易成功后，不能再申请退货'], ".confirmReceiptBtn", "cancelFun", "完成");
        }

    })
    $("#MContainer").off("click", "a.cancelBtn").on("click", "a.cancelBtn", function (event) {
        //取消订单
        cancelUrl = $(this).attr("href");
        ordercode = $(this).attr("data-ordercode");
        FSH.EventUtil.preventDefault(event);
        FSH.commonDialog(2, ['您确定要取消该订单吗？'], ".cancelBtn", "cancelFun");
    })
})
function cancelFun() {
    if (ajaxFlag) {
        ajaxFlag = false;
        FSH.Ajax({
            type: "post",
            url: cancelUrl,
            dataType: 'json',
            data: { "ordercode": ordercode },
            jsonp: 'callback',
            jsonpCallback: "success_jsonpCallback",
            success: function (json) {
                if (json.Type == 1) {
                    window.location.reload();

                } else {
                    FSH.commonDialog(1, [json.Content]);
                }
            },
            error: function (err) {
                FSH.commonDialog(1, ['请求超时，请刷新页面']);
            }
        })
    } else {
        FSH.smallPrompt("请勿重复提交");
    }
}
$(function () {
    $("[name=chakanwuliu]").click(function () {
        var wlype = $(this).attr("wlype");

        if (wlype.trim() == "德邦物流") {
            wlype = 'debangwuliu'
        }
        if (wlype.trim() == "宅急送") {
            wlype = 'zhaijisong'
        }
        var wlcode = $(this).attr("wlcode");
        var ordercode = $(this).attr("ordercode");
        window.location.href = "https://m.kuaidi100.com/index_all.html?type=" + wlype + "&postid=" + wlcode + "&callbackurl=http://www.wdnzmt9.com/my/detail?orderCode=" + ordercode;
    })
})

