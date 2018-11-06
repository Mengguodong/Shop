$(function () {
    $("#export").click(function () {
        Export();
    });

    $(".js_win_cancel").click(function () {
        closeOverLayAndShowAlert(1, 1);
    });
    $(".js_win_cancel_norefesh").click(function () {
        closeOverLayAndShowAlert(1, 0);
    });
    $('.check_title li a').click(function () {
        $(this).parent().removeClass('current');
        $(this).parent().addClass('current');
        $("#RegionCode").val($(this).attr("code"));
        Query(1);
    })
    $("#ExpressList").blur(function () {
        if (!validate.required($(this))) {
            $("#expressListError").show().text("物流單號不能為空");
            return false;
        } else {
            $("#expressListError").hide().text("");
        }
        if (validate.chinese($(this))) {
            $("#expressListError").show().text("物流單號不能含有漢字");
            return false;
        }
        else {
            $("#expressListError").hide().text("");
        }
        return true;
    })
    $("#TradeCode").blur(function () {
        if (!validate.required($(this))) {
            $("#tradeCodeError").show().text("付款流水編號不能為空");
            return false;
        } else {
            $("#tradeCodeError").hide().text("");
        }
        if (validate.chinese($(this))) {
            $("#tradeCodeError").show().text("付款流水編號不能含有漢字");
            return false;
        } else {
            $("#tradeCodeError").hide().text("");
        }
    })

    $("#pickUpSubmit").click(function () {
        var eList = $("#ExpressList").val();
        if (!validate.required($("#ExpressList"))) {
            $("#expressListError").show().text("物流單號不能為空");
            return false;
        }
        if (validate.chinese($("#ExpressList"))) {
            $("#expressListError").show().text("物流單號不能含有漢字");
            return false;
        }
        var rCode = $("#RefundCodeE").val();
        var eCompany = $("#ExpressCompany").val();
        $.ajax({
            type: 'POST',
            url: "/Refund/RefundPickUp",
            data: "refundCode=" + rCode + "&expressCompany=" + eCompany + "&expressList=" + eList,
            async: false,
            success: function (data) {
                    closeOverLayAndShowAlert(0, 0);
                    showMsgDiv(data.Msg);
            }
        });
    });
    $("#refundSubmit").click(function () {
        var tradeCode = $("#TradeCode").val();
        if (!validate.required($("#TradeCode"))) {
            $("#tradeCodeError").show().text("付款流水編號不能為空");
            return false;
        } else {
            $("#tradeCodeError").hide().text("");
        }
        if (!validate.maxLength($("#TradeCode"), 21)) {
            $("#tradeCodeError").show().text("付款流水編號不能超過20字符");
            return false;
        } else {
            $("#tradeCodeError").hide().text("");
        }
        var rCode = $("#RefundCodeR").val();
        var payPlatform = $("#PayPlatform").val();
        var rmbTotalAmount = $("#RmbTotalAmount").text();
        var settlementTime = $("#SettlementTime").val();

        $.ajax({
            type: 'POST',
            url: "/Refund/ReturnRefund",
            data: "refundCode=" + rCode + "&payPlatform=" + payPlatform + "&rmbTotalAmount=" + rmbTotalAmount + "&settlementTime=" + settlementTime + "&tradeCode=" + tradeCode,
            async: false,
            success: function (data) {
                    closeOverLayAndShowAlert(0, 0);
                    showMsgDiv(data.Msg);
            }
        });
    });

    $('#SellerName').chosen({ disable_search: true });
});
function Export() {
    window.location.href = "/Refund/ExportRefundList?startTime=" + $("#CreateTimeStart").val() + "&endTime=" + $("#CreateTimeEnd").val() + "&sku=" + $("#Sku").val() + "&refundCode=" + $("#RefundCode").val() + "&buyerName=" + $("#BuyerName").val() + "&sellerName=" + $("#SellerName").val() + "&orderCode=" + $("#OrderCode").val() + "&refundType=" + $("#RefundType").val() + "&refundStatus=" + $("#RefundStatus").val() + "&regionCode=" + $("#RegionCode").val() + "&isFinance=" + $("#IsFinance").val();
}
function showPickUpDiv(refundCode) {
    showOverlay();
    $("#pickUpDiv").show();
    adjust("#pickUpDiv");
    $('#ExpressCompany').chosen({ disable_search: true, width: "268px;" });
    $("#RefundCodeE").val(refundCode);
}
function showMsgDiv(msgTips) {
    $("#msgTips").text(msgTips);
    showOverlay();
    $("#msgTipDiv").show();
    adjust("#msgTipDiv");
}
function closeOverLayAndShowAlert(isHideOverLay, isRefesh) {
    //var params ="?StartTime=" + $("#CreateTimeStart").val() + "&EndTime=" + $("#CreateTimeEnd").val() + "&Sku=" + $("#Sku").val() + "&RefundCode=" + $("#RefundCode").val() + "&BuyerName=" + $("#BuyerName").val() + "&SellerName=" + $("#SellerName").val() + "&OrderCode=" + $("#OrderCode").val() + "&RefundType=" + $("#RefundType").val() + "&RefundStatus=" + $("#RefundStatus").val() + "&RegionCode=" + $("#RegionCode").val() + "&IsFinance=" + $("#IsFinance").val()
    
    if (isHideOverLay == 1)
        hideOverlay();
    $(".js_win_show").hide();
    if (isRefesh == 1)
        location.href = location.href;
    
    
}
function showRefundDiv(refundCode, rmbTotalAmount) {
    showOverlay();
    $("#refundDiv").show();
    adjust("#refundDiv");
    $('#PayPlatform').chosen({ disable_search: true, width: "225px;" });
    $("#RefundCodeR").val(refundCode);
    $("#RefundCodeER").val(refundCode);
    $("#RmbTotalAmount").text(rmbTotalAmount);
}
function showRefundInfoDiv(refundCode) {
    $("#rSettlementTime").val();
    $("#rTradeCode").val();
    $("#rRmbTotalAmount").text();
    $.ajax({
        type: 'POST',
        url: "/Refund/RefundPaymentInfo",
        data: "refundCode=" + refundCode,
        async: false,
        success: function (data) {
            if (data.IsOk) {
                $("#rSettlementTime").val(data.PayInfo.PayCompleteTimeStr);
                $("#rTradeCode").val(data.PayInfo.TradeCode);
                $("#rRmbTotalAmount").text(data.PayInfo.PaidAmount);
                showOverlay();
                $("#refundInfoDiv").show();
                adjust("#refundInfoDiv");
                $('#rPayPlatform').chosen({ disable_search: true, width: "225px;" });
            }
        }
    });

}