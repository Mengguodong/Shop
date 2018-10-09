$(function () {
    $('#SupplierStatus').chosen({ disable_search: true });
  
    $("#search").click(function () {
        Query(1);
    });
    Query(1);
})

function Query(pageIndex) {
    if (pageIndex == undefined)
        pageIndex = 1;

    $.ajax({
        type: 'POST',
        url: "/supplier/SupplierQueryList",
        data: $('#queryForm').serialize() + "&PageSize=" + 50 + "&PageNo=" + pageIndex,
        async: true,
        success: function (data) {
            $("#suppliers").html(data);
            displayPage(Query, $("#recordCount").val(), $("#recordCount").val(), $("#pageIndex").val(), 50)
        }
    });
}

function Export()
{
    var params = {
        startTime: $("#CreateTimeStart").val(),
        endTime: $("#CreateTimeEnd").val(),
        companyName: $("#CompanyName").val(),
        accountName: $("#AccountName").val(),
        supplierStatus: $("#SupplierStatus").val()
    };

    window.location.href = "/Supplier/ExportSupplierQueryList?startTime=" + $("#CreateTimeStart").val() + "&endTime=" + $("#CreateTimeEnd").val() + "&companyName=" + $("#CompanyName").val() + "&accountName=" + $("#AccountName").val() + "&supplierStatus=" + $("#SupplierStatus").val();
}

function ShowActiuonAlert(actionID) {
    var action = $("#" + actionID);
    showOverlay();
    adjust("#warning");
    $("#warning").show();

    if (action.text() == "凍結") {
        $("#title").text("凍結商家賬號");
        $("#message").text("凍結后，商家將不能登入商家管理中心，其所有在售商品將全部下架。您是否要凍結此商家賬號？");
    }
    else {
        $("#title").text("恢復商家賬號");
        $("#message").text("您確認要解除此商家賬號的凍結狀態嗎？");
    }

    $("#actionId").val(actionID);
}

function CancleAction() {
    hideOverlay();
    $("#warning").hide();
}

function updateSupplierStatus() {
    var actionId = $("#actionId").val();

    var supplierId = actionId.split('_')[1];
    var status = 0;

    if ($("#" + actionId).text() == "凍結") {
        status = 2;
    }
    else {
        status = 1;
    }


    $.ajax({
        type: 'POST',
        url: "/supplier/UpdateSupplierStatus",
        data: "supplierId=" + supplierId + "&status=" + status,
        async: true,
        success: function (data) {
            if (status == 2) {
                $("#" + actionId).text("解除");
                $("#td_" + supplierId).text("凍結");
            }
            else {
                $("#" + actionId).text("凍結");
                $("#td_" + supplierId).text("正常");
            }

            CancleAction();
        }
    });
}