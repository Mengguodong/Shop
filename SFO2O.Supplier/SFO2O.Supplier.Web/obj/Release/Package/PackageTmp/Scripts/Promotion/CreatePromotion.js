$(document).ready(function () {

    $("#proSkuCount").val(0);

    $("#showLable").mouseover(function () {
        $("#pLable").show();
    }).mouseout(function () {
        $("#pLable").hide();
    });

    $("#lable").blur(function () {
        CheckLable();
    }).focus(function () {
        $("#lable").parent().next().hide();
        $("#lable").parents("td").removeClass("error").addClass("focus");
    });

    $("#promotionName").blur(function () {
        CheckPromotionName();
    }).focus(function () {
        $("#promotionName").next().hide();
        $("#promotionName").parents("td").removeClass("error").addClass("focus");
    });

    $("#promotionPercent").blur(function () {
        CheckPromotionName();
    }).focus(function () {
        $("#promotionName").next().hide();
        $("#promotionName").parents("td").removeClass("error").addClass("focus");
    });

    $("#promotionPercent").blur(function () {
        if (CheckPromotionPercent() == true) {
            if ($.trim($("#promotionPercent").val).length == 0) {
                $("#promotionPercent").next().hide();
            }
        }
        else {
            $("#promotionPercent").next().hide();
        }
    }).focus(function () {
        $("#promotionPercent").next().show();
        $("#promotionPercent").parents("td").removeClass("error").addClass("focus");
        $("#promotionPercent").parent().next().hide();
    });

    $("#firstLi").click(function () {
        SwitchTab("firstLi");
    });

    $("#secondLi").click(function () {
        SwitchTab("secondLi");
    });

    GetSupplierSkus(1);

    $("#search").click(function () {
        GetSupplierSkus(1);
    });

    $("#submitPromotion").click(function () {
        SavePromotion();
    });

    if ($("#promotionId").val().length > 0) {
        GetSelectSkuCount();
    }

    $("#querySkuName").focus(function () {
        $("#querySkuName").parent().animate({
            width: "310px"
        }, 1000);
        $("#querySkuName").animate({
            width: "254px"
        }, 1000);

    }).blur(function () {
        $("#querySkuName").parent().animate({
            width: "110px"
        }, 1000);
        $("#querySkuName").animate({
            width: "54px"
        }, 1000);
    }).keyup(function (event) {
        if (event.keyCode == 13) {
            $("#querySkuName").blur();
            $("#search").click();
        };
    });
});

//活动标签
function CheckLable() {
    var lable = $("#lable");

    if ($.trim(lable.val()).length == 0) {
        return true;
    }

    if ($.trim(lable.val()).length < 2) {
        lable.parents("td").removeClass("focus").addClass("error");
        lable.next().next().text("活動標籤必須大於等於2個字").show();
        return false;
    }

    lable.parents("td").removeClass("focus");

    return true;
}

//活动名称
function CheckPromotionName() {
    var name = $("#promotionName");

    if ($.trim(name.val()).length == 0) {
        name.parent().parent().removeClass("focus").addClass("error");
        name.next().text("請輸入活動名稱").show();
        return false;
    }

    name.parent().parent().removeClass("focus");

    return true;
}

//活动费用
function CheckPromotionPercent() {
    var percent = $("#promotionPercent");

    if ($.trim(percent.val()).length == 0) {
        percent.parents("td").removeClass("focus").addClass("error");
        percent.parent().next().text("請輸入活動費用").show();
        return false;
    }

    var percentNumber = Number($.trim(percent.val()));

    if (percentNumber < 0) {
        percent.parents("td").removeClass("focus").addClass("error");
        percent.parent().next().text("請輸入0-100的整數").show();
        return false;
    }

    if (percentNumber > 100) {
        percent.parents("td").removeClass("focus").addClass("error");
        percent.parent().next().text("請輸入0-100的整數").show();
        return false;
    }

    var reg = /^\d+$/;
    if (!reg.test(percent.val())) {
        percent.parents("td").removeClass("focus").addClass("error");
        percent.parent().next().text("請輸入0-100的整數").show();
        return false;
    }

    percent.parents("td").removeClass("focus")

    return true;
}

function GetSupplierSkus(pageIndex) {
    if (pageIndex == undefined || pageIndex == "")
        pageIndex = 1;

    $.ajax({
        type: 'POST',
        url: "/Promotion/GetSupplierSkus",
        data: "&PageSize=" + 20 + "&PageNo=" + pageIndex + "&productName=" + $("#querySkuName").val() + "&redisNo=" + $("#redisNo").val() + "&promotionId=" + $("#promotionId").val(),
        async: true,
        success: function (data) {
            $("#skuList").html(data);
            displayPage1(GetSupplierSkus, $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pager"));
        }
    });
}

function CheckPromotion() {
    if ($.trim($("#lable").val()).length > 0 && CheckLable() == false) {
        return false;
    }

    if (CheckPromotionName() == false) {
        return false;
    }

    if (CheckPromotionPercent() == false) {
        return false;
    }
}


function ShowPromotionDiv(sku, type) {
    $("div.mg_l10").each(function () {
        $(this).removeClass("posR");
    });

    $(".Recommended_text01").hide();

    $("#operationType_" + sku).val(type);

    $("#pd_" + sku).parent().addClass("posR");

    $("#pd_" + sku).show();
    $("#pd_" + sku).parents("td").removeClass("error");//取消错误样式

    HidePromotionDivError(sku);

    $("#pd_PP_" + sku).val($.trim($("#pp_" + sku).text().replace("$", "")));
    $("#pd_PR_" + sku).val($.trim($("#pr_" + sku).text().replace("折", "")));
}

function HideAddTips(sku) {
    $("#pd_" + sku).hide();
    $("#pd_" + sku).parent().removeClass("posR");
}

function CheckPromotionPrice(sku) {
    var proPrice = $("#pd_PP_" + sku);

    if ($.trim(proPrice.val()).length == 0) {
        proPrice.parents("td").removeClass("focus").addClass("error");
        proPrice.parent().next().text("請輸入價格").show();
        return false;
    }

    var proPriceNumber = Number($.trim(proPrice.val()));

    if (proPriceNumber <= 0) {
        proPrice.parents("td").removeClass("focus").addClass("error");
        proPrice.parent().next().text("價格必須大於零").show();
        return false;
    }

    proPrice.parents("td").removeClass("focus");

    var oprice = $("#sp_" + sku).text().replace("$", "");
    $("#pd_PR_" + sku).val(((proPriceNumber / oprice) * 100 / 10).toFixed(2));
    HidePromotionDivError(sku);

    return true;
}

function CheckPromotionRate(sku) {
    var proRate = $("#pd_PR_" + sku);

    if ($.trim(proRate.val()).length == 0) {
        proRate.parents("td").removeClass("focus").addClass("error");
        proRate.parent().parent().next().text("請輸入折扣").show();

        proRate.next().hide();
        return false;
    }

    var rateNumber = Number($.trim(proRate.val()));

    if (rateNumber <= 0) {
        proRate.parents("td").removeClass("focus").addClass("error");
        proRate.parent().parent().next().text("折扣必須大於零").show();

        proRate.next().hide();
        return false;
    }

    if (rateNumber >= 10) {
        proRate.parents("td").removeClass("focus").addClass("error");
        proRate.parent().parent().next().text("折扣必須小於十").show();

        proRate.next().hide();
        return false;
    }

    proRate.parents("td").removeClass("focus");
    proRate.next().show();

    var oprice = $("#sp_" + sku).text().replace("$", "");
    $("#pd_PP_" + sku).val(((rateNumber * 10 / 100) * oprice).toFixed(2));

    HidePromotionDivError(sku);

    return true;
}

function PromotionPriceOnfouce(sku) {
    var proPrice = $("#pd_PP_" + sku);

    proPrice.parents("td").removeClass("error").addClass("focus");
    proPrice.next().show();
}

function PromotionRateOnfouce(sku) {
    var proRate = $("#pd_PR_" + sku);

    proRate.parents("td").removeClass("error").addClass("focus");
    proRate.parent().parent().next().hide();

    proRate.next().show();
}

function SubmitTips(sku) {
    if (CheckPromotionPrice(sku) == false) {
        return false;
    }

    if (CheckPromotionRate(sku) == false) {
        return false;
    }

    $("#pp_" + sku).text("").text("$" + $.trim($("#pd_PP_" + sku).val()));
    $("#pr_" + sku).text("").text($.trim($("#pd_PR_" + sku).val()) + "折");
    AddPromotionSku(sku);

    HideAddTips(sku);
    if ($("#operationType_" + sku).val() == 1) {

        CountPromotionSkuCount(1);
    }

    $("#addPro_" + sku).hide();
    $("#caPro_" + sku).show();
    $("#moPro_" + sku).show();
}

function RemovePromotion(sku, type) {

    RemovePromotionSku(sku);
    GetSelectPromotionSkus(type);

    $("#addPro_" + sku).show();
    $("#caPro_" + sku).hide();
    $("#moPro_" + sku).hide();
    $("#pd_" + sku).hide();

    $("#pp_" + sku).text("");
    $("#pr_" + sku).text("");

    CountPromotionSkuCount(0);
}


//隐藏设置促销层错误信息
function HidePromotionDivError(sku) {
    $("#pd_" + sku).find(".wrong_tips").hide();
}

//计算选中的SKU数量
function CountPromotionSkuCount(dirction) {
    var skuCount = 0;
    if (dirction == 1) {
        skuCount = Number($("#proSkuCount").val()) + 1;
    }
    else {
        skuCount = Number($("#proSkuCount").val()) - 1;
    }

    OperationBySkuCount(skuCount);
}

function OperationBySkuCount(skuCount) {
    $("#proSkuCount").val(skuCount);

    if (skuCount > 99) {
        $("#emSkuCount").text("99+");
    }
    else {
        $("#emSkuCount").text(skuCount);
    }

    if (skuCount <= 0) {
        $("#emSkuCount").text(0);
        $("#submitPromotion").addClass("btn_shenhe_off");
        $("#firstLi").click();
    }
    else {
        $("#submitPromotion").removeClass("btn_shenhe_off");
    }
}

function AddPromotionSku(sku) {
    $.ajax({
        type: 'POST',
        url: "/Promotion/AddPromotionSku",
        data: "&redisNo=" + $("#redisNo").val() + "&spu=" + $("#spu_" + sku).val() + "&sku=" + sku + "&promotionPrice=" + $("#pp_" + sku).text().replace("$", "") + "&promotionRate=" + $("#pr_" + sku).text().replace("折", ""),
        async: false,
        success: function (data) {

        }
    });
}

function RemovePromotionSku(sku) {
    $.ajax({
        type: 'POST',
        url: "/Promotion/RemovePromotionSku",
        data: "&redisNo=" + $("#redisNo").val() + "&spu=" + $("#spu_" + sku).val() + "&sku=" + sku,
        async: false,
        success: function (data) {
        }
    });
}

function GetSelectPromotionSkus(type) {
    $.ajax({
        type: 'POST',
        url: "/Promotion/GetSelectPromotionSkus",
        data: "&redisNo=" + $("#redisNo").val(),
        async: false,
        success: function (data) {
            if (type == 1) {
                $("#chosenSkuList").html(data).show();
            }
        }
    });
}

function GetSelectSkuCount() {
    $.ajax({
        type: 'POST',
        url: "/Promotion/GetPromotionSkus",
        data: "&redisNo=" + $("#redisNo").val() + "&promotionId=" + $("#promotionId").val(),
        async: false,
        success: function (data) {
            if (data > 0) {
                OperationBySkuCount(data);
            }
        }
    });
}

function SwitchTab(liId) {
    if (liId == "firstLi") {
        $("#skuList").show();
        $("#chosenSkuList").hide();

        $("#firstLi").addClass("current");
        $("#secondLi").removeClass("current");
    }
    else {
        if (Number($("#proSkuCount").val()) > 0) {
            $("#skuList").hide();
            $("#chosenSkuList").show();

            $("#firstLi").removeClass("current");
            $("#secondLi").addClass("current");

            GetSelectPromotionSkus(1);
        }
    }
}

function SavePromotion() {
    if ($("#submitPromotion").hasClass("btn_shenhe_off")) {
        return false;
    }

    if (CheckPromotion() == false) {
        return false;
    }

    var label = $.trim($("#lable").val());

    if (label.length == 0) {
        label = "促销价";
    }

    $.ajax({
        type: 'POST',
        url: "/Promotion/SavePromotion",
        data: "&redisNo=" + $("#redisNo").val() + "&promotionId=" + $("#promotionId").val() + "&promotionName=" + $("#promotionName").val() + "&startTime="
            + $("#StartTime").val() + "&endTime=" + $("#EndTime").val() + "&promotionLable=" + label + "&promotionCost=" + $("#promotionPercent").val(),
        async: false,
        success: function (data) {
            if (data.IsSuccess == true) {
                $.dialog({
                    content: "申請成功！",
                    onConfirm: function () {
                        window.location.href = "/Promotion/PromotionList";
                    }
                })
            }
            else {
                $.dialog({
                    content: data.Message,
                    onConfirm: function () {
                        return true;
                    }
                });
            }
        }
    });
}
