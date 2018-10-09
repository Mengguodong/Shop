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
    $("#RmbSupplierBearDutyAmount").blur(function () {
        checkOtherRmbSupplierBearDutyAmount($(this));
    });
    $("#OtherAmount").blur(function () {
        checkOtherAmount($(this));
    });
    $("#TradeCode").blur(function () {
        $("#tradeCodeError").hide().text("");
        if (!validate.required($("#TradeCode"))) {
            $("#tradeCodeError").show().text("付款流水編號不能為空");
            return false;
        } else {
            $("#tradeCodeError").hide().text("");
        }
        if (validate.chinese($("#TradeCode"))) {
            $("#tradeCodeError").show().text("付款流水編號不能有漢字");
            return false;
        } else {
            $("#tradeCodeError").hide().text("");
        }
        return true;
    });
    $("#confirmSettlement").click(function () {
        confirmSettlement();
    });
    $("em[name='SCode']").click(function () {
        if ($(this).attr("settlementstatus") == "2") {
            if ($(this).parent().hasClass("active")) {

                $(this).parent().removeClass("active")
            } else {
                $(this).parent().addClass("active")
            }
        }
    });
    $(".js_checkedAll").click(function () {
        if (!$(this).parent().hasClass("active")) {
            $(this).parent().addClass("active");
            $("em[name='SCode'][settlementstatus='2']").parent().addClass("active");
        }
        else {
            $(this).parent().removeClass("active");
            $("em[name='SCode'][settlementstatus='2']").parent().removeClass("active");
        }

    });
    $(".js_Pay").click(function () {
        if ($(this).attr("id") != "batchPay") {

            if ($(this).closest("tr").children('td').eq(0).children('p').eq(0).hasClass("active"))
                $(this).closest("tr").children('td').eq(0).children('p').eq(0).removeClass("active");
            else
                $(this).closest("tr").children('td').eq(0).children('p').eq(0).addClass("active");
        }
        var supplierIds = [];
        var sCodes = [];
        var settlementAmounts = 0.00;
        $("em[name='SCode']").each(function () {
            if ($(this).parent().hasClass("active")) {
                if ($.inArray($(this).attr("supplierid"), supplierIds) == -1)
                    supplierIds.push($(this).attr("supplierid"));
                if ($.inArray($(this).attr("id"), sCodes) == -1)
                    sCodes.push($(this).attr("id"));
                settlementAmounts += parseFloat($(this).attr("settlementAmount"));
            }
        });
        if (supplierIds.length < 1) {
            showMsgDiv("請選擇需要結算的結算單");
        }
        else if (supplierIds.length > 1) {
            showMsgDiv("您不能同時向多個結算方進行支付");
        }
        else {
            if (sCodes.length == 1 && settlementAmounts == 0) {
                showNoPayDiv();
            }
            else {
                $('#PayPlatform').chosen({ disable_search: true, width: "224px;" });
                showPayDiv(settlementAmounts.toFixed(2));
            }
        }
    });
    $("#paySubmit").click(function () {
        settlementPay(0);
    });
    $("#noPaySubmit").click(function () {
        settlementPay(1);
    });
});
function settlementPay(iszeropay) {
    $("#tradeCodeError").hide().text("");
    var supplierIds = [];
    var sCodes = [];
    var settlementAmounts = 0.00;
    $("em[name='SCode']").each(function () {
        if ($(this).parent().hasClass("active")) {
            if ($.inArray($(this).attr("supplierid"), supplierIds) == -1)
                supplierIds.push($(this).attr("supplierid"));
            if ($.inArray($(this).attr("id"), sCodes) == -1)
                sCodes.push($(this).attr("id"));
            settlementAmounts += parseFloat($(this).attr("settlementAmount"));
        }
    });
    if (supplierIds.length < 1) {
        showMsgDiv("請選擇需要結算的結算單");
        return;
    }
    if (supplierIds.length > 1) {
        showMsgDiv("您不能同時向多個結算方進行支付");
        return;
    }
    var tradeCode = $("#TradeCode").val();
    var settlementTime = $("#SettlementTime").val();
    var payPlatform = $("#PayPlatform").val();
    var settlementAmount = $("#SettlementAmounts").text();
    if (iszeropay != 1) {
        if (!validate.required($("#TradeCode"))) {
            $("#tradeCodeError").show().text("付款流水編號不能為空");
            return;
        }
        if (validate.chinese($("#TradeCode"))) {
            $("#tradeCodeError").show().text("付款流水編號不能有漢字");
            return;
        }
    }
    else {
        tradeCode = "";
        settlementAmount = 0;
        settlementTime = (new Date()).toDateString();
    }
    $.ajax({
        type: 'POST',
        url: "/Settlement/SettlementPay",
        data: "settlementCodes=" + "'" + sCodes.join("','") + "'" + "&settlementTime=" + settlementTime + "&payPlatform=" + payPlatform + "&tradeCode=" + tradeCode + "&settlementAmount=" + settlementAmount,
        async: false,
        success: function (data) {
            closeOverLayAndShowAlert(0, 0);
            showMsgDiv(data.Msg);
        }
    });
}
function confirmSettlement() {
    if (checkOtherRmbSupplierBearDutyAmount($("#RmbSupplierBearDutyAmount")) && checkOtherAmount($("#OtherAmount"))) {
        $.ajax({
            type: 'POST',
            url: "/Settlement/ConfirmSettlement",
            data: "settlementCode=" + $("#SettlementCodeL").val() + "&rmbProductSettlementAmount=" + $("#RmbProductSettlementAmount").text() + "&rmbSupplierBearDutyAmount=" + $("#RmbSupplierBearDutyAmount").val() + "&rmbDutyAmount=" + $("#RmbDutyAmount").val() + "&dutyAmount=" + $("#DutyAmount").val() + "&exchangeRate=" + $("#ExchangeRate").val() + "&otherAmount=" + ($("#OtherAmount").val() == "" ? "0" : $("#OtherAmount").val()),
            async: false,
            success: function (data) {                
                    closeOverLayAndShowAlert(0, 0);
                    showMsgDiv(data.Msg);
            }
        });
    }
}
function checkOtherRmbSupplierBearDutyAmount(obj) {
    var str = $(obj).val();
    $("#dutyAmountCodeError").hide().text("");

    if (validate.isfloat($(obj)) == false) {
        $("#dutyAmountCodeError").show().text("關稅金額必須是數字");
        $(obj).val($("#RmbDutyAmount").val());
        return false;
    }
    else {
        $("#dutyAmountCodeError").hide().text("");
    }
    if (str < 0) {
        $("#dutyAmountCodeError").show().text("關稅金額必須大於等於零");
        $(obj).val($("#RmbDutyAmount").val());
        return false;
    }
    else {
        $("#dutyAmountCodeError").hide().text("");
    }
    if (validate.max_float($(obj), $("#RmbDutyAmount").val())) {
        $("#dutyAmountCodeError").show().text("關稅金額必須小於等於" + $("#RmbDutyAmount").val());
        $(obj).val($("#RmbDutyAmount").val());
        return false;
    }
    else {
        $("#dutyAmountCodeError").hide().text("");
    }
    calculateSettlementAmount();
    return true;
}
function checkOtherAmount(obj) {
    var str = $(obj).val();
    $("#otherAmountError").hide().text("");
    if (str != "") {
        if (validate.isfloat($(obj)) == false) {
            $("#otherAmountError").show().text("其他費用金額必須是數字");
            $(obj).val("");
            return false;
        }
        else {
            $("#otherAmountError").hide().text("");
        }
        if (str < 0) {
            $("#otherAmountError").show().text("其他費用金額必須大於等於零");
            $(obj).val("");
            return false;
        }
        else {
            $("#otherAmountError").hide().text("");
        }
        calculateSettlementAmount();
        return true;
    }
    else {
        return true;
    }
}
function showSettlementAuditDiv(settlementCode, pSettlementAmount, pRmbDudyAmount, pDudyAmount, exchangeRate) {
    showOverlay();
    $("#settlementAuditDiv").show();
    adjust("#settlementAuditDiv");
    $("#SettlementCodeL").val(settlementCode);
    $("#RmbDutyAmount").val(pRmbDudyAmount);
    $("#DutyAmount").val(pDudyAmount);
    $("#ExchangeRate").val(exchangeRate);
    $("#RmbSupplierBearDutyAmount").val(pRmbDudyAmount);
    
    $("#RmbProductSettlementAmount").text(pSettlementAmount);
    calculateSettlementAmount();

}
function calculateSettlementAmount() {
    
    var rmbSettlementAmount = decimalOperation.accSub(parseFloat($("#RmbProductSettlementAmount").text()), parseFloat($("#RmbSupplierBearDutyAmount").val()));
    var rmbOtherAmount = decimalOperation.accMul(parseFloat($("#OtherAmount").val()), parseFloat($("#ExchangeRate").val()));
    var settlementAmount = decimalOperation.accDiv(parseFloat(rmbSettlementAmount), parseFloat($("#ExchangeRate").val()));
    if ($("#OtherAmount").val() != "") {
        rmbSettlementAmount = decimalOperation.accSub(parseFloat(rmbSettlementAmount), parseFloat(rmbOtherAmount));
        settlementAmount = decimalOperation.accSub(parseFloat(settlementAmount), parseFloat($("#OtherAmount").val()));
    }
    $("#RmbSettlementAmount").text(parseFloat(rmbSettlementAmount).toFixed(2));
    $("#SettlementAmount").text(parseFloat(settlementAmount).toFixed(2));

}
function showNoPayDiv() {
    showOverlay();
    $("#noPayDiv").show();
    adjust("#noPayDiv");
}
function showPayDiv(settlementAmounts) {
    showOverlay();
    $("#payDiv").show();
    adjust("#payDiv");
    $("#SettlementAmounts").text(settlementAmounts);
}
function showPayInfoDiv(settlementCode, settlementAmount, settlementTime, tradeCode) {
    showOverlay();
    $("#payInfoDiv").show();
    adjust("#payInfoDiv");
    $("#tdScode").text(settlementCode);
    $("#tdSAmount").text("$" + settlementAmount);
    $("#tdStime").text(settlementTime);
    $('#tdTradeCode').text(tradeCode);
}
function showMsgDiv(msgTips) {
    $("#msgTips").text(msgTips);
    showOverlay();
    $("#msgTipDiv").show();
    adjust("#msgTipDiv");
}
function closeOverLayAndShowAlert(isHideOverLay, isRefesh) {
    if (isHideOverLay == 1)
        hideOverlay();
    $(".js_win_show").hide();
    if (isRefesh == 1)
        location.href = location.href;
}
function Export() {
    window.location.href = "/Settlement/ExportSettlementList?startTime=" + $("#CreateTimeStart").val() + "&endTime=" + $("#CreateTimeEnd").val() + "&settlementCode=" + $("#SettlementCode").val() + "&orderCode=" + $("#OrderCode").val() + "&settlementStatus=" + $("#SettlementStatus").val() + "&companyName=" + $("#CompanyName").val();
}