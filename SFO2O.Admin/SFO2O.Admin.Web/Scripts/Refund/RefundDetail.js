$(function () {
    $("#refuseRefund").click(function () {
        showRefuse();
    });
    $(".js_win_cancel").click(function () {
        closeOverLayAndShowAlert(1, 1);
    });

    $("#refuseOk").click(function () {
        submitRefuseRefund();
    });
    $("#agreeRefund").click(function () {
        showAgreeRefund();
    });
    $("#refuseReason").blur(function () {
        if (!validate.required($(this))) {
            $("#refuseReasonError").show().text("駁回原因不能為空");
            return false;
        }
        else
            $("#refuseReasonError").hide().text("");
        if (!validate.maxLength($(this), 301)) {
            $("#refuseReasonError").show().text("駁回原因長度不能超過300字符");
            return false;
        }
        else
            $("#refuseReasonError").hide().text("");
        return true;
    });

    $("#CollectionCode").blur(function () {
        if (!validate.required($(this))) {
            $("#collectionCodeError").show().text("代收編碼不能為空");
            return false;
        }
        else {
            $("#collectionCodeError").hide().text("");
        }
        if (validate.chinese($(this))) {
            $("#collectionCodeError").show().text("代收編碼不能含有漢字");
            $(this).val("");
            return false;
        }
        else {
            $("#collectionCodeError").hide().text("");
        }
        return true;
    });

    InitRadioChecked("IsReturnDuty");
    InitRadioChecked("IsQualityProblem");
    InitRadioChecked("RefundType");
    BindingRadioChange("IsReturnDuty");
    BindingRadioChange("RefundType");
    BindingRadioChange("IsQualityProblem");
    $("#agreeRefundOk").click(function () {
        closeOverLayAndShowAlert(0, 0);
        var isQualityProblem = $('input[type="radio"][name="IsQualityProblem"]:checked').val();
        if (isQualityProblem == 1) {
            var isRd = $("#IsBearDuty").val() == "0" ? 1 : 0;
            $("#isReturnDutyDiv").hide();
            $('input[type="radio"][name="IsReturnDuty"][value=' + isRd + ']').prop("checked", "checked");
        }
        else {
            if ($("#IsBearDuty").val() == "0")
                $("#isReturnDutyDiv").show();
            else {
                $("#isReturnDutyDiv").hide();
                $('input[type="radio"][name="IsReturnDuty"][value=0]').prop("checked", "checked");
            }
        }
        if ($("#IsReturn").val() == 0)
            $("#isNotReturn").show();
        else
            $("#isNotReturn").hide();
        InitRadioChecked("IsReturnDuty");
        $("#CollectionCode").val("");
        $("#collectionCodeError").hide().text("");
        InitRefundAmount();
        showAgreeRefund1();
    });
    $("#agreeRefundSubmit").click(function () {
        var cCode = $("#CollectionCode").val();
        var rCode = $("#RefundCode").val();
        var isQualityProblem = $('input[type="radio"][name="IsQualityProblem"]:checked').val();
        var refundType = $('input[type="radio"][name="RefundType"]:checked').val();
        var isReturnDuty = $('input[type="radio"][name="IsReturnDuty"]:checked').val();
        var rmbTotalAmount = $("#RmbTotalAmount").text();
        var totalAmount = $("#TotalAmount").text();
        var rmbDutyAmount = $("#RmbDutyAmount").val();
        var dutyAmount = $("#DutyAmount").val();
        var tobePickUpTime = $("#TobePickupTime").val();

        if (!validate.required($("#CollectionCode"))) {
            $("#collectionCodeError").show().text("代收編碼不能為空");
            return false;
        }
        if (validate.chinese($("#CollectionCode"))) {
            $("#collectionCodeError").show().text("代收編碼不能含有漢字");
            return false;
        }
        $.ajax({
            type: 'POST',
            url: "/Refund/AgreeRefund",
            data: "refundCode=" + rCode + "&isQualityProblem=" + isQualityProblem + "&refundType=" + refundType + "&isReturnDuty=" + isReturnDuty + "&rmbTotalAmount=" + rmbTotalAmount + "&totalAmount=" + totalAmount + "&rmbDutyAmount=" + rmbDutyAmount + "&dutyAmount=" + dutyAmount + "&collectionCod=" + cCode + "&tobePickUpTime=" + tobePickUpTime,
            async: false,
            success: function (data) {
                closeOverLayAndShowAlert(0, 0);
                showMsgDiv(data.Msg);
            }
        });
    });
});
function BindingRadioChange(radioName) {
    $('input[type="radio"][name="' + radioName + '"]').change(function () {
        InitRadioChecked(radioName);
    });
}
function InitRadioChecked(radioName) {
    $('input[type="radio"][name="' + radioName + '"]').each(function () {
        $(this).parent().removeClass("color_red");
        if ($(this).prop("checked"))
            $(this).parent().addClass("color_red");
    });
    InitRefundAmount();
}
function InitRefundAmount() {
    var isReturnDuty = $('input[type="radio"][name="IsReturnDuty"]:checked').val();
    var orderCustomerDuty = $('#customsDuties').val();
    var IsBearDuty = $('#IsBearDuty').val();
    var taxRate = decimalOperation.accDiv(parseFloat($("#TaxRate").val()), parseFloat(100));
    var addTaxRate = decimalOperation.accAdd(parseFloat(1), parseFloat(taxRate));
    var rmbProductAmount = 0;
    var productAmount = 0;
    var rmbTotalAmount = 0;
    var totalAmount = 0;
    var rmbDutyAmount = 0;
    var dutyAmount = 0;
    if (orderCustomerDuty > 0 && IsBearDuty == 0) {
        var rmbProductAmount = decimalOperation.accMul(parseFloat($("#RmbUnitPrice").val()), parseFloat($("#Quantity").val()));
        var productAmount = decimalOperation.accMul(parseFloat($("#UnitPrice").val()), parseFloat($("#Quantity").val()));
        var rmbTotalAmount = decimalOperation.accMul(parseFloat(rmbProductAmount), parseFloat(addTaxRate));
        var totalAmount = decimalOperation.accMul(parseFloat(productAmount), parseFloat(addTaxRate));
        var rmbDutyAmount = decimalOperation.accMul(parseFloat(rmbProductAmount), parseFloat(taxRate));
        var dutyAmount = decimalOperation.accMul(parseFloat(productAmount), parseFloat(taxRate));
    }
    else {
        var rmbProductAmount = decimalOperation.accMul(parseFloat($("#RmbUnitPrice").val()), parseFloat($("#Quantity").val()));
        var productAmount = decimalOperation.accMul(parseFloat($("#UnitPrice").val()), parseFloat($("#Quantity").val()));
        var rmbTotalAmount = decimalOperation.accMul(parseFloat(rmbProductAmount), parseFloat(addTaxRate));
        var totalAmount = decimalOperation.accMul(parseFloat(productAmount), parseFloat(addTaxRate));
        var rmbDutyAmount = 0;
        var dutyAmount = 0;

    }
    if (isReturnDuty == 1 && (orderCustomerDuty > 0 && IsBearDuty == 0)) {
        $("#RmbTotalAmount").text(rmbTotalAmount.toFixed(2));
        $("#TotalAmount").text(totalAmount.toFixed(2));
        $("#RmbDutyAmount").val(rmbDutyAmount.toFixed(2));
        $("#DutyAmount").val(dutyAmount.toFixed(2));
    }
    else {
        $("#RmbTotalAmount").text(rmbProductAmount.toFixed(2));
        $("#TotalAmount").text(productAmount.toFixed(2));
        $("#RmbDutyAmount").val(0.00);
        $("#DutyAmount").val(0.00);
    }

}
function submitRefuseRefund() {
    $("#refuseReasonError").hide().text("");

    if (!validate.required($("#refuseReason"))) {
        $("#refuseReasonError").show().text("駁回原因不能為空");
        return false;
    }
    else
        $("#refuseReasonError").hide().text("");
    if (!validate.maxLength($("#refuseReason"), 301)) {
        $("#refuseReasonError").show().text("駁回原因長度不能超過300字符");
        return false;
    }
    else
        $("#refuseReasonError").hide().text("");
    $.ajax({
        type: 'POST',
        url: "/Refund/RefuseRefund",
        data: "refundCode=" + $("#RefundCode").val() + "&refuseReason=" + $("#refuseReason").val(),
        async: false,
        success: function (data) {
            closeOverLayAndShowAlert(0, 0);
            showMsgDiv(data.Msg);
        }
    });
}


function showRefuse() {
    showOverlay();
    $("#refuseDiv").show();
    adjust("#refuseDiv");
}
function showAgreeRefund() {
    showOverlay();
    $("#confirmRefundDiv").show();
    adjust("#confirmRefundDiv");
}
function showAgreeRefund1() {
    showOverlay();
    $("#confirmRefundDiv1").show();
    adjust("#confirmRefundDiv1");
}
function showRefundInfo() {
    showOverlay();
    $("#refundInfoDiv").show();
    adjust("#refundInfoDiv");
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