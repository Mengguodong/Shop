$(function () {
    $("#mainLi").click(function () { switchTab("mainLi") });
    $("#hkLi").click(function () { switchTab("hkLi") });

    $('#OrderStatus').chosen({ disable_search: true });
    $('#SellerId').chosen({ disable_search: true });
    if ($("#isExcludeCloseOrder").val() == "1") {
        $("#emCheckBox").addClass("active");
    }
    else {
        $("#emCheckBox").removeClass("active");
    }

    $("#emCheckBox").parent().click(function () {
        IsExcludeCloseOrderFun();
    });

    $("#search").click(function () {
        Query(1);
    });

    $(".win_close").click(function () {
        HideLogistics();
        HideOrderStockOut();
    });

    $("#orderStockOut").click(function () {
        showOverlay();
        $("#divOrderStockOut").show();
        adjust($("#divOrderStockOut"));
    });

    $("#orderStockOutDownload").click(function () {
        OrderStockOutDownload();
    });

    Query(1);
});

function switchTab(liId) {
    $("#" + liId).parent().children().each(function () {
        $(this).removeClass("current");
    });

    $("#" + liId).addClass("current");

    if (liId == "mainLi") {
        $("#main").show();
        $("#HK").hide();
        $("#pagerMain").show();
        $("#pagerHK").hide();
        QueryMain($("#pagerMain").find("li.pgCurrent").text());
    }
    else {
        $("#main").hide();
        $("#HK").show();
        $("#pagerMain").hide();
        $("#pagerHK").show();
        QueryHK($("#pagerHK").find("li.pgCurrent").text());
    }
}

function IsExcludeCloseOrderFun() {
    if ($("#emCheckBox").hasClass("active")) {
        $("#emCheckBox").removeClass("active")
        $("#isExcludeCloseOrder").val(0);
        return;
    }
    else {
        $("#emCheckBox").addClass("active")
        $("#isExcludeCloseOrder").val(1);
        return;
    }
}

function Query(pageIndex) {
    if (pageIndex == undefined || pageIndex == "")
        pageIndex = 1;

    $("#countryCode").val(1);

    $.ajax({
        type: 'POST',
        url: "/Order/GetOrderList",
        data: $('#queryForm').serialize() + "&PageSize=" + 20 + "&PageIndex=" + pageIndex + "&isPaging=" + 0,
        async: true,
        success: function (data) {
            $("#totalTable").remove();
            $("#hidDiv").html("");

            $("#hidDiv").html(data);
            $("#total").html($("#totalTable"));
            $("#mainLandDiv").html($("#orderList").html());

            displayPage1(QueryMain, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pagerMain"));

            $("#hidDiv").html("");
            $("#recordCount").remove();
            $("#pageIndex").remove();

            if ($("#recordCount").val() <= 0) {
                $("#hkLi").click();
            }
        }
    });
}


function QueryMain(pageIndex) {
    if (pageIndex == undefined || pageIndex == "")
        pageIndex = 1;

    $("#countryCode").val(1);

    $.ajax({
        type: 'POST',
        url: "/Order/GetOrderList",
        data: $('#queryForm').serialize() + "&PageSize=" + 20 + "&PageIndex=" + pageIndex + "&isPaging=" + 1,
        async: true,
        success: function (data) {
            $("#totalTable").remove();
            $("#hidDiv").html("");
            

            $("#hidDiv").html(data);
            $("#total").html($("#totalTable"));
            $("#mainLandDiv").html($("#orderList").html());

            displayPage1(QueryMain, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pagerMain"));

            $("#hidDiv").html("");

            ClearElements();
        }
    });
}

function QueryHK(pageIndex) {
    if (pageIndex == undefined || pageIndex=="")
        pageIndex = 1;

    $("#countryCode").val(2);

    $.ajax({
        type: 'POST',
        url: "/Order/GetOrderList",
        data: $('#queryForm').serialize() + "&PageSize=" + 20 + "&PageIndex=" + pageIndex + "&isPaging=" + 1,
        async: true,
        success: function (data) {
            $("#totalTable").remove();
            $("#hidDiv").html("");

            $("#hidDiv").html(data);
            $("#total").html($("#totalTable"));
            $("#HKDiv").html($("#orderList").html());
            displayPage1(QueryHK, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 20, $("#pagerHK"));

            $("#hidDiv").html("");

            ClearElements();
        }
    });
}

function ClearElements() {
    $("#recordCount").remove();
    $("#pageIndex").remove();

    $("#Export").focus();
}

function ShowLogistics(orderCode) {
    $.ajax({
        type: 'POST',
        url: "/Order/GetOrderLogistics",
        data: "orderCode=" + orderCode,
        async: true,
        success: function (data) {
            $("#logs").html(data);
            showOverlay();
            $("#divLogistics").show();
            adjust($("#divLogistics"));

            //var h = $(id).height();
            //var t = (windowHeight() / 2) - (h / 2)-1000;

            //$("#divLogistics").css({top: t + 'px' });
        }
    });
}

function HideLogistics() {
    $("#divLogistics").hide();
    hideOverlay();
}

function HideOrderStockOut() {
    $("#divOrderStockOut").hide();
    hideOverlay();
}

function Export() {
    window.location.href = "/Order/ExportOrderList?startTime=" + $("#CreateTimeStart").val() + "&endTime=" + $("#CreateTimeEnd").val() + "&sku=" + $("#SKU").val() + "&orderStatus=" + $("#OrderStatus").val() + "&buyerAccount=" + $("#buyerAccount").val() + "&supplierId=" + $("#SellerId").val() + "&orderCode=" + $("#orderCode").val() + "&isExcludeCloseOrder=" + $("#isExcludeCloseOrder").val();
}

function OrderStockOutDownload() {
    window.location.href = "/Order/ExportOrderStockOutInfos?startTime=" + $("#orderStockOutStartTime").val() + "&endTime=" + $("#orderStockOutEndTime").val();
    HideOrderStockOut();
};