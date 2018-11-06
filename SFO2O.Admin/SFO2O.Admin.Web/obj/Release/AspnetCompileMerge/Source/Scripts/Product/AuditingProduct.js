$(function () {
    if ($("#editLi").length > 0) {
        $("#editLi").click(function () { switchTab("editLi") });
        $("#onlineLi").click(function () { switchTab("onlineLi") });
    }

    checkButton();

    $("#passing").click(function () {
        passing();
    });

    $("#rejecting").click(function () {
        rejection();
    });

    $("#tempStorage").click(function () {
        customReportTemStorage(true);
    });

    $(".bg_f1f1f1").find(":input").focusout(function () {
        checkButton();
    });

    $("#rejBtn").click(function () {
        closeOverLayAndShowAlert();
        $("#returnBack").click();
    });

    $("#temBtn").click(function () {
        closeOverLayAndShowAlert();
    });

    $("#passBtn").click(function () {
        closeOverLayAndShowAlert();
        $("#returnBack").click();
    });

    $("#rejReasonCancle").click(function () {
        closeOverLayAndShowAlert();
    });

    $("#rejReasonBtn").click(function () {
        doRejection();
    });

    $(".win_close").click(function () {
        closeOverLayAndShowAlert();
    });

    $("#returnBack").click(function () {
        ReturnToListPage();
    });

    $("#desT").attr("href", "../product/ShowProductDes?spu=" + $("#spuID").val() + "&languageVersion=2&isOnline=false");
    $("#desS").attr("href", "../product/ShowProductDes?spu=" + $("#spuID").val() + "&languageVersion=1&isOnline=false");
    $("#desE").attr("href", "../product/ShowProductDes?spu=" + $("#spuID").val() + "&languageVersion=3&isOnline=false");

    
})


//标签页切换
function switchTab(liId) {
    $("#" + liId).parent().children().each(function () {
        $(this).removeClass("current");
    });

    $("#" + liId).addClass("current");

    if (liId == "editLi") {
        $("#editTab").show();
        $("#onlineTab").hide();

        $(".zanc_shangc").show();
    }
    else {
        $("#editTab").hide();
        $("#onlineTab").show();

        $(".zanc_shangc").hide();

        getOnlineDiv();
    }
}

function getOnlineDiv() {
    if ($("#onlineTab").children().length == 0) {
        $.ajax({
            type: 'POST',
            url: "/product/AuditingProductOnline",
            data: "spu=" + $("#spuID").val(),
            async: true,
            success: function (data) {
                $("#onlineTab").html(data);
            }
        });
    }
}

function checkButton() {
    var isShowPassing = true;

    $(".bg_f1f1f1").find(":input").each(function () {
        if ($.trim($(this).val()) == '') {
            isShowPassing = false;
            return false;
        }
    });


    if (isShowPassing == true) {
        $("#passing").removeClass("btn_green_off");
    }
    else {
        $("#passing").addClass("btn_green_off");
    }
    return isShowPassing;
}

function passing() {
    if (checkButton() == false) {
       
    }

    showOverlay();
    doPassing();
}

function rejection() {
    showOverlay();
    $("#rejRea").show();
    adjust("#rejRea");

    $("#rejReasonName").text($("#productName").text() + "（" + $("#spuID").val() + "）將被駁回，請說明駁回原因。");
}

function doPassing() {
    customReportTemStorage(false);
    upDateSpuStatus(1);
    $("#passName").text($("#productName").text() + "（" + $("#spuID").val() + "）已審核通過");
    //$("#returnBack").click();
}

function doRejection() {
    $("#rejReasonError").hide().text("");

    if ($.trim($("#rejReason").val()).length == 0) {
        $("#rejReasonError").show().text("駁回原因不能為空");
        return false;
    }

    if ($.trim($("#rejReason").val()).length > 500) {
        $("#rejReasonError").show().text("駁回原因長度不能超過500字符");
        return false;
    }

    upDateSpuStatus(2);
    $("#rejName").text($("#productName").text() + "（" + $("#spuID").val() + "）已被駁回");
}

function upDateSpuStatus(spuStatus) {
    var reason = "";

    if (spuStatus == 2) {
        reason = $("#rejReason").val();
    }

    $.ajax({
        type: 'POST',
        url: "/product/UpdateSpuStatus",
        data: "spu=" + $("#spuID").val() + "&status=" + spuStatus + "&reason=" + reason,
        async: true,
        success: function (data) {
            if (spuStatus == 1) {
                $("#passingSuccess").show();
                adjust("#passingSuccess");
            }

            if (spuStatus == 2) {
                $("#rejRea").hide();
                $("#rejectingSuccess").show();
                adjust("#rejectingSuccess");
            }
        }
    });
}

function closeOverLayAndShowAlert() {
    hideOverlay();
    $("#passingSuccess").hide();
    $("#rejectingSuccess").hide();
    $("#rejRea").hide();
    $("#temSuccess").hide();
}

function customReportTemStorage(isShowAlert) {
    var json = {};
    var cusReps = new Array()

    $("#cusReports").find("table").each(function (index) {
        var rep = {
            "Sku": $("#sku_" + index).text(),
            "CustomsUnit": $("#CustomsUnit_" + index).val(),
            "InspectionNo": $("#InspectionNo_" + index).val(),
            "HSCode": $("#HSCode_" + index).val(),
            "UOM": $("#UOM_" + index).val(),
            "PrepardNo": $("#PrepardNo_" + index).val(),
            "GnoCode": $("#GnoCode_" + index).val(),
            "TaxRate": $.trim($("#TaxRate_" + index).val()) == "" ? "" : parseFloat($("#TaxRate_" + index).val()),
            "TaxCode": $("#TaxCode_" + index).val(),
            "ModelForCustoms": $("#ModelForCustoms_" + index).val(),
        };

        cusReps[index] = rep;
    });

    json.CustomReports = cusReps;

    var isChangeRS = $("#salesTerritory").val() != 2 ? true : false;

    $.ajax({
        type: 'POST',
        url: "/product/CustomReportTempStorage",
        data: "json=" + JSON.stringify(json) + "&isChangeRS=" + isChangeRS,
        async: true,
        success: function (data) {
            if (isShowAlert == true) {
                showOverlay();
                $("#temSuccess").show();
                adjust("#temSuccess");
            }
        }
    });
}

function ReturnToListPage()
{
    window.location.href = "/product/ProductAuditingIndex?StartTime=" + $("#createTimeStart").val() + "&EndTime=" + $("#createTimeEnd").val() + "&Spu=" + $("#spu").val() + "&Sku=" + $("#sku").val() + "&ProductName=" + $("#productName").val() + "&EditType=" + $("#editType").val() + "&ReportStatus=" + $("#reportStatus").val() + "&Supplierid=" + $("#supplierId").val() + "&SalesTerritory=" + $("#salesTerritory").val();

    //$("#returnForm").submit();
}